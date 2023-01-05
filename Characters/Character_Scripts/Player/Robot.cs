#region description
// This is the script of the robot
// this class inherits from the Basic_Player

// controls
// refer Basic_Player for basic movements

// pressed F to jump and attack
// press shift+s to shoot bullet
// press shift+l to use the laser_beam attack  .. the most powerfull attack of the player
// combination of all this attack can make the player more powerful


#endregion

using Godot;
using System;
using System.Collections;

public class Robot : Basic_Player
{


    Timer bullet_timer;

    bool is_can_shoot;
    Laser_Bean laser_bean;

    // Timer emitter_setter_timer;

    public override void _Ready()
    {

        // calling the base class
        base._Ready();


        custom_constructor(700, 9000, basic_attack_name: "Melee");


        is_busy = false;
        available_moves = new ArrayList() { "death", "idle", "jump", "jump_melee", "jump_shoot", "melee", "run", "run_shoot", "shoot", "slide", "laser_beam" };
        available_moves_consumption = new int[11] { 0, 0, 0, 10, 15, 10, 0, 10, 8, 0, 10 };
        available_moves_damage = new int[11] { 0, 0, 5, 40, 100, 20, 1, 120, 0, 1, 200 };
        settle_damage_increment_possible_moves(4);
        // damage_increment_possible_moves = new ArrayList(){"melee"};



        // bullet_scene = ResourceLoader.Load<PackedScene>("res://Weapons_And_Animation/scenes/Bullet.tscn");
        // b_rightchange = 100;
        // b_leftchange = -100;
        // b_height_change = 0;
        // can_shoot = true;
        this.load_basic_weapon("Bullet", 0, -10, 100);

        bullet_timer = GetNode<Timer>("Bullet_Timer");

        laser_bean = GetNode<Laser_Bean>("Laser_Bean");
        // emitter_setter_timer = create_timer(2,"")



    }


    public override void _Process(float delta)
    {
        base._Process(delta);


        if (!laser_bean.is_animation_performing)
        {

            basic_animation_changing_condition = !is_busy;

            custom_process(delta);

            if (Input.IsActionPressed("F") && !is_on_ground)
            {
                perform_move("Jump_Melee");
            }

            if (Input.IsActionJustPressed("Laser_Beam"))
            {
                if (can_perform_move("laser_beam") && !laser_bean.bean_animation.Visible)
                {
                    laser_bean.perform_bean((animations.FlipH) ? Direction.Left : Direction.Right, animations.Position);
                }
            }


            if (moving_speed.x != 0 && is_on_ground && shoot_pressed && shooting_condition)
            {
                if (perform_move("Run_Shoot"))
                {
                    add_new_bullet(20);
                }
            }



            // to set the animation idle in the the following animation ends
            // description of the this method can be founded in the Basic_Player class
            set_animation_idle("Shoot");
            set_animation_idle("Jump_Shoot");
            set_animation_idle("Run_Shoot");
            set_animation_idle("Jump_Melee");

            move();
        }

    }



    public override void collided_with_body(Node body)
    {
        base.collided_with_body(body);

        Global_Variables_F_A_T old_tile = (Global_Variables_F_A_T)body;

        // if (old_tile._node_type == "block")
        // {
        // GD.Print(old_tile._node_type);
        // }
    }

    public override void fire_bullet()
    {
        base.fire_bullet();
        add_new_bullet_action(!is_on_ground, "Jump_Shoot");
    }


}