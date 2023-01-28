using Godot;
using System.Collections;
using Godot.Collections;
using System;



public class Instantaneous_Buyer : Node2D
{
    Basic_Func basf;

    ArrayList all_items = new ArrayList();

    Item_Searcher item_searcher;

    Data_Manager user_data;

    Button target_button = null;

    public override void _Ready()
    {
        basf = new Basic_Func(this);

        item_searcher = this.GetNode<Item_Searcher>("Item_Searcher");

        ArrayList all_item_names = new ArrayList();
        foreach (string data_url in basf.global_Variables.item_data_field_urls)
        {

            Data_Manager manager = new Data_Manager(data_url);
            // we have taken here the data_field identifier as the first value in the field names as first field in the data_field is the name only in all the cases
            ArrayList all_items_names = manager.get_set_of_field_values(manager.all_field_names[0].ToString());

            foreach (string item in all_items_names)
            {
                all_items.Add(basf.get_dictionary<string>(new string[2] { "name", "data_field_url" }, new string[2] { item, data_url }));
                all_item_names.Add(item);
            }
        }

        item_searcher.load_data(all_item_names);
        item_searcher.is_double_pressed_vis_toggled = false;

        user_data = new Data_Manager(basf.global_paths.User_Data_Field_Url);
        user_data.load_data("Aj");


    }

    public override void _Process(float delta)
    {

        item_searcher.GetNode<Label>("Money_Label").Text = user_data.get_data("Money"); // showing the available money to the user

        var buying_button_containers = item_searcher.GetNode<VBoxContainer>("Buying_Buttons_Container").GetChildren();

        if (Input.IsActionJustPressed("Toggle_Instant_Buyer"))
        {
            if (item_searcher.Visible)
            {
                item_searcher.hide();
            }
            else
            {
                item_searcher.pop();
            }
        }



        for (int i = 0; i < item_searcher.item_boxes.Count; i++)
        {
            Container item_box = item_searcher.item_boxes[i] as Container; // the boxes in the item_searcher

            Container buying_button_container = buying_button_containers[i] as Container; // the buying buttons and other stuffs in the buying button container


            if (item_box.Visible)
            {
                buying_button_container.Visible = true;

                string item_name = item_box.GetNode<Button>("Select_Button").Text;

                Button buying_button = buying_button_container.GetNode<Button>("Button");
                Data_Manager item_dm = null;

                // int item_price = 0;
                // int available_money = 0;

                int item_price  = 0;
                int available_money =int.Parse(user_data.get_data("Money"));;

                int item_count = 0;
                foreach (Dictionary<string, string> item in all_items)
                {
                    if (item["name"] == item_name)
                    {

                        item_dm = new Data_Manager(item["data_field_url"]);
                        item_dm.load_data(item["name"]);

                        buying_button.Text = item_dm.get_data("Per_Price");

                        item_count = int.Parse(item_dm.get_data("Available_Count"));

                        item_price = int.Parse(buying_button.Text);

                        buying_button.GetNode<Label>("Count").Text = item_count.ToString();
                        buying_button.Disabled = (item_price>available_money || item_count>=100); // disabling the buy button if no sufficient money

                    }
                }
                if (target_button == null)
                {
                    if (buying_button.Pressed)
                    {
                        if (buying_button.Pressed && item_dm != null)
                        {
                            if (available_money >= item_price && item_count < 100)
                            {
                                available_money -= item_price;
                                user_data.set_value("Money", available_money.ToString());
                                item_dm.set_value("Available_Count", (item_count + 1).ToString());
                                item_dm.save_data();
                                user_data.save_data();
                                user_data.load_previous_data_again();
                                // reloading all the stuffs on the item_using_menu new or more item is been bought
                                basf.global_Variables.item_Using_Menu.add_view();
                                target_button = buying_button;
                            }
                        }
                    }
                }
                else if (!target_button.Pressed)
                {
                    target_button = null;
                }
            }
            else
            {
                buying_button_container.Visible = false;
            }
        }

    }

}
