using Godot;
using System;
using System.Collections;


public class Female_Zombie : Basic_Zombie
{


	public override void _Ready()
	{
		character_name = "Female_Zombie";
		settle_fields(200,4000);
		base._Ready();



		available_moves = new ArrayList() { "attack", "death", "idle", "walk" ,"damaged","hurt","damaged"};
		available_moves_consumption = new int[6] { 14, 0, 0, 0 ,0,0};
		available_moves_damage = new int[6] { 2, 0, 0, 0 ,0,0};

		attack_move_names = new ArrayList() { "attack" };

		jump_intensity = 4000;

	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		
	}
}
