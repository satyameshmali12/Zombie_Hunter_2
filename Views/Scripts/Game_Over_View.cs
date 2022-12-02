using Godot;
using System;

public class Game_Over_View : Control
{
    Basic_Func basf;
    TextureButton restart_button,home_button;
    public bool is_to_stop; // to stop all the processes and to renavigate somewhere else
    public override void _Ready()
    {
        restart_button = this.GetNode<TextureButton>("Restart");
        home_button = this.GetNode<TextureButton>("Home_Button");

        is_to_stop = false;


        basf = new Basic_Func(this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(!is_to_stop){
            if(restart_button.Pressed){
                terminate_all_task();
                // basf.global_Variables.is_level_added = false;
                basf.navigateTo(this,basf.main_game_scene_path);
                // is_to_stop = true;
            }
            if(home_button.Pressed){
                terminate_all_task();
                basf.navigateTo(this,basf.home_scene_path);
            }
        }

    }

    public void terminate_all_task(){
        is_to_stop = true;
        basf.global_Variables.is_level_added  = false;
    }
}
