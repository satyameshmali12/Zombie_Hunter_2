using Godot;
using System;
using System.Collections;

public class Shop : Basic_View
{

    int menu_selection = 0; // the selection of the type of view in the shop
    int view_index = 0; // the index of the character that user wanted to see

    ArrayList urls = new ArrayList() { };

    ArrayList characters_urls = new ArrayList() { "Robot", "Ninja" };
    ArrayList zombie_urls = new ArrayList(){};
    ArrayList bomb_urls = new ArrayList(){"Skew_Bomb"};
    ArrayList base_urls = new ArrayList() { "res://Characters/Characters_Scene/Player/", "res://Characters/Characters_Scene/Zombie/" , "res://Bomb's/Scenes"};
    ArrayList script_url = new ArrayList() { "res://Views/Scripts/Shop_Scripts/Player_Show_View.cs" };
    bool is_button_pressed = false;

    public TextureButton left_button, right_button;
    public override void _Ready()
    {
        base._Ready();
        // basf = new Basic_Func()
        urls.Add(characters_urls);
        urls.Add(zombie_urls);
        urls.Add(bomb_urls);
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
                is_button_pressed = true;
                var increment = (left_button.Pressed) ? -1 : 1;
                GD.Print(increment);
                var new_value = view_index + increment;
                if (new_value < 0 || new_value > characters_urls.Count)
                {
                    new_value = 0;
                }
            }
        }
        else
        {
            is_button_pressed = false;
        }



    }

    public bool add_the_view()
    {
        // first removing all the childs
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

        var view = scene.Instance<Basic_Player>();

        var animation = view.GetNode<AnimatedSprite>("Movements");

        // var new_animation = new AnimatedSprite();
        var new_animation = basf.get_the_packed_scene("res://Weapons_And_Animation/components/scenes/Empty_Animation.tscn").Instance<AnimatedSprite>();
        // new_animation.Frames.Animations = animation.Frames.Animations;
        // new_animation.Frames.AddAnimation("kjdf");
        // new_animation.Frames.AddFrame()
        // new_animation.Frames.Frames = animation.Frames.Frames;
        foreach (Array item in animation.Frames.Animations)
        {
            // var name = item.ResourceName;
            // GD.Print(name);
            foreach (SpriteFrames item2 in item)
            {
                var name = item2.ResourceName;
                GD.Print(name);
            }
            // foreach (Godot.Texture item2 in item.Frames)
            // {
            //     item.AddFrame(name,item2);
            // }
        }

        

        // this.AddChild(new_animation);


        // var view = scene.Instance<Basic_Player>();
        // var view = scene.Instance<Node2D>();
        // var animation = view.GetNode<AnimatedSprite>("Movements");
        return true;
    }
}
