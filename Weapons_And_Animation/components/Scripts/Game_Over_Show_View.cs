using Godot;
using System;

public class Game_Over_Show_View : Node2D
{
    Sprite image1;
    public override void _Ready()
    {
        image1 = GetNode<Sprite>("WinCrown");
    }

    public override void _Process(float delta)
    {

    }
    public void set_w_l(bool is_win){
        image1.Visible = is_win;
    }
}
