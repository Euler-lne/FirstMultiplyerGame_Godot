using Godot;
using System;

public partial class EnemyManager : Node
{
	[Export] private PackedScene enemyScene;
	[Export] private ReferenceRect spawnRect;
	private Timer spawnInternalTimer;

	public override void _Ready()
	{
		spawnInternalTimer = GetNode<Timer>("SpawnInternalTimer");
		spawnInternalTimer.Timeout += OnSpawnInternalTimeout;
		spawnInternalTimer.Start();
	}

	public override void _ExitTree()
	{
		spawnInternalTimer.Timeout -= OnSpawnInternalTimeout;
	}


	private void OnSpawnInternalTimeout()
	{
		if (IsMultiplayerAuthority())
			SpawnEnemy();
	}

	private void SpawnEnemy()
	{
		Enemy enemy = enemyScene.Instantiate<Enemy>();
		enemy.GlobalPosition = GetRandomPosition();
		GetParent().AddChild(enemy, true);  // 只能在Parent中进行初始化，应为联机孵化器设置的路径是Parent的路径
	}

	private Vector2 GetRandomPosition()
	{
		Random random = new();
		int x = random.Next(0, (int)spawnRect.Size.X);
		int y = random.Next(0, (int)spawnRect.Size.Y);
		return spawnRect.GlobalPosition + new Vector2(x, y);
	}

}
