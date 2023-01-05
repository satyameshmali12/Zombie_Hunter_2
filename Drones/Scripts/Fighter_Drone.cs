using Godot;
using System;

public class Fighter_Drone : Basic_Drone
{

    public override void _Ready()
    {

        bomb_drop_interval = 1;
        this.drone_name = "Fighter_Drone";

        base._Ready();


        is_can_fall_bomb = true;

        bomb_name = "Explosion_Gas";

        var points = this.GetParent().GetNode<Node2D>("Points");

        is_map_drop_available = false;
        // spawn_drone(Vector2.Zero, Vector2.Zero, );



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

    public override void spawn_drone(Vector2 spawn_position, Vector2 target_position, Basic_Character parent)
    {
        var points = this.GetParent().GetNode<Node2D>("Points");

        spawn_position = points.GetNode<Position2D>("Start_Point").GlobalPosition;
        target_position = points.GetNode<Position2D>("End_Point").GlobalPosition;

        base.spawn_drone(spawn_position, target_position, parent);
    }
}
