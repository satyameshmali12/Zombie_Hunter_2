using Godot;
using System;
using System.Collections;

public class Wild_Zombie : Basic_Zombie
{
    public override void _Ready()
    {
        character_name = "Wild_Zombie";
        settle_fields(200,9000);

        base._Ready();

        available_moves = new ArrayList() { "attack", "attack_2", "attack_3", "death", "eating", "hurt", "idle", "jump", "run", "walk" };
        available_moves_consumption = new int[10] { 2, 2, 2, 0, 2, 0, 0, 0, 0, 0 };
        available_moves_damage = new int[10] { 2, 2, 3, 0, 2, 0, 0, 0, 0, 0 };

        attack_move_names = new ArrayList() { "attack", "attack_2", "attack_3", "eating" };
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
    }
}
