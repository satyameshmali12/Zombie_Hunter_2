using Godot;
using System;

public class Initial_Faller : Particles2D
{

    public bool is_bombermant_started;
    string bomb_name;
    Basic_Func basf;
    int data_start_index;

    
    public override void _Ready()
    {
        is_bombermant_started = false;
        

        // here data bomb_data is been loaded using the dm but it is used only for getting the data
        // lefting getting the data we are not using any function from the dm(data_manager)
        basf = new Basic_Func(this,"data//bomb_data.zhd");
        data_start_index = basf.get_index_in_array(basf.dm.level_data,bomb_name);
        GD.Print(data_start_index);

        
    }

    Data_Manager dm;
    
    public override void _Process(float delta)
    {
        if(is_bombermant_started && !this.Emitting){
            this.QueueFree();
        }
        
    }

    public bool settle_values(string bomb_name,Vector2 position){
        this.Position = position;
        this.bomb_name = bomb_name;
        return true;
    }
}
