#region Description
	// Basic Character
	// this is the basic character which will be inherited by all the respective characters
	// Basic Character's inherit's two class RigidBody2D and the Global_Variables_F_A_T
	// This class will be inherited by all the character
	// This class will help to alter the basic movement(left right movement) and will provide some basic properties e.g health,basic_movements etc.
#endregion


using Godot;
using System;
using System.Collections;
using System.Collections.Generic;


public class Basic_Character : RigidBody2D,Global_Variables_F_A_T
{
	
	// inherited properties from the Global_Variables_F_A_T
	// entire description can be finded in the respective class
	public string _node_type{get;set;} 

	public int power_available;



	// basic properties of a basic character
	ArrayList Basic_Movements;
	public int speed;
	public int jump_intensity;


	#region Gravity
		// gravity for a player
		// this property will help to make the player fall down with more speed if the player not on the ground
		[Export]
		public int advanced_gravity = 300; 
		public int default_gravity = 300;   // this is to 
	#endregion



	public bool is_on_ground; // to check whether the player is on the ground or not
	// this is_on_ground will help us to determine whether to attack or not



	public AnimatedSprite animations;


	public Vector2 moving_speed;


	public List<RayCast2D> collider_rays;

	public bool basic_animation_changing_condition;
	

	int health;

	
	// getting the components 
	ProgressBar health_bar;



	// to check the height of the player from the ground
	public RayCast2D height_checker;
	public Timer fall_damage_timer;
	public bool is_to_give_fall_damage;  // it will help us to determine whether to give the fall damage but also to play the animation at the time of the fall damage


	public void custom_constructor(int speed,int jump_intensity,int health=100){

		
		_node_type = "player";

		power_available = 100;

		Basic_Movements = new ArrayList(){"Idle","Run","Walk","Jump","Dead"};


		this.health = health;// setting the health of the player which is by default hundred but for powerfull character it could be more....



		#region Collision_rays
			// Iterating all the ray's availble in the Rays node
			// And then making all the ray's enabled so that they can collide with the object
			collider_rays = new List<RayCast2D>();

			var rays = GetNode<Node2D>("Rays").GetChildren();
			foreach (RayCast2D item in rays)
			{
				collider_rays.Add(item);
			}


			foreach (var item in collider_rays)
			{
				item.Enabled = true;
			}
		#endregion


		
		animations = GetNode<AnimatedSprite>("Movements");
		animations.Animation = "Idle"; // setting the initial animation to idle
		animations.Playing = true; // running the animation

		this.speed = speed;
		this.jump_intensity = jump_intensity;

		is_on_ground = true;

		// setting the initial moving speed to Zero
		moving_speed = Vector2.Zero;


		basic_animation_changing_condition = true;


		this.ContactMonitor = true;
		this.ContactsReported = 10;

		
		health_bar = GetNode<ProgressBar>("Health_Bar");
		health_bar.Value = 100;


		height_checker = GetNode<RayCast2D>("Height_Checker");


		#region Making the fall_damage_timer_
			fall_damage_timer = new Timer();
			fall_damage_timer.Autostart = true;
			fall_damage_timer.OneShot = true;
			fall_damage_timer.WaitTime = 1;
			this.AddChild(fall_damage_timer);
			fall_damage_timer.Stop();
			fall_damage_timer.Connect("timeout",this,"fall_damage_there");
		#endregion


		this.Connect("body_entered",this,"collided_with_body");

	}

	// this function will work as the _Process as in the Godot node's
	public void custom_process(float delta)
	{


		moving_speed = Vector2.Zero;

		if(Input.IsActionPressed("move_left")){
			moving_speed.x = -speed;
			// fall_damage_timer.Start();
		}
		
		else if(Input.IsActionPressed("move_right")){
			moving_speed.x = speed;
		}
		
		
		foreach (var item in collider_rays)
		{
			// in this part first we checked whether the player is on ground and thereby declared whether the player is jumping or not
			// here the is_on_ground is the whether the player is jumping or not
			if(item.IsColliding()){
				if(Input.IsActionJustPressed("Jump")){
					moving_speed.y = -jump_intensity;// making the player to jump
				}
				is_on_ground = true;
				break;
			}
			else{
				is_on_ground = false;
			}

		}

		if(moving_speed.x!=0){
			animations.FlipH = (moving_speed.x<0)?true:false;  // changing the direction of the player
		}


		if(basic_animation_changing_condition){
			if(!is_on_ground){
				animations.Animation = "Jump";
			}
			else if(moving_speed.x!=0){
				animations.Animation = "Run";
			}
			else{
				animations.Animation = "Idle";
			}
		}


		#region Giving the fall damage to the player

			// if(!height_checker.IsColliding()){
			// 	if(fall_damage_timer.IsStopped()){
			// 		GD.Print("hey i am there");
			// 		fall_damage_timer.WaitTime = 2;
			// 		fall_damage_timer.Start();
			// 	}
			// }
			// else if(!fall_damage_timer.IsStopped() && is_on_ground){
			// 	fall_damage_timer.Stop();
			// }

			// else if(is_to_give_fall_damage && is_on_ground){
			// 	health-=10;
			// 	is_to_give_fall_damage = false;
			// }

		#endregion


		// adding the advanced gravity to the player 
		// it will make the user to fall more faster
		moving_speed.y += advanced_gravity;

		health_bar.Value = health;
	}


	// to get the whether any movement is a basic movement or not
	public bool get_w_is_basic_movement(string move_name){
		return Basic_Movements.Contains(move_name);
	}

	public int get_health(){
		return health;
	}


	

	// giving the fall damage to the player
	public virtual void fall_damage_there(){
		// GD.Print("hey the timer is blitted up or poped up do what you want stupid!!..");
		// if(advanced_gravity==default_gravity){
		// 	is_to_give_fall_damage = true;
		// }	
	}


	// this method will  be inherited by the respective child classes of its 
	// the logic will be as per the strength and the level of the character  
	public virtual void collided_with_body(Node body){

	}


}