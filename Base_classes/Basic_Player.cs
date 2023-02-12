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


public class Basic_Player : Basic_Character
{

    public string basic_attack_name;

    [Export]
    public int slide_speed_increment = 200;

    // basic properties of a basic character
    ArrayList Basic_Movements;

    public bool basic_animation_changing_condition;

    // getting the components 
    ProgressBar power_bar;
    int damage_increment;
    public ArrayList damage_increment_possible_moves;


    #region All related to the bomb
    public bool can_shoot, shoot_pressed, shooting_condition = false;
    public PackedScene bullet_scene;
    public int b_leftchange, b_rightchange, b_height_change = 0;
    #endregion



    int number_of_hits = 0;
    public int max_number_hits = 1;

    public Camera2D camera;
    public bool is_AI = false;
    public bool is_main_character = false;   // the main character is the character which is been controlled by the user 

    Vector2 game_gui_position = Vector2.Zero;

    Node2D AI_Stuff;
    Timer AI_Jump_Timer;

    public override void settle_fields(int speed_x, int jump_intensity, string basic_attack_name = "Attack", string hurt_move_name = "hurt")
    {
        base.settle_fields(speed_x, jump_intensity, basic_attack_name);
        this.basic_attack_name = basic_attack_name;
        this.hurt_move_name = hurt_move_name;
    }

    public override void _Ready()
    {
        this.data_field_url = basf.global_paths.Heros_Data_Field_Url;

        base._Ready();

        AI_Jump_Timer = basf.create_timer(2, "Avail_AI_Jump");


        _node_type = _Type_of_.Player;

        this.health = health;// setting the health of the player which is by default hundred but for powerfull character it could be more....

        setPowerBars();

        camera = this.GetNode<Camera2D>("Camera");



        // Dead changed to Death
        Basic_Movements = new ArrayList() { "Idle", "Run", "Walk", "Jump", "Death" };





        animations = GetNode<AnimatedSprite>("Movements");
        animations.Animation = "Idle"; // setting the initial animation to idle
        animations.Playing = true; // running the animation


        is_on_ground = true;

        // setting the initial moving speed to Zero
        moving_speed = Vector2.Zero;


        basic_animation_changing_condition = true;


        moving_direction = Direction.Right;

        colliding_condition = "all";

        // loading the sound for a basic player
        this.death_sound_url = "res://assets/audio/Player/Player_Death.mp3";
        this.hurt_sound_url = "res://assets/audio/Player/Player_Hurt.mp3";

        // loading the data of the player
        var dm = basf.dm;
        dm = new Data_Manager("data//data_fields/heros_data_field.zhd");
        dm.load_data(this.character_name);
        damage_increment = Convert.ToInt32(dm.get_data("Damage_Increment"));
        max_number_hits = Convert.ToInt32(dm.get_data("Max_No_Of_Hits"));
        // settle_deduction(dm);


        damage_increment_possible_moves = new ArrayList();

        can_collide_with = new ArrayList() { _Type_of_.Zombie, _Type_of_.Drone };


    }

