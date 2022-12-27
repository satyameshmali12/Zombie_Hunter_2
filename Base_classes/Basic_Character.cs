#region Basic_Character
// This class contains all the properties and function which a basic_character requires
// this class will be inherited by the zombie as well as the player's(different heros) 
// this class inherits one class named rigid_body_2d and the interface global_variables_F_A_T
#endregion

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Godot;


public class Basic_Character : RigidBody2D, Global_Variables_F_A_T
{

	// node_type of the character e.g player,zombie etc....
	// inherited properties from the Global_Variables_F_A_T
	// entire description can be finded in the respective class

	// [Export]
	public _Type_of_ _node_type { get; set; }


	public string character_name = null; // the name of the character as per the node name of the following player



	// all the properties of a basic character
	public Vector2 moving_speed;

	// exporting the speed_x of every character in the game 
	// so that the speed can be changed right from the editor
	[Export]
	public int speed_x = 200;  // this will toggle the moving speed
	public Direction moving_direction;// this will give the direction of the player in which the player is confronting

	public int power_available;
	public int health; // the health of a basic player
	int past_health;

	public Particles2D blood_effect;
	bool is_to_settle_past_health;




	// all the nodes of basic character
	public AnimatedSprite animations;



	#region Gravity
	// gravity for a player
	// this property will help to make the player fall down with more speed if the player not on the ground
	[Export]
	public int advanced_gravity = 300;
	[Export]
	public int jump_intensity = 10000;

	public string current_move = null;

	public readonly int default_gravity = 300;   // to set the gravity to the default wherever needed

	#endregion




	#region Associated with the health and power of a basic_character

	#endregion 

	public ArrayList available_moves;
	public int[] available_moves_damage;
	public int[] available_moves_consumption;
	public string[] available_moves_damage_condition;




	[Export]
	public int power_increment_wait_time = 5;
	[Export]
	public int power_increment = 4;




	public bool is_on_ground; // to check whether the player is on ground so that ground attack's and movements can be performed
	public bool is_busy;
	public bool is_hitted; // to hit only once in a attack


	public ArrayList ground_collider_rays; // this will help us know whether the player is on ground or not.


	// to get all the global as well as the player information
	public Global_Variables global_variables;



	// to check the collision in the left and right direction using the rays
	public ArrayList Left_Collision_Rays;
	public ArrayList Right_Collision_Rays;


	// left_ray :- L_R and R_R :- Right_Ray
	public bool L_R_Colliding, R_R_Collding;

	public string colliding_condition; // the node which should collide with the player



	public Timer Power_Enhancer_Timer;


	// to check the height of the player from the ground
	public RayCast2D height_checker;
	public Timer One_Second_Timer;
	public bool is_to_give_fall_damage;  // it will help us to determine whether to give the fall damage but also to play the animation at the time of the fall damage
	public int no_of_seconds;  // to check for how much time the player was in the sky it will help us to manage the intensity of the fall damage


	public ProgressBar health_bar;

	public bool is_resisted;

	public Basic_Func basf; // containing the basic_function 


	public string death_sound_url;
	bool is_death_sound_played;

	public string hurt_sound_url;


	// public ArrayList sound_available_move_names;
	// public ArrayList available_sound_urls;
	// public bool is_arr_initialized;
	public string audio_base_url = "res://assets/audio/";






	public override void _Ready()
	{
		
		// global_Variables = GetNode<Global_Variables>("./root/Global_Varaibles");

		// GD.Print("hey there I am right from the basic_character not the basic player..");
		_node_type = _Type_of_.Nothing;

		basf = new Basic_Func(this);



		power_available = 100;
		health = 100;
		past_health = health;
		animations = GetNode<AnimatedSprite>("Movements");
		animations.Play("Idle"); // given the initial state to all the characters

		blood_effect = GetNode<Particles2D>("Blood_Animation");
		blood_effect.OneShot = true;


		this.ContactMonitor = true;
		this.ContactsReported = 10;

		is_hitted = false; // default is_hitted is setteled down to false




		#region Collision_rays
		// Iterating all the ray's availble in the Rays node
		// And then making all the ray's enabled so that they can collide with the object

		ground_collider_rays = basf.get_the_node_childrens("Rays", true);


		foreach (RayCast2D item in ground_collider_rays)
		{
			item.Enabled = true;
		}
		#endregion

		this.Connect("body_entered", this, "collided_with_body");


		global_variables = GetNode<Global_Variables>("/root/Global_Variables");



		// getting the collision ray on the left side and right side of the zombie        
		Left_Collision_Rays = basf.get_the_node_childrens("Left_Collision_Rays", true);
		Right_Collision_Rays = basf.get_the_node_childrens("Right_Collision_Rays", true);

		colliding_condition = "all"; // default the colliding condition is setted to zero


		Power_Enhancer_Timer = basf.create_timer(power_increment_wait_time, "Increase_Power");
		Power_Enhancer_Timer.Start();


		height_checker = GetNode<RayCast2D>("Height_Checker");


		#region Making the Timer which will be called after every one second and a second timer for Increasing the power
		One_Second_Timer = basf.create_timer(1, "One_Second_Timer_Out");

		#endregion

		is_resisted = false;

		// var game_gui = GetNode<Node2D>("Game_Gui");

		// health_bar = game_gui.GetNode<ProgressBar>("Health_Bar");
		// health_bar.Value = 100;

		death_sound_url = "";
		is_death_sound_played = false;


		// sound_available_move_names = new ArrayList();
		// available_sound_urls = new ArrayList();
		// GD.Print("Initailizing first..!!");
		// audios_in_queue = new ArrayList();


		


	}

