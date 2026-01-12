using Godot;
using System;

public partial class PlayerInputSynchronizerComponent : MultiplayerSynchronizer
{
	[Export] private Node2D aimRoot;
	private Vector2 movementVector = Vector2.Zero; // [Export] 是为了在Godot中选中这个变量进行同步，不写可以直接写变量名
	private Vector2 aimVector = Vector2.Right;

	private bool isAttack = false;


	public override void _Process(double delta)
	{
		if (IsMultiplayerAuthority())  // 设置了自己的UniqueId为Multiplayer Authority，所以这个不是在服务器上执行的，而是在本地客户端上执行的
		{
			GatherInput();
		}
	}

	private void GatherInput()
	{
		movementVector = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		aimVector = aimRoot.GlobalPosition.DirectionTo(aimRoot.GetGlobalMousePosition()); // 只有Node2D才有GetGlobalMousePosition属性
		isAttack = Input.IsActionPressed("attack");
	}

	public Vector2 GetMovementVector()
	{
		return movementVector;
	}

	public Vector2 GetAimVector()
	{
		return aimVector;
	}

	public bool IsAttacking()
	{
		return isAttack;
	}
}

