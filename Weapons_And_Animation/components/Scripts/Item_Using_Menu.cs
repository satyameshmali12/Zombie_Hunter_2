using Godot;
using Godot.Collections;
using System;
using System.Collections;

/*

Item can be put at a specific point in the game
You can perfrom normal drop as well as map drop

in normal drop just press the left click on the cursor to put the drone to a specific point in the game
whereas in the normal drop a map will be toggled just simply select your target and confirm the target the item will be automatically putted at the target position

map drop has its own pros and cons
pros:-
    Item can be spawned at desired point
cons:-
    While the item is dropped using the map drop the area around is not check due to which if you spawned any item inside a block it will get spawned over there
    without checking the space around it and for maximum chances it will get destroyed


Normal_Drop
   :- Requrires the drop mouse right from the left click on the screen

Instant_Drop
   :- Does not requires any target position (it has a specific position in the map)
   :- you may notice for a instant drop position is given.This position is given just as per the syntax but if you see the inner working it has no use in the instant drop since it has no use related to the instant_drop

Map_Drop
   :- It requires a target position
   :- But this position is given right from the map
    

*/





/// <summary>
/// this class is relative of that of the shop class
///<para>in this part the data manager of the each component is stored in the Item_Using_Menu_Components</para>
///<para>the Item_Using_Menu_Components contains all the components in it which contains dm in them</para>
///</summary>
public class Item_Using_Menu : Node2D
{

    public Basic_Func basf;
    // public string[] base_urls = new string[1]{"res://Drones/Scenes/"};
    ArrayList render_data = new ArrayList()
        {
            new Dictionary<string,string>(){{"base_url","res://Drones/Scenes/"},{"data_field_url","data/data_fields/drone_data_fields.zhd"},{"data_field_identifier","Drone_Name"}},
            new Dictionary<string,string>(){{"base_url","res://Spells/Scenes/"},{"data_field_url","data/data_fields/spell_data_fields.zhd"},{"data_field_identifier","Spell_Name"}},
        };

    ArrayList Item_Using_Menu_Components = new ArrayList();

    ArrayList data = new ArrayList();

    int menu_index, specific_index = 0;
    int per_view_boxes = 3;

    // ArrayList items_scene_o_script = new ArrayList(); // this list will contain the data_manager for each item
    Godot.Collections.Array boxes;
    Button left_button, right_button;

    bool is_changing_button_pressed = false;



    // bool is_map_drop_performed;
    /*
    All related the map drop of the items
    */
    Node2D map_node;
    Map main_map;
    Button confirm_map_drop_button, map_drop_cancel_button;

    Vector2 map_drop_adding_position = Vector2.Zero;
    bool is_map_drop_availaled = false; // when the map is been opened
    bool is_to_perfrom_map_drop = false; // when the target for the map drop is given and the map drop is to be successfully performed

    bool is_to_add_instantaneouly = false;
    Button pressed_button = null;

    bool item_can_be_drop = true;
    string item_in_hand_url = null;

    bool is_to_show_warning = false;


    AcceptDialog warning_box;

    Notification notification;
    // bool is_warning_accepted = false;
    // bool is_warning_box_fetched = false;

    // Timer item_can_be_dropped
    // Button item_cancal_button;

    public override void _Ready()
    {

        basf = new Basic_Func(this);


        basf.global_Variables.visibility_list.Add(this);

        // loading the data

        var loading_dm = new Data_Manager();

        foreach (Dictionary<string, string> dic in render_data)
        {
            var data_field_url = dic["data_field_url"];
            var data_field_identifier = dic["data_field_identifier"];
            loading_dm.reload_data(data_field_url);

            var item_names = loading_dm.get_set_of_field_values(data_field_identifier);
            data.Add(item_names);
        }

        boxes = this.GetNode("Preview_Box").GetChildren();

        left_button = this.GetNode<Button>("Left");
        right_button = this.GetNode<Button>("Right");

        map_node = this.GetNode<Node2D>("Map_Drop");
        main_map = map_node.GetNode<Map>("Map");
        confirm_map_drop_button = map_node.GetNode<Button>("Drop_Button");
        map_drop_cancel_button = map_node.GetNode<Button>("Cancel_Map_Drop");


        basf.add_guitickle_button(this.GetNode<Button>("CurverBg"));

        // warning_box = this.GetNode<AcceptDialog>("Warning_Box");
        // warning_box.Connect("confirmed",this,"Warning_Accepted");
        // warning_box.ShowOnTop = true;
        // warning_box.Visible = true;
        // warning_box.PopupCentered();

        notification = this.GetParent().GetNode<Notification>("Notification");

        basf.global_Variables.menu = this;


        add_view();



    }

