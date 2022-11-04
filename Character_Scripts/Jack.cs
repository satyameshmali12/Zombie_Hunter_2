using Godot;
using System;

public class Jack : RigidBody2D,Global_Variables_F_A_T
{
	public string _node_type{get;set;}


	public override void _Ready()
	{
		_node_type = "player";

	}

	
	public override void _Process(float delta)
	{
		
	}
}
