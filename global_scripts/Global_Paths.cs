using Godot;
using System;

public class Global_Paths:Node
{

    public string main_directory_path = System.IO.Directory.GetCurrentDirectory();
    public string Main_Game_Scene_Path = "res://Views/Scenes/Main_Game_Scene.tscn";
    public string Home_Path = "res://Views/Scenes/Game_Over_View.tscn";
    public string Shop_Path = "res://Views/Scenes/Shop.tscn";
    public string Game_Over_Scene_Path = "res://Views/Scenes/Game_Over_View.tscn";
    public string Level_View_Path = "res://Views/Scenes/Level_View.tscn";

    public string Weapons_Base_Url = "res://Weapons_And_Animation/scenes/Weapons/";
    public string basic_throwable_weapons_data_fields_Url = "data/data_fields/basic_throwable_weapons_data_fields.zhd";
    public string Heros_Data_Field_Url = "data/data_fields/heros_data_field.zhd";
    public string Level_Scenes_Directory_Url = "Levels\\Scenes";
    public Global_Paths(){
        
    }
}