    public override void _Process(float delta)
    {
        // item_cancal_button = basf.global_Variables.item_cancel_button;


        if (!basf.global_Variables.is_game_quitted)
        {
            Godot.Collections.Array gui_buttons = this.GetNode<Node2D>("Gui_Buttons").GetChildren();

            foreach (Button item in gui_buttons)
            {
                if (item.Pressed)
                {
                    menu_index = int.Parse(item.Name);
                    specific_index = 0;
                    add_view();
                }
            }





            ArrayList items = data[menu_index] as ArrayList;
            var change = (left_button.Pressed && specific_index > 0) ? -1 : (right_button.Pressed && specific_index + per_view_boxes < items.Count) ? 1 : 0;
            if (left_button.Pressed || right_button.Pressed)
            {
                if (change != 0)
                {
                    if (!is_changing_button_pressed)
                    {
                        specific_index += change;
                        add_view();
                    }
                }
                is_changing_button_pressed = true;
            }
            else
            {
                is_changing_button_pressed = false;
            }

            if (pressed_button == null)
            {
                foreach (Button item in boxes)
                {
                    var index = boxes.IndexOf(item);
                    var use_button = item.GetNode<Button>("Use");
                    var map_drop_button = item.GetNode<Button>("Map_Drop");

                    if (use_button.Pressed || map_drop_button.Pressed)
                    {

                        pressed_button = use_button;

                        var selected_item = (Item_Using_Menu_Component)Item_Using_Menu_Components[index];
                        basf.global_Variables.item_in_hand = selected_item.rendering_url;
                        basf.global_Variables.item_using_menu_comp = selected_item;

                        if (use_button.Pressed)
                        {
                            selected_item.use_item(this, basf);
                        }

                        is_to_add_instantaneouly = selected_item.is_to_add_instantaneouly;

                        if (map_drop_button.Pressed)
                        {
                            pressed_button = map_drop_button;
                            is_map_drop_availaled = true;
                            map_node.Visible = true;
                        }

                        if (selected_item.is_to_show_warning)
                        {
                            notification.pop(selected_item.warning_message);
                        }


                    }

                }
            }
            else if (!pressed_button.Pressed)
            {
                pressed_button = null;
            }



            /* 
            performed either when the notification is denied or when the the notication is opened the item_using_menu is being toggled(closed or open)
            */
            if (notification.is_denied || (!this.Visible && notification.Visible))
            {
                basf.nullify_item_in_hand();
                is_to_perfrom_map_drop = false;
                is_to_add_instantaneouly = false;
                item_can_be_drop = false;
                notification.reset();
                reset_stuffs();
            }

            item_in_hand_url = basf.global_Variables.item_in_hand;


            /*

            all the things just as normal drop, quick drop or instant_drop as well as the map drop is performed below
            the difference in this simple i.e the target position

            */
            bool is_normal_drop = Input.IsActionJustPressed("Mouse_Pressed") && item_in_hand_url != null && pressed_button == null && !basf.global_Variables.is_guiticle_button_pressed && !is_map_drop_availaled;
            

            if ((is_normal_drop || is_to_add_instantaneouly || is_to_perfrom_map_drop) && (!notification.Visible && !notification.is_denied))
            {
                if (item_can_be_drop)
                {
                    add_item_to_the_scene();
                }
            }
            else
            {
                item_can_be_drop = true;
            }


            /*
            operation for cancel and confirming the map drop
            */

            if (is_map_drop_availaled)
            {
                /*
                you may notice that in the below condition many of the things are been repeated
                but just for the sake of readibility it is kept so this
                */
                if (confirm_map_drop_button.Pressed && main_map.is_point_given)
                {
                    is_to_perfrom_map_drop = true;
                    map_drop_adding_position = main_map.adding_position;
                    // map_node.Visible = false;
                    reset_stuffs();
                    // is_map_drop_availaled = false;
                    // main_map.reset();
                }
                else if (map_drop_cancel_button.Pressed)
                {
                    reset_stuffs();
                    // map_node.Visible = false;
                    // is_map_drop_availaled = false;
                    is_to_perfrom_map_drop = false;
                    // main_map.reset();
                    basf.nullify_item_in_hand();
                }
            }

        }

    }


