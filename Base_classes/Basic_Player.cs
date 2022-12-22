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


    #region All related to the bomb
    public bool can_shoot,shoot_pressed,shooting_condition = false;
    public PackedScene bullet_scene;
    public int b_leftchange,b_rightchange,b_height_change = 0;
    #endregion


    
    int number_of_hits = 0;
    public int max_number_hits = 1;




    public void custom_constructor(int speed, int jump_intensity, int health = 100, string basic_attack_name = "Attack")
    {

        _node_type = _Type_of_.Player;

        this.basic_attack_name = basic_attack_name;

        var game_gui = GetNode<Node2D>("Game_Gui");


        power_bar = game_gui.GetNode<ProgressBar>("Power_Bar");

        // Dead changed to Death
        Basic_Movements = new ArrayList() { "Idle", "Run", "Walk", "Jump", "Death" };


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
        max_number_hits = Convert.ToInt32(dm.get_data("Max_No_Of_Hits"));
        GD.Print("max number of hits from the basic player ..:: ",max_number_hits);


        damage_increment_possible_moves = new ArrayList();


    }

    // this function will work as the _Process as in the Godot node's
    public void custom_process(float delta)
    {

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
                perform_move("Jump");
            }
            else if (moving_speed.x != 0)
            {
                perform_move("Run");
            }
            else
            {
                perform_move("Idle");
            }
        }


        // performing the slide movement
        // it is not a basic movement
        // a character may or may not have the slide movement
        if (Input.IsActionPressed("Slide") && is_on_ground && available_moves.Contains("Slide".ToLower()))
        {
            is_busy = true;
            perform_move("Slide");
            var speed_x = moving_speed.x;
            moving_speed.x = (speed_x < 0) ? speed_x - slide_speed_increment : speed_x + slide_speed_increment;
        }
        else
        {
            if (animations.Animation == "Slide")
            {
                perform_move("Idle");
                is_busy = false;
            }
        }


        /* Logic for adding bullet on the screen*/
        if (can_shoot)
        {
            
            shoot_pressed = Input.IsActionPressed("S");
            shooting_condition = animations.Animation!="Shoot" && animations.Animation!="Jump_Shoot" && animations.Animation!="Run_Shoot";
            
            if(shoot_pressed & shooting_condition){
                fire_bullet();
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
        global_variables.player_position = this.Position+this.GetNode<AnimatedSprite>("Movements").Position;
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
    public bool settle_damage_increment_possible_moves(int min_damage)
    {
        for (var i = 0; i < available_moves_damage.Length; i++)
        {
            var num = available_moves_damage[i];
            if (num > min_damage)
            {
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
            if (number_of_hits<max_number_hits)
            {
                Basic_Zombie collided_zombie = (Basic_Zombie)collided_one;
                GD.Print(damage_increment);
                var current_move = animations.Animation.ToLower();
                var damage = get_moves_damage(current_move) + (damage_increment_possible_moves.Contains(current_move) ? damage_increment : 0);
                GD.Print("basic player", damage);
                collided_zombie.change_health(-damage);
                basf.global_Variables.score += damage;
                is_hitted = true;
                number_of_hits++;
            }
        }
    }
    




    /*
    This three function are related to each other

    add_new_bullet is the main function which adds the bullet on the screen
    where add_new_bullet_action uses the add_new_bullet as well as changes the animation or the action of the player
    and the last fire_bullet is the one from where all of this function are been taken in consideration or used and this function can be overrided as per the logic of different characters

    */
    
    /// <summary>Adds the bullet on the screen</summary>
    public void add_new_bullet(int speed_increment = 0)
    {
        if (true)
        {
            var bullet = (Basic_Throwable_Weapon)bullet_scene.Instance();
            bullet.weapon_speed = (is_on_ground) ? 15 : 30;
            bullet.weapon_speed += speed_increment;
            bullet.spawn_weapon(this.Position, moving_direction,b_rightchange,b_leftchange,b_height_change);
            this.AddChild(bullet);
        }
    }

    /// <summary>Adds the bullet on the screen by using the add_the_bullet function as well as changes the animation of the player</summary>
    public void add_new_bullet_action(bool is_on_or_not_on_ground, string move_name)
    {
        if (is_on_or_not_on_ground && can_perform_move(move_name.ToLower()))
        {
            perform_move(move_name);
            add_new_bullet();
        }
    }

    /// <summary>It is the function which is used while player wanted to shooted thus we add all the adding logic on this as per the character or players needed.</summary>
    public virtual void fire_bullet(){
        add_new_bullet_action(is_on_ground,"Shoot");
        // GD.Print("hey there the bullet is been added on the screen are you able to see it right from the basic_player.cs..!!");
        // GD.Print("right form the basic_player adding the bullet hahah..!!");
    }

    public override bool resettle_of_hitness()
    {
        base.resettle_of_hitness();
        number_of_hits = 0;
        return true;
    }




}

