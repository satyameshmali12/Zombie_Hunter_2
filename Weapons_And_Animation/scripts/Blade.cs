using Godot;
using System;

public class Blade : Basic_Throwable_Weapon
{
    public override void _Ready()
    {
        base._Ready();
        damage = 55;
        weapon_speed = 12;
        
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
