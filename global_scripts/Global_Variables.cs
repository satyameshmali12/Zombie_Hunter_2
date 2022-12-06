using Godot;
using System;
using System.Collections;

public class Global_Variables : Node2D
{

    public Vector2 player_position = Vector2.Zero;
    public bool is_game_over;    

    public string _main_character_name; // this will help us in the multi character match
    public string level_name;
    public bool is_level_added;
    public int score;

    public bool is_arcade;

    // this is used for adding the gamer_over_view after the death of the player 
    // this is used in the level file
    // If you are in VsCode you can view the use by clicking on the refererence below or can move to the respective
    public Vector2 game_over_view_adding_position; 

    public ArrayList bomb_Buttons; // this is the list of the bomb button's which is present on the game gui of the character(here)


    public ArrayList level_data_all_field_names = new ArrayList(){"Name","Total_Zombie","Max_Zombie","Difficulty_Level","Score","Is_Level_Unlocked"};
    // public bool is_spell_in_hand;

    public string current_level_name = null;

    public string bomb_data_field_location = "data//data_fields/bomb_data_fields.zhd";
    public string view_scene_base_url = "res://Views/Scenes";
    
    public string spell_in_hand;
    public override void _Ready()
    {
        is_game_over = false;
        _main_character_name = null;
        score = 0;
        level_name = null;
        is_level_added = false;

        is_arcade = false;
        
        // is_spell_in_hand = false;
        spell_in_hand = null;
    }


    public override void _Process(float delta)
    {


    }
}
