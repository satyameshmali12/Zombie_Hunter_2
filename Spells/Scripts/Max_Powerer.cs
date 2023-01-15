using Godot;
using System.Collections;

public class Max_Powerer : Basic_Spell
{

    public Max_Powerer()
    {
        add_instant_setting();
        is_to_add_to_character = true;
        
        restriction_list = new ArrayList()
        {
            // create_restriction_dic("Move_Stopper",true),
            create_restriction_dic("Max_Powerer",false)
        };
    }


    public override void _Ready()
    {
        base._Ready();

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        
        
    }

    public override void effect()
    {
        base.effect();
        parent.power_available = 100;
        parent.is_to_change_power = false; 
        
    }

    public override void end_effect()
    {
        base.end_effect();
        parent.is_to_change_power = true;
    }


}



