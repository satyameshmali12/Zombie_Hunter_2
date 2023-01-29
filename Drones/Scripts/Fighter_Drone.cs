using Godot;
using System;

public class Fighter_Drone : Basic_Drone
{
    
    /*
    since this data is required before adding them in the scene tree
    thus we have given this value over here
    */
    public Fighter_Drone()
    {
        is_to_add_instantaneouly = true;
        is_map_drop_available = false;
    }

    public override void _Ready()
    {

        bomb_drop_interval = 1;
        this.name = "Fighter_Drone";

        base._Ready();


        is_can_fall_bomb = true;
        bomb_name = "Explosion_Gas";


        var points = this.GetParent().GetNode<Node2D>("Points");

        is_map_drop_available = false;



    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        move_to_target_position();
        if (!ground_collision_ray.IsColliding())
        {
            drop_bomb();
        }

        if (Math.Abs(target_position.x - this.Position.x) < 10)
        {
            this.health = 0;
        }
    }

    public override void spawn_item(Vector2 spawn_position, Vector2 target_position, Basic_Character parent)
    {
        var level_scene = basf.global_Variables.level_scene;
        var points = level_scene.GetNode<Node2D>("Points");

        spawn_position = points.GetNode<Position2D>("Start_Point").GlobalPosition;
        target_position = points.GetNode<Position2D>("End_Point").GlobalPosition;

        base.spawn_item(spawn_position, target_position, parent);

    }

}
