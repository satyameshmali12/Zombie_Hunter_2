using Godot;
using System;
using System.Collections;

public class Initial_Faller : Particles2D
{

    public bool is_bombermant_started;
    public string bomb_name;
    Basic_Func basf;
    // int data_start_index;

    ArrayList side_collision_rays;
    RayCast2D ground_checker;

    bool can_burst;

    int available_Bomb_Count;

    public const string bomb_scene_path = "res://Bomb's/Scenes";
    
    Vector2 collision_point;
    public override void _Ready()
    {
        is_bombermant_started = false;
        can_burst = false;

        // here data bomb_data is been loaded using the dm but it is used only for getting the data
        // lefting getting the data we are not using any function from the dm(data_manager)

        
        basf = new Basic_Func(this,"data//data_fields/bomb_data_fields.zhd");
        var data = basf.dm;
        // data.all_field_names = new ArrayList(){"Bomb_Name","No_Of_Bomb_Available"};
        basf.dm.load_data(bomb_name);

        available_Bomb_Count = Convert.ToInt32(data.get_data("No_Of_Bomb_Available"));
        can_burst = available_Bomb_Count>0;


        ground_checker = this.GetNode<RayCast2D>("Ground_Checker");
        ground_checker.Enabled = true;
        side_collision_rays = basf.get_the_node_childrens("Side_Collision_Rays");

        collision_point = Vector2.Zero;
    }

    Data_Manager dm;
    
    public override void _Process(float delta)
    {
        if(ground_checker.IsColliding() && basf.get_number_of_ray_colliding(side_collision_rays)<side_collision_rays.Count/2 && can_burst && !is_bombermant_started){
            this.Emitting = true;
            is_bombermant_started = true;
            var data = basf.dm;
            data.set_value("No_Of_Bomb_Available",(available_Bomb_Count-1).ToString());
            data.save_data();
            collision_point = ground_checker.GetCollisionPoint();
        }
        else{
            if(!is_bombermant_started){
                this.QueueFree();
            }
        }

        if(is_bombermant_started && !this.Emitting){
            GD.Print("hey there buddy the node is been added..!!");
            PackedScene bomb_scene = basf.get_the_packed_scene($"{bomb_scene_path}/{bomb_name}.tscn");
            Basic_Bomb bomb = bomb_scene.Instance() as Basic_Bomb;
            // bomb.Position = basf.global_Variables.player_position;
            bomb.Position = collision_point;
            bomb.bomb_name = bomb_name;
            // bomb.GetNode<AnimatedSprite>("Animation").Position = basf.global_Variables.player_position;
            // bomb.GetNode<Node2D>("Collision_Rays").Position = basf.global_Variables.player_position;
            GD.Print(collision_point);
            GD.Print(basf.global_Variables.player_position);
            this.GetParent().AddChild(bomb);
            this.QueueFree();
        }
        
    }

    public bool settle_values(string bomb_name,Vector2 position){
        this.Position = position;
        this.bomb_name = bomb_name;
        return true;
    }
}
