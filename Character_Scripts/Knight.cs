using Godot;
using System;

public class Knight : Basic_Character
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		custom_constructor(600,10000);	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		custom_process(delta);

		
		LinearVelocity = moving_speed;
		
	}
}
