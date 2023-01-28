using Godot;
using System.Collections;
using System;

public class Basic_Func : Node
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
                ray.CollideWithAreas = true;
                ray.CollideWithBodies = true;
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
        add_changing_scene(node, path, true, true);
    }
    public Changing_Scene add_changing_scene(Node node, string path, bool is_to_reverse, bool is_to_navigate)
    {

        var scene = this.get_the_packed_scene("res://Weapons_And_Animation/components/scenes/Changing_Scene.tscn");

        Changing_Scene changing_Scene = scene.Instance<Changing_Scene>();

        changing_Scene.RectPosition = Vector2.Zero;
        changing_Scene.Visible = false;

        changing_Scene.set_values(node, path, is_to_reverse, is_to_navigate);

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
    public Custom_Audio create_a_sound(string path_of_audio, Node parent, bool is_to_play_one_time, float music_duration = 1, float volume = 1, float pitch_scale = 1)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Weapons_And_Animation/components/scenes/Simple_Audio.tscn");
        Custom_Audio audio = scene.Instance<Custom_Audio>();
        audio.set_properties(path_of_audio, is_to_play_one_time, music_duration, volume, pitch_scale);
        parent.AddChild(audio);
        return audio;
    }


    public bool increment_loading_percent(int increment)
    {

        global_Variables.loading_percent += (global_Variables.loading_percent + increment > 100) ? (100 - global_Variables.loading_percent) : increment;
        return true;
    }

    public void pause_tree(Node node, bool is_to_pause = true)
    {
        node.GetTree().Paused = is_to_pause;
    }

    public bool remove_all_childs()
    {
        var children = node.GetChildren();
        foreach (Node item in children)
        {
            item.QueueFree();
        }
        return true;
    }

    public void disvisible_visible_list(Node2D except = null)
    {
        foreach (Node2D item in global_Variables.visibility_list)
        {
            if (except != item)
            {
                item.Visible = false;
            }
        }
    }

    /// <summary>If the given ray is collided It gives the object collider else null</summary>
    /// <returns>It returns the collider of type Global_Varaible_F_A_T</returns>
    public Global_Variables_F_A_T getcollider<Thing>(RayCast2D ray) where Thing : class
    {
        if (ray.IsColliding())
        {
            Global_Variables_F_A_T collider = (Global_Variables_F_A_T)ray.GetCollider();
            return collider;
        }

        return null;
    }

    public void nullify_item_in_hand()
    {
        global_Variables.item_in_hand = null;
        global_Variables.item_using_menu_comp = null;
    }

    /// <summary>It makes a button a guitickle button</summary>
    public void add_guitickle_button(params BaseButton[] button)
    {
        foreach (BaseButton item in button)
        {
            if (!this.global_Variables.guiticke_buttons.Contains(item))
            {
                this.global_Variables.guiticke_buttons.Add(item);
            }
        }
    }

    public bool is_any_guitickle_button_pressed()
    {

        foreach (BaseButton gui_button in global_Variables.guiticke_buttons)
        {
            if(gui_button==null)
            {
               global_Variables.guiticke_buttons.Clear();
               break; 
            }
            if (gui_button.Pressed)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary> 
    /// This helps to clean the stuff i.e the objects when the level is finshed
    /// <para>This cleaning of the object and the resettling of the varialbes is consider as the clearing of the garbage</para>
    /// </summary> 
    public void clear_garbage()
    {
        global_Variables.visibility_list.Clear();
        global_Variables.guiticke_buttons.Clear();
        nullify_item_in_hand();
        global_Variables.item_Using_Menu = null;
    }

    public Vector2 abs_a_vector(Vector2 point)
    {
        return new Vector2(Math.Abs(point.x), Math.Abs(point.y));
    }

    /// <summary>Splits the string in int into listwith an n (The many identifier you want) number of identifers</summary>
    public ArrayList get_format_array_string(string sentence, ArrayList identifiers, bool is_to_trim)
    {
        ArrayList new_values = new ArrayList();
        int start_index = 0;


        void add_value(string value)
        {
            if (value.Length > 0)
            {
                new_values.Add((!is_to_trim) ? value : value.Trim());
            }
        }

        for (int i = 0; i < sentence.Length; i++)
        {

            foreach (string identifer in identifiers)
            {
                var last_index = ((i + identifer.Length) < sentence.Length) ? (identifer.Length) : sentence.Length - i;
                var checking_value = sentence.Substring(i, last_index);
                if (checking_value == identifer)
                {
                    add_value(sentence.Substring(start_index, i - start_index));
                    i += identifer.Length;
                    start_index = i;
                    break;
                }
            }
        }
        if (start_index < sentence.Length)
        {
            add_value(sentence.Substring(start_index, sentence.Length - start_index));
        }

        return new_values;
    }

    public bool is_in_box(Vector2 c_point, Vector2 position, Vector2 size)
    {
        if (c_point.x > position.x && c_point.y > position.y && c_point.x < position.x + size.x && c_point.y < position.y + size.y)
        {
            return true;
        }

        return false;
    }

    public Godot.Collections.Dictionary<string, string> get_dictionary<Thing>(Thing[] names, Thing[] values)
    {
        Godot.Collections.Dictionary<string, string> item = new Godot.Collections.Dictionary<string, string>();
        for (int i = 0; i < names.Length; i++)
        {
            item.Add(names[i].ToString(), values[i].ToString());
        }
        return item;
    }

    public int get_scroller_start_index_change(int render_start_index, int box_per_screen, ArrayList items)
    {
        int change = 0;
        change = (Input.IsActionJustPressed("Move_Up") && (render_start_index > 0)) ? -1 : change;
        change = (Input.IsActionJustPressed("Move_Down") && (render_start_index + box_per_screen) < items.Count) ? 1 : change;

        return change;
    }

    public ArrayList get_split_data_in_wdm(string sentence)
    {
        // string sentence = "|Drone1:Hey there danger||Drone12:Hey there danger is it joke|";
        // string sentence = data;
        int count = 0;
        ArrayList items = new ArrayList();
        for (int i = 0; i < sentence.Length && sentence.Trim()!="-"; i++)
        {
            if (count < sentence.Length)
            {

                var character = sentence[count].ToString();
                if (character == "|")
                {
                    var remaining_sentence = sentence.Substring(count, sentence.Length - count);
                    var next_sl_index = remaining_sentence.Remove(0, 1).IndexOf("|");
                    /* as the index function returns a zero based index and another 1 as we have remove one character above */
                    var data = remaining_sentence.Substring(0, next_sl_index + 2);
                    count += data.Length;

                    data = data.Replace("|", "");
                    // var colon_index = data.IndexOf(":");
                    var three_data_values = data.Split(":");
                    // var item =
                    //     new Godot.Collections.Dictionary<string, string>() 
                    //         {
                    //             { "name", data.Substring(0, colon_index) },
                    //             { "message", data.Substring(colon_index+1, data.Length - colon_index-1)} 
                    //         };
                    // items.Add(item);
                    var item =
                        new Godot.Collections.Dictionary<string, string>() 
                            {
                                { "name", three_data_values[0] },
                                { "message", three_data_values[1]},
                                {"is_completely_restricted",three_data_values[2]} 
                            };

                    items.Add(item);

                }
                else
                {
                    count++;
                }
            }
            else
            {
                break;
            }
        }

        return items;
    }

    // public ArrayList get_dictionary_set_of_field_values(ArrayList dics,string field_name)
    // {
    //     ArrayList field_values = new ArrayList();

    //     foreach (Godot.Collections.Dictionary<string,string> item in dics)
    //     {
    //         field_values.Add(item[field_name]);
    //     }

    //     return  field_values;
    // }
}

