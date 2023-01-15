using Godot;
using System;

/*
All powerfull zombie will inherit this script instead of only basic_zombie script 
this class is the child of the basic zombie class so that a powerful zombie will also be able to do all the basic things
*/

public class Powerful_Zombie : Basic_Zombie
{

    public RayCast2D upward_ray;


    /*
    this list will contain all the types character upon which player can jump onto
    */
    public _Type_of_[] Can_Jump_On_To;



    public override void _Ready()
    {
        base._Ready();
        _node_type = _Type_of_.Zombie;

        upward_ray = this.GetNode<RayCast2D>("Upward_Ray");

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

    }

    public virtual void Perform_Special_Move()
    {


    }

    public virtual void Upward_Ray_Colliding(){
        
    }

}
