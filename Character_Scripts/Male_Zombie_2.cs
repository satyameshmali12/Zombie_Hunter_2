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

        
        available_moves = new ArrayList(){"attack","attack_2","attack_3","bite","dead","hurt","idle","jump","run","walk"};
        available_moves_consumption = new int[10]{4,8,8,10,0,0,0,0,0,0};
        available_moves_damage = new int[10]{2,3,3,4,0,0,0,0,0,0};

        attack_move_names = new ArrayList(){"attack","attack_2","attack_3","bite"};

        distancing_error = 300;
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        LinearVelocity = moving_speed;
    }
}
