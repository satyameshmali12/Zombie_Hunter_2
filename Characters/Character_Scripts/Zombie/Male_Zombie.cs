using Godot;
using System;
using System.Collections;

public class Male_Zombie : Basic_Zombie
{



    public override void _Ready()
    {
        character_name = "Male_Zombie";
        settle_fields(200,3500);
        base._Ready();

        // speed_x += 200;
        available_moves = new ArrayList() { "attack", "death", "idle", "walk" };
        available_moves_consumption = new int[4] { 10, 0, 0, 0 };
        available_moves_damage = new int[4] { 1, 0, 0, 0 };

        attack_move_names = new ArrayList() { "attack" };

        // jump_intensity = 3500;

    }


    public override void _Process(float delta)
    {
        base._Process(delta);

        move();
    }


}
