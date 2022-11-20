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


public class Basic_Player : Basic_Character
{


    public string basic_attack_name;


    [Export]
    public int slide_speed_increment = 200;



    // basic properties of a basic character
    ArrayList Basic_Movements;




    

    public bool basic_animation_changing_condition;

    // int health; // health of the player

    // getting the components 
    ProgressBar power_bar;






    
    public void custom_constructor(int speed, int jump_intensity, int health = 100, string basic_attack_name = "Attack")
    {

        _node_type = "player";

        this.basic_attack_name = basic_attack_name;

        var game_gui = GetNode<Node2D>("Game_Gui");


        power_bar = game_gui.GetNode<ProgressBar>("Power_Bar");

        Basic_Movements = new ArrayList() { "Idle", "Run", "Walk", "Jump", "Dead" };


        this.health = health;// setting the health of the player which is by default hundred but for powerfull character it could be more....



        animations = GetNode<AnimatedSprite>("Movements");
        animations.Animation = "Idle"; // setting the initial animation to idle
        animations.Playing = true; // running the animation

        this.speed_x = speed;
        this.jump_intensity = jump_intensity;

        is_on_ground = true;

        // setting the initial moving speed to Zero
        moving_speed = Vector2.Zero;


        basic_animation_changing_condition = true;




        health_bar = game_gui.GetNode<ProgressBar>("Health_Bar");
        health_bar.Value = 100;
        

        moving_direction = Direction.Right;
        
        colliding_condition = "all";

    }

    // this function will work as the _Process as in the Godot node's
    public void custom_process(float delta)
    {


        // moving_speed = Vector2.Zero;

        if (Input.IsActionPressed("move_left"))
        {
            moving_speed.x = -speed_x;
        }

        else if (Input.IsActionPressed("move_right"))
        {
            moving_speed.x = speed_x;
        }


        if (Input.IsActionJustPressed("Jump") && is_on_ground)
        {
            moving_speed.y = -jump_intensity;// making the player to jump
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


        // performing the slide movement
        // it is not a basic movement
        // a character may or may not have the slide movement
        if (Input.IsActionPressed("Slide") && is_on_ground && available_moves.Contains("Slide".ToLower()))
        {
            is_busy = true;
            animations.Animation = "Slide";
            var speed_x = moving_speed.x;
            moving_speed.x = (speed_x < 0) ? speed_x - slide_speed_increment : speed_x + slide_speed_increment;
        }
        else
        {
            if (animations.Animation == "Slide")
            {
                animations.Animation = "Idle";
                is_busy = false;
            }
        }


        // performing the basic attack
        if (available_moves.Contains(basic_attack_name.ToLower()))
        {
            if (Input.IsActionPressed("F") && !is_busy && animations.Animation != "Jump_Attack" && animations.Animation != basic_attack_name)
            {
                animations.Animation = basic_attack_name;
                is_busy = true;
            }

            set_animation_idle(basic_attack_name);

            // setting the moving speed to zero if the the player is attacking
            if (animations.Animation == basic_attack_name)
            {
                moving_speed = new Vector2(0, moving_speed.y);
            }
        }
        

        health_bar.Value = health;
        power_bar.Value = power_available;



        #region data_transfer to the global script
            // passing the data of the player to the player or global script
            // to perform all the other logics
            player_variable.player_position = this.Position;
        #endregion


        
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


    public override void collided_with_body(Node body)
    {
        base.collided_with_body(body);

    }

    public override void collided_with_L_R_ray(Godot.Object collided_obj){

        Global_Variables_F_A_T collided_one = collided_obj as Global_Variables_F_A_T;
        if(collided_one._node_type=="zombie"){
            if(!is_hitted){
                Basic_Zombie collided_zombie = (Basic_Zombie)collided_one;
                collided_zombie.health-=available_moves_damage[available_moves.IndexOf(animations.Animation)];
                GD.Print("hey there zombie health decreased");
            }
        }
    }




    

}

