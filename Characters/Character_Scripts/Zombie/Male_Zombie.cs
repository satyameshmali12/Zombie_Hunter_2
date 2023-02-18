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

        available_moves = new ArrayList() { "attack", "death", "idle", "walk" ,"hurt","damaged"};
        available_moves_consumption = new int[6] { 10, 0, 0, 0 ,0,0};
        available_moves_damage = new int[6] { 1, 0, 0, 0 ,0,0};

        attack_move_names = new ArrayList() { "attack" };

    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        
    }


}
