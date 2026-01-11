using Godot;
using System;
using System.Collections.Generic;

public partial class MainScene : Node2D
{
    [Export] private MultiplayerSpawner multiplayerSpawner;
    [Export] private PackedScene playerScene;
    public override void _Ready()
    {
        /**
        这段代码的作用：通知服务器"我准备好了"  
        具体流程：
            1. 每个玩家进入主场景时
            2. 向服务器(ID=1)发送一个RPC消息："PeerReady"
            3. 服务器收到后，知道该玩家已经准备就绪
            4. 当所有玩家都发送了"PeerReady"，服务器可以开始游戏
        */
        // GD.Print("MainScene ready. My ID is " + Multiplayer.GetUniqueId());
        // RpcId(1, nameof(PeerReady));
        // 执行到这里所有对等体（Peer）都会向服务器发送一个RPC调用，服务器本身也会给自己发，因为 CallLocal = true

        // GD.Print("--------------------------------");
        // GD.Print("No Rpc");
        // PeerReady();

        // 解决方案：使用 Callable.From 执行 multiplayerSpawner.Spawn 时调用
        multiplayerSpawner.SpawnFunction = Callable.From((Variant variantData) =>
        {
            var godotDict = variantData.AsGodotDictionary();
            var player = playerScene.Instantiate<Player>();
            player.Name = godotDict["peerId"].ToString();
            return player;
        });

        RpcId(1, nameof(PeerReady));
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void PeerReady()
    {
        // Multiplayer.GetRemoteSenderId()  作用：获取发送这个RPC的玩家ID
        // GD.Print("Current peer " + Multiplayer.GetUniqueId() + ". Peer " + Multiplayer.GetRemoteSenderId() + " is ready.");
        // 注意 Multiplayer.GetUniqueId() 和 Multiplayer.GetRemoteSenderId() 的区别；
        // 当前是谁调用这个方法，谁就是 UniqueId，而发送这个RPC的玩家ID是 RemoteSenderId
        // 可以验证没有使用Rpc call的话 Multiplayer.GetRemoteSenderId() 会返回0

        Godot.Collections.Dictionary<string, int> data = new()  // 只能使用 Godot 的 Dictionary 进行网络传输，否则会报错 主要是转换为 Variant 时出错
        {
            { "peerId", Multiplayer.GetRemoteSenderId()}
        };
        Variant variantData = Variant.From(data);
        multiplayerSpawner.Spawn(variantData);
    }
}
