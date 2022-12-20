using Godot;
using System;
using System.Collections;

public class Ninja_Monk : Basic_Player
{

    public override void _Ready()
    {
        base._Ready();
        custom_constructor(900,13000);
        is_busy = false;
        
        available_moves = new ArrayList() { "attack", "attack_2", "blade", "cast", "death", "hurt", "idle", "jump", "run", "walk" };
        available_moves_consumption = new int[10] { 0, 2, 0, 2, 0, 0, 5, 0, 0, 10 };
        available_moves_damage = new int[10] { 20, 0, 0, 5, 0, 5, 0, 0, 3, 0 };
        settle_damage_increment_possible_moves(3);

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        custom_process(delta);
        
        
        LinearVelocity = this.moving_speed;
        
    }
}
