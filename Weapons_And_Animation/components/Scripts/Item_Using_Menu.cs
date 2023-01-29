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
    without checking the space around it and for maximum chances it will get destroyed if spawned inside the blocks

Normal_Drop
   :- Requrires the drop mouse right from the left click on the screen
Instant_Drop
   :- Does not requires any target position (it has a specific position in the map)
   :- you may notice for a instant drop position is given.This position is given just as per the syntax but if you see the inner working it has no use in the instant drop since it has no use related to the instant_drop
Map_Drop
   :- It requires a target position
   :- But this position is given right from the map

Dropping Message
    :- While adding any spell or item there are chances when you may add multiple spell which may don't have any effect 
        thus we have introduced message availing for the user which will help the user to use the item carefully
        you also can disable the warning right from the settings page
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

    bool is_urgent_add = false;
    bool is_td_ext_settting = false;
    Vector2 last_ms_click_pos = Vector2.Zero;
    bool is_last_ms_click_pos_given = false;

    bool is_selected_from_use_button = false;


    bool can_give_warning_during_game = false;

    string restriction_message = null;



    public override void _Ready()
    {

        basf = new Basic_Func(this);

        can_give_warning_during_game = bool.Parse(basf.user_data.get_data("Can_Given_Warning_During_Match"));


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

        notification = this.GetParent().GetNode<Notification>("Notification");

        basf.global_Variables.menu = this;

        basf.add_guitickle_button(left_button, right_button);

        basf.global_Variables.item_Using_Menu = this;


        add_view();



    }

    public override void _Process(float delta)
    {
        // item_cancal_button = basf.global_Variables.item_cancel_button;
        var item_in_hand = basf.global_Variables.item_using_menu_comp;
        if (item_in_hand != null && !is_selected_from_use_button && !basf.global_Variables.is_guiticle_button_pressed && can_give_warning_during_game)
        {
            var restriction_level = get_restriction_level(item_in_hand);

            if (restriction_level == 0 && Input.IsActionJustPressed("Mouse_Pressed"))
            {
                if (!is_last_ms_click_pos_given)
                {
                    last_ms_click_pos = this.GetGlobalMousePosition();
                    is_last_ms_click_pos_given = true;
                }
                notification.pop(item_in_hand.warning_message);
                is_td_ext_settting = true;
            }
            else if (restriction_level == 1)
            {
                entire_reset();
            }
        }


        if (!basf.global_Variables.is_game_quitted)
        {
            Godot.Collections.Array gui_buttons = this.GetNode<Node2D>("Gui_Buttons").GetChildren();

            foreach (Button item in gui_buttons)
            {
                basf.add_guitickle_button(item);
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

                        is_selected_from_use_button = true;
                        if (is_selected_from_use_button)
                        {
                            is_td_ext_settting = false;
                        }

                        // is_td_ext_settting = false;

                        if (use_button.Pressed)
                        {
                            // is_urgent_add = false;
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
            // || (!this.Visible && notification.Visible)
            if (notification.is_denied)
            {
                is_last_ms_click_pos_given = false;
                last_ms_click_pos = Vector2.Zero;
                is_urgent_add = false;
                is_td_ext_settting = false;
                entire_reset();

            }

            // when the user it given warning during the selective drop
            if (notification.is_accepted)
            {
                if (is_td_ext_settting)
                {
                    is_td_ext_settting = false;
                    is_urgent_add = true;
                    is_last_ms_click_pos_given = false;
                    notification.reset();
                }
                else if (is_selected_from_use_button)
                {
                    is_selected_from_use_button = false;
                    notification.reset();
                    is_last_ms_click_pos_given = false;
                }
            }


            item_in_hand_url = basf.global_Variables.item_in_hand;


            /*

            all the things just as normal drop, quick drop or instant_drop as well as the map drop is performed below
            the difference in this simple i.e the target position

            */
            bool is_normal_drop = Input.IsActionJustPressed("Mouse_Pressed") && item_in_hand_url != null && pressed_button == null && !basf.global_Variables.is_guiticle_button_pressed && !is_map_drop_availaled;


            if (((is_normal_drop || is_to_add_instantaneouly || is_to_perfrom_map_drop) && (!notification.Visible && !notification.is_denied)) || (is_urgent_add && item_in_hand != null))
            {
                if (item_can_be_drop)
                {
                    add_item_to_the_scene();
                    is_urgent_add = false;
                    is_selected_from_use_button = false;
                    is_last_ms_click_pos_given = false;
                    is_td_ext_settting = false;
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
                    reset_stuffs();
                }
                else if (map_drop_cancel_button.Pressed)
                {
                    reset_stuffs();
                    is_to_perfrom_map_drop = false;
                    basf.nullify_item_in_hand();
                }
            }

        }

    }


    /// <summary>
    /// It fetches the data right from the given url
    /// <para>It takes the value and give it to the boxes present on the item_using_menu</para>
    /// <para>Simultaneouly it stores all the component so that it's value can be used while dropping the item in the game</para>
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

                var data_field_url = obj["data_field_url"];
                var dm = new Data_Manager(data_field_url);
                dm.load_data(item_name);
                item_scene.dm = dm;
                item_scene.rendering_url = item_url;
                item_scene.name = item_scene.Name;

                // getting the item url
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
                    using_item.restriction_list = basf.get_split_data_in_wdm(item_data.get_data("Warning_List"));


                    var available_count = Convert.ToInt32(item_data.get_data("Available_Count"));
                    var map_drop_button = item.GetNode<Button>("Map_Drop");
                    var use_button = item.GetNode<Button>("Use");

                    int restriction_level = get_restriction_level(using_item);

                    avai_no.Text = available_count.ToString();

                    if(can_give_warning_during_game)
                    {
                        use_button.Disabled = available_count <= 0 || restriction_level == 1;
                        using_item.is_to_show_warning = (restriction_level == 0) ? true : false;
                    }


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



    /// <summary>For adding the item on the scene as per the item in hand url</summary>
    public void add_item_to_the_scene()
    {
        // if(item_can_be_drop){

        var adding_position = this.GetGlobalMousePosition();
        if (is_urgent_add)
        {
            adding_position = last_ms_click_pos;
        }
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

        // at the time of map drop setting the position as per the map drop

        if (is_to_perfrom_map_drop)
        {
            adding_position = basf.abs_a_vector(map_drop_adding_position) - basf.abs_a_vector(main_map.start_point);
        }

        new_item.add_to_scene(basf,adding_position,adding_position);


        if (is_to_add_instantaneouly || is_to_perfrom_map_drop)
        {
            basf.nullify_item_in_hand();
            is_to_perfrom_map_drop = false;
        }

        is_to_add_instantaneouly = false;
        item_can_be_drop = false;
    }


    /// <summary>To get the restricition level of the item on the shop
    /// <para>Three levels -1,0,1</para>
    /// <para> -1 :- no restriction</para>
    /// <para> 0 :- Show warning </para>
    /// <para> 1 :- completely restricted </para> 
    ///</summary>
    public int get_restriction_level(Item_Using_Menu_Component component)
    {
        int restriction = -1;

        foreach (Godot.Collections.Dictionary<string, string> item in component.restriction_list)
        {
            string name = item["name"];
            foreach (Item_Using_Menu_Component item2 in basf.global_Variables.item_in_progression)
            {
                if (item2.name == name)
                {
                    var is_completely_restricted = bool.Parse(item["is_completely_restricted"]);
                    if (is_completely_restricted)
                    {
                        return 1;
                    }
                    else
                    {
                        restriction = 0;

                        var is_universal_message_their = bool.Parse(component.dm.get_data("Is_Universal_Warning_Given"));

                        /*
                            setting the item warning which is founded first 
                            or 
                            setting the universal message if given 
                        */
                        component.warning_message = (is_universal_message_their)?component.dm.get_data("Universal_Warning"):item["message"];
                    }
                }

            }

        }
        return restriction;
    }


    /// <summary>To reset all the things related to the droping of the item</summary>
    public void entire_reset()
    {
        basf.nullify_item_in_hand();
        is_to_perfrom_map_drop = false;
        is_to_add_instantaneouly = false;
        item_can_be_drop = false;
        notification.reset();
        reset_stuffs();
    }

    
    /// <summary>To resettle the stuffs such as map while toggling the node(for map)</summary>
    public void reset_stuffs()
    {

        main_map.reset();
        map_node.Visible = false;
        is_map_drop_availaled = false;

    }

}
