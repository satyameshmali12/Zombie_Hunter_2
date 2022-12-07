using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class Basic_View:Control,Global_Variables_F_A_T
{
    public _Type_of_ _node_type{get;set;}
    
    public Basic_Func all_func;

    string level_base_url;

    public override void _Ready()
    {
        _node_type = _Type_of_.View;
        
        all_func = new Basic_Func(this);
        all_func.create_a_sound("res://assets/audio/GUI/Scene_Enter.mp3",this,true);

        var global = all_func.global_Variables;
        global.current_scene = this;
        global.click_sound = global.navigation_sound_url;

        level_base_url = "res://Levels/Scenes/";
    }

    public bool start_level(string level_name){
        GetTree().ChangeScene($"{level_base_url}{level_name}.tscn");
        return true;
    }

    public void initialize_view(){

    }
        
}
