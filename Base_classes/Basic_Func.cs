using Godot;
using System.Collections;
using System;

public class Basic_Func
{
    public Node node;

    public Data_Manager dm;
    public Data_Manager user_data = new Data_Manager("data//data_fields/user_data_fields.zhd");
    public Data_Manager throwable_weapons_dm;
    public Global_Variables global_Variables;
    public Global_Paths global_paths;

    public string main_game_scene_path = "res://Views/Scenes/Main_Game_Scene.tscn";
    public string home_scene_path = "res://Views/Scenes/Home_View.tscn";

    public Basic_Func(Node node, string data_path = "data//data_fields/level_data_fields.zhd")
    {
        this.node = node;
        dm = new Data_Manager(data_path);
        user_data.load_data("Aj");

        global_Variables = node.GetNode<Global_Variables>("/root/Global_Variables");
        
        global_paths = node.GetNode<Global_Paths>("/root/Global_Paths");

        throwable_weapons_dm = new Data_Manager(global_paths.basic_throwable_weapons_data_fields_Url);
    }

    public Timer create_timer(float wait_time, string signal_func_name)
    {
        var new_timer = new Timer();
        new_timer.WaitTime = wait_time;
        node.AddChild(new_timer);
        new_timer.Stop();
        new_timer.Connect("timeout", node, signal_func_name);
        return new_timer;
    }


    public ArrayList get_the_node_childrens(string node_name, bool is_ray_is_raycast2d = false)
    {
        var collider_rays = new ArrayList();
        var rays = node.GetNode<Node2D>(node_name).GetChildren();
        foreach (var item in rays)
        {
            collider_rays.Add(item);
            if (is_ray_is_raycast2d)
            {
                RayCast2D ray = item as RayCast2D;
                ray.Enabled = true;
            }
        }
        return collider_rays;
    }

    public bool make_all_raycast2d_enable(ArrayList arr)
    {
        try
        {
            foreach (RayCast2D item in arr)
            {
                item.Enabled = true;
            }
            return true;
        }
        catch (System.Exception)
        {
            throw new System.InvalidCastException("Check for you node type may it differs..!!");
        }
    }

    public bool clear_children_nodes(Node node_name)
    {
        foreach (Node2D item in node_name.GetChildren())
        {
            item.QueueFree();
        }
        return true;
    }
    public void navigateTo(Node node, string path)
    {
        // node.GetTree().ChangeScene(path);
        global_Variables.current_scene = node;
        add_changing_scene(node,path,true,true);
    }
    public Changing_Scene add_changing_scene(Node node,string path,bool is_to_reverse,bool is_to_navigate){

        var scene = this.get_the_packed_scene("res://Weapons_And_Animation/components/scenes/Changing_Scene.tscn");

        Changing_Scene changing_Scene = scene.Instance<Changing_Scene>();

        changing_Scene.RectPosition = Vector2.Zero;
        changing_Scene.Visible = false;

        changing_Scene.set_values(node,path,is_to_reverse,is_to_navigate);

        node.AddChild(changing_Scene);
        changing_Scene.Visible = true;

        return changing_Scene;
    }

    
    public bool is_any_one_button_pressed(ArrayList button_list)
    {
        
        foreach (BaseButton item in button_list)
        {
            if (item.Pressed)
            {
                return true;
            }
        }
        return false;
    }

    public PackedScene get_the_packed_scene(string path_of_the_scene)
    {
        try
        {
            PackedScene scene = ResourceLoader.Load<PackedScene>(path_of_the_scene);
            return scene;
        }

        catch (System.IO.FileNotFoundException)
        {
            throw new System.IO.FileNotFoundException("Hey no such file exits..!!");
        }

    }
    public int get_index_in_array<Thing, Value>(Thing[] arr, Value value)
    {
        for (var i = 0; i < arr.Length; i++)
        {
            GD.Print(arr[i], value.ToString());
            if (arr[i].ToString().Trim() == value.ToString().Trim())
            {
                return i;
            }
        }
        return -1;
    }

    public int get_number_of_ray_colliding(ArrayList raycast_2ds_arr)
    {
        var count = 0;
        foreach (RayCast2D item in raycast_2ds_arr)
        {
            if (!item.Enabled)
            {
                item.Enabled = true;
            }
            if (item.IsColliding())
            {
                count++;
            }

        }
        return count;
    }

    // music duration should be given only when if the music is to be played one time
    public Custom_Audio create_a_sound(string path_of_audio, Node parent, bool is_to_play_one_time, float music_duration = 1,float volume = 1,float pitch_scale = 1)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Weapons_And_Animation/components/scenes/Simple_Audio.tscn");
        Custom_Audio audio = scene.Instance<Custom_Audio>();
        audio.set_properties(path_of_audio,is_to_play_one_time,music_duration,volume,pitch_scale);
        parent.AddChild(audio);
        return audio;
    }

    
    public bool increment_loading_percent(int increment){

        global_Variables.loading_percent+= (global_Variables.loading_percent+increment>100)?(100-global_Variables.loading_percent):increment;
        return true;
    }

    public void pause_tree(Node node,bool is_to_pause = true){
        node.GetTree().Paused = is_to_pause;
    }
}