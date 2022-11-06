using Godot;
using System;

public class Knight : Basic_Character
{

	// bool is_busy;
	
	[Export]
	public int slide_speed_increment;


	public override void _Ready()
	{
		custom_constructor(600,20000);
		// _node_type = "player";
		
		is_busy = false;

		




	}

	public override void _Process(float delta)
	{
		basic_animation_changing_condition = !is_busy;

		custom_process(delta);

		if(Input.IsActionJustPressed("Jump_Attack") && !is_on_ground){
			animations.Animation = "Jump_Attack";
			is_busy = true;
		}
		else if(animations.Animation=="Jump_Attack" && is_on_ground){
			animations.Animation = "Idle";
			is_busy = false;
		}

		LinearVelocity = moving_speed;

		// health_bar.SetPosition(health_bar.RectPosition+new Vector2(100,100));
		// health_bar.RectPosition+=moving_speed;


		
	}

	public override void collided_with_body(Node body)
	{
		base.collided_with_body(body);
		
	}
}
