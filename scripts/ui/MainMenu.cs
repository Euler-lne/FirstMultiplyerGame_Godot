using Godot;
using System;

public partial class MainMenu : Control
{
	const int PORT = 7777;
	[Export] private Button hostButton;
	[Export] private Button joinButton;
	[Export] private PackedScene mainScene;

	public override void _Ready()
	{
		hostButton.Pressed += OnPressHostButton;
		joinButton.Pressed += OnPressJoinButton;
		// Multiplayer.PeerConnected += OnPeerConnected;
		Multiplayer.ConnectedToServer += OnConnectedToServer;

	}


	public override void _ExitTree()
	{
		hostButton.Pressed -= OnPressHostButton;
		joinButton.Pressed -= OnPressJoinButton;
		// Multiplayer.PeerConnected -= OnPeerConnected;
		Multiplayer.ConnectedToServer -= OnConnectedToServer;
	}

	private void OnConnectedToServer()
	{
		// GD.Print("Connected to server. My ID is " + Multiplayer.GetUniqueId());
		GetTree().ChangeSceneToPacked(mainScene);
	}


	private void OnPressJoinButton()
	{
		ENetMultiplayerPeer client_peer = new();
		client_peer.CreateClient("127.0.0.1", PORT);
		Multiplayer.MultiplayerPeer = client_peer;
		// GD.Print("My ID is " + Multiplayer.GetUniqueId());
	}


	private void OnPressHostButton()
	{
		ENetMultiplayerPeer server_peer = new();
		server_peer.CreateServer(PORT);
		Multiplayer.MultiplayerPeer = server_peer;
		// GD.Print("My ID is " + Multiplayer.GetUniqueId());
		GetTree().ChangeSceneToPacked(mainScene);
	}


	/// <summary>
	/// 当有新的网络对等体（Peer）连接时调用
	/// 
	/// 重要：这个事件在双方都会触发！
	/// 
	/// 举例：玩家B连接玩家A的服务器
	/// 1. 玩家A（服务器）触发此事件，id = 玩家B的ID（2）
	///    → "有新玩家（ID=2）连接到我"
	///    
	/// 2. 玩家B（客户端）触发此事件，id = 服务器ID（1）
	///    → "我连接到了服务器（ID=1）"
	///    
	/// 注意：参数id是对方的ID，不是自己的ID
	/// </summary>
	/// <param name="id">对方Peer的ID</param>
	private void OnPeerConnected(long id)
	{
		GD.Print("Current ID: " + Multiplayer.GetUniqueId() + ". Peer connected with ID: " + id);
	}

}