    /// <summary>
    /// It fetches the data right from the given url
    /// <para>It takes the value and give it to the boxes present on the item_using_menu</para>
    /// <para>Simultaneouly it stores all the component so that it can ren rendered</para>
    /// </summary>
    public void add_view()
    {
        Item_Using_Menu_Components.Clear();

        for (int i = 0; i < boxes.Count; i++)
        {
            var box = boxes[i] as Node;

            ArrayList specific_views = data[menu_index] as ArrayList;

            Dictionary<string, string> obj = (Dictionary<string, string>)render_data[menu_index];

            var item_index = specific_index + i;
            if (item_index < specific_views.Count)
            {
                var item_name = specific_views[item_index].ToString();

                var item_url = obj["base_url"].ToString() + item_name + ".tscn";
                // loading the scene
                var item_scene = basf.get_the_packed_scene(item_url).Instance<Item_Using_Menu_Component>();


                GD.Print("right from the item using item adding the scene haha..!!");


                var data_field_url = obj["data_field_url"];
                var dm = new Data_Manager(data_field_url);
                dm.load_data(item_name);
                item_scene.dm = dm;
                item_scene.rendering_url = item_url;
                item_scene.name = item_scene.Name;


                GD.Print(item_scene.Name," ",item_scene.name);

                GD.Print(item_scene.restriction_list.Count);

                foreach (Godot.Collections.Dictionary<string, string> item2 in item_scene.restriction_list)
                {
                    GD.Print(item2["name"]);
                }

                GD.Print("end");


                // getting the item url
                // items_scene_o_script.Add(item_scene);
                Item_Using_Menu_Components.Add(item_scene);


                var ani_name = "Thumbnail";

                var preview_thumbnail = box.GetNode<AnimatedSprite>(ani_name);
                var item_thumbnail = item_scene.GetNode<AnimatedSprite>(ani_name);

                preview_thumbnail.Scale = item_thumbnail.Scale;
                preview_thumbnail.Frames.Clear(ani_name);
                for (int j = 0; j < item_thumbnail.Frames.GetFrameCount(ani_name); j++)
                {
                    preview_thumbnail.Frames.AddFrame(ani_name, item_thumbnail.Frames.GetFrame(ani_name, j));
                }

                preview_thumbnail.Play(ani_name);
                item_scene.preview_thumbnail = preview_thumbnail;

            }

        }
        GD.Print("complete end");
        re_render_view_data();
    }

    /// <summary>
    /// It renders the all the child present in the boxes using the data present in the item_using_component
    ///</summary>
    public void re_render_view_data()
    {
        if (!basf.global_Variables.is_game_quitted)
        {
            foreach (Button item in boxes)
            {
                var index = boxes.IndexOf(item);
                if (index < Item_Using_Menu_Components.Count)
                {
                    item.Visible = true;
                    Item_Using_Menu_Component using_item = Item_Using_Menu_Components[index] as Item_Using_Menu_Component;


                    // using_item.is_to_show_warning = false;
                    var item_data = using_item.dm;
                    var avai_no = item.GetNode<Label>("Available_No");


                    var available_count = Convert.ToInt32(item_data.get_data("Available_Count"));
                    var map_drop_button = item.GetNode<Button>("Map_Drop");
                    var use_button = item.GetNode<Button>("Use");

                    int restriction_level = get_restriction_level(using_item);

                    // GD.Print("Restriction level item_using_menu : ", restriction_level);


                    avai_no.Text = available_count.ToString();

                    use_button.Disabled = available_count <= 0 || restriction_level == 1;
                    use_button.HintTooltip = "No tooltip";

                    // if (restriction_level == 0)
                    // {
                    //     using_item.is_to_show_warning = true;
                    // }
                    using_item.is_to_show_warning = (restriction_level == 0) ? true : false;
                    // get_restriction_level(using_item);


                    // use_button.Text = "hello world";


                    if (using_item == basf.global_Variables.item_using_menu_comp && use_button.Disabled)
                    {
                        basf.nullify_item_in_hand();
                    }

                    map_drop_button.Disabled = use_button.Disabled || !using_item.is_map_drop_available;

                    item.GetNode<Label>("Name").Text = item_data.all_field_values[0].ToString();
                }
                else
                {
                    item.Visible = false;
                }


            }
        }


    }

