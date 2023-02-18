using Godot;
using System;
using System.Collections;

/*
Containing the AI for the Zombie
*/

public class Basic_Zombie : Basic_Character
{


    // this will help us know all the attack a zombie have 
    // as different zombies will have their respective attack (attack_name)

    public bool is_obstruction_in_between;



    // to check the left and right upward collision 
    // will help us to train our zombie AI
    RayCast2D Left_Upward_Ray, Right_Upward_Ray, Upward_Ray;

    // this two array will help us to detect the block forward or backward the character
    // will help us to train the AI of the Zombie..
    ArrayList Fall_Down_Left_Rays, Fall_Down_Right_Rays;

    public bool can_jump_over, is_attacking, is_on_edge;

    public int kill_score_increment; // the amount of increment in the score as the zombie is been killed

    public string groan_sound_url = "res://assets/audio/Zombie/Zombie_Groan.mp3";

    public ArrayList groan_sound_urls;

    // this will be used by the character if there will be any issure regarding the distance between the playe and the specific zombie 
    public int distancing_error;

    public bool is_associated_with_main_level = true;

    public bool isToAvoidBaseLogic = false;




    public override void _Ready()
    {
        this.data_field_url = basf.global_paths.Zombie_Data_Field_Url;

        base._Ready();

        _node_type = _Type_of_.Zombie;

        kill_score_increment = 10;
        colliding_condition = "player";

        can_be_resistance_breaken = true;


        basf.dm.load_data(basf.global_Variables.current_level_name);
        // GD.Print("the Zombie_Speed_Increment is :- ", basf.dm.get_data("Zombie_Speed_Increment"));
        speed_x += Convert.ToInt32(basf.dm.get_data("Zombie_Speed_Increment"));

        distancing_error = 0;

        attack_move_names = new ArrayList();

        is_hitted = false;

        Left_Upward_Ray = GetNode<RayCast2D>("Left_Upward_Ray");
        Right_Upward_Ray = GetNode<RayCast2D>("Right_Upward_Ray");
        Upward_Ray = GetNode<RayCast2D>("Upward_Ray");

        is_obstruction_in_between = false;


        colliding_condition = "all";


        // gettting all the Left and Right Fall down rays which fill help us to detect were the zombie should walk and where not
        // the stronger the zombie the greator will be it's range of the Left and Right Fall Down Rays....
        Fall_Down_Left_Rays = basf.get_the_node_childrens("Fall_Down_Left_Rays", true);
        Fall_Down_Right_Rays = basf.get_the_node_childrens("Fall_Down_Right_Rays", true);


        // for powerfull zombie only
        // not every zombie can jump above a another zombie
        can_jump_over = false;
        // is_attacking = false;

        health_bar = GetNode<ProgressBar>("Health_Bar");

        groan_sound_urls = new ArrayList() { "res://assets/audio/Zombie/Zombie_Roar_2.mp3", "res://assets/audio/Zombie/Zombie_Roar_3.mp3", "res://assets/audio/Zombie/Zombie_Groan.mp3" };

        this.death_sound_url = "res://assets/audio/Zombie/Zombie_Death.mp3";



        var roar_sound = groan_sound_urls[Convert.ToInt32(GD.RandRange(0, groan_sound_urls.Count - 1))].ToString();
        var rand_max_time = (float)GD.RandRange(0, 1f);
        // basf.create_a_sound(roar_sound, this, false, rand_max_time, rand_max_time, 1);

        this.hurt_sound_url = "res://assets/audio/Zombie/Zombie_Hurt.mp3";

        can_collide_with = new ArrayList() { _Type_of_.Player, _Type_of_.Drone };
    }


