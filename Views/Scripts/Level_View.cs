using Godot;
using System;
using System.Collections;
// using System.IO;

public class Level_View : Basic_View
{
    ArrayList level_buttons;

    Basic_Func basf;

    int button_per_screen,total_number_of_button;
    int start_button_number = 0;

    bool is_button_pressed = false;
    // Data_Manager data_Manager;  

    Button forward,backward;

    public override void _Ready()
    {
        base._Ready();
        basf = new Basic_Func(this);

        level_buttons = all_func.get_the_node_childrens("Level_Buttons");

        button_per_screen = 4;

        total_number_of_button = System.IO.Directory.GetFiles("C:\\Users\\hp\\OneDrive\\Desktop\\Zombie Hunter 2\\Levels\\Scenes").Length;
        GD.Print(total_number_of_button);

        forward = GetNode<Button>("Forward");
        backward = GetNode<Button>("Backward");
        change_the_level(0);

        // setting the field name's of the level_data
        basf.dm.all_field_names = basf.global_Variables.level_data_all_field_names;
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        foreach (Button item in level_buttons)
        {
            if (item.Pressed)
            {
                if (basf.dm.is_level_unlocked(item.Name))
                {
                    GD.Print("first_stage");
                    basf.global_Variables.is_level_added = false;
                    basf.global_Variables.level_name = item.Name;
                    GetTree().ChangeScene("res://Views/Scenes/Main_Game_Scene.tscn");
                    GD.Print("Second Stage");
                }
            }
        }
        
        var f_press = forward.Pressed;
        var b_press = backward.Pressed;
        if(f_press || b_press){
            if(!is_button_pressed){
                change_the_level((f_press)?4:-4);
            }
            is_button_pressed = true;
        }
        else{
            is_button_pressed = false;
        }

        

    }

    public bool change_the_level(int step){
        var index = start_button_number+step;
        if(index>=0 && index<total_number_of_button){
            start_button_number+=step;
        }

        for (var i = 1; i < level_buttons.Count+1; i++)
        {
            var button = (Button)level_buttons[i-1];
            if(start_button_number+i<=total_number_of_button){
                var text = $"Level{start_button_number+i}";
                button.Text = text;
                button.Name = text;
                button.Visible = true;
                button.Disabled = false;
            }
            else{    
                button.Visible = false;
                button.Disabled = true;
            }
            
        }
            
        return true;
    }
}
