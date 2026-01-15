using Godot;
using System;

public partial class HitComponent : Area2D
{
	[Export] private int damge = 1;
	public event Action<HurtComponent> HitEvent;
	// 这个事件由HurtComponent引起，然后让子弹订阅，不在这里删除子弹，当子弹自己删除自己
	// hitcomponent并不检测hurtcomponent的碰撞事件，没有设置掩码
	public override void _Ready()
	{
	}
	public override void _ExitTree()
	{
	}

	public void RegisterHurtBoxHit(HurtComponent hurtComponent)
	{
		HitEvent?.Invoke(hurtComponent);
	}

	public int Damge
	{
		get { return damge; }
		private set { damge = value; }
	}
}
