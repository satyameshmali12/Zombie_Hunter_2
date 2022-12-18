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


    // this array is method with respect to the above index🔼
    string[] specific_data_fields_urls = new string[3] { "data//data_fields/heros_data_field.zhd", "data//data_fields/zombie_data_fields.zhd", "data//data_fields/bomb_data_fields.zhd" };
    string[] names = new string[3] { "Character_Name", "Zombie_Name", "Bomb_Name" };

    // making all the condition
    public bool[] can_be_buyed_in_numbers = new bool[3]{false,false,true};

    string[] base_urls = new string[3] { "res://Characters/Characters_Scene/Player/", "res://Characters/Characters_Scene/Zombie/", "res://Bomb's/Scenes/" };
    bool is_button_pressed , is_button_pressed_2 = false;

    // getting some buttons on the screen
    public TextureButton left_button, right_button;

    public string specific_name = null;

    public Data_Manager shop_data_manager;
    public bool is_new_scene_toggled_in_shop = false;// whether new scene is toggled or not


    AnimatedSprite current_animation;
    bool is_current_animation_initialized;

    public override void _Ready()
    {
        base._Ready();


        basf = new Basic_Func(this);
        var dm = basf.dm;

        for (var i = 0; i < names.Length; i++)
        {
            dm = new Data_Manager(specific_data_fields_urls[i]);
            urls.Add(dm.get_set_of_field_values(names[i]));
        }

        add_the_view();
        left_button = this.GetNode<TextureButton>("Left_Button");
        right_button = this.GetNode<TextureButton>("Right_Button");

        is_current_animation_initialized = false;

        // now loading the user data to get and set some of the requred value e.g money
        load_user_data();

        // GD.Print("hey first stage form the shopt view hahah..!!");


    }

    public override void _Process(float delta)
    {

        // getting the option_button on screen and thereby setting the animation available in the node
        var option_button = this.GetNode<OptionButton>("OptionButton");
        current_animation.Animation = option_button.GetItemText(option_button.GetSelectedId()); 
        current_animation.Playing = this.GetNode<CheckButton>("Animation_Playing").Pressed;

        
        // showing the available money on the screen
        this.GetNode<Label>("Money").Text = $"Money : - {basf.dm.get_data("Money")}";

        // make the character and zombie to be separated for the shop render so that concern been seperated
        // Caution🔺
        base._Process(delta);
        if (left_button.Pressed || right_button.Pressed)
        {
            if (!is_button_pressed)
            {
                ArrayList arr = (ArrayList)urls[menu_selection];
                is_button_pressed = true;
                var increment = (left_button.Pressed) ? -1 : 1;

                var new_value = view_index + increment;
                GD.Print(new_value);
                if (new_value >= 0 && new_value < arr.Count)
                {
                    view_index = new_value;
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
            if(but.Pressed){
                if(!is_button_pressed){
                    view_index = 0;
                    menu_selection = Convert.ToInt32(but.Name);
                    add_the_view();
                }
                is_button_pressed_2 = true;
            }
            else{
                is_button_pressed_2 = false;
            }
        }





    }

    // this function is used to add the view on the screen
    public bool add_the_view()
    {
        var loading_label = this.GetNode<Label>("Loading");
        loading_label.Visible = true;
        if(is_current_animation_initialized){
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
            var start_index = value.Length-"_name".Length;
            this.GetNode<Label>("Menu_Name").Text = value.Left(start_index);


            // getting the packed scene and instancing it with the basic_player
            PackedScene scene = basf.get_the_packed_scene(path);

            var view = scene.Instance<Node2D>();

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
                if(!item.ToLower().Trim().Contains("default")){
                    option_button.AddItem(item.ToString());
                }
            }
            if(menu_selection!=bomb_view_index){

            }

        }
        loading_label.Visible = false;
        #endregion
        return true;
    }

    // function to reload some data and other stuff too..
    public void reload_scene_features(bool is_new_scene_toggled = true){
        // loading specific data and sending the specific data as shop_data to the global_varaibles
        var new_data_manager = new Data_Manager(specific_data_fields_urls[menu_selection].ToString());
        new_data_manager.load_data(specific_name);
        shop_data_manager = new_data_manager;
        is_new_scene_toggled_in_shop = is_new_scene_toggled;
        load_user_data();
    }

    // function to load the user_data
    public void load_user_data(){
        basf.dm = new Data_Manager("data//data_fields/user_data_fields.zhd");
        basf.dm.load_data("Aj");
    }
}
