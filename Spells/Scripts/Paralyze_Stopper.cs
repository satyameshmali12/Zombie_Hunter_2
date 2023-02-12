using Godot;
using System;

public class Paralyze_Stopper : Basic_Spell
{
    public Paralyze_Stopper()
    {
        add_instant_setting();
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
        parent.can_paralyze = false;
    }
    public override void Remove_Spell()
    {
        base.Remove_Spell();
        parent.can_paralyze = true;
    }
}
