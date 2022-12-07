using Godot;
using System;

public class Custom_Audio : AudioStreamPlayer2D
{

    public bool is_to_play_one_time;

    public int music_duration = 1;  // this need to be passed if the music is_to_play_one_time
    
    Basic_Func basf;
    public override void _Ready()
    {
        basf = new Basic_Func(this);
        Autoplay = false;
        if(is_to_play_one_time){
            GD.Print("hey there buddy");
            var timer = basf.create_timer(music_duration,"Queue_Free_The_Music");
            timer.Start();
        }

    }

    public void Queue_Free_The_Music(){
        this.QueueFree();
    }
}
