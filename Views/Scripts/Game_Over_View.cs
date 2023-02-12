using Godot;
using System;

public class Game_Over_View : Basic_View
{

	// Basic_Func basf;
	TextureButton restart_button, home_button,next_level_button;
	
	public bool is_to_stop; // to stop all the processes and to renavigate somewhere else
	public bool is_next_level_available; // storign the value of whether is next level is available or not
	public int new_level_count; // this available only when the next level exits

	public override void _Ready()
	{
		base._Ready();

		basf = new Basic_Func(this);
		basf.clear_garbage();
		
		basf.global_Variables.is_game_quitted = false;


		var dm = basf.dm;

		is_next_level_available = false;

		// loading the level data
		dm = new Data_Manager();

		
		restart_button = this.GetNode<TextureButton>("Restart");
		home_button = this.GetNode<TextureButton>("Home_Button");
		next_level_button = this.GetNode<TextureButton>("Next_Level_Button");


		next_level_button.Visible = false; // initially making the level_button invisible

		if(basf.global_Variables.custom_url==null){
			var level_num = Convert.ToInt32(basf.global_Variables.current_level_name.Remove(0,"Level".Length)); // extracting the level number from the name 
			if(level_num<dm.get_set_of_field_values("Name").Count){
				new_level_count = level_num+1;
				if(dm.is_level_unlocked($"Level{new_level_count}")){
					is_next_level_available = true;
				}
			}
			if(is_next_level_available){
				next_level_button.Disabled = false;
				next_level_button.Visible = true;
			}
			else{
				next_level_button.Visible = false;
			}
			// note
			// make the level_changer button i.e back and forward button on the game_over_screen
		}

		// loading the user_data
		dm = new Data_Manager("data/data_fields/user_data_fields.zhd");
		dm.load_data("Aj");

		is_to_stop = false;



		// setting the text of the label on the screen
		var money = basf.global_Variables.score * 4;
		set_text_o_label("Score_Label", $"Score :- {basf.global_Variables.score}");
		set_text_o_label("Earn_Label", $"Earned :- {basf.global_Variables.score * 4} $");
		set_text_o_label("Win_Label", (basf.global_Variables.had_win_the_game) ? "Hey YOu W!n" : "yOu L0se,");

		// saving the data of the user
		dm.set_value("Money", $"{Convert.ToInt32(dm.get_data("Money")) + money}");
		dm.save_data();
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		base._Process(delta);
		
		if (!is_to_stop)
		{
			if (restart_button.Pressed)
			{
				terminate_all_task();
				// basf.global_Variables.is_level_added = false;
				basf.navigateTo(this, basf.main_game_scene_path);
				// is_to_stop = true;
			}
			else if (home_button.Pressed)
			{
				terminate_all_task();
				basf.navigateTo(this, basf.home_scene_path);
			}

			else if(next_level_button.Pressed){
				GD.Print("Hey there navigated to the main_game_scene_path from the game_over_view");
				terminate_all_task();
				basf.global_Variables.level_name = $"Level{new_level_count}";
				basf.navigateTo(this,basf.main_game_scene_path);
			}
		}

	}

	public void terminate_all_task()
	{
		is_to_stop = true;
		basf.global_Variables.is_level_added = false;
	}

	// set text of a label
	public void set_text_o_label(string node_name, string text)
	{
		this.GetNode<Label>(node_name).Text = text;

	}
}
