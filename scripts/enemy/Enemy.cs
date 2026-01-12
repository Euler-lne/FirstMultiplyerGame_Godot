using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] private Area2D area2D;
	private int maxHealth = 5;
	private int currentHealth;
	private float speed = 50f;

	private Vector2 targetPosision;
	private Timer targetAcquisitionTimer;

	public override void _Ready()
	{
		targetAcquisitionTimer = GetNode<Timer>("TargetAcquisitionTimer");
		targetAcquisitionTimer.Timeout += OnTargetAcquisitionTimerTimeout;
		area2D.AreaEntered += OnAreaEntered;
		currentHealth = maxHealth;
		if (IsMultiplayerAuthority())
			SetTargetAcquisition();
	}
	public override void _ExitTree()
	{
		area2D.AreaEntered -= OnAreaEntered;
		targetAcquisitionTimer.Timeout -= OnTargetAcquisitionTimerTimeout;

	}

	public override void _Process(double delta)
	{
		if (IsMultiplayerAuthority())
			Velocity = Position.DirectionTo(targetPosision) * speed;
		MoveAndSlide();
	}


	private void OnTargetAcquisitionTimerTimeout()
	{
		if (IsMultiplayerAuthority())
			SetTargetAcquisition();
	}

	private void SetTargetAcquisition()
	{
		Player nearestPlayer = null;
		float nearestDistance = 0.0f;
		foreach (var item in GetTree().GetNodesInGroup("player"))
		{
			if (item is Player player)
			{
				if (nearestPlayer == null || nearestDistance > GlobalPosition.DistanceSquaredTo(player.GlobalPosition))
				{
					nearestPlayer = player;
					nearestDistance = GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
				}
			}
			else
				GD.PrintErr("group player 中的类型不是 Player");
		}
		if (nearestPlayer != null)
			targetPosision = nearestPlayer.GlobalPosition;
		else
			GD.PrintErr("没有找到任何玩家");
	}

	private void OnAreaEntered(Area2D area)
	{
		if (!IsMultiplayerAuthority()) return;
		if (area.Owner is Bullet bullet)
		{
			bullet.QueueFree();
			TakeDamage(1);
		}
	}


	private void TakeDamage(int damage)
	{
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			QueueFree();
		}
	}

}
