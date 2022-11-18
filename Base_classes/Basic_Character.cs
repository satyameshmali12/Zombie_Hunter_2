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

    [Export]
    public string _node_type { get; set; }


    public string character_name = null; // the name of the character as per the node name of the following player



    // all the properties of a basic character
    public Vector2 moving_speed;
    public int speed_x;  // this will toggle the moving speed
    public Direction moving_direction;// this will give the direction of the player in which the player is confronting

    public int power_available;
    public int health; // the health of a basic player




    // all the nodes of basic character
    public AnimatedSprite animations;



    #region Gravity
    // gravity for a player
    // this property will help to make the player fall down with more speed if the player not on the ground
    [Export]
    public int advanced_gravity = 300;

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

    public ArrayList ground_collider_rays; // this will help us know whether the player is on ground or not.


    // to get all the global as well as the player information
    public Global_Variables player_variable;



    // to check the collision in the left and right direction using the rays
    public ArrayList Left_Collision_Rays;
    public ArrayList Right_Collision_Rays;


    // left_ray :- L_R and R_R :- Right_Ray
    public bool L_R_Colliding,R_R_Collding;

    public string colliding_condition; // the node which should collide with the player



    public Timer Power_Enhancer_Timer;


    public override void _Ready()
    {
        GD.Print("hey there I am right from the basic_character not the basic player..");
        _node_type = null;


        power_available = 100;
        animations = GetNode<AnimatedSprite>("Movements");
        animations.Play("Idle"); // given the initial state to all the characters


        this.ContactMonitor = true;
        this.ContactsReported = 10;




        #region Collision_rays
        // Iterating all the ray's availble in the Rays node
        // And then making all the ray's enabled so that they can collide with the object

        ground_collider_rays = get_the_collider_rays("Rays");


        foreach (RayCast2D item in ground_collider_rays)
        {
            item.Enabled = true;
        }
        #endregion

        this.Connect("body_entered", this, "collided_with_body");

        
        player_variable = GetNode<Global_Variables>("/root/Global_Variables"); 



        // getting the collision ray on the left side and right side of the zombie        
        Left_Collision_Rays = get_the_collider_rays("Left_Collision_Rays");
        Right_Collision_Rays = get_the_collider_rays("Right_Collision_Rays");

        colliding_condition = "all"; // default the colliding condition is setted to zero

        
        Power_Enhancer_Timer = this.create_timer(power_increment_wait_time, "Increase_Power");
        Power_Enhancer_Timer.Start();


    }

    public override void _Process(float delta)
    {
        moving_speed = Vector2.Zero;

        
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

        
        // performing the colision detection as well as calling the collision method
        // the collision method is called right from the below function 
        // the description can be founded in the respective classes
        L_R_Colliding = is_collider_ray_colliding(Left_Collision_Rays,true,colliding_condition);
        R_R_Collding = is_collider_ray_colliding(Right_Collision_Rays,true,colliding_condition);


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

    public Timer create_timer(int wait_time, string signal_func_name)
    {
        var new_timer = new Timer();
        new_timer.WaitTime = 1;
        this.AddChild(new_timer);
        new_timer.Stop();
        new_timer.Connect("timeout", this, signal_func_name);
        return new_timer;
    }


    public void perform_move(string move_name)
    {
        if (available_moves.Contains(move_name.ToLower()))
        {
            is_busy = true;
            animations.Animation = move_name;
        }

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
    public bool set_animation_idle(string animation_name, int decrement_count = 1)
    {
        if (animations.Animation == animation_name && animations.Frame == animations.Frames.GetFrameCount(animation_name) - decrement_count)
        {
            is_busy = false;
            animations.Animation = "Idle";
            return true;
        }
        return false;
    }
    public ArrayList get_the_collider_rays(string node_name){
        var collider_rays = new ArrayList();
        var rays = GetNode<Node2D>(node_name).GetChildren();
        foreach (RayCast2D item in rays)
        {
            item.Enabled = true;
            collider_rays.Add(item);
        }
        return collider_rays;
    }



    // to get whether a single collider rays is been colliding to any object
    // pass all as the collider_name for considering all for the collision
    public bool is_collider_ray_colliding(ArrayList collider_rays,bool is_to_call_colliding_func=false,string collider_name="all"){
        var is_collided = false;
        foreach (RayCast2D item in collider_rays)
        {
            if(!item.Enabled){
                item.Enabled = true;
            }
            else if(item.IsColliding()){
                Global_Variables_F_A_T collided_item = (Global_Variables_F_A_T)item.GetCollider();
                if(collided_item._node_type=="player" || collider_name.ToLower()=="all"){
                    is_collided = true;
                    if(is_to_call_colliding_func){
                        collided_with_L_R_ray(item.GetCollider());
                    }
                    break;
                }
            }
            else{
                is_collided = false;
            }
            
        }
        return is_collided;
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