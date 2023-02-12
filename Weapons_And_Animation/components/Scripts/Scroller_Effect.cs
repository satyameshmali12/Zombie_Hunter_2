using Godot;
using System.Collections;

public class Scroller_Effect : VBoxContainer
{
    Basic_Func basf;
    public int start_index = 0;
    int max_count = 0;
    ArrayList items = new ArrayList();
    bool can_change = true;
    Timer can_change_timer;
    public override void _Ready()
    {
        base._Ready();
        basf = new Basic_Func(this);
        can_change_timer =  basf.create_timer(.1f,"Can_Change_Toggle");
    }

    public override void _Process(float delta)
    {

        int change = basf.get_scroller_start_index_change(start_index,max_count,items);

        if(change!=0 && can_change)
        {
            start_index+=change;
            can_change = false;
            can_change_timer.Start();
        }

    }

    public void Can_Change_Toggle()
    {
        can_change = true;
        can_change_timer.Stop();
    }

    public void set_data(int max_count,ArrayList items){
        this.max_count = max_count;
        this.items = items;
    }
}
