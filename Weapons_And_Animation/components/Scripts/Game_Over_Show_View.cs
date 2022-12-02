using Godot;
using System;

public class Game_Over_Show_View : Node2D
{
    Sprite image1;
    public override void _Ready()
    {
        // GD.Print("Hey I am the constructor from the game_over_show_view so talk to you later on hahaha..!!");
        image1 = GetNode<Sprite>("WinCrown");
    }

    public override void _Process(float delta)
    {
        // image1.Visible = false;

    }
    public void set_w_l(bool is_win){
        // GD.Print(is_win,"hey see it is here");
        image1.Visible = is_win;
    }
}
