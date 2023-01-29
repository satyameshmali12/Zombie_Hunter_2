using Godot;
using System.Collections;

public class Jack : Basic_Player
{
	// public string _node_type{get;set;}


	public override void _Ready()
	{
		this.character_name = "Jack";
		base._Ready();


        is_busy = false;
        available_moves = new ArrayList(){"death","idle","jump","melee","run","shoot","slide"};
        available_moves_consumption = new int[7]{0,0,2,2,0,0,0};
        available_moves_damage = new int[7]{0,0,5,25,1,1,3};
        settle_damage_increment_possible_moves(3);
        
	}

	public override void _Process(float delta)
	{
		base._Process(delta);

		move();


		
	}

}
