using Godot;
using System;
using System.Collections;

public class Level_View : Basic_View
{
    ArrayList level_buttons;     
    public override void _Ready()
    {
        base._Ready();

        level_buttons = all_func.get_the_collider_rays("Level_Buttons");

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
        foreach (Button item in level_buttons)
        {
            if(item.Pressed){
                GD.Print(item.Name);
            }
        }
        
    }

}
