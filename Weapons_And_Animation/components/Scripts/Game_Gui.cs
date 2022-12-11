using Godot;
using System;
using System.Collections;

public class Game_Gui : Node2D
{


    Basic_Func basf;

    Label score_label;

    public override void _Ready()
    {
        basf = new Basic_Func(this, "data//data_fields/bomb_data_fields.zhd");
        // bomb_Buttons = this.GetNode<Node2D>("Bomb_Button's").GetChildren();
        basf.global_Variables.bomb_Buttons = basf.get_the_node_childrens("Bomb_Button's");

        score_label = GetNode<Label>("Score_Label");
    }
    
    public override void _Process(float delta)
    {
        var score = basf.global_Variables.score;
        
        score_label.Text = $"Sc0re:- {score}";

        var bomb_Buttons = basf.global_Variables.bomb_Buttons;
        foreach (TextureButton item in bomb_Buttons)
        {
            if (item.Pressed)
            {
                basf.global_Variables.spell_in_hand = item.Name;
            }
        }

        var dm = basf.dm;
        var bomb_buttons = basf.get_the_node_childrens("Bomb_Button's");
        foreach (TextureButton item in bomb_buttons)
        {
            // reading the data again as to detect the use of the bomb
            basf.dm.read_data();
            // loading the data for a particular bomb
            basf.dm.load_data(item.Name);
            var count = Convert.ToInt32(dm.get_data("No_Of_Bomb_Available"));

            // giving the value of the data to the count(label) node of the button
            // it will display the number of the bomb available
            item.GetNode<Label>("Count").Text = $"{count}";

        }
        // if(Input.IsActionPressed("Mouse_Pressed")){
        //     // GD.Print("hello world this is from the game)ui side so make sure to see it on the terminal");
        // }
    }


}
