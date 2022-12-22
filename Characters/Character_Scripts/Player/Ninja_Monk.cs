using Godot;
using System;
using System.Collections;


/*
movements
press T for Attack 2
press ctrl+c for cast
press s for shoot
*/

public class Ninja_Monk : Basic_Player
{
    bool is_casting = false;

    public override void _Ready()
    {
        base._Ready();

        custom_constructor(900, 13000);

        is_busy = false;

        available_moves = new ArrayList() { "attack", "attack_2", "cast", "shoot", "death", "hurt", "idle", "jump", "run", "walk" };

        /* 
        here moves consumption for cast is kept the half of that to the desired as two times perform move is runned
        one time here and one time in the add_new_bullet function which can be founded in the Basic_Player
        */
        available_moves_consumption = new int[10] { 0, 2, 10, 5, 0, 0, 5, 0, 0, 10 };
        available_moves_damage = new int[10] { 20, 40, 0, 0, 0, 5, 0, 0, 3, 0 };
        settle_damage_increment_possible_moves(3);
        max_number_hits = 3;


        bullet_scene = ResourceLoader.Load<PackedScene>("res://Weapons_And_Animation/scenes/Blade.tscn");
        b_rightchange = 100;
        b_leftchange = -100;
        b_height_change = 0;

        can_shoot = true;

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        basic_animation_changing_condition = !is_busy;
        
        custom_process(delta);


        if (Input.IsActionJustPressed("T"))
        {
            perform_move("Attack_2");
        }

        if (Input.IsActionPressed("Cast") && !is_busy)
        {
            is_casting = perform_move("Cast");
            if (is_casting)
            {
                fire_bullet();
            }
        }


        set_animation_idle("Attack_2");
        set_animation_idle("Jump");
        set_animation_idle("Shoot");
        set_animation_idle("Cast");


        LinearVelocity = this.moving_speed;

    }

    public override void fire_bullet()
    {
        base.fire_bullet();
        if (is_casting)
        {
            add_new_bullet(40);
            is_casting = false;
        }
    }
}
