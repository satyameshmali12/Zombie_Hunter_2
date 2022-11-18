using Godot;
using System;
using System.Collections;

public class Female_Zombie_2 : Basic_Zombie
{
    [Export]
    int hit_damage = 3;

    public override void _Ready()
    {
        base._Ready();

        character_name = "Female_Zombie";
        speed_x = 200;

        available_moves = new ArrayList(){"Attack","Dead","Idle","Walk"};
        available_moves_consumption = new int[4]{4,0,0,0};
        available_moves_damage = new int[4]{hit_damage,0,0,0};
        distancing_error = 100;
        
    }


    public override void _Process(float delta)
    {
        base._Process(delta);

        // GD.Print(player_variable.player_position);
        LinearVelocity = moving_speed;
        
    }
}
