using Godot;
using System;

public class Home_View : Basic_View
{
    Button but1;

    Button arcade_button;
    // Basic_Func basf;
    public override void _Ready()
    {
        base._Ready();

        but1 = GetNode("Start_Button") as Button;
        basf = new Basic_Func(this);

        arcade_button = this.GetNode<Button>("Arcade");
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        
        if(but1.Pressed){
            basf.navigateTo(this,basf.global_paths.Level_View_Path);
        }
        else if(arcade_button.Pressed){
            
            basf.global_Variables.is_level_added = false;
            basf.global_Variables.level_name = "Arcade";
            basf.global_Variables.custom_url = "res://Levels/Arcade/Arcade.tscn";
            basf.navigateTo(this,basf.global_paths.Main_Game_Scene_Path);
        }
        
    }
}
