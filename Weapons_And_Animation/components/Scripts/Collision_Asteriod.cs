using Godot;
using System.Collections;
using System;

public class Collision_Asteriod : Node2D
{
    ArrayList killing_types = new ArrayList() { _Type_of_.Zombie, _Type_of_.Player, _Type_of_.Drone };
    public override void _Ready() { }

    public override void _Process(float delta)
    {

        var collision_rays = this.GetNode<Node2D>("Collision_Rays").GetChildren();
        foreach (RayCast2D ray in collision_rays)
        {
            var collider = ray.GetCollider();
            if (collider != null)
            {
                Global_Variables_F_A_T type = collider as Global_Variables_F_A_T;
                if (killing_types.Contains(type._node_type))
                {
                    Character killing_character = collider as Character;
                    killing_character.health = 0;
                }
            }

        }

    }
}