	public override void _Process(float delta)
	{
		
		moving_speed = Vector2.Zero;

		current_move = this.animations.Animation;


		// in this part first we checked whether the player is on ground and thereby declared whether the player is jumping or not
		// here the is_on_ground is the whether the player is jumping or not
		foreach (RayCast2D item in ground_collider_rays)
		{
			if (item.IsColliding())
			{
				is_on_ground = true;
				break;
			}
			else
			{
				is_on_ground = false;
			}
		}

		// adding the advanced gravity to the player 
		// it will make the user to fall more faster

		moving_speed.y += advanced_gravity;
		moving_direction = (animations.FlipH) ? Direction.Left : Direction.Right;


		#region Giving the fall damage to the player

		if (!height_checker.IsColliding())
		{
			if (One_Second_Timer.IsStopped())
			{
				One_Second_Timer.WaitTime = 1;
				One_Second_Timer.Start();
			}
		}
		else if (!One_Second_Timer.IsStopped() && is_on_ground)
		{
			// change the amount below to give more stronger fall damage
			health = (no_of_seconds > 1) ? health - (5 + no_of_seconds) : health;
			One_Second_Timer.Stop();
			no_of_seconds = 0;
			// if (_node_type == "zombie")
			// {
			//     GD.Print("health of the Zombie is :", health);
			// }
		}

		#endregion



		// performing the colision detection as well as calling the collision method
		// the collision method is called right from the below function 
		// the description can be founded in the respective classes
		L_R_Colliding = is_collider_ray_colliding(Left_Collision_Rays, true, colliding_condition);
		R_R_Collding = is_collider_ray_colliding(Right_Collision_Rays, true, colliding_condition);



		// setting the attack to the idle 
		// complete description can be founded in the respective class i.e set_animation_idle()
		// and also making the hitted false
		if (available_moves_damage[available_moves.IndexOf(animations.Animation.ToLower())] != 0)
		{
			var is_settled = set_animation_idle(this.animations.Animation);
			if (is_settled)
			{
				// is_hitted = false;
				resettle_of_hitness();
			}
		}

		health_bar.Value = health;

		var is_player = this._node_type == _Type_of_.Player;

		// both the condition will loop out loop throught each other
		if (health != past_health && !blood_effect.Emitting)
		{
			blood_effect.Emitting = true;
			is_to_settle_past_health = true;
			past_health = health;
			basf.create_a_sound(hurt_sound_url, this, true, .9f, (is_player) ? .5f : .2f, 1);
		}
		if (is_to_settle_past_health && !blood_effect.Emitting)
		{
			is_to_settle_past_health = false;
			// blood_effect.Emitting = false;
		}


		if (health_bar.Value <= 0)
		{
			perform_move("Death");
			if (!is_death_sound_played)
			{
				basf.create_a_sound(death_sound_url, basf.global_Variables.current_scene, true, (is_player) ? 2 : .7f);
				is_death_sound_played = true;
			}
			is_busy = true;
		}
		if (animations.Animation == "Death" && animations.Frame >= animations.Frames.GetFrameCount("Death") - 2)
		{
			if (global_variables._main_character_name == this.Name)
			{
				global_variables.is_game_over = true;
				global_variables.game_over_view_adding_position = this.GetNode<Node2D>("Game_Gui").GetNode<Node2D>("Adding_Postion").Position;
			}
			if (this._node_type == _Type_of_.Zombie)
			{
				Basic_Zombie died_zombie = this as Basic_Zombie;
				
				// this is the default score increment
				// that means if the player wins then it is the minimum score
				global_variables.score += died_zombie.kill_score_increment;
			}

			this.QueueFree();
		}


	}


