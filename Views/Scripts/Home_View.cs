using Godot;
using System;

public class Home_View : Basic_View
{
    Button but1;
    public override void _Ready()
    {
        but1 = GetNode("Start_Button") as Button;
        
    }
    public override void _Process(float delta)
    {
        if(but1.Pressed){
            GetTree().ChangeScene("res://Views/Scenes/Level_View.tscn");
        }
        
    }
}
