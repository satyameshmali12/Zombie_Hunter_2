using Godot;
using System;

public class Equipe : Buy
{
    bool is_button_pressed = false;

    Basic_Func basf;
    string current_character_name = null; 
    
    bool is_unlocked = false;
    

    // string shop_data_path = "data/data_fields/shop_data_field.zhd";

    public override void _Ready()
    {
        base._Ready();
        basf = new Basic_Func(this);
        button_name = "Avail_Button";

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        // shop_data.reload_data(shop_data_path);

        current_character_name = this.GetNode<Label>("%Specific_Name").Text;

        // GD.Print(current_character_name);

        // this.Visible =  is_unlocked && parent.menu_selection==parent.player_view_index;
        this.Visible = is_unlocked && parent.can_be_equiped[menu_selection];


    }

    public override void data_change_logic()
    {
        base.data_change_logic();

        user_data.set_value("Current_Character",current_character_name);
        user_data.save_data();
        // GD.Print(current_character_name);
    }

    public override void toggle_visibility()
    {
        base.toggle_visibility();

        is_unlocked = Convert.ToBoolean(shop_data.get_data("Is_Unlocked"));

        if(is_unlocked && parent.menu_selection==parent.player_view_index){
            var character_avail_button = this.GetNode<Button>("Avail_Button");
            character_avail_button.Text = (user_data.get_data("Current_Character")==current_character_name)?"Equiped!":"Equipe";
        }
        parent.reload_scene_features();


    }
}
