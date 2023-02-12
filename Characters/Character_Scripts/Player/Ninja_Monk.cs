using Godot;
using System;
using System.Collections;


/*
movements
press T for Attack 2
press ctrl+c for cast
press s for shoot
*/

public class Ninja_Monk : Basic_Ninja
{
    bool is_casting = false;

    public override void _Ready()
    {
        this.character_name = "Ninja_Monk";

        base._Ready();

        this.load_basic_weapon("Blade", 0, -100, 100);
        available_moves = new ArrayList() { "attack", "attack_2", "cast", "damaged", "death", "hurt", "idle", "jump", "run", "shoot", "walk" };
        /* 
		here moves consumption for cast is kept the half of that to the desired as two times perform move is runned
		one time here and one time in the add_new_bullet function which can be founded in the Basic_Player
		*/
        available_moves_consumption = new int[11] { 0, 2, 12, 0, 0, 0, 0, 0, 0, 5, 0 };
        available_moves_damage = new int[11] { 10, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        attack_move_names = new ArrayList(){"attack","attack_2"};
        settle_damage_increment_possible_moves(3);
        basic_attack_name = "Attack";
    }
    public override void _Process(float delta)
    {
        base._Process(delta);

    }

    public override void fire_bullet()
    {
        base.fire_bullet();
        if (is_casting)
        {
            add_new_bullet(40);
            is_casting = false;
        }
    }

    public override void custom_movements()
    {
        base.custom_movements();
        
        if (Input.IsActionPressed("Cast") && !is_busy)
        {
            is_casting = perform_move("Cast");
            if (is_casting)
            {
                fire_bullet();
            }
        }
        set_animation_idle("Cast");
    }

}
