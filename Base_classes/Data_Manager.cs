using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using Godot;
using System.IO;
using Godot;
using System.Collections;

public class Data_Manager
{
    public int data_start_index; // In entire data finding the start index of a specific level with the help of the level name

    public string[] level_data;
    // public ArrayList level_data;
    public int total_zombie; // the total number of zombie that will be dropped throught the entire game
    public int max_zombie_per_screen; // the total_nubmer of zombie that can stay on in the game
    public int difficulty_level;

    // to reach to the data of the level
    public int total_zombie_incre,max_zombie_per_screen_incre,difficulty_level_incre,unlocked_incre;

    string data_path = "data//level_data.zhd";
    public Data_Manager(string data_path = "data//level_data.zhd"){
        // laoding the data 
        this.data_path = data_path;
        level_data = System.IO.File.ReadAllLines(data_path);
        total_zombie_incre = 1;
        max_zombie_per_screen_incre = 2;
        difficulty_level_incre = 3;
        unlocked_incre = 5;

    }   

    public void load_data(string current_level_name){
        
        for (var i = 0; i < level_data.Length; i++)
        {
            if (level_data[i].Trim().TrimEnd() == current_level_name)
            {
                data_start_index = i;
                break;
            }
            else{
                data_start_index = -1;
            }
        }

        if(data_start_index == -1){
            // GD.Print(current_level_name);
            throw new System.Exception("Hey No Such Level Founded Have a look to you data as well as directory..!!");
        }

        total_zombie = Convert.ToInt32(level_data[data_start_index + total_zombie_incre]);

        max_zombie_per_screen = Convert.ToInt32(level_data[data_start_index + max_zombie_per_screen_incre]);

        difficulty_level = Convert.ToInt32(level_data[data_start_index + difficulty_level_incre]);

        // GD.Print("data",total_zombie," ",max_zombie_per_screen," ",difficulty_level);
        
    }

    public bool is_level_unlocked(string current_level_name){
        var is_unlocked = false;
        for (var i = 0; i < level_data.Length; i++)
        {
            if(level_data[i]==current_level_name){
                is_unlocked = Convert.ToBoolean(level_data[i+unlocked_incre]);
                break;
            }
        }
        return is_unlocked;
    }

    // call this function only when the level is over as once this function is called will change the entrir data
    // it returns the name of the level of which it save's the data
    public string unlock_next_level(string current_level_name){
        save_data();
        var new_level = current_level_name;
        var present_level =  Convert.ToInt32(current_level_name.Remove(0,"Level".Length).TrimStart().TrimEnd());
        var total_number_of_levels = System.IO.Directory.GetFiles("C:\\Users\\hp\\OneDrive\\Desktop\\Zombie Hunter 2\\Levels\\Scenes").Length;

        if(present_level<total_number_of_levels){
            new_level =  $"Level{present_level+1}";
        }

        load_data(new_level);
        // unlocking the next 
        // if there is no further more level then the similar leve is been unlocked again in othe words in such case there is no change in the data
        level_data[data_start_index+unlocked_incre] = "true"; 
        save_data(); // saving the data once again

        return new_level;
    }

    public void save_data(){
        System.IO.File.WriteAllLines("data/level_data.zhd", level_data);
    }

    
}