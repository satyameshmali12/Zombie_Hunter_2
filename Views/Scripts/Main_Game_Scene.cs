using Godot;
using System;
using System.Collections;
public class Main_Game_Scene : Basic_Game_View
{
    // Basic_Func basf;

    Node2D level_area;
    Node2D added_level;

    PackedScene level_scene;

    Node2D loading_view;
    bool is_loading_view_queue_free;

    public string level_path;
    public override void _Ready()
    {
        base._Ready();
        basf = new Basic_Func(this);
        level_area = this.GetNode<Node2D>("Level_Area");
        basf.global_Variables.loading_percent = 0;

        loading_view = this.GetNode<Node2D>("loading_view");
        is_loading_view_queue_free = false;
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (!is_loading_view_queue_free)
        {
            loading_view.GetNode<ProgressBar>("Loading_Bar").Value = basf.global_Variables.loading_percent;
        }


        if (!basf.global_Variables.is_level_added)
        {
            basf.global_Variables.is_level_added = true;
            basf.clear_children_nodes(level_area);

            var custom_url = basf.global_Variables.custom_url;
            
            level_path = (custom_url == null) ? $"res://Levels/Scenes/{basf.global_Variables.level_name}.tscn" : custom_url;
            
            level_scene = ResourceLoader.Load(level_path) as PackedScene;
            
            var new_level = level_scene.Instance();
            
            added_level = (Node2D)new_level;
            
            added_level.Visible = false;
            

            basf.increment_loading_percent(50);

            // adding the level to the level_area node
            level_area.AddChild(new_level);



        }

        // checking whether the loading is done or not
        if (basf.global_Variables.loading_percent >= 100 && !is_loading_view_queue_free)
        {
            loading_view.QueueFree();
            is_loading_view_queue_free = true;
            added_level.Visible = true;
        }

        // GD.Print(loading_view.IsQueuedForDeletion());

    }
}
