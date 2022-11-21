using Godot;
using System;
using System.Collections;



public class Basic_Spell : Particles2D,Global_Variables_F_A_T
{
    public _Type_of_ _node_type{get;set;}

    RayCast2D downward_ray;

    public override void _Ready()
    {

        
        
    }
    public override void _Process(float delta)
    {

        if(downward_ray.IsColliding()){

        }

    }



}