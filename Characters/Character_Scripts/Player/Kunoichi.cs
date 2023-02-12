using Godot;
using System;
using System.Collections;


/*
CTRL+i to eat food and increase the health
This character have the power to break the resistance if the zombie through the normal attack without the use of the any spell
*/
public class Kunoichi : Basic_Ninja
{
    [Export]
    public int food_eaten_health_increment = 20;
    public override void _Ready()
    {
        this.character_name = "Kunoichi";
        base._Ready();

        this.load_basic_weapon("Dart", 50, -100, 100);
        available_moves = new ArrayList() { "attack", "attack_2", "damaged", "death", "eat", "hurt", "idle", "jump", "run", "shoot", "walk" };
        available_moves_consumption = new int[11] { 0, 4, 0, 3, 39, 0, 0, 0, 0, 5, 0 };
        available_moves_damage = new int[11] { 20, 30, 0, 0, 100, 0, 0, 2, 0, 0, 0 };

        attack_move_names = new ArrayList() { "attack", "attack_2", "eat" };
        settle_damage_increment_possible_moves(3);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }
    public override void collided_with_L_R_ray(Godot.Object collided_obj)
    {
        base.collided_with_L_R_ray(collided_obj);

        if (current_move == "Attack_2")
        {
            Global_Variables_F_A_T type = collided_obj as Global_Variables_F_A_T;
            if (type._node_type == _Type_of_.Zombie)
            {
                Basic_Zombie zombie = type as Basic_Zombie;
                zombie.is_resisted = false;
            }
        }
    }

    public override void custom_movements()
    {
        base.custom_movements();

        if (Input.IsActionJustPressed("Kunoichi_Eat"))
        {
            bool is_move_performed = perform_move("Eat");
            if (is_move_performed)
            {
                health += food_eaten_health_increment;
            }
        }

        set_animation_idle("Eat");

    }

}
