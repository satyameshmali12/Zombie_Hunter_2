using Godot;
using System;

public class Arcade : Level
{

    Timer max_zombie_incrementor;
    int per_max_zombie_increment = 4; // nubmer of zombies to be incremented as the max_zombie_increment timer pops up
    int max_zombie_increment_time = 20;

    Timer diffculty_level_incrementor;
    int diffculty_level_increment_time = 60;
    int diffculty_increment = 1;
    public override void _Ready()
    {
        base._Ready();
        is_arcade = true;

        difficulty_level = 1;

        max_zombie_incrementor = basf.create_timer(max_zombie_increment_time,"Increment_Max_Zombie");
        max_zombie_incrementor.Start();

        diffculty_level_incrementor = basf.create_timer(diffculty_level_increment_time,"Increment_Diffculty_Level");
        diffculty_level_incrementor.Start();

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
        
    }

    public void Increment_Max_Zombie(){
        max_zombie_per_screen+=per_max_zombie_increment;
        // resetting the time of the max_zombie_increment
        max_zombie_increment_time+=15;
        
        max_zombie_incrementor.WaitTime = max_zombie_increment_time;

        per_max_zombie_increment++;
    }

    public void Increment_Diffculty_Level(){
        difficulty_level+=diffculty_increment;
        diffculty_level_increment_time+=30;
        diffculty_level_incrementor.WaitTime = diffculty_level_increment_time;

    }
}
