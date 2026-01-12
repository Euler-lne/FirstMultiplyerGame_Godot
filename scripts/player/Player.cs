using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private PlayerInputSynchronizerComponent synchronizer;
	[Export] private Node2D weaponRoot;
	[Export] private PackedScene bullet;
	[Export] private Node2D bulletSpawnPos;
	[Export] private Timer fireTimer;
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
		bool isDigital = int.TryParse(Name, out _);
		if (isDigital)
			synchronizer.SetMultiplayerAuthority(int.Parse(Name));
		else
		{
			GD.PrintErr("Player name is not digital, cannot set Multiplayer Authority!");
		}
	}


	public override void _Process(double delta)
	{
		if (synchronizer.GetAimVector() != Vector2.Zero)
			weaponRoot.Rotation = synchronizer.GetAimVector().Angle();
		// 只有在服务器上执行移动，武器的朝向在各自的客户端上执行
		if (IsMultiplayerAuthority())
		{
			Velocity = synchronizer.GetMovementVector() * Speed;
			MoveAndSlide();
			if (synchronizer.IsAttacking())
				SpawnBullet();
		}

		// startDrawPosition = weaponRoot.GlobalPosition;
		// endDrawPosition = GetGlobalMousePosition();
		// aimVector = startDrawPosition.DirectionTo(endDrawPosition);
		// QueueRedraw();
	}

	private void SpawnBullet()
	{
		if (!fireTimer.IsStopped())
			return;
		Bullet newBullet = bullet.Instantiate<Bullet>();
		newBullet.InitBullet(bulletSpawnPos.GlobalPosition, synchronizer.GetAimVector());
		// 由于子弹的各种参数都是Never同步的，所以设置位置的代码必须要在子弹加载到树上之前执行
		GetParent().AddChild(newBullet, true);// 保证每个子弹都有一个名字，不然同步不了
		fireTimer.Start();
	}
}
