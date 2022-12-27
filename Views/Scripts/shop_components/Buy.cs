using Godot;
using System;

#region description
/*
This class is the parent class for all the components allocated on the shop view
all the data is from the parent is fetched through this file
*/
#endregion

public class Buy : Control
{

    public Basic_Func basf;

    bool is_button_presesd = false;

    public bool new_scene_toggled = false;

    public Data_Manager user_data, shop_data;
    public int available_money, menu_selection;

    public int required_money = 0;


    public Shop parent;

    public string button_name = "Buy_Button";

    public AcceptDialog warning_dialog;
    public Global_Variables_F_A_T parent_funcs;



    public override void _Ready()
    {
        basf = new Basic_Func(this);
        warning_dialog = this.GetNode<AcceptDialog>("%Warning");

    }

    public override void _Process(float delta)
    {

        // parent_funcs = parent as Global_Variables_F_A_T;
        parent_funcs = this.GetParent().GetParent<Shop>().render_char as Global_Variables_F_A_T;
        parent = this.GetParent<Control>().GetParent<Shop>();

        // parent_funcs = this.GetParent().GetParent<Shop>().parent as Global_Variables_F_A_T;



        #region getting all the data or the properties from the shop as the shop is the parent of this node

        // var data = parent.shop_data_manager;

        user_data = parent.user_data_manager;
        shop_data = parent.shop_data_manager;

        menu_selection = parent.menu_selection;
        available_money = Convert.ToInt32(user_data.get_data("Money"));
        #endregion

        var specific_name = parent.specific_name;

        // if (parent.menu_selection != parent.zombie_view_index)
        // {
            toggle_visibility();

            #region data changing logic


            if (this.Visible)
            {
                if (shop_data.all_field_names.Contains("Price"))
                {
                    required_money = Convert.ToInt32(shop_data.get_data("Price"));
                }

                var buy_button = this.GetNode<Button>(button_name);
                if (buy_button.Pressed)
                {
                    if (!is_button_presesd)
                    {
                        data_change_logic();
                        parent.reload_scene_features();
                        is_button_presesd = true;
                    }
                }
                else
                {
                    is_button_presesd = false;
                }
            }
            #endregion

        // }

    }

    // function to settle data
    public void settle_data(int required_money, string another_field_name, string another_field_data)
    {
        available_money -= required_money;
        user_data.set_value("Money", $"{available_money}");
        // user_data.save_data();
        shop_data.set_value(another_field_name, another_field_data);
        // shop_data.save_data();
        parent.reload_scene_features();
    }

    public virtual void data_change_logic() { }

    public virtual void toggle_visibility() { }

    public virtual void toggle_warninig_message(string message = "Can't Buy"){
        warning_dialog.DialogText = message;
        warning_dialog.PopupCentered();   
    }



}
