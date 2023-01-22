using Godot;
using System;
using System.Collections;
using Godot.Collections;

public class Warning_Editor : Basic_View
{

    /*
    items list contains all the name and the path to the data_field of that following item
    itemx_boxes is the list in which all the data will be placed with respect to that of the items list
    */

    Godot.Collections.Dictionary<string, string> selected_item = null;
    ArrayList items = new ArrayList(); // all the items
    ArrayList items_copy = new ArrayList();
    ArrayList item_data_field_urls = new ArrayList() { "data/data_fields/spell_data_fields.zhd", "data/data_fields/drone_data_fields.zhd" };


    VBoxContainer items_box;


    int item_start_index = 0;
    bool can_change = true;

    Timer can_change_timer;

    Node2D warning_edit_panel;


    ArrayList render_warning_item_list, new_item_warning_adding_list;
    // bool is_specific_item_data_loaded;
    bool is_item_warning_data_loaded = false;

    Control inputs;

    Data_Manager selected_item_dm;

    bool can_delete_items = true;

    Item_Searcher item_Searcher;


    public override void _Ready()
    {
        base._Ready();


        items_box = this.GetNode<VBoxContainer>("Items/Items_Box");
        warning_edit_panel = this.GetNode<Node2D>("Warning_Edit");

        // gettings all the items

        foreach (string data_url in item_data_field_urls)
        {

            Data_Manager manager = new Data_Manager(data_url);
            // we have taken here the data_field identifier as the first value in the field names as first field in the data_field is the name only in all the cases
            ArrayList all_items_names = manager.get_set_of_field_values(manager.all_field_names[0].ToString());

            foreach (string item in all_items_names)
            {
                items.Add(basf.get_dictionary<string>(new string[2] { "name", "data_field_url" }, new string[2] { item, data_url }));
            }
        }


        items_copy = items.Clone() as ArrayList; // getting the shallow copy of all the items

        can_change_timer = basf.create_timer(.1f, "Can_Change_Toggle");

        inputs = this.GetNode<Control>("Inputs");

        item_Searcher = this.GetNode<Item_Searcher>("Item_Searcher");
    }

    public override void _Process(float delta)
    {

        var main_item_node = this.GetNode<Control>("Items");
        main_item_node.Visible = (selected_item == null);

        warning_edit_panel.Visible = (selected_item != null);
        inputs.Visible = warning_edit_panel.Visible;

        var item_search_bar = this.GetNode<TextEdit>("Items/Item_Search_Box");

        if (main_item_node.Visible)
        {
            // items_box.set_data(items_box.GetChildCount(),items);
            // item_start_index = items_box.start_index;
            scroller_effect(items_box.GetChildCount(), items);
            re_render_items_data();

            var serach_text = item_search_bar.Text;
            item_search_bar.GetNode<Label>("Label").Visible = (serach_text.Length == 0);

            if (serach_text.Length > 0)
            {
                var per_view_boxes = items_box.GetChildCount();
                if (item_start_index + per_view_boxes >= items.Count)
                {
                    var new_index = items.Count - per_view_boxes;
                    item_start_index = (new_index < 0) ? 0 : new_index;
                }

                items.Clear();
                foreach (Dictionary<string, string> obj in items_copy)
                {
                    string name = obj["name"];
                    // GD.Print(name);
                    if (name.ToLower().Contains(serach_text.ToLower()))
                    {
                        items.Add(obj);
                    }
                }
            }
            else
            {
                items = items_copy.Clone() as ArrayList;
            }

        }
        else
        {

            // ones the mouse pressed is leaved letting one more item to delete
            if (!Input.IsActionPressed("Mouse_Pressed"))
            {
                can_delete_items = true;
            }

            warning_edit_panel.GetNode<Label>("Item_Name").Text = selected_item["name"];
            var no_item_label = warning_edit_panel.GetNode<Label>("No_Item");
            no_item_label.Visible = (render_warning_item_list.Count <= 0);
            var add_button = warning_edit_panel.GetNode<Button>("Add_Button");



            var universal_message_box = warning_edit_panel.GetNode<TextEdit>("Universal_Message_Box");
            var u_message_bx_to = warning_edit_panel.GetNode<CheckButton>("Universal_Message_GIven_Toggle");

            universal_message_box.Visible = u_message_bx_to.Pressed;




            /*
            adding the new item in the warning_list given by the user
            */
            if (add_button.Pressed && !item_Searcher.Visible)
            {
                item_Searcher.load_data(new_item_warning_adding_list);
                item_Searcher.pop();
            }
            else if (item_Searcher.is_item_selected)
            {
                render_warning_item_list.Add(new Dictionary<string, string>() { { "name", item_Searcher.selected_item }, { "message", "Don't use it" }, { "is_completely_restricted", "false" } });
                item_Searcher.reset();
                is_item_warning_data_loaded = false;
            }



            Button save_data_button = warning_edit_panel.GetNode<Button>("Save_Data");

            Button back_button = warning_edit_panel.GetNode<Button>("Back_Button");

            var warning_item_boxes = warning_edit_panel.GetNode<VBoxContainer>("VBoxContainer").GetChildren();


            // performing_the_scroller_effect
            scroller_effect(warning_item_boxes.Count, render_warning_item_list);


            //  saving the data or the input or the warnings given by the user
            if (save_data_button.Pressed)
            {
                string message = "";
                foreach (Dictionary<string, string> item in render_warning_item_list)
                {
                    var name = item["name"];
                    var specific_message = item["message"];
                    var is_completely_restricted = item["is_completely_restricted"];
                    message += $"|{name}:{specific_message}:{is_completely_restricted}|";
                }
                // setting the message to hyphen(-) which means null when the render_warning_item_list length is less than zero
                message = (render_warning_item_list.Count > 0) ? message : "-";

                selected_item_dm.set_value("Warning_List", message);
                selected_item_dm.save_data();
                selected_item_dm.load_previous_data_again();
                selected_item_dm.set_value("Is_Universal_Warning_Given", u_message_bx_to.Pressed.ToString());
                selected_item_dm.set_value("Universal_Warning", universal_message_box.Text);

            }

            else if (back_button.Pressed)
            {
                selected_item = null;
                item_start_index = 0;
            }


            // loading the initial-data after every change either in start index or during the landing on this on part
            if (!is_item_warning_data_loaded)
            {
                reset_new_item_w_add_list();


                u_message_bx_to.Pressed = bool.Parse(selected_item_dm.get_data("Is_Universal_Warning_Given"));
                universal_message_box.Text = selected_item_dm.get_data("Universal_Warning");


                foreach (MarginContainer item in warning_item_boxes)
                {
                    var index = warning_item_boxes.IndexOf(item);
                    var name = item.GetNode<Label>("Name");
                    var text_holder = inputs.GetChildren()[index] as TextEdit;

                    var is_res_toggle_button = item.GetNode("HBoxContainer/VBoxContainer2/MarginContainer/Toggle_Is_Restricted") as CheckButton;

                    if (item_start_index + index < render_warning_item_list.Count)
                    {
                        item.Visible = true;
                        text_holder.Visible = true;
                        Godot.Collections.Dictionary<string, string> item_warning_data = render_warning_item_list[item_start_index + index] as Godot.Collections.Dictionary<string, string>;
                        name.Text = item_warning_data["name"];
                        text_holder.Text = item_warning_data["message"];
                        is_res_toggle_button.Pressed = Convert.ToBoolean(item_warning_data["is_completely_restricted"]);

                    }
                    else
                    {
                        item.Visible = false;
                        text_holder.Visible = false;
                    }
                }

                is_item_warning_data_loaded = true;
            }
            else
            {

                for (int i = 0; i < warning_item_boxes.Count; i++)
                {
                    if (item_start_index + i < render_warning_item_list.Count)
                    {
                        var item_box = warning_item_boxes[i] as MarginContainer;
                        var is_restricted = item_box.GetNode<CheckButton>("HBoxContainer/VBoxContainer2/MarginContainer/Toggle_Is_Restricted").Pressed;

                        var input = inputs.GetChildren()[i] as TextEdit;

                        input.Visible = !u_message_bx_to.Pressed;
                        input.Readonly = is_restricted; // making the input readonly if the item is_completely restricted

                        var dic = render_warning_item_list[item_start_index + i] as Dictionary<string, string>;

                        // adding user input in our data list by simpling updating the dictionary in which data exists
                        dic["is_completely_restricted"] = is_restricted.ToString();
                        dic["message"] = input.Text;


                    }

                    // getting whether the remove button is being pressed or not
                    MarginContainer con = warning_item_boxes[i] as MarginContainer;

                    Button remove_button = con.GetNode<Button>("HBoxContainer/VBoxContainer/MarginContainer/Button");
                    if (remove_button.Pressed && can_delete_items)
                    {
                        render_warning_item_list.Remove(render_warning_item_list[item_start_index + i]);
                        is_item_warning_data_loaded = false;
                        can_delete_items = false;
                        break;
                    }
                }
            }

        }


    }

