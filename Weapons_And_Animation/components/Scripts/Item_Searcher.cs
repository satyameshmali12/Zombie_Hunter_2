using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using Godot.Collections;

public class Item_Searcher : Control
{

    /*
    to know whether the item is selected or not
    */
    #region It will provide this two thing as the source of output 
    public bool is_item_selected = false;
    public string selected_item = null;
    #endregion


    public ArrayList render_items, render_items_copy = new ArrayList();
    int item_start_index = 0;

    Basic_Func basf;

    bool can_change = true;
    bool is_instantaneous_click = false;

    Button done_button;
    Button last_pressed_button = null;

    Timer instantaneous_click_timer;
    Timer can_change_timer;


    TextEdit search_bar;

    Vector2 re_distance = Vector2.Zero;
    bool is_re_distance_settled = false;

    bool is_double_clicked = false;
    Timer double_click_timer;
    bool is_to_make_double_clicked = false;
    Vector2 original_position;
    public Godot.Collections.Array item_boxes;
    public List<BaseButton> gui_tickle_buttons = new List<BaseButton>();

    public bool is_double_pressed_vis_toggled = true;


    public override void _Ready()
    {
        basf = new Basic_Func(this);


        can_change_timer = basf.create_timer(.1f, "Can_Change_Toggle");
        instantaneous_click_timer = basf.create_timer(.5f, "Instantanoues_Click");
        done_button = this.GetNode<Button>("Done");

        search_bar = this.GetNode<TextEdit>("Search_Bar");

        double_click_timer = basf.create_timer(2, "Over_Double_Click");
        original_position = this.RectPosition;

        item_boxes = this.GetNode<VBoxContainer>("Items").GetChildren();


    }

    public override void _Process(float delta)
    {
        Label selected_item_name = this.GetNode<Label>("Selected_Item_Name");
        selected_item_name.Text = (selected_item != null) ? selected_item : "";
        done_button.Visible = (selected_item != null);

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
            var per_view_boxes = item_boxes.Count;
            if (item_start_index + per_view_boxes >= render_items.Count)
            {
                var new_index = render_items.Count - per_view_boxes;
                item_start_index = (new_index < 0) ? 0 : new_index;
            }

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
                        if(is_double_pressed_vis_toggled){
                            GD.Print("hey there right from the item_seracher.cs haha.!!");
                            this.Visible = false;
                        }
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

        if (Input.IsActionJustPressed("Mouse_Pressed"))
        {
            if (!is_to_make_double_clicked)
            {
                is_to_make_double_clicked = true;
                double_click_timer.Start();
            }
            else
            {
                is_double_clicked = true;
            }
        }


        if (Input.IsActionPressed("Mouse_Pressed"))
        {
            {
                var mouse_pos = this.GetGlobalMousePosition();
                var mx = mouse_pos.x;
                var my = mouse_pos.y;

                var rect = this.GetNode<ReferenceRect>("Rect");
                var rect_pos = rect.RectGlobalPosition;

                if (mx > rect_pos.x && my > rect_pos.y && mx < rect_pos.x + rect.RectSize.x && my < rect_pos.y + rect.RectSize.y)
                {
                    if(is_double_pressed_vis_toggled)
                    {
                        if (!is_re_distance_settled)
                        {
                            is_re_distance_settled = true;
                            re_distance = basf.abs_a_vector(mouse_pos) - basf.abs_a_vector(this.RectGlobalPosition);
                        }

                        if (is_double_clicked)
                        {
                            this.RectGlobalPosition = mouse_pos - re_distance;

                        }
                    }
                }
                else{
                    bool is_any_gui_button_pressed =false;
                    foreach (BaseButton button in gui_tickle_buttons)
                    {
                        if(button.Pressed)
                        {
                            is_any_gui_button_pressed = true;
                        }
                    }
                    if(!is_any_gui_button_pressed)
                    {
                        hide();
                    }
                }
            }
        }

        else
        {
            is_re_distance_settled = false;
            is_double_clicked = false;
        }





    }

    public void Over_Double_Click()
    {
        is_to_make_double_clicked = false;
        double_click_timer.Stop();
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
        this.RectPosition = original_position;
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

    public void add_guitickle_button(params BaseButton[] new_guitickle_buttons)
    {
        foreach (BaseButton button in new_guitickle_buttons)
        {
            if (!this.gui_tickle_buttons.Contains(button)){
                this.gui_tickle_buttons.Add(button);
            }
        }
    }
}
