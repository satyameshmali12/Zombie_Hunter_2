using Godot;
using System;

public class Equipe : Buy
{
    bool is_button_pressed = false;

    // Basic_Func basf;
    string current_character_name = null; 
    
    bool is_unlocked = false;
    

    public override void _Ready()
    {
        base._Ready();
        basf = new Basic_Func(this);
        button_name = "Avail_Button";

    }

    public override void _Process(float delta)
    {

        base._Process(delta);

    }

    public override void data_change_logic()
    {
        base.data_change_logic();
        user_data.set_value("Current_Character",current_character_name);
    }

    public override void toggle_visibility()
    {
        base.toggle_visibility();

        current_character_name = this.GetNode<Label>("%Specific_Name").Text;
        if(parent.can_be_equiped[menu_selection])
        {   
            is_unlocked = Convert.ToBoolean(shop_data.get_data("Is_Unlocked"));
            this.Visible = is_unlocked;
            var character_avail_button = this.GetNode<Button>("Avail_Button");
            character_avail_button.Text = (user_data.get_data("Current_Character")==current_character_name)?"Equiped!":"Equipe";

        }
        else{
            this.Visible = false;
        }

    }
}
