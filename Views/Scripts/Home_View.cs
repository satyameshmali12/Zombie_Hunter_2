using Godot;
using System;

public class Home_View : Basic_View
{
    Button but1;
    // Basic_Func basf;
    public override void _Ready()
    {
        base._Ready();

        but1 = GetNode("Start_Button") as Button;
        basf = new Basic_Func(this);
    }
    public override void _Process(float delta)
    {
        if(but1.Pressed){
            GetTree().ChangeScene("res://Views/Scenes/Level_View.tscn");
        }
        
    }
}
