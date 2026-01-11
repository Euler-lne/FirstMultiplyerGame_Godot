using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private PlayerInputSynchronizerComponent synchronizer;
	public const float Speed = 150.0f;

	public override void _Ready()
	{
		// GD.Print("Player " + Name + " ready. My ID is " + Multiplayer.GetUniqueId() + ". Am I Authority " + IsMultiplayerAuthority());
		SetProcess(IsMultiplayerAuthority());  // 只有拥有 Multiplayer Authority 的节点才会执行 _Process 方法,也就是只有服务器才会执行
		synchronizer.SetMultiplayerAuthority(int.Parse(Name));
	}


	public override void _Process(double delta)
	{
		Velocity = synchronizer.GetMovementVector() * Speed;
		MoveAndSlide();
	}
}
