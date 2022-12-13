using Godot;
using System;
using System.Collections;

public class Shop : Basic_View
{

    int menu_selection = 1; // the selection of the type of view in the shop
    int view_index = 0; // the index of the character that user wanted to see

    ArrayList urls = new ArrayList() { };

    const int player_view_index = 0;
    const int zombie_view_index = 1;
    const int bomb_view_index = 2;

    // this array is method with respect to the above index🔼
    string[] specific_data_fields_urls = new string[3] { "data//data_fields/heros_data_field.zhd", "data//data_fields/zombie_data_fields.zhd", "data//data_fields/bomb_data_fields.zhd" };
    string[] names = new string[3] { "Character_Name", "Zombie_Name", "Bomb_Name" };

    string[] base_urls = new string[3] { "res://Characters/Characters_Scene/Player/", "res://Characters/Characters_Scene/Zombie/", "res://Bomb's/Scenes/" };
    bool is_button_pressed , is_button_pressed_2 = false;

    public TextureButton left_button, right_button;
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
    }

    public override void _Process(float delta)
    {

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

    public bool add_the_view()
    {
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
            var path = $"{base_urls[menu_selection].ToString()}/{desired_urls[view_index].ToString()}.tscn";
            GD.Print(path);

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

            // some other setting's
            new_animation.Animation = new_animation.Frames.GetAnimationNames()[0];
            new_animation.Position = adding_position.Position;
            new_animation.Scale = animation.Scale;
            new_animation.ZIndex = 5;
            new_animation.Playing = true;


            adding_area.AddChild(new_animation);

        }
        #endregion
        return true;
    }
}