    public override void _Process(float delta)
    {
        if(!isToAvoidBaseLogic)
        {
            base._Process(delta);
        }

        if (!is_death)
        {

            // this is the position of the defender of the game
            // on which our zombie will attack

            var player_global_position = global_variables.player_position;

            var player_x = player_global_position.x;

            var is_left_rays_colliding = this.is_collider_ray_colliding(Left_Collision_Rays);
            var is_right_rays_colliding = this.is_collider_ray_colliding(Right_Collision_Rays);

            var player_configured_position = player_x - distancing_error;
            if (Math.Abs(player_configured_position - this.Position.x) > 10)
            {
                this.animations.FlipH = player_configured_position < this.Position.x;

                moving_speed.x = (moving_direction == Direction.Right) ? speed_x : -speed_x; // moving the zombie in the direction's
            }

            if (is_obstruction_in_between)
            {
                var is_left_upward_ray_colliding = Left_Upward_Ray.IsColliding();

                var is_right_upward_ray_colliding = Right_Upward_Ray.IsColliding();


                if (!is_left_upward_ray_colliding && moving_speed.x < 0 || !is_right_upward_ray_colliding && moving_speed.x > 0)
                {
                    if (is_on_ground)
                    {
                        moving_speed.y = -jump_intensity;
                    }

                }
            }

            if (!L_R_Colliding && moving_speed.x < 0 || !R_R_Collding && moving_speed.x > 0)
            {
                if (is_obstruction_in_between && !is_on_ground)
                {
                    is_obstruction_in_between = false;
                }
            }


            // checking whether the zombie is on the edge or not
            // this will help the zombie to make itselt safe to that of the fall damage

            var is_left_fall_ray_colliding = is_collider_ray_colliding(Fall_Down_Left_Rays);
            var is_right_fall_ray_colliding = is_collider_ray_colliding(Fall_Down_Right_Rays);

            is_on_edge = !is_left_fall_ray_colliding && moving_speed.x < 0 || !is_right_fall_ray_colliding && moving_speed.x > 0;

            moving_speed.x = (is_on_edge) ? 0 : moving_speed.x;



            // making the zombie to be idle if it can't move forward
            // Idle and Walk are the one which doesn't make's the is_busy to true
            if (!is_busy)
            {
                if (moving_speed.x == 0)
                {
                    perform_move("Idle");
                }
                else
                {
                    perform_move("Walk");
                }
                is_busy = false;
            }


        }

        move();


    }

    public virtual void customAI()
    {

    }

    public override void collided_with_body(Node body)
    {
        base.collided_with_body(body);

        Global_Variables_F_A_T collided_body = body as Global_Variables_F_A_T;

    }


    public override void collided_with_L_R_ray(Godot.Object collided_obj)
    {
        base.collided_with_L_R_ray(collided_obj);

        Global_Variables_F_A_T collided_one = collided_obj as Global_Variables_F_A_T;

        if (is_on_ground && collided_one._node_type == _Type_of_.Zombie && can_jump_over)
        {
            if (L_R_Colliding && moving_speed.x < 0 || R_R_Collding && moving_speed.x > 0)
            {
                jump(jump_intensity - 500);
            }
        }

        if (collided_one._node_type == _Type_of_.Block)
        {
            is_obstruction_in_between = true;
        }


        if (!is_busy)
        {
            if (can_collide_with.Contains(collided_one._node_type) && !is_on_edge)
            {
                string random_attack = (string)attack_move_names[Convert.ToInt32(GD.RandRange(0, attack_move_names.Count - 1))];

                ArrayList splited_name = basf.get_format_array_string(random_attack, new ArrayList() { "_" }, false);
                string edited_attack_name = "";
                foreach (string item in splited_name)
                {
                    edited_attack_name += (item[0].ToString().ToUpper() + item.Substring(1, item.Length - 1)) + "_";
                }
                if (edited_attack_name.Length > 0)
                {
                    edited_attack_name = edited_attack_name.Remove(edited_attack_name.Length - 1, 1);
                }
                perform_move(edited_attack_name);
            }
        }
        else if (!is_hitted)
        {
            if (can_collide_with.Contains(collided_one._node_type))
            {
                Character character = collided_obj as Character;
                character.change_health(-available_moves_damage[available_moves.IndexOf(this.animations.Animation.ToLower())]);
                is_hitted = true;
            }
        }
    }

    public override void killCharacter()
    {
        base.killCharacter();
        animations.Stop();
        this.CollisionLayer = 2;
        this.LinearVelocity = Vector2.Zero;
        is_death = true;
        moving_speed = Vector2.Zero;
    }

}
