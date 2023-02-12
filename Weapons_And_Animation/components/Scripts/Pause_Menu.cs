#region description
/*
This will help to make the game pause
*/
    
#endregion

using Godot;
using System;

public class Pause_Menu : Node2D
{
    Button resume_button,quit_button;
    Basic_Func basf;

    public override void _Ready()
    {
        resume_button = this.GetNode<Button>("Resume_Button");
        quit_button = this.GetNode<Button>("Quit_Button");
        this.PauseMode = PauseModeEnum.Process;
        basf = new Basic_Func(this);
        basf.global_Variables.visibility_list.Add(this);

        basf.add_guitickle_button(resume_button,quit_button);
    }

    public override void _Process(float delta)
    {
        if(resume_button.Pressed){
            basf.pause_tree(this,false);
            this.Visible = false;
        }
        else if(quit_button.Pressed){
            this.Visible = false;
            basf.pause_tree(this,false);
            foreach (Basic_Player hero in basf.global_Variables.herosArea.GetChildren())
            {
                hero.perform_move("Death");
            }
        }
    }
}
