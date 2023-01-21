using Godot;
using System.Collections;

public class Item_Remover : Node2D
{
    Basic_Func basf;

    Godot.Collections.Array item_boxes;
    ArrayList items;
    int render_start_index;

    Timer can_change_timer;
    bool can_change = true;


    Button pressed_button = null;

    int past_items_length = 0;

    public override void _Ready()
    {
        basf = new Basic_Func(this);
        item_boxes = this.GetNode<VBoxContainer>("Items_Box").GetChildren();
        render_start_index = 0;
        basf.global_Variables.visibility_list.Add(this);
        can_change_timer = basf.create_timer(.3f, "alter_can_change");
        can_change_timer.Start();

        /*
        since items are coming from the global_varaibles so it can be just initialized ones 
        instead of fetching of data again and again
        */
        basf.add_guitickle_button(this.GetNode<Button>("Bg"));
        items = basf.global_Variables.item_in_progression;
        basf.global_Variables.item_removing_screen = this;

        this.Connect("tree_exiting",this,"Removal");

    }

    public override void _Process(float delta)
    {
        re_render_box_data();
        // int change = 0;
        // change = (Input.IsActionJustPressed("Move_Up") && (render_start_index > 0)) ? -1 : change;
        // change = (Input.IsActionJustPressed("Move_Down") && (render_start_index + item_boxes.Count) < items.Count) ? 1 : change;
        int change = basf.get_scroller_start_index_change(render_start_index,item_boxes.Count,items);

        if (can_change && change!=0)
        {
            render_start_index += change;
            // render_start_index = basf.get_scroller_start_index(render_start_index,item_boxes.Count,item_boxes);
            can_change = false;
            can_change_timer.Start();
        }


        this.GetNode<Label>("No_Item").Visible = (items.Count==0);





    }

    public void re_render_box_data()
    {


        for (int i = 0; i < item_boxes.Count; i++)
        {
            /* getting the one out out fours boxes */
            var item_box = item_boxes[i] as MarginContainer;

            /* Getting the index of the item as per the start index
                if the index becomes greator then the items list then it is maked invisible
            */
            var item_index = render_start_index + i;

            if (item_index < items.Count)
            {
                var item = items[item_index] as Item_Using_Menu_Component;
                item_box.Visible = true;
                item_box.GetNode<Label>("Name").Text = item.Name;

                // var remove_button = item.GetNode<HBoxContainer>("Items_Box/0/Hbo") <Button>("Remove_Button");
                var remove_button = item_box.GetNode<Button>("HBoxContainer/MarginContainer/Remove_Button");
                var box_bg_button = item_box.GetNode<Button>("Item_Bg");
                var position_label = item_box.GetNode<Label>("HBoxContainer2/MarginContainer/Position");
                position_label.Visible = item.is_to_display_position;
                position_label.Text = $"x : {item.GlobalPosition.x}  y :{item.GlobalPosition.y}";
                basf.add_guitickle_button(remove_button,box_bg_button);
                if (remove_button.Pressed && pressed_button == null)
                {
                    var count = items.Count;
                    item.Clear();

                    if (items.Count > 4 && render_start_index + item_boxes.Count > count - 1)
                    {
                        render_start_index--;
                    }
                    pressed_button = remove_button;
                }
                else if (pressed_button == remove_button && !remove_button.Pressed)
                {
                    pressed_button = null;
                }

            }
            else
            {
                item_box.Visible = false;
            }


        }

    }

    public void alter_can_change()
    {
        can_change = true;
        can_change_timer.Stop();
    }

    public void Removal()
    {
        basf.global_Variables.item_removing_screen = null;
    }
}
