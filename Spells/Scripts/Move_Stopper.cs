using Godot;
using System;
using Godot.Collections;
using System.Collections;

public class Move_Stopper : Basic_Spell
{
    public Move_Stopper()
    {
        add_instant_setting();
        is_to_add_to_character = true;
        restriction_list = new ArrayList()
        {
            create_restriction_dic("Move_Stopper",false)
            // create_restriction_dic("Health_Stopper",true),
            // create_restriction_dic("Max_Powerer",true)

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
        parent.is_able_to_perform_moves = false;
        parent.can_move = false;
    }

    public override void end_effect()
    {
        base.end_effect();
        parent.is_able_to_perform_moves = true;
        parent.can_move = true;
    }
}
