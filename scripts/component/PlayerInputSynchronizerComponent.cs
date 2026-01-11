using Godot;
using System;

public partial class PlayerInputSynchronizerComponent : MultiplayerSynchronizer
{
	private Vector2 movementVector = Vector2.Zero; // [Export] 是为了在Godot中选中这个变量进行同步，不写可以直接写变量名


	public override void _Process(double delta)
	{
		if (IsMultiplayerAuthority())
		{
			GatherInput();
		}
	}

	private void GatherInput()
	{
		movementVector = Input.GetVector("move_left", "move_right", "move_up", "move_down");
	}

	public Vector2 GetMovementVector()
	{
		return movementVector;
	}

}
