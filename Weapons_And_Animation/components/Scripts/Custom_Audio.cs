using Godot;
using System;

public class Custom_Audio : AudioStreamPlayer2D
{

    public bool is_to_play_one_time;

    public float music_duration = 1;  // this need to be passed if the music is_to_play_one_time
    
    Basic_Func basf;
    public override void _Ready()
    {
        basf = new Basic_Func(this);
        Autoplay = false;
        if(is_to_play_one_time){
            var timer = basf.create_timer(music_duration,"Queue_Free_The_Music");
            timer.Start();
        }

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
        if(!this.Playing){
            Queue_Free_The_Music();
        }
    }

    public void Queue_Free_The_Music(){
        this.QueueFree();
    }

    // make sure to set the stream of the audio before adding it to the scene
    public bool set_properties(string path_of_audio,bool is_to_play_one_time, float music_duration = 1,float volume = 1,float pitch_scale = 1){
        
        this.is_to_play_one_time = is_to_play_one_time;
        this.music_duration = music_duration;
        this.Autoplay = !is_to_play_one_time; // autoplay is settled to true if not user wanted to run one time
        this.Stream = ResourceLoader.Load<AudioStream>(path_of_audio);
        this.Play();
        this.VolumeDb = volume;
        this.PitchScale = pitch_scale;
        return true;
    }
}