    public void re_render_items_data()
    {
        /* here the boxes terminology refers to the box in which the data is being placed*/
        var boxes = items_box.GetChildren();

        for (int i = 0; i < boxes.Count; i++)
        {
            var data_box = boxes[i] as Control;
            if (item_start_index + i < items.Count)
            {
                data_box.Visible = true;
                int item_index = item_start_index + i;
                Godot.Collections.Dictionary<string, string> item = items[item_index] as Godot.Collections.Dictionary<string, string>;
                data_box.GetNode<Label>("Name").Text = item["name"];

                Button edit_button = data_box.GetNode<Button>("HBoxContainer/MarginContainer/Edit_Button");
                if (edit_button.Pressed)
                {
                    item_start_index = 0;
                    selected_item = item; // setting the selected item
                    selected_item_dm = new Data_Manager(selected_item["data_field_url"]);  // loading the data
                    selected_item_dm.load_data(selected_item["name"]);

                    render_warning_item_list = basf.get_split_data_in_wdm(selected_item_dm.get_data("Warning_List")); // getting the earlier warning_list
                    is_item_warning_data_loaded = false;
                    reset_new_item_w_add_list();
                }
            }
            else
            {
                data_box.Visible = false;
            }

        }

    }

    public void Can_Change_Toggle()
    {
        can_change = true;
        can_change_timer.Stop();
    }

    public void reset_new_item_w_add_list()
    {

        // getting the items whoes warning is not given yet

        new_item_warning_adding_list = new ArrayList();

        ArrayList render_warning_item_name_list = new ArrayList();

        foreach (Godot.Collections.Dictionary<string, string> obj in render_warning_item_list)
        {
            // new_item_warning_adding_list.Add(obj["name"]);
            render_warning_item_name_list.Add(obj["name"]);
        }

        foreach (Godot.Collections.Dictionary<string, string> obj in items)
        {
            string name = obj["name"];
            if (!render_warning_item_name_list.Contains(name))
            {
                new_item_warning_adding_list.Add(name);
            }

        }
    }


    public void scroller_effect(int box_count, ArrayList items)
    {
        int change = basf.get_scroller_start_index_change(item_start_index, box_count, items);
        if (can_change && change != 0)
        {
            item_start_index += change;
            can_change = false;
            is_item_warning_data_loaded = false;
            can_change_timer.Start();
        }

    }
}

