using Godot;
using System;

public partial class EnemyManager : Node
{
	const int ROUND_BASE_TIME = 10;
	const int ROUND_GROWTH = 5;
	const float BASE_ENEMY_SPAWN_TIME = 2f;
	const float ENEMY_SPAWN_TIME_GROWTH = -0.15f;
	[Export] private PackedScene enemyScene;
	[Export] private ReferenceRect spawnRect;
	private Timer spawnInternalTimer;
	private Timer roundTimer;
	private int roundCount = 0;

	public override void _Ready()
	{
		spawnInternalTimer = GetNode<Timer>("SpawnInternalTimer");
		roundTimer = GetNode<Timer>("RoundTimer");
		spawnInternalTimer.Timeout += OnSpawnInternalTimeout;
		roundTimer.Timeout += OnRoundTimerTimeout;
		spawnInternalTimer.Start();
		BeginRound();
	}

	public override void _ExitTree()
	{
		spawnInternalTimer.Timeout -= OnSpawnInternalTimeout;
		roundTimer.Timeout -= OnRoundTimerTimeout;
	}

	private void OnRoundTimerTimeout()
	{
		if (IsMultiplayerAuthority())
			spawnInternalTimer.Stop();
	}

	private void OnSpawnInternalTimeout()
	{
		if (IsMultiplayerAuthority())
		{
			SpawnEnemy();
			spawnInternalTimer.Start();
		}
	}

	public void BeginRound()
	{
		roundCount += 1;
		roundTimer.WaitTime = ROUND_BASE_TIME + (roundCount - 1) * ROUND_GROWTH;
		roundTimer.Start();

		// 减少生成时间相当于增加了每一波敌人的生成数量
		spawnInternalTimer.WaitTime = BASE_ENEMY_SPAWN_TIME + (roundCount - 1) * ENEMY_SPAWN_TIME_GROWTH;
		spawnInternalTimer.Start();
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
