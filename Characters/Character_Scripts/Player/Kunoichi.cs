using Godot;
using System;
using System.Collections;

public class Kunoichi : Basic_Ninja
{
    public override void _Ready()
    {
        base._Ready();

        this.load_basic_weapon("Blade", 0, -100, 100);
        available_moves = new ArrayList() { "attack", "attack_2", "death", "eat", "hurt", "idle", "jump", "run", "shoot", "walk" };
        available_moves_consumption = new int[10]{0,4,3,70,0,0,0,0,5,0};
        available_moves_damage = new int[10]{20,30,0,100,0,0,2,0,0,0};
        settle_damage_increment_possible_moves(3);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if(Input.IsActionJustPressed("Kunoichi_Eat"))
        {
            perform_move("kunoichi");
        }


    }
}
