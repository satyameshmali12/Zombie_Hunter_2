using Godot;
using System;
using System.Collections;
using Godot.Collections;

public class Dark_King : Powerful_Zombie
{

    Timer zombie_spawing_timing;
    int available_power = 100;
    [Export]
    int per_power_increment = 10;
    [Export]
    int max_power = 200;
    [Export]
    Timer available_power_incrementor;


    ArrayList child_zombies =
        new ArrayList(){
            new Dictionary<string,string>(){{"Name","Male_Zombie"},{"Req_Power","20"}},
            new Dictionary<string,string>(){{"Name","Female_Zombie"},{"Req_Power","40"}},
            new Dictionary<string,string>(){{"Name","Male_Zombie_2"},{"Req_Power","60"}},
            new Dictionary<string,string>(){{"Name","Female_Zombie_2"},{"Req_Power","100"}}
        };
    int zombie_spawn_range = 2;
    int one_time_spawn_count = 3;

    public override void _Ready()
    {
        base._Ready();

        character_name = "Dark_King";
        speed_x += 200;

        available_moves = new ArrayList() { "attack", "death", "falling_down", "hurt", "idle", "idle_blinking", "jump_attack", "jump_loop", "jump_start", "kick", "run", "run_attack", "run_throwing", "sliding", "throwing", "walk" };
        // available_moves = new ArrayList(){"attack","walk","death","idle","jump","idle_blinking","falling_down"};
        available_moves_consumption = new int[16] { 0, 0, 0, 0, 0, 0, 1, 5, 5, 4, 4, 3, 5, 0, 5, 0 };
        available_moves_damage = new int[16] { 2, 0, 0, 0, 0, 0, 3, 0, 0, 2, 0, 3, 2, 0, 0, 0 };

        attack_move_names = new ArrayList() { "attack", "run_attack", "kick", "jump_attack" };

        jump_intensity = 5000;

        zombie_spawing_timing = basf.create_timer(20, "Add_Kings_Child_Zombie");
        zombie_spawing_timing.Start();

        available_power_incrementor = basf.create_timer(3, "Increment_Power");
        available_power_incrementor.Start();
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        move();
    }

    void Add_Kings_Child_Zombie()
    {

        var rand_num = (int)GD.RandRange(0, 2);
        bool is_to_spawn_zombie = (power_available >= 100 || rand_num == 1);

        if (is_to_spawn_zombie)
        {

            for (int i = 0; i < one_time_spawn_count; i++)
            {

                Dictionary<string, string> selected_zombie = null;
                ArrayList child_zombie_copy = child_zombies.Clone() as ArrayList;

                for (int j = 0; j < child_zombie_copy.Count; j++)
                {
                    GD.Print("Count from the Dark_King .cs ", " ",child_zombie_copy.Count);
                    int random_zombie = (int)GD.RandRange(0, child_zombie_copy.Count);
                    GD.Print(random_zombie);
                    Dictionary<string, string> zombie_data = child_zombie_copy[random_zombie] as Dictionary<string, string>;

                    int required_power = int.Parse(zombie_data["Req_Power"]);

                    if (available_power - required_power >= 0)
                    {
                        selected_zombie = zombie_data;
                        break;
                    }
                    else
                    {
                        child_zombie_copy.Remove(zombie_data);
                    }
                }
                if (selected_zombie == null)
                {
                    break;
                }
                else
                {
                    string new_zombie_scene_url = basf.global_paths.Zombie_Scene_Base_Url + selected_zombie["Name"] + ".tscn";
                    Basic_Zombie new_zombie_scene = basf.get_the_packed_scene(new_zombie_scene_url).Instance<Basic_Zombie>();
                    new_zombie_scene.is_associated_with_main_level = false;
                    int x_change = ((int)GD.RandRange(0, 2) == 0) ? -100 : 100; // to place either in left or right
                    new_zombie_scene.GlobalPosition = this.GlobalPosition - new Vector2(x_change, 100);
                    basf.global_Variables.level_scene.AddChild(new_zombie_scene);
                }


            }
        }
    }

    void Increment_Power()
    {
        power_available += (power_available + per_power_increment <= max_power) ? per_power_increment : max_power - power_available;
    }

}
