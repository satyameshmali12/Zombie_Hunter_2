using Godot;
using System;
using System.Collections;

public class Peasant : Basic_Ninja
{
    public override void _Ready()
    {
        this.character_name = "Peasant";

        base._Ready();
        this.load_basic_weapon("Dart", 60, -100, 100);
        available_moves = new ArrayList() { "attack", "attack_2", "damaged","death", "hurt", "idle", "jump", "run", "shoot", "walk" };
        available_moves_consumption = new int[11] { 0, 4,0, 3, 39, 0, 0, 0, 0, 5, 0};
        available_moves_damage = new int[11] { 20, 30,0, 0, 100, 0, 0, 2, 0, 0, 0 };
        attack_move_names = new ArrayList(){"attack","attack_2"};
        settle_damage_increment_possible_moves(3);

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

    }
}
