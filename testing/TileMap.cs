using Godot;
using System;
using System.Drawing.Imaging;

public class TileMap : Godot.TileMap,Global_Variables_F_A_T
{
	public string _node_type{get;set;}
	public int health;

	
	public override void _Ready()
	{
		_node_type = "block";
		health = 100;
		// var img = Image.Image
				
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		// this.TileSet.
		

	}
}
