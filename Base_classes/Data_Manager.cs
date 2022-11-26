using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using Godot;
using System.IO;
using Godot;

public class Data_Manager
{
    public int data_start_index; // In entire data finding the start index of a specific level with the help of the level name

    public string[] level_data;
    public int total_zombie; // the total number of zombie that will be dropped throught the entire game
    public int max_zombie_per_screen; // the total_nubmer of zombie that can stay on in the game
    public int difficulty_level;

    // to reach to the data of the level
    public int total_zombie_incre,max_zombie_per_screen_incre,difficulty_level_incre,unlocked_incre;

    string data_path = "data//level_data.zhd";
    public Data_Manager(){
        // laoding the data 
        level_data = System.IO.File.ReadAllLines(data_path);
        total_zombie_incre = 1;
        max_zombie_per_screen_incre = 2;
        difficulty_level_incre = 3;
        unlocked_incre = 5;

    }   

    public void load_data(string level_name){
        
        for (var i = 0; i < level_data.Length; i++)
        {
            if (level_data[i].Trim().TrimEnd() == level_name)
            {
                data_start_index = i;
                break;
            }
            else{
                data_start_index = -1;
            }
        }

        if(data_start_index == -1){
            GD.Print(level_name);
            throw new System.Exception("Hey No Such Level Founded Have a look to you data as well as directory..!!");
        }

        total_zombie = Convert.ToInt32(level_data[data_start_index + total_zombie_incre]);

        max_zombie_per_screen = Convert.ToInt32(level_data[data_start_index + max_zombie_per_screen_incre]);

        difficulty_level = Convert.ToInt32(level_data[data_start_index + difficulty_level_incre]);

        GD.Print("data",total_zombie," ",max_zombie_per_screen," ",difficulty_level);
        
    }

    public bool is_level_unlocked(string level_name){
        var is_unlocked = false;
        for (var i = 0; i < level_data.Length; i++)
        {
            if(level_data[i]==level_name){
                is_unlocked = Convert.ToBoolean(level_data[i+unlocked_incre]);
                break;
            }
        }
        return is_unlocked;
    }
}