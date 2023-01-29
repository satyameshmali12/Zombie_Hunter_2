using Godot;
using System;

public class Health_Incrementor : Basic_Spell
{
    int health_increment = 2;
    Timer health_increment_timer;
    public Health_Incrementor()
    {
        add_instant_setting();
        // is_to_add_to_character = true;
    }

    public override void _Ready()
    {
        base._Ready();
        is_to_add_instantaneouly = true;
        health_increment += int.Parse(dm.get_data("Damage_Increment"));
        health_increment_timer = basf.create_timer(5,"Increment_Health");
        health_increment_timer.Start();

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
    }
    public override void effect()
    {
        base.effect();
    }

    public override void end_effect()
    {
        base.end_effect();
    }

    public void Increment_Health()
    {
        if(!this.IsQueuedForDeletion())
        {
            parent.health += health_increment;
        }
        else{
            health_increment_timer.Stop();
        }
    }
}
