using Godot;
using System;
using System.Collections;


public class Female_Zombie : Basic_Zombie
{


    public override void _Ready()
    {
        base._Ready();

        character_name = "Female_Zombie";
        speed_x = 200;

        distancing_error = 120;


        available_moves = new ArrayList() { "attack", "death", "idle", "walk" };
        available_moves_consumption = new int[4] { 14, 0, 0, 0 };
        available_moves_damage = new int[4] { 2, 0, 0, 0 };

        attack_move_names = new ArrayList() { "attack" };

        jump_intensity = 4000;

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        LinearVelocity = moving_speed;
    }
}
