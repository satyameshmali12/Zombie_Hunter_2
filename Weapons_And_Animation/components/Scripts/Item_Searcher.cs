using Godot;
using System;
using System.Collections;

public class Item_Searcher : Control
{
    ArrayList render_items = new ArrayList();
    int item_start_index = 0;

    Basic_Func basf;

    bool can_change = true;
    Timer can_change_timer;
    public override void _Ready()
    {
        basf = new Basic_Func(this); 
        render_items = new ArrayList() { "hello", "challo", "go", "come", "out", "sout", "why", "kyu", "kya why", "but", "how" };
        can_change_timer = basf.create_timer(.1f,"Can_Change_Toggle"); 
    }

    public override void _Process(float delta)
    {

        var item_boxes = this.GetNode<VBoxContainer>("Items").GetChildren();

        foreach (MarginContainer item in item_boxes)
        {
            var index = item_boxes.IndexOf(item);
            if(index+item_start_index<item_boxes.Count)
            {
                item.Visible = true;
            }
            else{
                item.Visible = false;
            }

        }

        int change = basf.get_scroller_start_index_change(item_start_index,item_boxes.Count,render_items);
        if(change!=0 && can_change)
        {
            item_start_index+=change;
            can_change_timer.Start();
        }
    }

    public void Can_Change_Toggle(){
        can_change = true;
        can_change_timer.Stop();
    }
}
