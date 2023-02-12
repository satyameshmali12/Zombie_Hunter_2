using Godot;

#region Description
/*

Make sure to give to use the load_description box before using any function
To use this component make sure to use to add a button named Show_Description_Button in the main parent scene
For adding a new line in the message so add \n as in the usual case or scenario

*/
    
#endregion

public class Desc : Node2D
{


    // to the toggle the descrpition of the items on the shop
    ReferenceRect description_rect;

    bool is_description_toggled = false;
    bool is_description_button_pressed = false;
    RichTextLabel description_text;
    Button show_description_button;
    AnimationPlayer description_transition;

    Basic_Func basf;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {

        // toggling the description
        if (show_description_button.Pressed)
        {
            if (description_transition.CurrentAnimation != "Show_Description")
            {
                description_transition.CurrentAnimation = "Show_Description";
            }
            if (!is_description_button_pressed)
            {
                is_description_button_pressed = true;

                toggle_description();
            }
        }

        else if (Input.IsActionJustPressed("Mouse_Left_Pressed") && is_description_toggled)
        {
            var mouse_pos = this.GetGlobalMousePosition();

            var m_x = mouse_pos.x;
            var m_y = mouse_pos.y;
            var d_x = description_rect.RectGlobalPosition.x;
            var d_y = description_rect.RectGlobalPosition.y;

            if (!(m_x > d_x && m_y > d_y && m_x < d_x + description_rect.RectSize.x && m_y < d_y + description_rect.RectSize.y))
            {
                toggle_description();
            }
        }

        else
        {
            is_description_button_pressed = false;
        }


    }

    /// <summary>Helps to toggle the description_box</summary>
    public void toggle_description()
    {
        if (!is_description_toggled)
        {
            description_transition.Play();
            is_description_toggled = true;
        }
        else
        {
            description_transition.PlayBackwards();
            is_description_toggled = false;
        }
    }
    
    ///<summary>Make sure to run this function as a constructor of the program This function loads all the stuff for the program</summary>
    public void load_description_box()
    {

        basf = new Basic_Func(this);
        var second_parent = this.GetNode("Description");
        var descripton_sprite = this.GetNode<Sprite>("Description");
        show_description_button = basf.global_Variables.current_scene.GetNode<Button>("Show_Description_Button");
        description_transition = this.GetNode<AnimationPlayer>("Description_Transition");
        description_rect = descripton_sprite.GetNode<ReferenceRect>("Description_S_P");
        description_text = descripton_sprite.GetNode<RichTextLabel>("Description_Text");
    }

    /// <summary>Use this function fall back the description box</summary>
    public void close_desc()
    {
        if (is_description_toggled)
        {
            toggle_description();
        }
    }

    ///<summary>This helps in setting the text or the message of the description box</summary>
    public void reload_text(string text)
    {
        description_text.Clear();
        System.Collections.ArrayList lines = basf.get_format_array_string(text,new System.Collections.ArrayList(){"\\n","//n"},true);
        foreach (string item in lines)
        {
            description_text.AddText(item.Trim());
            description_text.Newline();
        }
    }
}
