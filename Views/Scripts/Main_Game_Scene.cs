using Godot;
using System;
using System.Collections;
public class Main_Game_Scene : Node2D
{
    Basic_Func basf;
    Node2D level_area;
    PackedScene level_scene;
    public override void _Ready()
    {
        basf = new Basic_Func(this);
        level_area = this.GetNode<Node2D>("Level_Area");
    }

    public override void _Process(float delta)
    {
        if(!basf.global_Variables.is_level_added){
            basf.global_Variables.is_level_added = true;      
            basf.clear_children_nodes(level_area);
            GD.Print(basf.global_Variables.level_name);
            var level_path = $"res://Levels/Scenes/{basf.global_Variables.level_name}.tscn";
            level_scene = ResourceLoader.Load(level_path) as PackedScene;
            var new_level = level_scene.Instance();
            level_area.AddChild(new_level);
        }
        
    }
}