	// this method will help us to dedut the power of the player
	// power is required to perform the major attack e.g jump_attack to through a weapon on some other movement or power's
	public bool can_perform_move(string move_name, bool is_to_deduct = true)
	{
		try
		{
			var num = available_moves.IndexOf(move_name.ToLower());
			var power_required = available_moves_consumption[num];
			if (power_available - power_required >= 0)
			{
				power_available -= (is_to_deduct) ? power_required : 0;
				return true;
			}
			else
			{
				return false;
			}
		}
		catch (System.IndexOutOfRangeException)
		{
			throw;
		}
		catch (System.Exception)
		{
			throw;
		}
	}



	// if the player has performed the move then the function will return true
	public bool perform_move(string move_name, bool is_to_make_busy = true)
	{
		if (available_moves.Contains(move_name.ToLower()) && animations.Animation != move_name)
		{
			if(can_perform_move(move_name.ToLower())){
				is_busy = is_to_make_busy;
				animations.Animation = move_name;
				return true;
			}
		}
		return false;
	}

	public virtual void Increase_Power()
	{
		if (power_available + power_increment <= 100)
		{
			power_available += power_increment;
		}
		else
		{
			power_available += (100 - power_available);
		}
	}

	// to set gravity to default
	public void set_gravity_default()
	{
		advanced_gravity = default_gravity;
	}


	// to set the animation idle
	// if the animation is settled to idle then it returns true else false
	public bool set_animation_idle(string animation_name, int decrement_count = 1,bool is_to_immediately = false)
	{
		if (animations.Animation == animation_name && animations.Frame == animations.Frames.GetFrameCount(animation_name) - decrement_count || is_to_immediately)
		{
			is_busy = false;
			animations.Animation = "Idle";
			// GD.Print("hey there ")
			return true;
		}
		return false;
	}


	// to get whether a single collider rays is been colliding to any object
	// pass all as the collider_name for considering all for the collision
	public bool is_collider_ray_colliding(ArrayList collider_rays, bool is_to_call_colliding_func = false, string collider_name = "all")
	{
		var is_collided = false;
		foreach (RayCast2D item in collider_rays)
		{
			if (!item.Enabled)
			{
				item.Enabled = true;
			}
			else if (item.IsColliding())
			{
				Global_Variables_F_A_T collided_item = (Global_Variables_F_A_T)item.GetCollider();
				if (collided_item._node_type == _Type_of_.Player || collider_name.ToLower() == "all")
				{
					is_collided = true;
					if (is_to_call_colliding_func)
					{
						collided_with_L_R_ray(item.GetCollider());
					}
					break;
				}
			}
			else
			{
				is_collided = false;
			}

		}
		return is_collided;
	}


	// giving the fall damage to the player
	public virtual void One_Second_Timer_Out()
	{
		// checking whether the player is gliding or not as in the case of the ninja
		if (advanced_gravity == default_gravity)
		{
			no_of_seconds++;
		}

	}

	// this function contain's only one parameter
	// this parameter will be used to use some other intensity instead of the jump_intensity
	public bool jump(int new_jump_intensity = 0)
	{

		moving_speed.y = (new_jump_intensity == 0) ? -jump_intensity : new_jump_intensity;
		return true;
	}

	public bool change_health(int change)
	{
		health += change;
		return true;
	}


	public int get_moves_damage(string attack_name){
		return available_moves_damage[available_moves.IndexOf(attack_name)];
	}

	public virtual bool disconnect_all_signals(){
		this.Disconnect("body_entered", this, "collided_with_body");
		Power_Enhancer_Timer.Disconnect("timeout",this,"Increase_Power");
		return true;
	}

	
	/*<summary>To resettle the some values after the hitting so that the character can hit again in the next move </summary>*/
	public virtual bool resettle_of_hitness(){
		is_hitted = false;
		return true;
	}

	public virtual void update_logic(Data_Manager shop_data,Data_Manager user_data,Data_Manager throwable_weapons_dm){

	}

	// this method will  be inherited by the respective child classes of its 
	// the logic will be as per the strength and the level of the character  
	public virtual void collided_with_body(Node body)
	{



	}

	// L_R_Ray :- Left_Right_Ray
	// this method will be used both by the enemy as well as on the player
	public virtual void collided_with_L_R_ray(Godot.Object collided_obj)
	{


	}

}