    public override void _Process(float delta)
    {

        base._Process(delta);
        // basic_animation_changing_condition = !is_busy;
        if (!animations.Playing && !is_paralyzed())
        {
            animations.Animation = "Idle";
            animations.Play();
            is_busy = false;
        }


        if (is_main_character && !camera.Current)
        {
            camera.Current = true;
        }

        if (!is_AI)
        {
            // performing the basic attack
            if (available_moves.Contains(basic_attack_name.ToLower()))
            {
                if (Input.IsActionPressed("F") && animations.Animation != basic_attack_name)
                {
                    perform_move("Attack");
                    is_busy = true;
                }
                // setting the moving speed to zero if the the player is attacking
                if (animations.Animation == basic_attack_name)
                {
                    moving_speed = new Vector2(0, moving_speed.y);
                }
            }

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
            else if (animations.Animation == "Slide")
            {
                perform_move("Idle");
                is_busy = false;
            }



            /* Logic for adding bullet on the screen*/
            if (can_shoot)
            {

                shoot_pressed = Input.IsActionPressed("S");
                shooting_condition = animations.Animation != "Shoot" && animations.Animation != "Jump_Shoot" && animations.Animation != "Run_Shoot";

                if (shoot_pressed & shooting_condition)
                {
                    fire_bullet();
                }

            }


            custom_movements();


            health_bar.Value = health;
            power_bar.Value = power_available;


            #region data_transfer to the global script
            // passing the data of the player to the player or global script
            // to perform all the other logics
            // make sure data is being transfered only when the player is not an AI one
            global_variables.player_position = this.Position + this.GetNode<AnimatedSprite>("Movements").Position;
            #endregion

        }

        else
        {
            this.GetNode<AI_Player>("AI").AI_FUNC();
        }


        move();

        if (moving_speed.x != 0)
        {
            animations.FlipH = (moving_speed.x < 0) ? true : false;  // changing the direction of the player
        }


        if ((basic_animation_changing_condition && !is_AI) || (is_AI && !is_busy))
        {
            bool is_to_make_busy = !is_AI;
            if (!is_on_ground)
            {
                perform_move("Jump", is_to_make_busy);
            }
            else if (moving_speed.x != 0)
            {
                perform_move("Run", is_to_make_busy);
            }
            else
            {
                perform_move("Idle", is_to_make_busy);
            }
        }

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
        if (!is_AI)
        {
            if (can_collide_with.Contains(collided_one._node_type))
            {
                if (number_of_hits < max_number_hits)
                {
                    Character collided_char = (Character)collided_one;
                    var current_move = animations.Animation.ToLower();
                    var damage = get_moves_damage(current_move) + (damage_increment_possible_moves.Contains(current_move) ? damage_increment : 0);
                    collided_char.change_health(-damage);
                    basf.global_Variables.score += damage;
                    is_hitted = true;
                    number_of_hits++;
                }
            }
        }
        else if (collided_one._node_type == _Type_of_.Block && is_on_ground && AI_Jump_Timer.IsStopped())
        {
            jump();
            AI_Jump_Timer.Start();
        }
        else
        {
            perform_randomize_attack(collided_one, true);
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
            bullet.spawn_weapon(this.Position, moving_direction, b_rightchange, b_leftchange, (basf.global_Variables.current_level_type == Level_Type.Multi_AI) ? b_height_change : 0);
            bullet.weapon_name = bullet.Name;
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
    public virtual void fire_bullet()
    {
        add_new_bullet_action(is_on_ground, "Shoot");
        // GD.Print("hey there the bullet is been added on the screen are you able to see it right from the basic_player.cs..!!");
        // GD.Print("right form the basic_player adding the bullet hahah..!!");
    }

    public override bool resettle_of_hitness()
    {
        base.resettle_of_hitness();
        number_of_hits = 0;
        return true;
    }

    // use this function only when a player can have a weapon with him
    public bool load_basic_weapon(string weapon_name, int b_height_change, int b_leftchange, int b_rightchange)
    {

        bullet_scene = ResourceLoader.Load<PackedScene>($"{basf.global_paths.Weapons_Base_Url}/{weapon_name}.tscn");
        this.b_rightchange = 100;
        this.b_leftchange = -100;
        this.b_height_change = b_height_change;
        can_shoot = true;
        return true;
    }

    public override void update_logic(Data_Manager shop_data, Data_Manager user_data, Data_Manager throwable_weapons_dm)
    {
        base.update_logic(shop_data, user_data, throwable_weapons_dm);

        var max_number_of_hits = Convert.ToInt32(shop_data.get_data("Max_No_Of_Hits"));
        var max_number_of_hits_increment = Convert.ToInt32(shop_data.get_data("Hits_Increment_Per_Update"));
        var new_max_number_of_hits = max_number_of_hits + max_number_of_hits_increment;

        var max_damage_count = shop_data.get_integer_data("Max_Damage_Count");
        var max_damage_count_increment = shop_data.get_integer_data("Max_Damage_Count_Increment");
        var new_max_damage_count = max_damage_count + max_damage_count_increment;

        shop_data.set_value<int>(new string[2] { "Max_No_Of_Hits", "Max_Damage_Count" }, new int[2] { new_max_number_of_hits, new_max_damage_count });

    }

    /// <summary>
    ///<para>This is called to kill the player by avoiding the deduction range and deduction power</para>
    /// <para>In this context we have called this function right from the pause_menu when the user quits the game manually</para>
    ///</summary>
    public virtual void kill_player()
    {
        this.health = 0;
    }

    public void MakeConPlayer()
    {
        // Game_Gui gui = this.GetNode<Game_Gui>("Game_Gui");

        // checking whether their is an existing gui exist or not
        Game_Gui gui = null;
        foreach (Node item in this.GetChildren())
        {
            if (item.Name == "Game_Gui")
            {
                gui = item as Game_Gui;
                break;
            }
        }
        if (gui == null)
        {
            Game_Gui new_gui = basf.get_the_packed_scene(basf.global_paths.Game_Gui_Path).Instance<Game_Gui>();
            new_gui.Name = "Game_Gui";
            this.AddChild(new_gui);
            new_gui.Position = game_gui_position;
        }

        is_AI = false;

        setPowerBars();

        this.is_main_character = true;

    }

    public void MakeAIPlayer()
    {
        Game_Gui gui = null;
        foreach (Node item in this.GetChildren())
        {
            if (item.Name == "Game_Gui")
            {
                gui = item as Game_Gui;
                break;
            }
        }

        if (gui != null)
        {
            this.RemoveChild(gui);
        }

        is_AI = true;

        this.is_main_character = false;
    }

    public void setPowerBars()
    {
        var game_gui = GetNode<Node2D>("Game_Gui");
        game_gui_position = game_gui.Position;

        power_bar = game_gui.GetNode<ProgressBar>("Power_Bar");

        health_bar = game_gui.GetNode<ProgressBar>("Health_Bar");
        health_bar.Value = 100;
    }

    public void Avail_AI_Jump()
    {
        AI_Jump_Timer.Stop();
    }



    public virtual void custom_movements() { }



}

