using Godot;
using System;

public class Buy : Control
{

    Basic_Func basf;

    bool is_button_presesd = false;

    public override void _Ready()
    {
        basf = new Basic_Func(this);
    }

    public override void _Process(float delta)
    {


        var parent = this.GetParent<Shop>();

        #region getting all the data or the properties from the shop as the shop is the parent of this node
        var new_scene_toggled = parent.is_new_scene_toggled_in_shop;
        var data = parent.shop_data_manager;
        var user_data = parent.basf.dm;
        var shop_data = parent.shop_data_manager;
        var menu_selection = parent.menu_selection;
        #endregion

        var unlock_control_node = this.GetNode<Control>("Unlock");

        var buy_in_num_node = this.GetNode<Control>("Buy_In_Number"); /* e.g bomb it could be buyed in numbers*/

        var specific_name = parent.specific_name;

        if (parent.menu_selection != parent.zombie_view_index)
        {
            if (new_scene_toggled)
            {
                var is_unlocked = Convert.ToBoolean(data.get_data("Is_Unlocked"));
                unlock_control_node.Visible = (!is_unlocked) ? true : false;
                parent.reload_scene_features(false);
                unlock_control_node.GetNode<Label>("Money").Text = $"{shop_data.get_data("Price")}$";

                buy_in_num_node.Visible = parent.can_be_buyed_in_numbers[menu_selection] && !unlock_control_node.Visible;
            }

            #region for unlocking the characters
            if (unlock_control_node.Visible)
            {
                var buy_button = unlock_control_node.GetNode<Button>("Buy_Button");
                if (buy_button.Pressed)
                {
                    if (!is_button_presesd)
                    {
                        is_button_presesd = true;
                        int availble_money = Convert.ToInt32(user_data.get_data("Money"));
                        int required_money = Convert.ToInt32(shop_data.get_data("Price"));
                        if (availble_money > required_money)
                        {
                            availble_money -= required_money;
                            user_data.set_value("Money", $"{availble_money}");
                            user_data.save_data();
                            shop_data.set_value("Is_Unlocked", "true");
                            shop_data.save_data();
                            parent.reload_scene_features();
                        }
                    }
                }
                else
                {
                    is_button_presesd = false;
                }
            }
            #endregion

            else if(buy_in_num_node.Visible){
                
            }

        }

    }


}
