using Godot;
using System;
using System.Collections;

// Note 
// From the shop the data is been sended to the it's child component 
// all the update and buy logic is writtened in the component's script of the shop
// and whenever the data is changed buy updating or buying we just simply call reload_scene_features

// in the basf.dm the user data is been stored whereas shop data is stored in the shop_data_manager

public class Shop : Basic_View
{

    public int menu_selection = 0; // the selection of the type of view in the shop
    public int view_index = 0; // the index of the character that user wanted to see

    ArrayList urls = new ArrayList() { };

    // all the types of views available
    public int player_view_index = 0;
    public int zombie_view_index = 1;
    public int bomb_view_index = 2;
    public int weapons_view_index = 3;

    // this array is method with respect to the above index🔼
    string[] specific_data_fields_urls =
            new string[6] { "data//data_fields/heros_data_field.zhd",
                "data//data_fields/zombie_data_fields.zhd",
                "data//data_fields/bomb_data_fields.zhd",
                "data//data_fields/basic_throwable_weapons_data_fields.zhd",
                "data//data_fields/drone_data_fields.zhd",
                "data//data_fields/spell_data_fields.zhd"
              };
    string[] names = new string[6] { "Character_Name", "Zombie_Name", "Bomb_Name", "Weapon_Name", "Drone_Name","Spell_Name" };

    // making all the condition
    // all this arrays are made with respect to the abobe name in a definite or corresponding indexing order
    public bool[] can_be_buyed_in_numbers = new bool[6] { false, false, true, false, true, true };
    public bool[] can_be_unlocked = new bool[6] { true, false, true, false, false, false };
    public bool[] can_be_equiped = new bool[6] { true, false, false, false, false, false };
    public bool[] can_be_updated = new bool[6] { true, false, true, true, true, true };

    string[] base_urls = new string[6] { "res://Characters/Characters_Scene/Player/", "res://Characters/Characters_Scene/Zombie/", "res://Bomb's/Scenes/", "res://Weapons_And_Animation/scenes/Weapons/", "res://Drones/Scenes/", "res://Spells/Scenes/" };
    bool is_button_pressed, is_button_pressed_2 = false;

    // getting some buttons on the screen
    public TextureButton left_button, right_button;

    public string specific_name = null;

    public Data_Manager shop_data_manager;
    public Data_Manager user_data_manager;
    public bool is_shop_data_loaded, is_user_data_loaded = false;



    AnimatedSprite current_animation;
    bool is_current_animation_initialized;

    public Node render_char;
    Desc description_box;

    ArrayList changing_button_nav_names = new ArrayList() { "Characters", "Zombie", "Bomb", "Weapons", "Drones" ,"Spells"};
    // per render is three button but since due to zero base index it setted to 2
    Button left_change_button, right_change_button;
    int number_of_button_per_view = 3;
    int render_num = 0;
    bool is_r_changing_button_pressed = false;
    bool is_first_render = true;
    public override void _Ready()
    {
        base._Ready();


        basf = new Basic_Func(this);
        var dm = basf.dm;

        // making the rendering list of the items to be displayed on the shop
        for (var i = 0; i < names.Length; i++)
        {
            dm = new Data_Manager(specific_data_fields_urls[i]);
            var data = dm.get_set_of_field_values(names[i]);
            var is_to_add = dm.get_set_of_field_values("Is_To_Display_On_Shop");
            var removal_index = 0;
            for (int j = 0; j < data.Count; j++)
            {
                var is_to_show_on_shop = Convert.ToBoolean(is_to_add[j - removal_index].ToString().Trim());
                if (!is_to_show_on_shop)
                {
                    data.Remove(data[j]);
                    removal_index++;
                }
            }

            urls.Add(data);
        }

        // removing the item


        menu_selection = Convert.ToInt32(basf.user_data.get_data("Menu_Selection"));
        view_index = Convert.ToInt32(basf.user_data.get_data("Specific_Selection"));

        var mx_rend_num = changing_button_nav_names.Count - number_of_button_per_view;
        render_num = (menu_selection < mx_rend_num) ? menu_selection : mx_rend_num;

        left_button = this.GetNode<TextureButton>("Left_Button");
        right_button = this.GetNode<TextureButton>("Right_Button");

        is_current_animation_initialized = false;

        description_box = this.GetNode<Desc>("Desc");
        description_box.load_description_box();

        var l_r_menu = this.GetNode<Node2D>("L_R_Menu_Button");
        left_change_button = l_r_menu.GetNode<Button>("Left");
        right_change_button = l_r_menu.GetNode<Button>("Right");

        add_the_view();
    }

