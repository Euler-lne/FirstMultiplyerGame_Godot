using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private PlayerInputSynchronizerComponent synchronizer;
	[Export] private Node2D weaponRoot;
	public const float Speed = 100.0f;

	// private Vector2 startDrawPosition = Vector2.Zero;
	// private Vector2 endDrawPosition = Vector2.Zero;
	// private Vector2 aimVector = Vector2.Right;

	public override void _Draw()
	{
		// DrawLine(startDrawPosition, endDrawPosition, Colors.Red, 2f);
		// DrawLine(Vector2.Zero, aimVector * 50, Colors.Green, 2f);
	}


	public override void _Ready()
	{
		// GD.Print("Player " + Name + " ready. My ID is " + Multiplayer.GetUniqueId() + ". Am I Authority " + IsMultiplayerAuthority());
		// SetProcess(IsMultiplayerAuthority());  // 只有拥有 Multiplayer Authority 的节点才会执行 _Process 方法,也就是只有服务器才会执行
		synchronizer.SetMultiplayerAuthority(int.Parse(Name));
	}


	public override void _Process(double delta)
	{
		weaponRoot.Rotation = synchronizer.GetAimVector().Angle();
		// 只有在服务器上执行移动，武器的朝向在各自的客户端上执行
		if (IsMultiplayerAuthority())
		{
			Velocity = synchronizer.GetMovementVector() * Speed;
			MoveAndSlide();
		}

		// startDrawPosition = weaponRoot.GlobalPosition;
		// endDrawPosition = GetGlobalMousePosition();
		// aimVector = startDrawPosition.DirectionTo(endDrawPosition);
		// QueueRedraw();
	}
}
