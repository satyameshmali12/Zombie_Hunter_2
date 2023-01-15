using Godot;
using System.Collections;

public class Health_Stopper : Basic_Spell
{

    int initial_health;

    public Health_Stopper()
    {
        add_instant_setting();
        is_to_add_to_character = true;
        
        restriction_list = new ArrayList()
        {
            create_restriction_dic("Health_Stopper",false),
            // create_restriction_dic("Max_Powerer",false),
            // create_restriction_dic("Max_Powerer",false)
        };

    }

    public override void _Ready()
    {
        base._Ready();

        initial_health = parent.health;
        is_to_add_instantaneouly = true;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }

    public override void effect()
    {
        base.effect();
        parent.health = initial_health;
        parent.is_resisted = true;
        
    }

    public override void end_effect()
    {
        base.end_effect();
        parent.is_resisted = false;

    }


}
