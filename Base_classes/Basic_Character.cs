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
// using System.Collections


public class Basic_Character : RigidBody2D, Global_Variables_F_A_T
{

    // inherited properties from the Global_Variables_F_A_T
    // entire description can be finded in the respective class
    // [Export]
    public string _node_type { get; set; }

	public bool is_busy;

    public int power_available;

    [Export]
    public int power_increment_wait_time=5;
    [Export]
    public int power_increment = 10;



    // basic properties of a basic character
    ArrayList Basic_Movements;
    public int speed;
    public int jump_intensity;


    #region Gravity
    // gravity for a player
    // this property will help to make the player fall down with more speed if the player not on the ground
    [Export]
    public int advanced_gravity = 300;
    public int default_gravity;   // this is to 
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
    public Timer One_Second_Timer,Power_Enhancer_Timer;
    public bool is_to_give_fall_damage;  // it will help us to determine whether to give the fall damage but also to play the animation at the time of the fall damage
    public int no_of_seconds;  // to check for how much time the player was in the sky it will help us to manage the intensity of the fall damage

    

	public ArrayList available_moves;
	// int[] available_moves_damage = new int[10]{0,0,0,2,0,0,5,0,0,10};
	public int [] available_moves_consumption;

    

    public void custom_constructor(int speed, int jump_intensity, int health = 100)
    {


        _node_type = "player";

        default_gravity = advanced_gravity;

        power_available = 100;

        Basic_Movements = new ArrayList() { "Idle", "Run", "Walk", "Jump", "Dead" };


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


        #region Making the Timer which will be called after every one second
        One_Second_Timer = new Timer();
        One_Second_Timer.WaitTime = 1;
        this.AddChild(One_Second_Timer);
        One_Second_Timer.Stop();
        One_Second_Timer.Connect("timeout", this, "One_Second_Timer_Out");
        #endregion


        Power_Enhancer_Timer = new Timer();
        Power_Enhancer_Timer.WaitTime = power_increment_wait_time;
        this.AddChild(Power_Enhancer_Timer);
        Power_Enhancer_Timer.Stop();
        Power_Enhancer_Timer.Connect("timeout", this, "Increase_Power");




        this.Connect("body_entered", this, "collided_with_body");

    }

    // this function will work as the _Process as in the Godot node's
    public void custom_process(float delta)
    {


        moving_speed = Vector2.Zero;

        if (Input.IsActionPressed("move_left"))
        {
            moving_speed.x = -speed;
            // One_Second_Timer.Start();
        }

        else if (Input.IsActionPressed("move_right"))
        {
            moving_speed.x = speed;
        }


        foreach (var item in collider_rays)
        {
            // in this part first we checked whether the player is on ground and thereby declared whether the player is jumping or not
            // here the is_on_ground is the whether the player is jumping or not
            if (item.IsColliding())
            {
                if (Input.IsActionJustPressed("Jump"))
                {
                    moving_speed.y = -jump_intensity;// making the player to jump
                }
                is_on_ground = true;
                break;
            }
            else
            {
                is_on_ground = false;
            }

        }

        if (moving_speed.x != 0)
        {
            animations.FlipH = (moving_speed.x < 0) ? true : false;  // changing the direction of the player
        }


        if (basic_animation_changing_condition)
        {
            if (!is_on_ground)
            {
                animations.Animation = "Jump";
            }
            else if (moving_speed.x != 0)
            {
                animations.Animation = "Run";
            }
            else
            {
                animations.Animation = "Idle";
            }
        }


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
            health = (no_of_seconds >= 1) ? health - (5 + no_of_seconds) : health;
            One_Second_Timer.Stop();
            no_of_seconds = 0;
        }

        #endregion


        // adding the advanced gravity to the player 
        // it will make the user to fall more faster
        moving_speed.y += advanced_gravity;

        health_bar.Value = health;
    }


    // to get the whether any movement is a basic movement or not
    public bool get_w_is_basic_movement(string move_name)
    {
        return Basic_Movements.Contains(move_name);
    }

    public int get_health()
    {
        return health;
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

    public virtual void Increase_Power(){
        power_available+=power_increment;
    }


    // this method will  be inherited by the respective child classes of its 
    // the logic will be as per the strength and the level of the character  
    public virtual void collided_with_body(Node body)
    {

    }

    public void set_animation_idle(string animation_name,int decrement_count=1){
        if(animations.Animation==animation_name && animations.Frame==animations.Frames.GetFrameCount(animation_name)-decrement_count){
            is_busy = false;
            animations.Animation = "Idle";
        }
    }

    // this method will help us to dedut the power of the player
    // power is required to perform the major attack e.g jump_attack to through a weapon on some other movement or power's
    public bool can_perfrom_move(string move_name,bool is_to_deduct=true){
        var num = available_moves.IndexOf(move_name.ToLower());
        var power_required = available_moves_consumption[num];
        if(power_available-power_required>=0){
            GD.Print("power requried is",power_required);
            power_available-=(is_to_deduct)?power_required:0;
            GD.Print(power_available);
            return true;
        } 
        else{
            return false;
        }
    }


}