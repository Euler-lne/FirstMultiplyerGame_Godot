using Godot;
using System;

public partial class Bullet : Node2D
{
	private Vector2 direction = Vector2.Left;
	private float speed = 600.0f;
	[Export] private Timer lifeTimer;
	public override void _Ready()
	{
		lifeTimer.Timeout += OnLifeTimerTimeout;
	}

	private void OnLifeTimerTimeout()
	{
		if (IsMultiplayerAuthority())   // 所有玩家都会进入这个函数，但是只有服务器会执行 QueueFree
			QueueFree();
	}

	public override void _Process(double delta)
	{
		GlobalPosition += direction * speed * (float)delta;
	}

	public void InitBullet(Vector2 pos, Vector2 dir)
	{
		GlobalPosition = pos;
		direction = dir;
		Rotation = direction.Angle();
	}
}
