using Godot;
using System;
using System.Collections;

/*
Dropper

One of the different drone in version 1.0.0
this drone makes hangs up its enemy with him

Lefting the parent this drone can pick up any one 
and after reaching the desired height given to the drone it frees the character as well as drop a pretty cool bomb on it with with a few seconds thrust


ones the dropper catched any character in 99% chances the player will die
1% is the exception case where may the character has usen some kind of spell
*/

public class Dropper : Basic_Drone
{

    Basic_Character target_character = null;
    RayCast2D character_checker;

    ArrayList can_pick_up;

    bool is_direction_settled = false;

    bool have_captured = false;

    int zombie_dropping_altitude = 500;
    float distance_travelled_during_zombie_dropping = 0;

    bool is_bomb_dropped, is_to_resettle = false;
    Timer bomb_drop_timer, resettle_timer;
    bool is_resettle_tiemr_started = false;

    bool have_entered_some_block = false;

    bool can_alter = true;
    Timer can_alter_timer;


    public Dropper()
    {

        restriction_list = new ArrayList()
        {
            create_restriction_dic("Dropper",false),
            create_restriction_dic("Shooter_DD",true)
        };

    }

    public override void _Ready()
    {


        name = "Dropper"; // drone name

        base._Ready();

        is_can_fall_bomb = true;

        bomb_name = "Explosion_Gas"; // type of bomb drone will use

        character_checker = this.GetNode<RayCast2D>("Character_Checker");



        // giving the drone an initial_random_direction

        var random_direction = Convert.ToInt32(GD.RandRange(0, 3));

        speed = new Vector2((random_direction == 0) ? speed_x : -speed_x, 0);


        /*
        In case of zombie can_pick_up list will only contain the _Type_of_.player
        */
        can_pick_up = new ArrayList() { _Type_of_.Player, _Type_of_.Zombie };


        bomb_drop_timer = basf.create_timer(1.5f, "drop_bomb");


        resettle_timer = basf.create_timer(2f, "Resettle_All_Fields");

        can_alter_timer = basf.create_timer(2f, "Alter_The_Alter");

        this.Monitoring = true;



    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (health > 0)
        {



            // if no target character , finding the target character here
            if (can_alter)
            {

                if (target_character == null)
                {


                    if (character_checker.IsColliding())
                    {

                        Global_Variables_F_A_T type = character_checker.GetCollider() as Global_Variables_F_A_T;

                        if (can_pick_up.Contains(type._node_type))
                        {

                            var collided_character = character_checker.GetCollider() as Basic_Character;

                            if (collided_character != parent)
                            {

                                if (collided_character.can_move)
                                {

                                    target_character = collided_character;

                                    connect_target_character_signal();

                                }
                            }
                        }

                    }
                }
                else if (!have_captured)
                {
                    speed = new Vector2((this.Position.x > target_character.Position.x) ? -speed_x : speed_x, speed_y);

                    if (character_checker.IsColliding())
                    {
                        var type = character_checker.GetCollider() as Global_Variables_F_A_T;
                        if (type == target_character)
                        {
                            speed = new Vector2(0, speed_y + 4);
                        }
                        else if (type._node_type == _Type_of_.Block)
                        {

                        }
                    }
                }
                else if (distance_travelled_during_zombie_dropping < zombie_dropping_altitude)
                {
                    distance_travelled_during_zombie_dropping += speed_y;
                    speed = new Vector2(speed.x, -speed_y);
                    // this.Position = new Vector2(this.Position.x,this.Position.y-speed_y);/
                    target_character.Position = this.Position + new Vector2(0, 36);


                }
                else if (!is_bomb_dropped)
                {
                    target_character.can_move = true;
                    bomb_drop_timer.Start();
                    is_bomb_dropped = true;
                }

                else if (is_to_resettle)
                {

                    is_to_resettle = false;
                    resettle_timer.Start();
                }

            }

        }
    }





    public void connect_target_character_signal(bool is_to_connect = true)
    {
        if (is_to_connect)
        {
            if (!target_character.IsConnected("child_exiting_tree", this, "Target_Finished"))
            {
                target_character.Connect("child_exiting_tree", this, "Target_Finished");
            }
        }
        else
        {
            target_character.Disconnect("child_exiting_tree", this, "Target_Finished");
        }
    }


    // changing the direction of the drone if collided to any object in between
    public override void L_R_Ray_Collided()
    {
        base.L_R_Ray_Collided();
        if (left_collision_ray.IsColliding() && speed.x < 0 || right_collision_ray.IsColliding() && speed.x > 0)
        {
            speed = new Vector2((left_collision_ray.IsColliding() ? speed_x : -speed_x), speed.y);
        }

    }

    public override bool drop_bomb()
    {
        bomb_drop_timer.Stop();
        is_to_resettle = true;
        return base.drop_bomb();

    }

    public void Resettle_All_Fields()
    {
        is_bomb_dropped = false;

        if (target_character != null)
        {
            target_character.can_move = true;
        }
        target_character = null;

        distance_travelled_during_zombie_dropping = 0;

        have_captured = false;

        speed = new Vector2(((int)GD.RandRange(0, 2) == 0) ? -speed_x : speed_x, speed.y);

        resettle_timer.Stop();
    }

    public void Target_Finished(Node node)
    {
        if (target_character != null)
        {
            if (target_character.IsConnected("child_exiting_tree", this, "Target_Finished"))
            {
                target_character.Disconnect("child_exiting_tree", this, "Target_Finished");
                Resettle_All_Fields();
            }
        }
    }

    public override void Kill_Drone()
    {
        base.Kill_Drone();
        Resettle_All_Fields();
    }

    public override void Collided_With_Object(Node body)
    {
        base.Collided_With_Object(body);

        Global_Variables_F_A_T type = (Global_Variables_F_A_T)body;

        is_to_follow_ai_movement = true;

        if (type != parent && !have_captured && target_character == type)
        {
            Basic_Character collided_one = (Basic_Character)type;

            if (collided_one.can_move && collided_one._node_type == target_character._node_type)
            {
                have_captured = true;
                collided_one.can_move = false;
            }
            else
            {
                Resettle_All_Fields();
            }
        }

        // var type = (Global_Variables_F_A_T)node;
        if (type._node_type == _Type_of_.Block)
        {
            Resettle_All_Fields();
            can_alter_timer.Start();
            can_alter = false;
            speed = new Vector2(speed.x, -speed_y);
        }

    }

    // public void Entered_Some_Block(Node node){


    // }

    public void Alter_The_Alter()
    {
        can_alter = true;
        can_alter_timer.Stop();
    }


}