    public override void _Process(float delta)
    {

        // getting the option_button on screen and thereby setting the animation available in the node
        var option_button = this.GetNode<OptionButton>("OptionButton");

        current_animation.Animation = option_button.GetItemText(option_button.GetSelectedId());
        current_animation.Playing = this.GetNode<CheckButton>("Animation_Playing").Pressed;


        // showing the available money on the screen
        this.GetNode<Label>("Money").Text = $"Money : - {user_data_manager.get_data("Money")}";


        left_button.Visible = view_index!=0;
        var items = urls[menu_selection] as ArrayList;
        right_button.Visible = view_index+1<items.Count;

        // make the character and zombie to be separated for the shop render so that concern been seperated
        // Caution🔺
        base._Process(delta);
        if (left_button.Pressed || right_button.Pressed || Input.IsActionJustPressed("move_left") || Input.IsActionJustPressed("move_right"))
        {
            if (!is_button_pressed)
            {
                ArrayList arr = (ArrayList)urls[menu_selection];
                is_button_pressed = true;
                var increment = (left_button.Pressed || Input.IsActionJustPressed("move_left")) ? -1 : 1;

                var new_value = view_index + increment;
                // GD.Print(new_value);
                if (new_value > -1 && new_value < arr.Count)
                {
                    view_index = new_value;
                    user_data_manager.set_value("Specific_Selection", $"{view_index}");
                    user_data_manager.save_data();
                    add_the_view();
                }
            }
        }
        else
        {
            is_button_pressed = false;
        }


        // interating thought the buttons and changing the menu_selection simultaneouly
        var containers = this.GetNode<HBoxContainer>("Gui_Buttons_Container");
        foreach (Container item in containers.GetChildren())
        {
            Button but = (Button)item.GetChildren()[0];
            if (but.Pressed)
            {
                if (!is_button_pressed)
                {
                    view_index = 0;
                    menu_selection = Convert.ToInt32(but.Name);
                    user_data_manager.set_value("Menu_Selection", $"{menu_selection}");
                    user_data_manager.save_data();
                    add_the_view();
                }
                is_button_pressed_2 = true;
            }
            else
            {
                is_button_pressed_2 = false;
            }
        }


        var change = (left_change_button.Pressed) ? -1 : (right_change_button.Pressed) ? 1 : 0;

        if (change != 0 || is_first_render)
        {

            if (!is_r_changing_button_pressed)
            {
                // first renaming the box all the box so that the box can be renamed again 
                // as if the same name exist in the node some other characters get added into the node
                foreach (MarginContainer item in containers.GetChildren())
                {
                    item.Name = "no_name";
                }
                var button_change_start = render_num + change;
                render_num = (left_change_button.Pressed && render_num > 0) ? button_change_start : (right_change_button.Pressed && render_num + number_of_button_per_view < changing_button_nav_names.Count) ? button_change_start : render_num;
                for (int i = 0; i < number_of_button_per_view; i++)
                {
                    var con = containers.GetChildren()[i] as Container;
                    Button but1 = con.GetChildren()[0] as Button;

                    con.Name = changing_button_nav_names[render_num + i].ToString();
                    but1.Name = (render_num + i).ToString();
                    but1.Text = $"     {con.Name}     ";
                }
                is_first_render = false;
            }
            is_r_changing_button_pressed = true;
        }
        else
        {
            is_r_changing_button_pressed = false;
        }

    }

