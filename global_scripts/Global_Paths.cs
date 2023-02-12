using Godot;
using System;

public class Global_Paths:Node
{

    public string main_directory_path = System.IO.Directory.GetCurrentDirectory();
    public string Game_Gui_Path = "res://Weapons_And_Animation/components/scenes/Game_Gui.tscn";
    public string base_view_path = "res://Views/Scenes/";
    public string Main_Game_Scene_Path = "res://Views/Scenes/Main_Game_Scene.tscn";
    public string Home_Path = "res://Views/Scenes/Game_Over_View.tscn";
    public string Shop_Path = "res://Views/Scenes/Shop.tscn";
    public string Game_Over_Scene_Path = "res://Views/Scenes/Game_Over_View.tscn";
    public string Level_View_Path = "res://Views/Scenes/Level_View.tscn";

    
    public string Zombie_Scene_Base_Url = "res://Characters/Characters_Scene/Zombie/";

    public string Weapons_Base_Url = "res://Weapons_And_Animation/scenes/Weapons/";
    public string basic_throwable_weapons_data_fields_Url = "data/data_fields/basic_throwable_weapons_data_fields.zhd";
    public string Heros_Data_Field_Url = "data/data_fields/heros_data_field.zhd";
    public string Zombie_Data_Field_Url = "data/data_fields/zombie_data_fields.zhd";
    public string User_Data_Field_Url = "data/data_fields/user_data_fields.zhd";
    public string Level_Scenes_Directory_Url = "Levels\\Scenes";
    public string Character_Scenes_Base_Url = "res://Characters/Characters_Scene";

    public string Drone_Ini_Faller_Url = "res://Bomb's/Scenes/Drone_Ini_Faller.tscn";

    public string Shooter_DD_Bullet_Url = "res://Weapons_And_Animation/scenes/Weapons/Shooter_DD_Bullet.tscn";
    public Global_Paths(){
        
    }
}