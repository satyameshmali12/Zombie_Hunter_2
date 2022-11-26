using Godot;
using System;

public class Global_Variables : Node2D
{

    public Vector2 player_position = Vector2.Zero;
    public bool is_game_over;    

    public string _main_character_name; // this will help us in the multi character match
    public string level_name;
    public bool is_level_added;
    public int score;

    public override void _Ready()
    {
        is_game_over = false;
        _main_character_name = null;
        score = 0;
        level_name = null;
        is_level_added = false;
    }


    public override void _Process(float delta)
    {


    }
}
