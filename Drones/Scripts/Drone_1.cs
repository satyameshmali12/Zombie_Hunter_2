using Godot;
using System;

#region Description
// this drone first reach the target position and then it drops the number of bomb available with a some interval
// ones task done this drone get burst
#endregion

public class Drone_1 : Basic_Drone
{

    int bomb_available = 10;
    bool can_dropped_bomb = true;

    public override void _Ready()
    {
        name = "Drone_1";

        bomb_drop_interval = 2;


        base._Ready();


        
        is_can_fall_bomb = true;
        
        bomb_name = "Explosion_Gas";
        
    }



    public override void _Process(float delta)
    {
        base._Process(delta);
        move_to_target_position();

        if (bomb_available > 0)
        {
            if (have_reached_target_position)
            {
                var is_bomb_dropped = drop_bomb();
                if (is_bomb_dropped)
                {
                    bomb_available--;
                }
            }
        }
        else
        {
            task_complete = true;
        }
    }

    public override void spawn_item(Vector2 spawn_position, Vector2 target_position,Basic_Character parent)
    {

        spawn_position = basf.global_Variables.level_scene.GetNode("Points").GetNode<Position2D>("Start_Point").GlobalPosition;
        base.spawn_item(spawn_position,target_position,parent);
    }


}
;