    // this function is used to add the view on the screen
    public bool add_the_view()
    {

        description_box.close_desc();

        var loading_label = this.GetNode<Label>("Loading");
        loading_label.Visible = true;
        if (is_current_animation_initialized)
        {
            current_animation.Visible = false;
        }
        ArrayList arr = (ArrayList)urls[menu_selection];
        if (arr.Count > 0)
        {
            // first removing all the childs in other words first removing the old animation
            var adding_area = this.GetNode<Node2D>("Adding_Area");
            foreach (Node item in adding_area.GetChildren())
            {
                item.Free();
            }

            // getting the adding position
            var adding_position = this.GetNode("Adding_Position") as Position2D;

            // getting the url to the desired scene
            ArrayList desired_urls = (ArrayList)urls[menu_selection];

            specific_name = desired_urls[view_index].ToString();
            var specific_base_url = base_urls[menu_selection].ToString();
            reload_scene_features();


            var path = $"{specific_base_url}/{specific_name}.tscn";

            this.GetNode<Label>("Specific_Name").Text = specific_name;
            var value = names[menu_selection];
            var start_index = value.Length - "_name".Length;
            this.GetNode<Label>("Menu_Name").Text = value.Left(start_index);


            // getting the packed scene and instancing it with the basic_player
            PackedScene scene = basf.get_the_packed_scene(path);

            var view = scene.Instance<Node2D>();

            render_char = view;

            #region getting_the_animation the animation of the desired node and displaying it on the screen

            // var new_animation  = new AnimatedSprite();
            // new_animation.Frames = new SpriteFrames();


            //                  or
            // alternative of the above👇

            var new_animation = basf.get_the_packed_scene("res://Weapons_And_Animation/components/scenes/Empty_Animation.tscn").Instance<AnimatedSprite>();
            new_animation.Frames.ClearAll();
            var animation = view.GetNode<AnimatedSprite>("Movements");


            //iterating through the all the aimation thereby and thereby creating the animation and accessing their content        
            foreach (string item in animation.Frames.GetAnimationNames())
            {
                new_animation.Frames.AddAnimation(item);
                new_animation.Frames.SetAnimationSpeed(item, animation.Frames.GetAnimationSpeed(item));
                for (var i = 0; i < animation.Frames.GetFrameCount(item); i++)
                {
                    new_animation.Frames.AddFrame(item, animation.Frames.GetFrame(item, i));
                }
            }

            // some other setting's to the animation before adding them on the screen
            new_animation.Animation = new_animation.Frames.GetAnimationNames()[0];
            new_animation.Position = adding_position.Position;

            new_animation.Scale = animation.Scale;
            new_animation.ZIndex = 0;
            new_animation.Playing = true;


            adding_area.AddChild(new_animation);


            current_animation = new_animation;
            is_current_animation_initialized = true;
            var option_button = this.GetNode<OptionButton>("OptionButton");

            option_button.Clear();


            foreach (string item in current_animation.Frames.GetAnimationNames())
            {
                if (!item.ToLower().Trim().Contains("default"))
                {
                    option_button.AddItem(item.ToString());
                }
            }

            option_button.Visible = (current_animation.Frames.GetAnimationNames().Length-1)>1;

            if (menu_selection != bomb_view_index)
            {

            }

            var desc = shop_data_manager.get_data("Description");
            description_box.reload_text(desc);


        }
        loading_label.Visible = false;
        #endregion
        return true;
    }

    // function to reload some data and other stuff too..
    public void reload_scene_features()
    {

        if (is_shop_data_loaded)
        {
            shop_data_manager.save_data();
        }

        shop_data_manager = new Data_Manager(specific_data_fields_urls[menu_selection].ToString());
        shop_data_manager.load_data(specific_name);
        is_shop_data_loaded = true;

        load_user_data();
    }

    // function to load the user_data
    public void load_user_data()
    {
        if (is_user_data_loaded)
        {
            user_data_manager.save_data();
        }
        user_data_manager = new Data_Manager("data//data_fields/user_data_fields.zhd");
        user_data_manager.load_data("Aj");
        is_user_data_loaded = true;
    }

}
