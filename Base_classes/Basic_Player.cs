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

    int damage_increment;

    public ArrayList damage_increment_possible_moves;



    public void custom_constructor(int speed, int jump_intensity, int health = 100, string basic_attack_name = "Attack")
    {
        // _Ready();

        _node_type = _Type_of_.Player;

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

        // loading the sound for a basic player
        this.death_sound_url = "res://assets/audio/Player/Player_Death.mp3";
        this.hurt_sound_url = "res://assets/audio/Player/Player_Hurt.mp3";

        // loading the data of the player
        var dm = basf.dm;
        dm = new Data_Manager("data//data_fields/heros_data_field.zhd");
        dm.load_data(this.Name);
        damage_increment = Convert.ToInt32(dm.get_data("Damage_Increment"));
        damage_increment_possible_moves = new ArrayList();


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
                // animations.Animation = "Jump";
                perform_move("Jump");
            }
            else if (moving_speed.x != 0)
            {
                // animations.Animation = "Run";
                perform_move("Run");
            }
            else
            {
                perform_move("Idle");
                // animations.Animation = "Idle";
            }
        }


        // performing the slide movement
        // it is not a basic movement
        // a character may or may not have the slide movement
        if (Input.IsActionPressed("Slide") && is_on_ground && available_moves.Contains("Slide".ToLower()))
        {
            is_busy = true;
            perform_move("Slide");
            // animations.Animation = "Slide";
            var speed_x = moving_speed.x;
            moving_speed.x = (speed_x < 0) ? speed_x - slide_speed_increment : speed_x + slide_speed_increment;
        }
        else
        {
            if (animations.Animation == "Slide")
            {
                perform_move("Idle");
                // animations.Animation = "Idle";
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
        global_variables.player_position = this.Position;
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
    
    // here the min damage is the amount that a move should have so that it can be included in the damage_increment_possible_moves list
    public bool settle_damage_increment_possible_moves(int min_damage){
        for (var i = 0; i < available_moves_damage.Length; i++)
        {
            var num = available_moves_damage[i];
            if(num>min_damage){
                damage_increment_possible_moves.Add(available_moves[i]);
            }
            
        }
        return true;
    }


    public override void collided_with_body(Node body)
    {
        base.collided_with_body(body);

    }

    public override void collided_with_L_R_ray(Godot.Object collided_obj)
    {

        Global_Variables_F_A_T collided_one = collided_obj as Global_Variables_F_A_T;
        if (collided_one._node_type == _Type_of_.Zombie)
        {
            if (!is_hitted)
            {
                Basic_Zombie collided_zombie = (Basic_Zombie)collided_one;
                GD.Print(damage_increment);
                var current_move = animations.Animation.ToLower();
                var damage = get_moves_damage(current_move)+(damage_increment_possible_moves.Contains(current_move)?damage_increment:0);
                collided_zombie.change_health(-damage);
                basf.global_Variables.score+=damage;
                is_hitted = true;
            }
        }
    }






}

