using Godot;
using System;
using System.Collections;

public class Female_Zombie_2 : Basic_Zombie
{

    public override void _Ready()
    {
        character_name = "Female_Zombie";
        settle_fields(200,4600);
        base._Ready();

        available_moves = new ArrayList() { "attack", "attack_2", "attack_3", "death", "hurt", "idle", "jump", "walk", "run", "scream", "walk" , "damaged"};
        available_moves_consumption = new int[12] { 4, 4, 4, 0, 0, 0, 0, 0, 0, 10, 0 ,0};
        available_moves_damage = new int[12] { 1, 2, 3, 0, 0, 0, 0, 0, 0, 4, 0 ,0};

        attack_move_names = new ArrayList() { "attack", "attack_2", "attack_3", "scream" };


    }


    public override void _Process(float delta)
    {
        base._Process(delta);

    }
}
