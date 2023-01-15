using Godot;
using System.Collections;

public class Notification : Node2D
{
    Basic_Func basf;

    Button confirm_button,cancel_button;
    public bool is_denied = false;
    RichTextLabel label;

    public override void _Ready()
    {
        basf = new Basic_Func(this);
        label = this.GetNode<RichTextLabel>("Message");
        confirm_button = this.GetNode<Button>("Confirm");
        cancel_button = this.GetNode<Button>("Cancel");
    }

    public override void _Process(float delta)
    {

        if(confirm_button.Pressed || cancel_button.Pressed)
        {
            is_denied = cancel_button.Pressed;
            close();
        }
        

    }

    public void pop(string message)
    {
        label.Clear();
        ArrayList lines = basf.get_format_array_string(message,new ArrayList(){"\\n","//n"},true);
        
        foreach (var item in lines)
        {
            label.AddText(item.ToString());
            label.Newline();
        }
        this.Visible = true;
    }
    public void close()
    {
        this.Visible = false;
    }
    public void reset()
    {
        is_denied = false;
    }
}
