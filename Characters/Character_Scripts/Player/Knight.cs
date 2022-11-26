using Godot;
using System;
using System.Collections;

public class Knight : Basic_Player
{

	// bool is_busy;
	
	[Export]
	public int slide_speed_increment;


	public override void _Ready()
	{
		base._Ready();
		custom_constructor(650,10000);
		available_moves = new ArrayList(){"attack","death","idle","jump","jump_attack","run","walk"};
		available_moves_consumption = new int[7]{0,0,0,0,10,0,0};
		available_moves_damage = new int[7]{10,0,0,0,20,0,0};

		is_busy = false;
	}

	public override void _Process(float delta)
	{
		base._Process(delta);

		
		basic_animation_changing_condition = !is_busy;

		custom_process(delta);

		if(Input.IsActionJustPressed("Jump_Attack") && !is_on_ground && can_perform_move("Jump_Attack")){
			animations.Animation = "Jump_Attack";
			is_busy = true;
		}
		else if(animations.Animation=="Jump_Attack" && is_on_ground){
			animations.Animation = "Idle";
			is_busy = false;
		}

		if(Input.IsActionPressed("F") && !is_busy && animations.Animation!="Attack"){
			animations.Animation = "Attack";
		}
		set_animation_idle("Attack");

		LinearVelocity = moving_speed;
	}

	public override void collided_with_body(Node body)
	{
		base.collided_with_body(body);
		
	}
}
