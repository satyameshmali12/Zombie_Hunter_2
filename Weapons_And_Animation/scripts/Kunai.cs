using Godot;
using System;

public class Kunai : Basic_Throwable_Weapon
{

    public override void _Ready()
    {
        base._Ready();
        damage = 40;
        weapon_speed = 10;
        is_to_flip_vertically = true;
        var parent = this.GetParent<Basic_Character>();
        animation.FlipV = (parent.moving_direction == Direction.Right) ? false : true;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }

    public override void Life_Finished()
    {
        base.Life_Finished();
    }

    public override void Collided_With_An_Obj(Node2D body)
    {
        base.Collided_With_An_Obj(body);
    }



}
