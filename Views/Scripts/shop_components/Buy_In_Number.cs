using Godot;
using System;

public class Buy_In_Number : Buy
{

    int available_count = 0;
    public const int max_bomb_count_in_inventory = 100;

    public int per_price = 0;




    public override void _Ready()
    {
        base._Ready();

    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }

    public override void data_change_logic()
    {
        base.data_change_logic();

        if (available_count < max_bomb_count_in_inventory && available_money >= per_price)
        {
            available_count++;
            settle_data(per_price, "Available_Count", $"{available_count}");
        }
        else
        {
            toggle_warninig_message();
        }

    }

    public override void toggle_visibility()
    {
        base.toggle_visibility();

        if (parent.can_be_buyed_in_numbers[menu_selection])
        {
            var is_unlocked = Convert.ToBoolean(shop_data.get_data("Is_Unlocked"));
            this.Visible = is_unlocked;
            per_price = Convert.ToInt32(shop_data.get_data("Per_Price"));
            available_count = Convert.ToInt32(shop_data.get_data("Available_Count"));
            this.GetNode<Label>("Money").Text = $"{per_price}$";
            this.GetNode<Label>("No_Item").Text = $"Available : - {available_count.ToString()}";
        }
        else
        {
            this.Visible = false;
        }
    }


}
