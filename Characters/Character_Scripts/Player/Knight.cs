using Godot;
using System;
using System.Collections;

public class Knight : Basic_Player
{


    public override void _Ready()
    {
        this.character_name = "Knight";

        settle_fields(700, 11000);

        base._Ready();


        available_moves = new ArrayList() { "attack", "damaged", "death", "hurt", "idle", "jump", "jump_attack", "run", "walk" };
        available_moves_consumption = new int[9] { 0, 0, 0, 0, 0, 0, 10, 0, 0 };
        available_moves_damage = new int[9] { 10, 0, 0, 0, 0, 0, 20, 0, 0 };
        attack_move_names = new ArrayList(){"attack","jump_attack"};
        is_busy = false;
    }

    public override void _Process(float delta)
    {
        basic_animation_changing_condition = !is_busy;

        base._Process(delta);



    }

    public override void collided_with_body(Node body)
    {
        base.collided_with_body(body);
    }

    public override void custom_movements()
    {
        base.custom_movements();

        if (Input.IsActionJustPressed("Jump_Attack") && !is_on_ground)
        {
            perform_move("Jump_Attack");
        }

        else if (animations.Animation == "Jump_Attack" && is_on_ground)
        {
            perform_move("Idle", false);
        }

        if (Input.IsActionPressed("F"))
        {
            perform_move("Attack");
        }


        set_animation_idle("Attack");
        set_animation_idle("Jump");
        set_animation_idle("Run");
        set_animation_idle("Jump_Attack");


    }
}
