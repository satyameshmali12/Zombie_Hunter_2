using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class Basic_View:Control,Global_Variables_F_A_T
{
    public _Type_of_ _node_type{get;set;}
    
    public Basic_Func basf;

    string level_base_url;

    Changing_Scene changing_Scene;
    bool is_changing_scene_removed = false;

    public override void _Ready()
    {
        _node_type = _Type_of_.View;
        this.Visible = false;


        basf = new Basic_Func(this);
        changing_Scene = basf.add_changing_scene(this,"",false,false);
        
        basf.create_a_sound("res://assets/audio/GUI/Scene_Enter.mp3",this,true);

        var global = basf.global_Variables;
        global.current_scene = this;
        global.click_sound = global.navigation_sound_url;

        level_base_url = "res://Levels/Scenes/";

        
    }
    public override void _Process(float delta){

    }

    public bool start_level(string level_name){
        GetTree().ChangeScene($"{level_base_url}{level_name}.tscn");
        return true;
    }

    public void initialize_view(){

    }
        
}
