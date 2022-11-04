using Godot;
using System;

public class TileMap : Godot.TileMap,Global_Variables_F_A_T
{
	public string _node_type{get;set;}

	
	public override void _Ready()
	{
		_node_type = "block";
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{

	}
}
