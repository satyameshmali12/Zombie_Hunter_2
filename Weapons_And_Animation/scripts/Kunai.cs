using Godot;
using System;

public class Kunai : Area2D,Global_Variables_F_A_T
{

	public string _node_type{get;set;}

	[Export]
	int Kunai_Move_Speed=20;


	[Export]
	int max_no_of_collisions = 4;
	int no_of_collision = 0;

	CollisionShape2D collision_area;

	public TextureRect Kunai_Texture;

	public override void _Ready()
	{
		Kunai_Texture = GetNode<TextureRect>("Kunai_Texture");

		_node_type = "weapon"; // setting the _node_type to weapon to identify the collisions as the weapon
		this.Monitoring = true;
		this.Connect("body_entered",this,"Kunai_Collided");
		
		collision_area = GetNode<CollisionShape2D>("CollisionShape2D");
		collision_area.Disabled = true;

		
	}

	public override void _Process(float delta)
	{
		if(Visible){
			this.Position = new Vector2((Kunai_Texture.FlipV)?this.Position.x-Kunai_Move_Speed:this.Position.x+Kunai_Move_Speed,this.Position.y);
			collision_area.Disabled = false;
		}	
		else{
			collision_area.Disabled = true;
		}
	}

	public void Kunai_Collided(Node body){
		Global_Variables_F_A_T collided_body = (Global_Variables_F_A_T)body;
		if(collided_body._node_type=="block"){
			this.Visible = false;
			// TileSet tile = (TileSet)collided_body;
			GD.Print("hey I am the queuefree one..!!");
		}
		else{
			no_of_collision++;
			if(no_of_collision>=max_no_of_collisions){
				this.Visible = false;
			}
		}
	}
}
