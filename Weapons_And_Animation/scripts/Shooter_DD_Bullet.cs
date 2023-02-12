using Godot;
using System;

public class Shooter_DD_Bullet : Basic_Throwable_Weapon
{

    public override void _Ready()
    {
        weapon_name = "Shooter_DD_Bullet";
        base._Ready();
        damage = 30;
        weapon_speed = 8;

        this.exclude_list.Add(basf.global_Variables.character_scene);
        this.exclude_list.Add(this.GetParent());

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }

    
    public override void Life_Finished(){
        base.Life_Finished();
    }

    public override void Collided_With_An_Obj(Node2D body){
        base.Collided_With_An_Obj(body);
    }
}
