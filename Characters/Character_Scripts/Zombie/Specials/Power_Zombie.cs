using Godot;
using System;

/*
All powerfull zombie will inherit this script instead of only basic_zombie script 
this class is the child of the basic zombie class so that a powerful zombie will also be able to do all the basic things
*/

public class Powerful_Zombie : Basic_Zombie
{

    public override void _Ready()
    {
        base._Ready();

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
    }

}
