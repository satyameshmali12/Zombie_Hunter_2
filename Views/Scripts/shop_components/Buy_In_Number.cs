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
        if(this.Visible){
            this.GetNode<Label>("No_Item").Text = $"Available : - {available_count.ToString()}";
        }
    }

    public override void data_change_logic()
    {
        base.data_change_logic();
        




        // var per_bomb_price = this.shop_data - data_change_logic
        if (available_count < max_bomb_count_in_inventory && available_money  >= per_price)
        {
            available_count++;
            settle_data(per_price,"No_Of_Bomb_Available",$"{available_count}");
        }
        else{
            warning_dialog.DialogText = "Can't Buy..!!";
            warning_dialog.PopupCentered();
        }
        

    }

    public override void toggle_visibility()
    {
        base.toggle_visibility();
        var is_unlocked = Convert.ToBoolean(shop_data.get_data("Is_Unlocked"));
        this.Visible = (is_unlocked && parent.can_be_buyed_in_numbers[menu_selection]);
        if (this.Visible)
        {
            per_price = Convert.ToInt32(shop_data.get_data("Per_Price"));
            this.GetNode<Label>("Money").Text = $"{per_price}$";
            available_count = Convert.ToInt32(shop_data.get_data("No_Of_Bomb_Available"));
        }
        parent.reload_scene_features(true);


    }


}
