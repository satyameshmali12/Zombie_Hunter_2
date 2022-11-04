#region Description

#endregion


using Godot;
using System;


public class Ninja : Basic_Character
{
	
	int no_of_coll=0;

	// Is_Movement_Busy is_busy;
	bool is_busy;

	[Export]
	int slide_speed_increment=200;

	Kunai kunai; // this is the throw weapon of the player

	RayCast2D height_checker;

	bool is_gliding;

	int default_gravity;



	public override void _Ready()
	{
		custom_constructor(650,10000);

		is_busy = false;

		height_checker = GetNode<RayCast2D>("Height_Checker");

		is_gliding = false;


		kunai = GetNode("Kunai") as Kunai;
		kunai.SetAsToplevel(true);
		kunai.Visible = false;

		default_gravity = advanced_gravity;

		
		
	}

	public override void _Process(float delta)
	{
		basic_animation_changing_condition = !is_busy;
		
		custom_process(delta);
		
		// jump attack move of the ninja
		if(Input.IsActionJustReleased("Jump_Attack") && !is_on_ground){
			is_busy = true;
			animations.Animation = "Jump_Attack";
		}

		else if(animations.Animation=="Jump_Attack" && is_on_ground){
			is_busy = false;
		}

		// for the sliding of the player
		if(Input.IsActionPressed("Slide") && is_on_ground){
			is_busy = true;
			animations.Animation = "Slide";
			var speed_x = moving_speed.x;
			moving_speed.x = (speed_x<0)?speed_x-slide_speed_increment:speed_x+slide_speed_increment;
		}
		else if(animations.Animation == "Slide"){
			is_busy = false;
		}



		// to throw the kunai
		if(Input.IsActionJustPressed("T") || animations.Animation=="Throw"){

			is_gliding = false;

			if(!is_gliding){

				var throw_frame_count = animations.Frame;
				var throw_total_frame = animations.Frames.GetFrameCount("Throw");
				

				// setting the is_busy to false ones the animation is finished
				if(throw_frame_count>=throw_total_frame-1){
					is_busy = false;
				}

				// checking whether the kunai is visible or not
				// and if not then making it visible
				if(!kunai.Visible){
					is_busy = true;
					if(animations.Animation!="Throw"){
						animations.Animation = "Throw";
					}

					if(throw_frame_count>throw_total_frame-3){
						kunai.Position = this.Position;
						kunai.Visible = true;
						
						// setting the direction of the kunai
						kunai.Kunai_Texture.FlipV = animations.FlipH;
					}
				}
			}
		}
		// GD.Print(!height_checker.IsColliding());
		if(!is_gliding && !height_checker.IsColliding() && !is_on_ground){
			is_gliding = true;
			is_busy = true;
			animations.Animation = "Glide";
			advanced_gravity = 50;
		}

		else if(is_gliding && is_on_ground){
			GD.Print("hey there setting to ",is_gliding);
			is_gliding = false;
			is_busy = false;
			advanced_gravity = default_gravity;
		}



		LinearVelocity = moving_speed;

	}

	public override void collided_with_body(Node body)
	{
		base.collided_with_body(body);

		Global_Variables_F_A_T new_node = (Global_Variables_F_A_T)body;

		if(new_node._node_type=="player"){
			body.QueueFree();
			GD.Print("hey collided with an unknow body");		
		}
	}
}
