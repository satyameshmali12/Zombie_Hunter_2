using Godot;
using System;
using System.Collections;

public class Game_Gui : Node2D
{


    Basic_Func basf;


    bool is_button_pressed = false;
    Item_Using_Menu item_using_scene;
    Button item_using_button, pause_button;


    Pause_Menu pause_Menu;


    Label score_label;


    Node2D item_in_hand_show;
    Button remove_item_in_hand_button;



    public override void _Ready()
    {
        basf = new Basic_Func(this, "data//data_fields/bomb_data_fields.zhd");
        // bomb_Buttons = this.GetNode<Node2D>("Bomb_Button's").GetChildren();
        basf.global_Variables.bomb_Buttons = basf.get_the_node_childrens("Bomb_Button's");

        item_using_button = this.GetNode<Button>("Item_Using_Button");
        item_using_scene = this.GetNode<Item_Using_Menu>("Item_Using_Menu");

        score_label = GetNode<Label>("Score_Label");

        pause_button = this.GetNode<Button>("Pause_Button");

        pause_Menu = this.GetNode<Pause_Menu>("Pause_Menu");


        item_in_hand_show = this.GetNode<Node2D>("Item_In_Hand");
        remove_item_in_hand_button = item_in_hand_show.GetNode<Button>("Remove_Item_In_Hand");
        basf.add_guitickle_button(pause_button);

    }

    public override void _Process(float delta)
    {

        var score = basf.global_Variables.score;

        score_label.Text = $"Sc0re:- {score}";

        var bomb_Buttons = basf.global_Variables.bomb_Buttons;
        foreach (TextureButton item in bomb_Buttons)
        {
            if (item.Pressed)
            {
                basf.global_Variables.spell_in_hand = item.Name;
            }
        }

        var dm = basf.dm;
        var bomb_buttons = basf.get_the_node_childrens("Bomb_Button's");
        foreach (TextureButton item in bomb_buttons)
        {
            // reading the data again as to detect the use of the bomb
            basf.dm.read_data();
            // loading the data for a particular bomb
            basf.dm.load_data(item.Name);
            var count = Convert.ToInt32(dm.get_data("Available_Count"));

            // giving the value of the data to the count(label) node of the button
            // it will display the number of the bomb available
            item.GetNode<Label>("Count").Text = $"{count}";

        }


        // working with the item_using_button
        if (item_using_button.Pressed)
        {
            if (!is_button_pressed)
            {
                basf.disvisible_visible_list(item_using_scene);
                item_using_scene.Visible = (item_using_scene.Visible) ? false : true;
            }
            is_button_pressed = true;
        }
        else
        {
            is_button_pressed = false;
        }


        // working with the pause menu button
        if (pause_button.Pressed)
        {
            basf.disvisible_visible_list();
            pause_Menu.Visible = true;
            basf.pause_tree(this);
        }

        var item_u_m_c = basf.global_Variables.item_using_menu_comp;

        item_in_hand_show.Visible = (item_u_m_c != null);

        if (item_in_hand_show.Visible)
        {
            item_in_hand_show.GetNode<Label>("Item_Name").Text = item_u_m_c.name;
        }

        if (remove_item_in_hand_button.Pressed)
        {
            basf.nullify_item_in_hand();
        }





    }


}
