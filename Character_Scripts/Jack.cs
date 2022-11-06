using Godot;
using System;

public class Jack : Basic_Character
{
	// public string _node_type{get;set;}


	public override void _Ready()
	{
		custom_constructor(1000,10000);
		_node_type = "player";

	}

	public override void _Process(float delta)
	{

		custom_process(delta);

		LinearVelocity = moving_speed;


		
	}
}
