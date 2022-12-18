using Godot;
using System;


public class Update : Buy
{

    int current_level = 0;
    int update_cost = 0;
    int max_level = 0;

    public override void _Ready()
    {
        base._Ready();

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (this.Visible)
        {
            toggle_visibility();
            // update_cost = Convert.ToInt32(this.shop_data.get_data("Update_Cost"));
        }

    }


    public override void data_change_logic()
    {


        #region updating the power of the items

        var damage_data_field_name = (menu_selection == parent.player_view_index) ? "Damage_Increment" : "Bomb_Per_Damage";

        var update_cost_increment = Convert.ToInt32(this.shop_data.get_data("Update_Cost_Increment"));

        var damage = Convert.ToInt32(this.shop_data.get_data(damage_data_field_name));

        var damage_increment = Convert.ToInt32(shop_data.get_data("Per_Level_Damage_Increment"));

        GD.Print("udpate cost :- ", update_cost);

        GD.Print("update cost increment :- ", update_cost_increment);

        GD.Print("damage :-", damage);

        if (available_money > update_cost)

        {

            available_money -= update_cost;

            update_cost += update_cost_increment;

            damage += damage_increment;

            current_level++;

            var data_fields = new string[3]{"Update_Cost","Current_Level",damage_data_field_name};
            var data_fields_value = new string[3]{update_cost.ToString(),current_level.ToString(),damage.ToString()};
            shop_data.set_value(data_fields,data_fields_value);
            user_data.set_value("Money", available_money.ToString());
            shop_data.save_data();

            user_data.save_data();

            parent.reload_scene_features();

            // making the update node and do the closo of the shop first and then of the entire game

        }
        else{
            // this.GetNode<AcceptDialog>("%Warning").PopupCentered();
            warning_dialog.DialogText = "No Sufficient M0ney..!!";
            warning_dialog.PopupCentered();
        }


        #endregion
    }

    public override void toggle_visibility()
    {
        current_level = Convert.ToInt32(this.shop_data.get_data("Current_Level"));
        max_level = Convert.ToInt32(this.shop_data.get_data("Max_Level"));
        var is_unlocked = Convert.ToBoolean(this.shop_data.get_data("Is_Unlocked"));
        this.Visible = (is_unlocked && (current_level < max_level));


        if (this.Visible)
        {
            // GD.Print("hey there I am right form the update.cs and toggling the update cost text on the screen..!!");
            update_cost = Convert.ToInt32(this.shop_data.get_data("Update_Cost"));
            this.GetNode<Label>("Update_Price").Text = $"Price :- {update_cost}";
            this.GetNode<Label>("Current_Level").Text = $"Current_Level :- {current_level}";
        }

        this.GetNode<Label>("%Max_Level").Visible = (current_level == max_level);
        parent.reload_scene_features();
    }
}
