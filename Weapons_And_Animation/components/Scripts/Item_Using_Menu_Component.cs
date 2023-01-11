using Godot;
using System;

public class Item_Using_Menu_Component : Area2D
{
    public string name = null;
    public bool is_map_drop_available = true;
    public Basic_Func basf;

    public string item_name;
    public string thumbnail_url;
    public string rendering_url; // this url of the component in the file system
    public Data_Manager dm;  // this will be given right from render part 

    public bool is_to_add_instantaneouly = false;

    public AnimatedSprite preview_thumbnail = null;

    public override void _Ready()
    {
        basf = new Basic_Func(this);

    }

    public override void _Process(float delta)
    {

    }

    
    /*
    Note

    here in the paramters we have also given the basf still each class haves its basf
    as you may have seen that basf uses some of the node funciton which can be called only after the item enters the scene tree
    so thus as the use_item is called before adding the item in the scene tree thus we are giving basf so that we can perform our basic operations without adding the item in the scene
    */

    // spawn item will be called at the time of adding the item to the spawn
    public virtual void spawn_item(Vector2 spawn_position, Vector2 target_position, Basic_Character parent,Basic_Func basf) { }
    // use_item will be called as the item is selected
    public virtual void use_item(Item_Using_Menu menu,Basic_Func basf) { }
}
