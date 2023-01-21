using Godot;
using System;
using System.Collections;

public class Item_Searcher : Control
{

    /*
    to know whether the item is selected or not
    */
    #region It will provide this two thing as the source of output 
    public bool is_item_selected = false;
    public string selected_item = null;
    #endregion


    ArrayList render_items, render_items_copy = new ArrayList();
    int item_start_index = 0;

    Basic_Func basf;

    bool can_change = true;
    bool is_instantaneous_click = false;

    Button done_button;
    Button last_pressed_button = null;

    Timer instantaneous_click_timer;
    Timer can_change_timer;


    TextEdit search_bar;

    public override void _Ready()
    {
        basf = new Basic_Func(this);


        can_change_timer = basf.create_timer(.1f, "Can_Change_Toggle");
        instantaneous_click_timer = basf.create_timer(.5f, "Instantanoues_Click");
        done_button = this.GetNode<Button>("Done");

        search_bar = this.GetNode<TextEdit>("Search_Bar");

    }

    public override void _Process(float delta)
    {
        Label selected_item_name = this.GetNode<Label>("Selected_Item_Name");
        selected_item_name.Text = (selected_item != null) ? selected_item : "";
        done_button.Visible = (selected_item != null);

        var item_boxes = this.GetNode<VBoxContainer>("Items").GetChildren();
        var back_button = this.GetNode<Button>("Back");


        // scroll bar effect
        int change = basf.get_scroller_start_index_change(item_start_index, item_boxes.Count, render_items);
        if (change != 0 && can_change)
        {
            item_start_index += change;
            can_change_timer.Start();
        }



        // hiding it when back button is pressed
        if (back_button.Pressed)
        {
            hide();
        }
        // setting whether the item is selected
        else if (done_button.Pressed)
        {
            hide();
            is_item_selected = true;
        }


        if (last_pressed_button != null)
        {
            if (!last_pressed_button.Pressed)
            {
                last_pressed_button = null;
            }
        }


        // getting the search_bar text
        string text = search_bar.Text;
        if (text.Length > 0)
        {
            render_items.Clear();
            foreach (string item in render_items_copy)
            {
                if (item.ToLower().Contains(search_bar.Text.ToLower()))
                {
                    render_items.Add(item);
                }

            }
        }
        else
        {
            render_items = render_items_copy.Clone() as ArrayList;
        }




        // rendering the items on the screen and simultaneouly getting the desired clicked item
        foreach (MarginContainer item in item_boxes)
        {
            var index = item_boxes.IndexOf(item);
            if (index + item_start_index < render_items.Count)
            {
                item.Visible = true;
                Button select_button = item.GetNode<Button>("Select_Button");
                select_button.Text = render_items[index + item_start_index].ToString();
                if (select_button.Pressed)
                {
                    if (is_instantaneous_click && selected_item == render_items[index + item_start_index].ToString() && last_pressed_button == null)
                    {
                        this.Visible = false;
                        is_item_selected = true;
                    }
                    selected_item = select_button.Text;
                    is_instantaneous_click = true;
                    instantaneous_click_timer.Start();
                    last_pressed_button = select_button;

                }
            }
            else
            {
                item.Visible = false;
            }
        }



    }

    public void Can_Change_Toggle()
    {
        can_change = true;
        can_change_timer.Stop();
    }

    public void reset()
    {
        is_item_selected = false;
    }

    public void hide()
    {
        this.Visible = false;
    }

    public void pop()
    {
        this.Visible = true;
        this.selected_item = null;
        is_item_selected = false;
    }


    public void Instantanoues_Click()
    {
        is_instantaneous_click = false;
        instantaneous_click_timer.Start();
    }

    public void load_data(ArrayList selective_list)
    {

        render_items = selective_list;
        render_items_copy = render_items.Clone() as ArrayList;
    }
}
