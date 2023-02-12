using Godot;
using System.Collections;
using System;
using Godot.Collections;

public class Multi_AI_Match_Gui : Basic_View
{
    ArrayList heros = new ArrayList();
    ArrayList selected_heros = new ArrayList();

    ArrayList selected_heros_copy = new ArrayList();

    Data_Manager heros_data;

    int data_start_index = 0;

    Button pressed_button = null;

    bool can_play_mulit_ai_match = true;

    OptionButton ob ;





    public override void _Ready()
    {
        base._Ready();

        heros_data = new Data_Manager(basf.global_paths.Heros_Data_Field_Url);

        foreach (string hero_name in heros_data.get_set_of_field_values("Character_Name"))
        {
            heros_data.load_data(hero_name);
            if (bool.Parse(heros_data.get_data("Is_Unlocked")))
            {
                heros.Add(hero_name);

            }
        }
        can_play_mulit_ai_match = (heros.Count >= 3) ? true : false;

        this.GetNode<Control>("Can't_Play_View").Visible = !can_play_mulit_ai_match;

        ob = this.GetNode<OptionButton>("Hero_Choicer");



    }
    public override void _Process(float delta)
    {
        base._Process(delta);

        load_view_data();

        if (pressed_button != null && !pressed_button.Pressed)
        {
            pressed_button = null;
        }

        Button start_button = this.GetNode<Button>("Start_Match");
        if (start_button.Pressed && pressed_button == null)
        {
            if (can_play_mulit_ai_match && selected_heros.Count >= 3)
            {
                pressed_button = start_button;

                basf.global_Variables.is_level_added = false;
                basf.global_Variables.level_name = "Multi_Ai_Match";
                basf.global_Variables.custom_url = "res://Levels/Multi_Ai_Match/Multi_AI_Match.tscn";
                ArrayList mutli_ai_match_details = new ArrayList();
                string selected_character = ob.GetItemText(ob.Selected);
                foreach (string item in selected_heros)
                {
                    Dictionary<string, string> obj = new Dictionary<string, string>() { { "name", item }, { "is_main_character", (item==selected_character).ToString()} };
                    mutli_ai_match_details.Add(obj);
                }
                basf.global_Variables.multi_ai_character_details = mutli_ai_match_details;
                basf.navigateTo(this, basf.global_paths.Main_Game_Scene_Path);
            }
        }
    }


    /// <summary>To load the data on the screen so that user can interact</summary>
    public void load_view_data()
    {
        VBoxContainer p_con = this.GetNode<VBoxContainer>("Character_Selector/P_Con");

        int render_index = data_start_index;

        foreach (HBoxContainer c_con in p_con.GetChildren())
        {
            foreach (MarginContainer item in c_con.GetChildren())
            {
                // breaking the loop if render index exceeds the number of heros
                if (render_index < heros.Count)
                {
                    item.Visible = true;
                    Button but = item.GetNode<Button>("Button");
                    but.Text = heros[render_index].ToString();

                    if (but.Pressed && pressed_button == null)
                    {
                        pressed_button = but;
                        if (selected_heros.Count < 4)
                        {
                            heros.Remove(but.Text);
                            selected_heros.Add(but.Text);

                            break;
                        }
                    }
                }
                else
                {
                    item.Visible = false;
                }


                render_index++;
            }
        }

        Godot.Collections.Array selected_char_data_boxes = this.GetNode<HBoxContainer>("Character_Selector/Selected_Characters").GetChildren();
        foreach (MarginContainer item in selected_char_data_boxes)
        {
            int index = selected_char_data_boxes.IndexOf(item);
            if (index < selected_heros.Count)
            {
                item.Visible = true;
                Button but = item.GetNode<Button>("Button");
                but.Text = selected_heros[index].ToString();
                if (but.Pressed && pressed_button == null)
                {
                    pressed_button = but;
                    selected_heros.Remove(but.Text);
                    heros.Add(but.Text);
                    break;
                }
            }
            else
            {
                item.Visible = false;
            }
        }

        string selected_character_name = null;
        if (ob.Selected != -1)
        {
            selected_character_name = ob.GetItemText(ob.Selected);
        }
        ob.Clear();
        foreach (string hero_name in selected_heros)
        {
            ob.AddItem(hero_name);
        }

        for (int i = 0; i < ob.GetItemCount(); i++)
        {
            if (ob.GetItemText(i) == selected_character_name)
            {
                ob.Selected = i;
                break;
            }
        }

    }
}
