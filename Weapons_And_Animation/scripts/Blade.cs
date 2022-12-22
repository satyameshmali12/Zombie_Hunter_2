using Godot;
using System;

public class Blade : Basic_Throwable_Weapon
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
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
