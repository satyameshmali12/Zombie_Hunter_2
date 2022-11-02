using Godot;
using System;


public class Ninja : Basic_Character
{

	public override void _Ready()
	{
		custom_constructor();
	}

	public override void _Process(float delta)
	{
		custom_process(delta);

	}
}
