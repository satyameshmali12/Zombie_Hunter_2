using Godot;
using System;
using System.Collections;


/*
movements
press T for Attack 2
press ctrl+c for cast
press s for shoot
*/

public class Basic_Ninja : Basic_Player
{

	public override void _Ready()
	{
		settle_fields(900, 13000);

		base._Ready();

		is_busy = false;
	}

	public override void _Process(float delta)
	{
		basic_animation_changing_condition = !is_busy;
		
		base._Process(delta);

		
		if (Input.IsActionJustPressed("T"))
		{
			perform_move("Attack_2");
		}

		set_animation_idle("Attack_2");
		set_animation_idle("Jump");
		set_animation_idle("Shoot");
		set_animation_idle("Run");


		move();
		
	}

	
}
