#region Name
/*
this script can only switch between views
the button names should correspond with the view name
the button should be given the script specificely not to their parent
Make sure to give the button a appropriate name so that else the program will get crash or will throw error..!!
*/
#endregion

using Godot;
using System;

public class Navigational_Button : TextureButton
{


    bool is_button_pressed = false;
    Basic_Func basf;
    public string navi_url;
    public override void _Ready()
    {
        basf = new Basic_Func(this);
        navi_url = $"res://Views/Scenes/{this.Name}.tscn";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(this.Pressed && !is_button_pressed){
            is_button_pressed = true;
            basf.navigateTo(this,navi_url);
        }

    }
}
