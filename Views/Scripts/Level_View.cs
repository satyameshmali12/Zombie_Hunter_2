using Godot;
using System;
using System.Collections;

public class Level_View : Basic_View
{
    ArrayList level_buttons;

    Basic_Func basf;

    // Data_Manager data_Manager;  

    public override void _Ready()
    {
        base._Ready();
        basf = new Basic_Func(this);

        level_buttons = all_func.get_the_node_childrens("Level_Buttons");

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        foreach (Button item in level_buttons)
        {
            if (item.Pressed)
            {
                if (basf.dm.is_level_unlocked(item.Name))
                {
                    basf.global_Variables.is_level_added = true;
                    basf.global_Variables.level_name = item.Name;
                    GetTree().ChangeScene("res://Views/Scenes/Main_Game_Scene.tscn");
                }

            }
        }

    }

}
