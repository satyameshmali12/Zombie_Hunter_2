using Godot;
using System;
using System.Collections;


/// <summary>
/// This object or scene checks the surrounding area,where the user has dropped the item and if there are to many collisions then it neglect the drop
///<para>Before add this the item checker in the scene tree the target item given must have all it's field settled</para>
///<para>dm,parent and the name must be settled before adding it</para>
/// <para>In this context all this field is being settled right from the item_using_menu and from there only we have called add_item_to_scene function too.</para>
///</summary>
public class Area_Checker : Node2D
{
    Vector2 spawn_position;
    Vector2 target_position;
    public Item_Using_Menu_Component item;
    Basic_Func basf;
    Timer adding_timer;
    public override void _Ready()
    {
        basf = new Basic_Func(this);

        // giving item the basf because may the item can do some of its inner configuration which requires basf to be initialized without adding them in the scene tree
        item.basf = basf;
        item.spawn_item(spawn_position,target_position,item.parent);

        if(item.is_to_add_to_character)
        {
            basf.global_Variables.character_scene.AddChild(item);
            save_deduct_item_data();
        }
        else{
            adding_timer = basf.create_timer(.1f,"Collider_Checker");
            adding_timer.Start();
        }
    }

    public override void _Process(float delta)
    {
        
    }

    public void load_fields(Vector2 spawn_position,Vector2 target_position,Item_Using_Menu_Component target_item)
    {
        this.spawn_position = spawn_position;
        this.target_position = target_position;
        this.item = target_item;
    }

    public void save_deduct_item_data()
    {
        var dm = item.dm;
        var new_available_no = (Convert.ToInt32(dm.get_data("Available_Count")) - 1);
        dm.set_value("Available_Count", new_available_no.ToString());
        dm.save_data();
        dm.load_previous_data_again();
        basf.global_Variables.item_Using_Menu.re_render_view_data();
        this.QueueFree();

    }

    public void Collider_Checker()
    {
        adding_timer.Stop();
        
        Godot.Collections.Array collision_rays = this.GetNode<Node2D>("Rays").GetChildren();
        int no_of_collision  = 0;
        foreach (RayCast2D ray in collision_rays)
        {
            if(ray.IsColliding())
            {
                Global_Variables_F_A_T type = ray.GetCollider() as Global_Variables_F_A_T;
                if(type._node_type==_Type_of_.Block)
                {
                    no_of_collision++;
                }
            }
        }

        if(no_of_collision<collision_rays.Count/2)
        {
            basf.global_Variables.level_scene.AddChild(item);
            save_deduct_item_data();
        }
        else{
            basf.global_Variables.notification.pop("Hey can't it add in the walls!...");
            this.QueueFree();
        }        

    }
}
