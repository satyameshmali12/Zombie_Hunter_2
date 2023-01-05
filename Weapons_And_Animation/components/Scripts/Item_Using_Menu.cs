using Godot;
using System;

public class Item_Using_Menu : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text"

    public Basic_Func basf;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        basf = new Basic_Func(this);


        basf.global_Variables.visibility_list.Add(this);
        
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
