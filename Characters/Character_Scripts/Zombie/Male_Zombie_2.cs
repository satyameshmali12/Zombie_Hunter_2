using Godot;
using System;
using System.Collections;

public class Male_Zombie_2 : Basic_Zombie
{

    public override void _Ready()
    {
        character_name = "Male_Zombie_2";
        settle_fields(200,8000);

        base._Ready();

        // speed_x += 200;

        power_increment = 10;

        available_moves = new ArrayList() { "attack", "attack_2", "attack_3", "bite", "death", "hurt", "idle", "jump", "run", "walk" ,"damaged"};
        available_moves_consumption = new int[11] { 8, 16, 16, 12, 0, 0, 0, 0, 0, 0 ,0};
        available_moves_damage = new int[11] { 3, 3, 4, 4, 0, 0, 0, 0, 0, 0 ,0};

        attack_move_names = new ArrayList() { "attack", "attack_2", "attack_3", "bite" };

        can_jump_over = true;

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
    }

}
