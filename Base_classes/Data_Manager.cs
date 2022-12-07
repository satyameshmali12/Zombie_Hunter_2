#region Description
// DM :- Data_Manager
// this file handles the data over the entire game
// this file needs the file path which contain the respective field of a data and the url of that data
// DM contains the some function which make the data saving process easier
// Dm throws File_not_found,NO_SUCH_FIELD_FOUNDED_IN_Data exceptions this error are thrown manully from the Dm
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using Godot;
using System.IO;
using Godot;
using System.Collections;


[Serializable]
public class No_Such_Field_Founded_In_Data : Exception
{
    public No_Such_Field_Founded_In_Data() { }
    public No_Such_Field_Founded_In_Data(string message)
        : base(message) { }

}

public class Data_Manager
{
    public int data_start_index; // In entire data finding the start index of a specific level with the help of the level name

    // public ArrayList level_data;
    public string[] data;

    public ArrayList all_field_names;
    // ArrayList reahing_increment;
    public ArrayList all_field_values;

    string data_path = "data//data_fields/level_data_fields.zhd";

    public Data_Manager(string data_path = "data//data_fields/level_data_fields.zhd")
    {
        // var all_the_values = System.IO.File.ReadAllLines("data//data_fields/bomb_data_fields.zhd");
        string[] all_the_values = System.IO.File.ReadAllLines(data_path);

        this.data_path = all_the_values[0].ToString().Trim();
        // get_all_the_data_fields(all_the_values);

        all_field_names = get_all_the_data_fields(all_the_values);
        all_field_values = new ArrayList();


        read_data();

        // testing


    }
    public void read_data(){
        // laoding the data 
        data = System.IO.File.ReadAllLines(this.data_path);
    }

    public void load_data(string identifier)
    {
        all_field_values.Clear();
        for (var i = 0; i < data.Length; i++)
        {
            if (data[i].Trim().TrimEnd() == identifier)
            {
                data_start_index = i;
                break;
            }
            else
            {
                data_start_index = -1;
            }
        }

        if (data_start_index == -1)
        {
            throw new System.Exception("Hey No Such Level Founded Have a look to you data as well as directory..!!");
        }


        // GD.Print("start index ", data_start_index);



        for (var i = 0; i < all_field_names.Count; i++)
        {
            var index = i + data_start_index;
            all_field_values.Add(data[index]);
        }

    }
    // this function only for the level data
    public bool is_level_unlocked(string current_level_name)
    {
        var is_unlocked = false;
        for (var i = 0; i < data.Length; i++)
        {
            if (data[i] == current_level_name)
            {
                is_unlocked = Convert.ToBoolean(data[i + get_incre("Is_Level_Unlocked")]);
                // remove this below line later on
                // is_unlocked = Convert.ToBoolean(get_data("Is_Level_Unlocked"));
                break;
            }
        }
        return is_unlocked;
    }

    // call this function only when the level is over as once this function is called will change the entrir data
    // it returns the name of the level of which it save's the data
    public string unlock_next_level(string current_level_name)
    {
        save_data();
        var new_level = current_level_name;
        var present_level = Convert.ToInt32(current_level_name.Remove(0, "Level".Length).TrimStart().TrimEnd());
        var total_number_of_levels = System.IO.Directory.GetFiles("C:\\Users\\hp\\OneDrive\\Desktop\\Zombie Hunter 2\\Levels\\Scenes").Length;

        if (present_level < total_number_of_levels)
        {
            new_level = $"Level{present_level + 1}";
        }

        load_data(new_level);

        // unlocking the next 
        // if there is no further more level then the similar leve is been unlocked again in othe words in such case there is no change in the data
        data[data_start_index + get_incre("Is_Level_Unlocked")] = "true";
        save_data(); // saving the data once again

        return new_level;
    }

    public void save_data()
    {
        System.IO.File.WriteAllLines(data_path, data);
    }

    public string get_data(string field_name)
    {
        return all_field_values[all_field_names.IndexOf(field_name)].ToString();
    }

    public int get_incre(string field_name)
    {
        return all_field_names.IndexOf(field_name);
    }

    public bool set_value(string field_name, string value)
    {
        if (!all_field_names.Contains(field_name))
        {
            throw new No_Such_Field_Founded_In_Data("No such data field exits");
        }
        var incre = get_incre(field_name);
        data[data_start_index + incre] = value;
        return true;
    }

    public ArrayList get_all_the_data_fields(string[] data)
    {
        var selected_data_fields = new ArrayList();

        for (var i = 0; i < data.Length; i++)
        {
            var value = data[i].Trim();
            if (i != 0 && value.Length != 0)
            {
                selected_data_fields.Add(value);
            }
        }

        // foreach (var item in selected_data_fields)
        // {
        //     GD.Print(item);
        // }

        return selected_data_fields;

    }

}