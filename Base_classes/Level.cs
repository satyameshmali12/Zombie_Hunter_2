using System;
using System.Collections;
using Godot;
using System.IO;

public class Level : Node2D, Global_Variables_F_A_T
{
    public Basic_Func basf;
    // Data_Manager user_data;


    public _Type_of_ _node_type { get; set; }

    // spawn points
    public ArrayList zombie_spawn_points;
    Position2D player_spawn_point;

    // zombie level is arranged in the ascending order
    // arranging the zombie with their increasing power
    ArrayList zombie_level_list;


    // both the stuff i.e player_scene and the enemy_scene can be done using one scene but for the purpose of understanding both the concerns are kept seperated
    PackedScene player_scene;
    PackedScene enmey_scene;

    Global_Variables global_variables;

    // Data_Manager dm;

    bool is_data_saved;

    Basic_Player player;

    int game_over_timing;

    public const int max_time_time_on_screen = 20; // max time to be live on screen as the gameover as the max time is been over then the player is been navigated automatically to the game over screen


    public int total_zombie; // the total number of zombie that will be dropped throught the entire game
    public int max_zombie_per_screen; // the total_nubmer of zombie that can stay on in the game
    public int difficulty_level;

    public AudioStreamPlayer2D background_Music;

    public Button pause_button;

    public Node2D game_gui;

    // this boolean will be overrided from the aracde script of the game
    public bool is_arcade;




    public override void _Ready()
    {
        _node_type = _Type_of_.View;

        game_over_timing = 0;

        basf = new Basic_Func(this);

        // setting the metadata of the for the level
        // in other words resetting all the properties of the global variables
        basf.global_Variables.current_level_name = this.Name;  // passing the current level name to the global script
        basf.global_Variables.spell_in_hand = null;
        basf.global_Variables.score = 0;
        basf.global_Variables.had_win_the_game = false;


        basf.dm.load_data(this.Name);

        total_zombie = Convert.ToInt32(basf.dm.get_data("Total_Zombie"));

        max_zombie_per_screen = Convert.ToInt32(basf.dm.get_data("Max_Zombie"));

        difficulty_level = Convert.ToInt32(basf.dm.get_data("Difficulty_Level"));

        basf.increment_loading_percent(10);

        is_data_saved = false;

        // making the zombie_level_list_list
        zombie_level_list = new ArrayList() { "Male_Zombie", "Female_Zombie", "Female_Zombie_2", "Wild_Zombie", "Female_Zombie", "Male_Zombie_2" };

        zombie_spawn_points = basf.get_the_node_childrens("Zombie_Spawn_Points");
        player_spawn_point = this.GetNode<Position2D>("Player_Spawn_Point");


        var character_name = basf.user_data.get_data("Current_Character");
        player_scene = ResourceLoader.Load<PackedScene>($"res://Characters/Characters_Scene/Player/{character_name}.tscn");

        player = player_scene.Instance<Basic_Player>();
        player.Position = player_spawn_point.Position;
        this.AddChild(player);

        basf.increment_loading_percent(20);


        global_variables = GetNode<Global_Variables>("/root/Global_Variables");
        global_variables._main_character_name = player.Name;


        background_Music = basf.create_a_sound("res://assets/audio/Zombie/Zombie_Entire_Background.mp3", this, false, 1, .2f, .2f);


        game_gui = player.GetNode<Node2D>("Game_Gui");
        pause_button = game_gui.GetNode<Button>("Pause_Button");

        basf.increment_loading_percent(20);
        GD.Print("hey the loading is done");

    }

