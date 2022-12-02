using Godot;
using System;
using System.Collections;

public class Game_Gui : Node2D
{

    
    Basic_Func basf;

    public override void _Ready()
    {
        basf = new Basic_Func(this);
        // bomb_Buttons = this.GetNode<Node2D>("Bomb_Button's").GetChildren();
        basf.global_Variables.bomb_Buttons = basf.get_the_node_childrens("Bomb_Button's");
    }

    public override void _Process(float delta)
    {
        var bomb_Buttons = basf.global_Variables.bomb_Buttons;
        foreach (TextureButton item in bomb_Buttons)
        {
            if(item.Pressed){
                basf.global_Variables.spell_in_hand = item.Name;           
            }
        }
        // if(Input.IsActionPressed("Mouse_Pressed")){
        //     // GD.Print("hello world this is from the game)ui side so make sure to see it on the terminal");
        // }
    }

    
}
