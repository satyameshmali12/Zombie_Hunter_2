using Godot;
using System;
using System.Collections;

public class Global_Variables : Node2D
{

    public Vector2 player_position = Vector2.Zero;
    public bool is_game_over;

    public string _main_character_name; // this will help us in the multi character match
    public Node2D character_scene = null;
    public string level_name;
    public TileMap level_tiles = null;
    /* this custom url is been used on the main game scene to render the level
    this is used to render the zombie match which is not included in the the basic levels e.g Arcade*/
    
    public string custom_url = null;

    public bool is_level_added;
    public int score;
    public Node2D level_scene = null;

    public ArrayList visibility_list = new ArrayList();



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

    // this value will be suppllied right from the respective items right from the use item function
    #region both of this are interrelated to each other i.e the item is hand is the url to the scene whereas the item_using_menu_comp is the scene of that url
    public string item_in_hand = null;
    public Item_Using_Menu_Component item_using_menu_comp = null;
    #endregion
    // public Button item_cancel_button = null;

    public ArrayList guiticke_buttons = new ArrayList();
    public bool is_guiticle_button_pressed = false;

    public bool is_game_quitted = false;

    public Item_Using_Menu menu = null;
    public ArrayList item_in_progression = new ArrayList();

    public Item_Remover item_removing_screen = null;
    
    // public Notification notification = null;

    /// <summary>This will be given right from the game_gui and will be used in the item using menu</summary>


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

        // checking whether a guitickle is pressed or not
        if(!is_game_quitted)
        {
            if(!is_guiticle_button_pressed){
                var is_mouse_pressed_on_remover_button = false;
                if(item_removing_screen!=null){
                    var rect = item_removing_screen.GetNode<ReferenceRect>("Rect");
                    is_mouse_pressed_on_remover_button = item_removing_screen.Visible && basf.is_in_box(this.GetGlobalMousePosition(),rect.RectGlobalPosition,rect.RectSize);
                }
                is_guiticle_button_pressed = basf.is_any_guitickle_button_pressed() || is_mouse_pressed_on_remover_button;
            }
            else if(!Input.IsActionPressed("Mouse_Left_Pressed"))
            {
                is_guiticle_button_pressed = false;
            }

        }
    }
    

    public void increment_score(int increment){
        score+=increment;
    }

}