    /// <summary>To resettle the stuffs such as map while toggling the node</summary>
    public void reset_stuffs()
    {

        main_map.reset();
        map_node.Visible = false;
        is_map_drop_availaled = false;

    }

    public void add_item_to_the_scene()
    {
        // if(item_can_be_drop){

        var adding_position = this.GetGlobalMousePosition();
        var new_item = basf.get_the_packed_scene(item_in_hand_url).Instance<Item_Using_Menu_Component>();
        /*
        You may notice here that first the node is added and then the spawn_function is called 
        this is because all the function such as the getnode words only after the object enters in the scene tree
        thus first we have added it to the scene then and then called the spawn item function
        */

        /*
        giving the data to the item from the item_using_menu_component in the hand (it is been stored in the global variables)
        */
        new_item.dm = basf.global_Variables.item_using_menu_comp.dm;
        new_item.name = basf.global_Variables.item_using_menu_comp.name;
        new_item.parent = basf.global_Variables.character_scene as Basic_Character;

        new_item.add_to_scene(basf);
        // at the time of map drop setting the position as per the map drop

        if (is_to_perfrom_map_drop)
        {
            adding_position = basf.abs_a_vector(map_drop_adding_position) - basf.abs_a_vector(main_map.start_point);
        }

        new_item.spawn_item(adding_position, adding_position, basf.global_Variables.character_scene as Basic_Character, basf);
        var item_dm = basf.global_Variables.item_using_menu_comp.dm;

        /*
            resetling the data 
        */

        var new_available_no = (Convert.ToInt32(item_dm.get_data("Available_Count")) - 1);
        item_dm.set_value("Available_Count", new_available_no.ToString());
        item_dm.save_data();
        item_dm.load_previous_data_again();
        re_render_view_data();

        if (is_to_add_instantaneouly || is_to_perfrom_map_drop)
        {
            basf.nullify_item_in_hand();
            is_to_perfrom_map_drop = false;
        }

        is_to_add_instantaneouly = false;
        item_can_be_drop = false;
    }

    public int get_restriction_level(Item_Using_Menu_Component component)
    {
        int restriction = -1;
        // foreach (Godot.Collections.Dictionary<string, string> item in component.restriction_list)
        // {
        //     GD.Print(item["name"]);
        //     GD.Print(component.Name);
        // }
        // foreach (Item_Using_Menu_Component item2 in basf.global_Variables.item_in_progression)
        // { 
        //     GD.Print(item2.name);
        // }

        // GD.Print(basf.global_Variables.item_in_progression.Count);
        foreach (Godot.Collections.Dictionary<string, string> item in component.restriction_list)
        {
            string name = item["name"];
            // GD.Print("identifer :- ",name);
            foreach (Item_Using_Menu_Component item2 in basf.global_Variables.item_in_progression)
            {
                // GD.Print(name);
                if (item2.name == name)
                {
                    // GD.Print("founded :- ",name);

                    var is_completed_restricted = bool.Parse(item["is_completed_restricted"]);
                    if (is_completed_restricted)
                    {
                        return 1;
                    }
                    else
                    {
                        restriction = 0;
                    }
                }

            }

        }

        return restriction;
    }

}
