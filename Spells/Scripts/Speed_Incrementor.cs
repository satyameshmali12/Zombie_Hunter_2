using Godot;
using System;



/// <summary>
/// The more number of speed_increment the more the speed will increase
///</summary>
public class Speed_Incrementor : Basic_Spell
{

    int original_speed;
    int speed_increment;

    public Speed_Incrementor()
    {
        add_instant_setting();
        // is_to_add_to_character = true;
        // is_add_to_level = true;
    }


    public override void _Ready()
    {
        base._Ready();

        original_speed = parent.speed_x;

        speed_increment = 50 + int.Parse(dm.get_data("Damage_Increment"));

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

    }

    public override void effect()
    {
        base.effect();
        parent.speed_x = original_speed + speed_increment;
    }

    public override void end_effect()
    {
        
        base.end_effect();
        parent.set_back_original_speed();
        
    }

}
