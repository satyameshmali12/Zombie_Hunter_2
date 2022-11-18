using Godot;
using System;
using System.Collections;

public class Male_Zombie : Basic_Zombie
{

    [Export]
    int hit_damage = 3;

    public override void _Ready()
    {
        base._Ready();

        character_name = "Male_Zombie";
        speed_x = 200;

        available_moves = new ArrayList(){"attack","dead","idle","walk"};
        available_moves_consumption = new int[4]{20,0,0,0};
        available_moves_damage = new int[4]{hit_damage,0,0,0};

        attack_move_names = new ArrayList(){"attack"};

        distancing_error = 180;
        
    }


    public override void _Process(float delta)
    {
        base._Process(delta);

        // GD.Print(player_variable.player_position);
        LinearVelocity = moving_speed;
        
    }

    
}
