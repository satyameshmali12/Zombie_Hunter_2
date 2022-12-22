using Godot;
using System;
using System.Collections;

public class Global_Variables : Node2D
{

    public Vector2 player_position = Vector2.Zero;
    public bool is_game_over;

    public string _main_character_name; // this will help us in the multi character match
    public string level_name;


    /* this custom url is been used on the main game scene to render the level
    this is used to render the zombie match which is not included in the the basic levels e.g Arcade*/
    
    public string custom_url = null;

    public bool is_level_added;
    public int score;

    public bool is_arcade;

    // this is used for adding the gamer_over_view after the death of the player 
    // this is used in the level file
    // If you are in VsCode you can view the use by clicking on the refererence below or can move to the respective
    public Vector2 game_over_view_adding_position;

    public ArrayList bomb_Buttons; // this is the list of the bomb button's which is present on the game gui of the character(here)


    // public ArrayList level_data_all_field_names = new ArrayList() { "Name", "Total_Zombie", "Max_Zombie", "Difficulty_Level", "Score", "Is_Level_Unlocked" };
    // public bool is_spell_in_hand;

    public string current_level_name = null;

    public string bomb_data_field_location = "data//data_fields/bomb_data_fields.zhd";
    public string view_scene_base_url = "res://Views/Scenes";

    Basic_Func basf;

    public string spell_in_hand;

    public Node current_scene;

    public bool had_win_the_game = false;


    #region Related to play sound on the mouse pressed event
    public string navigation_sound_url = "res://assets/audio/GUI/navigation_sound.mp3";
    public string click_sound;
    public bool is_to_play_sound_on_click = true;
    #endregion


    public int loading_percent = 0;



    // public Data_Manager shop_data_manager;
    // public bool is_new_scene_toggled_in_shop = false;



    public override void _Ready()
    {

        is_game_over = false;
        _main_character_name = null;
        score = 0;
        level_name = null;
        is_level_added = false;

        is_arcade = false;

        basf = new Basic_Func(this);

        spell_in_hand = null;

        click_sound = navigation_sound_url;

        // shop_data_manager = new Data_Manager();

    }


    public override void _Process(float delta)
    {


        // playing the sound on every click
        // this sound will not be played during the navigation
        if (Input.IsActionJustPressed("Mouse_Pressed") && is_to_play_sound_on_click)
        {
            basf.create_a_sound(click_sound, current_scene, true);
        }
    }

    public void increment_score(int increment){
        score+=increment;
    }

}
