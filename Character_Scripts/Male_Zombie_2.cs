using Godot;
using System;
using System.Collections;

public class Male_Zombie_2 : Basic_Zombie
{

    public override void _Ready()
    {
        base._Ready();


        character_name = "Male_Zombie_2";
        speed_x = 200;

        power_increment = 10;

        available_moves = new ArrayList() { "attack", "attack_2", "attack_3", "bite", "death", "hurt", "idle", "jump", "run", "walk" };
        available_moves_consumption = new int[10] { 8, 16, 16, 12, 0, 0, 0, 0, 0, 0 };
        available_moves_damage = new int[10] { 3, 3, 4, 4, 0, 0, 0, 0, 0, 0 };


        jump_intensity = 8000;

        attack_move_names = new ArrayList() { "attack", "attack_2", "attack_3", "bite" };

        distancing_error = 300;

        can_jump_over = true;

        // is_resisted = true;



    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        LinearVelocity = moving_speed;
    }

}
