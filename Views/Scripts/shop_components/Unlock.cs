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

		var weapons_label = this.GetNode("%Buy").GetNode<Label>("Weapons_Label");
		if(menu_selection==parent.weapons_view_index){
			var character_name = shop_data.get_data("Associated_With");
			weapons_label.Text = $"This weapon is associted with {character_name}";
			weapons_label.Visible = !Convert.ToBoolean(shop_data.get_data("Is_Unlocked"));
		}
		else{
			weapons_label.Visible = false;
		}

	}

	public override void data_change_logic()
	{
		if (available_money >= required_money)
		{
			available_money -= required_money;
			user_data.set_value("Money", $"{available_money}");
			shop_data.set_value("Is_Unlocked", "true");
			GD.Print("hey their budyy right from the unlock.cs haha  trying to unlock the weapons haha..!!");
			// parent_funcs = parent as Global_Variables_F_A_T;
			GD.Print(parent_funcs._node_type);
			if (parent_funcs._node_type == _Type_of_.Player)
			{
				var weapon_name = shop_data.get_data("Weapon_Name");
				GD.Print(weapon_name);
				if (weapon_name.ToLower() != "no_weapon")
				{
					var dm = basf.throwable_weapons_dm;
					dm.load_data(weapon_name);
					dm.set_value("Is_Unlocked", "true");
					dm.save_data();
				}
			}
		}
		else{
			toggle_warninig_message("No Sufficient Money!");
		}
	}

	public override void toggle_visibility()
	{
		base.toggle_visibility();

		this.Visible = parent.can_be_unlocked[menu_selection];

		if(parent.can_be_unlocked[menu_selection]){
			var is_unlocked = Convert.ToBoolean(shop_data.get_data("Is_Unlocked"));
			this.Visible = (!is_unlocked) ? true : false;
			this.GetNode<Label>("Money").Text = $"{shop_data.get_data("Price")}$";

		}
		else{
			this.Visible = false;
		}
	}

}
