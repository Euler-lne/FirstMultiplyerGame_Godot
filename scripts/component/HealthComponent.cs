using Godot;
using System;
public partial class HealthComponent : Node
{
	[Export] private int maxHealth = 1;
	private int currentHealth;
	public event Action OnDeath;

	public int CurrentHealth
	{
		get { return currentHealth; }
		private set
		{
			currentHealth = value;
			if (value <= 0)
			{
				currentHealth = 0;
				OnDeath?.Invoke();
			}
		}
	}
	public void TakeDamge(int value)
	{
		CurrentHealth -= value;
	}
}
