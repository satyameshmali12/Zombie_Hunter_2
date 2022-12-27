using Godot;
using System.Collections;

public class Zombie_Knight_1 : Powerful_Zombie
{

	public override void _Ready()
	{
		base._Ready();
		
		
		character_name = "Male_Zombie";
		speed_x += 300;
		available_moves = new ArrayList() { "attack","attack_2","attack_3","death","defend","hurt","idle","jump","protect","run","run_attack","walk"};
		available_moves_consumption = new int[12]{2,5,8,0,3,0,0,0,3,0,5,0};
		available_moves_damage = new int[12] {1,2,3,0,0,0,0,0,0,1,5,0};

		attack_move_names = new ArrayList() { "attack","attack_2","attack_3","run_attack"};

		jump_intensity = 10000;

	}
	public override void _Process(float delta)
	{
		base._Process(delta);

		LinearVelocity = moving_speed;

	}

}
