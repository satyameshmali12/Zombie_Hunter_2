using Godot;
using System;

public class Arcade : Level
{

    Timer max_zombie_incrementor;
    const int per_max_zombie_increment = 4; // nubmer of zombies to be incremented as the max_zombie_increment timer pops up
    public override void _Ready()
    {
        base._Ready();
        is_arcade = true;

        max_zombie_incrementor = basf.create_timer(4,"Increment_Max_Zombie");
        max_zombie_incrementor.Start();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
        
    }

    public void Increment_Max_Zombie(){
        max_zombie_per_screen+=per_max_zombie_increment;
    }
}
