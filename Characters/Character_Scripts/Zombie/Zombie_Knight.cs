using Godot;
using System.Collections;
using System;

public class Zombie_Knight : Powerful_Zombie
{

	Timer protect_timer;
	
	[Export]
	float protection_time = 5;


	RayCast2D upper_detector_ray;

	ArrayList can_jump_on_to;
	Timer can_jump_on_to_timer;



	public override void _Ready()
	{
		character_name = "Male_Zombie";
		settle_fields(400,8000);

		base._Ready();

		
		available_moves = new ArrayList() { "attack","attack_2","attack_3","death","defend","hurt","idle","jump","protect","run","run_attack","walk"};
		available_moves_consumption = new int[12]{2,5,8,0,3,0,0,0,3,0,5,0};
		available_moves_damage = new int[12] {1,2,3,0,0,0,0,0,0,1,5,0};

		attack_move_names = new ArrayList() { "attack","attack_2","attack_3","run_attack"};


		protect_timer = basf.create_timer(protection_time,"Over_Protection");

		can_jump_on_to = new ArrayList(){_Type_of_.Player,_Type_of_.Zombie,_Type_of_.Drone};

		upper_detector_ray = this.GetNode<RayCast2D>("Personals/Upper_Detector");

		can_jump_on_to_timer = basf.create_timer(2f,"Can_Jump_Timer_Stopper");


	}
	public override void _Process(float delta)
	{
		base._Process(delta);

		var past_health = this.get_past_health();

		if(Math.Abs(past_health-health)>10 && protect_timer.IsStopped())
		{
			perform_move("Protect");
			GD.Print("first stage from the zombie_knight .cs haha..!!");
			this.is_resisted = true;
			protect_timer.Start();
		}

		set_animation_idle("Protect");

		

		var upper_collider = upper_detector_ray.GetCollider();
		if(upper_collider!=null && can_jump_on_to_timer.IsStopped())
		{
			Global_Variables_F_A_T type = upper_collider as Global_Variables_F_A_T;
			if(can_jump_on_to.Contains(type._node_type))
			{
				jump();
				can_jump_on_to_timer.Start();
			}
			
		}


		move();
	}

	public void Over_Protection()
	{
		this.is_resisted = false;
		protect_timer.Stop();
	}
	
	void Can_Jump_Timer_Stopper()
	{
		can_jump_on_to_timer.Stop();
	}

}
