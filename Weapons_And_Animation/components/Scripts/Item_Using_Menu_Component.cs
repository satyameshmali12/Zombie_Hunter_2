using Godot;
using System;
using Godot.Collections;
using System.Collections;


/*
Parent need to be given from respective child classes else it will throw object not set to an instance error
*/
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


    public Basic_Character parent = null;

    public Vector2 target_position = Vector2.Zero;

    public AnimatedSprite preview_thumbnail = null;

    public ArrayList restriction_list = new ArrayList();

    public bool is_to_show_warning = false;

    public string warning_message = "";

    public bool is_to_display_position = false;

    public bool is_parent_leaved_scene_tree = false;
    public bool is_to_add_to_character = false;


    public Item_Using_Menu_Component()
    {
        // GD.Print("name right from the item_usignmenu_component : ",name);
    }


    public override void _Ready()
    {
        basf = new Basic_Func(this);
        basf.global_Variables.item_in_progression.Add(this);
        this.Connect("tree_exiting", this, "Removal");

        parent.Connect("tree_exiting", this, "Parent_Leaved_Scene_Tree");

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
    /// <summary>To give the initial values to the spells</summary>
    public virtual void spawn_item(Vector2 spawn_position, Vector2 target_position, Basic_Character parent)
    {
        this.Position = spawn_position;
        this.target_position = target_position;
        this.parent = parent;
    }
    
    // use_item will be called as the item is selected
    /// <summary>It will be called as the item is selected i.e used</summary>
    public virtual void use_item(Item_Using_Menu menu, Basic_Func basf) { }

    /// <summary>To add the items in their respective node</summary>
    public virtual void add_to_scene(Basic_Func basf, Vector2 spawn_position, Vector2 target_position)
    {
        Area_Checker area_Checker = basf.get_the_packed_scene("res://Weapons_And_Animation/components/scenes/Area_Checker.tscn").Instance<Area_Checker>();
        area_Checker.load_fields(spawn_position, target_position, this);
        area_Checker.GlobalPosition = spawn_position;
        basf.global_Variables.level_scene.AddChild(area_Checker);
    }

    /// <summary>Use when need to add the item instantly as used</summary>
    public virtual void add_instant_setting()
    {
        is_to_add_instantaneouly = true;
        is_map_drop_available = false;

    }

    public Dictionary<string, string> create_restriction_dic(string name, bool is_completely_restricted, string message)
    {
        return new Dictionary<string, string>() { { "name", name }, { "is_completely_restricted", is_completely_restricted.ToString() }, { "message", message } };
    }

    public virtual void Clear()
    {
        // after the the clear function run only 5 second the item can stay on the 
        basf.create_timer(5, "Removal").Start();
    }

    /* This method will be used by the item remover to remove the item from the node tree */
    // when the item leaves the scene tree
    /// <summary>
    //  Will call when the item will leave the scene tree
    /// This method is connected to the tree_exiting signal
    /// </summary>
    public virtual void Removal()
    {
        if (basf.global_Variables.item_in_progression.Contains(this))
        {
            basf.global_Variables.item_in_progression.Remove(this);
            basf.global_Variables.menu.re_render_view_data();
        }
    }

    /// <summary>When parent of the item leaves the scene then this func is called</summary>
    public virtual void Parent_Leaved_Scene_Tree()
    { }


    /// <summary>
    /// This will run properly only when all the fields target position,adding_position and parent are being initialised
    ///<para>In this part we have given all this data right from the item_using_menu_componenet</para>
    ///</summary>

    // public virtual void add_area_checker()
    // {

    //     // area_Checker.sett

    // }
}
