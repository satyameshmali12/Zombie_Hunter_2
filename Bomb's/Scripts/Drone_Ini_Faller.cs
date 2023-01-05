using Godot;
using System;
using System.Collections;

public class Drone_Ini_Faller : Node2D
{
    string url_to_bomb;
    ArrayList collision_rays;
    Basic_Func basf;

    bool is_journey_overed = false;
    Vector2 spawn_point;
    



    public Vector2 speed = Vector2.Down;

    public int max_travelling_distance = 100;

    public int number_of_bursting = 1;

    public string bomb_name = "Explosion_Gas";


    public override void _Ready()
    {
        base._Ready();
        basf = new Basic_Func(this);

        spawn_point = this.Position;

        url_to_bomb = $"res://Bomb's/Scenes/{bomb_name}.tscn";

        collision_rays = basf.get_the_node_childrens("Collision_Rays",true);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (!is_journey_overed)
        {
            this.Position += (speed + new Vector2(0, .4f));


            if (Math.Abs(spawn_point.y - this.Position.y) >= max_travelling_distance)
            {
                is_journey_overed = true;
            }

            foreach (RayCast2D item in collision_rays)
            {
                if (item.IsColliding())
                {
                    is_journey_overed = true;
                    break;
                }

            }
        }

        else
        {

            if (number_of_bursting > 0)
            {
                spawn_bomb();
                number_of_bursting--;
            }
            else
            {
                QueueFree();
            }

        }

    }

    public void spawn_bomb()
    {
        this.GetNode<Sprite>("Bomb").Visible = false;

        var bomb_scene = basf.get_the_packed_scene(url_to_bomb);

        Basic_Bomb bomb = bomb_scene.Instance<Basic_Bomb>();

        bomb.bomb_name = bomb_name;

        bomb.Position = this.Position;
        basf.global_Variables.level_scene.AddChild(bomb);
    }
}
