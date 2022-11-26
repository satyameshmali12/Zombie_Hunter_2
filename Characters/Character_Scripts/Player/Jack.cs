using Godot;
using System;

public class Jack : Basic_Player
{
	// public string _node_type{get;set;}


	public override void _Ready()
	{
		custom_constructor(1000,10000);
		_node_type = _Type_of_.Player;

	}

	public override void _Process(float delta)
	{

		custom_process(delta);

		LinearVelocity = moving_speed;


		
	}
}
