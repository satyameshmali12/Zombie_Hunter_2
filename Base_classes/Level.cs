using System;
using System.Collections;
using Godot;
using System.IO;

public class Level : Node2D, Global_Variables_F_A_T
{
    Basic_Func basf;


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



    public override void _Ready()
    {
        _node_type = _Type_of_.View;

        basf = new Basic_Func(this);
        basf.dm = new Data_Manager();
        basf.dm.load_data(this.Name);

        GD.Print(basf.dm.data_start_index, basf.dm.difficulty_level, basf.dm.max_zombie_per_screen);

        // making the zombie_level_list_list
        zombie_level_list = new ArrayList() { "Male_Zombie", "Female_Zombie", "Female_Zombie_2", "Wild_Zombie", "Female_Zombie" };

        zombie_spawn_points = basf.get_the_node_childrens("Zombie_Spawn_Points");
        player_spawn_point = this.GetNode<Position2D>("Player_Spawn_Point");

        player_scene = ResourceLoader.Load<PackedScene>("res://Characters/Characters_Scene/Player/Robot.tscn");

        Basic_Player player = player_scene.Instance<Basic_Player>();
        player.Position = player_spawn_point.Position;
        this.AddChild(player);

        global_variables = GetNode<Global_Variables>("/root/Global_Variables");
        global_variables._main_character_name = player.Name;

    }

    public override void _Process(float delta)
    {
        var zombie_area = GetNode<Node2D>("Zombie_Area");
        spawn_zombie(zombie_area);

        if (global_variables.is_game_over)
        {
            GD.Print(global_variables.score);
            basf.dm.level_data[basf.dm.data_start_index + 4] = global_variables.score.ToString();
            GD.Print(basf.dm.level_data[basf.dm.data_start_index + 4]);
            System.IO.File.WriteAllLines("data/level_data.zhd", basf.dm.level_data);
            global_variables.is_game_over = false;
        }
    }

    public bool spawn_zombie(Node2D zombie_area)
    {
        var tot_num_of_zom = zombie_area.GetChildCount();
        var is_successfully_added = false;
        if (basf.dm.max_zombie_per_screen > tot_num_of_zom && basf.dm.total_zombie > 0)
        {
            // GD.Print(tot_num_of_zom);
            var new_zombie_url = $"res://Characters/Characters_Scene/Zombie/{zombie_level_list[(int)GD.RandRange(0, basf.dm.difficulty_level + 1)]}.tscn";
            enmey_scene = ResourceLoader.Load<PackedScene>(new_zombie_url);
            Basic_Zombie new_zombie = enmey_scene.Instance() as Basic_Zombie;
            Position2D new_zombie_position = (Position2D)zombie_spawn_points[(int)GD.RandRange(0, zombie_spawn_points.Count)];
            new_zombie.Position = new_zombie_position.Position;
            zombie_area.AddChild(new_zombie);
            is_successfully_added = true;
            basf.dm.total_zombie--;
        }
        return is_successfully_added;
    }





}