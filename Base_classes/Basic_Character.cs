using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class Basic_Character: RigidBody2D, Global_Variables_F_A_T
{

    // all the properties of a basic character
    public string _node_type{get;set;}
    public int power_available;
    public Vector2 moving_speed;
    // public 
    public Direction moving_direction;
    public int health;



    // all the nodes of basic character
    public AnimatedSprite animations;



    #region Gravity
    // gravity for a player
    // this property will help to make the player fall down with more speed if the player not on the ground
    [Export]
    public int advanced_gravity = 300;
    public int default_gravity;   // this is to 
    #endregion




    public override void _Ready()
    {
        _node_type = null;
        default_gravity = advanced_gravity;

    }

    public override void _Process(float delta)
    {
        
    }

    
}