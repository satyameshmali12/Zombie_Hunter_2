using Godot;
using System;

public class Unlock : Buy
{

    public override void _Ready()
    {
        base._Ready();
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

    }

    public override void data_change_logic(){
        if (available_money  > required_money)
        {

            available_money  -= required_money;
            user_data.set_value("Money", $"{available_money }");
            user_data.save_data();
            shop_data.set_value("Is_Unlocked", "true");
            shop_data.save_data();
            parent.reload_scene_features();
        }
    }

    public override void toggle_visibility(){
        var is_unlocked = Convert.ToBoolean(shop_data.get_data("Is_Unlocked"));
        this.Visible = (!is_unlocked)?true:false;
        this.GetNode<Label>("Money").Text = $"{shop_data.get_data("Price")}$";
        parent.reload_scene_features(true);
    }
    
}