    public override void _Process(float delta)
    {

        if(!is_data_saved && !global_variables.is_game_over){
            if(pause_button.Pressed){
                game_gui.GetNode<Pause_Menu>("Pause_Menu").Visible = true;
                basf.pause_tree(this);
            }
        }
        var zombie_area = GetNode<Node2D>("Zombie_Area");
        spawn_zombie(zombie_area);
        var win_condition = total_zombie == 0 && zombie_area.GetChildren().Count == 0;



        if (global_variables.is_game_over || win_condition)
        {
            if (!is_data_saved)
            {
                var game_over_view_label_pack = ResourceLoader.Load<PackedScene>("res://Weapons_And_Animation/components/scenes/Game_Over_Show_View.tscn");
                Game_Over_Show_View game_over_view_label = game_over_view_label_pack.Instance<Game_Over_Show_View>();

                global_variables.is_game_over = false;

                background_Music.Stop();
                // unlocking the next level
                // the complete description can be founded in the respective classes
                if (win_condition)
                {
                    // saving the data for the current level
                    // if the player win's then setting the score
                    var score = global_variables.score;
                    var dm = basf.dm;
                    if (score > Convert.ToInt32(dm.get_data("Score")))
                    {
                        dm.set_value("Score", score.ToString());
                        dm.save_data();
                    }

                    // supplying the data to the global_variables
                    global_variables.had_win_the_game = true;

                    // unlocking the next level
                    basf.dm.unlock_next_level(this.Name);
                    var adding_position = player.GetNode<Node2D>("Game_Gui").GetNode<Node2D>("Adding_Postion");
                    game_over_view_label.Position = adding_position.Position;
                    player.AddChild(game_over_view_label);
                }
                else
                {
                    game_over_view_label.Position = global_variables.game_over_view_adding_position;
                    this.AddChild(game_over_view_label);

                }

                is_data_saved = true;
                var one_s_p_a_o = basf.create_timer(1, "one_second_pass_after_over");
                one_s_p_a_o.Start();

                game_over_view_label.set_w_l(win_condition);
            }
            is_data_saved = true;
        }

        if (Input.IsActionJustPressed("Move_To_GOS") || game_over_timing == max_time_time_on_screen)
        {
            if (is_data_saved)
            {
                move_to_game_over_screen();
                global_variables.is_game_over = false;
            }
        }


        var is_bomb_in_hand = basf.global_Variables.spell_in_hand != null;
        if (is_bomb_in_hand && Input.IsActionPressed("Mouse_Pressed") && !basf.is_any_one_button_pressed(global_variables.bomb_Buttons))
        {
            GD.Print(global_variables.spell_in_hand);
            GD.Print();
            GD.Print("hey the mouse pressed event is been created..!!");
            // GD.Print("hello world pressed!!");
            var ini_faller = basf.get_the_packed_scene("res://Bomb's/Scenes/Initial_Faller.tscn").Instance() as Initial_Faller;
            GD.Print("from the basic level..!!");
            ini_faller.settle_values(global_variables.spell_in_hand, GetGlobalMousePosition());
            // ini_faller.Position = GetGlobalMousePosition();
            // ini_faller.Emitting = true;
            this.AddChild(ini_faller);
            global_variables.spell_in_hand = null;
        }
    }

    public void one_second_pass_after_over()
    {
        game_over_timing++;
    }

    public bool spawn_zombie(Node2D zombie_area)
    {
        var tot_num_of_zom = zombie_area.GetChildCount();
        var is_successfully_added = false;
        if (max_zombie_per_screen > tot_num_of_zom && total_zombie > 0 && !is_data_saved)
        {
            var new_zombie_url = $"res://Characters/Characters_Scene/Zombie/{zombie_level_list[(int)GD.RandRange(0, difficulty_level + 1)]}.tscn";

            enmey_scene = ResourceLoader.Load<PackedScene>(new_zombie_url);

            Basic_Zombie new_zombie = enmey_scene.Instance() as Basic_Zombie;

            Position2D new_zombie_position = (Position2D)zombie_spawn_points[(int)GD.RandRange(0, zombie_spawn_points.Count)];

            new_zombie.Position = new_zombie_position.Position;

            zombie_area.AddChild(new_zombie);

            is_successfully_added = true;

            if(!is_arcade){
                total_zombie--;
            }

        }
        return is_successfully_added;
    }

    void move_to_game_over_screen()
    {
        GetTree().ChangeScene("res://Views/Scenes/Game_Over_View.tscn");
    }




}