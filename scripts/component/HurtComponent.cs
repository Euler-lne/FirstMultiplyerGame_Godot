using Godot;
using System;

public partial class HurtComponent : Area2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] private HealthComponent healthComponent;
	public override void _Ready()
	{
		AreaEntered += OnAreaEnter;
	}

	public override void _ExitTree()
	{
		AreaEntered -= OnAreaEnter;
	}

	private void OnAreaEnter(Area2D area)
	{
		if (!IsMultiplayerAuthority() || area is not HitComponent)
			return;
		if (healthComponent == null)
			GD.PrintErr("health component not initialize!");
		HitComponent hitComponent = (HitComponent)area;
		healthComponent.TakeDamge(hitComponent.Damge);
		hitComponent.RegisterHurtBoxHit(this);
	}

}
