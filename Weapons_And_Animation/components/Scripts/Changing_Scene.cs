using Godot;
using System;

public class Changing_Scene : Control
{

    string url;
    public AnimationPlayer animation;
    Node parent;

    bool is_to_navigate;
    Basic_Func basf;

    public override void _Ready()
    {

        this.SetAsToplevel(true);
        basf = new Basic_Func(this);
        var timer = basf.create_timer(.1f,"timeout");
        timer.Start();
    }

    public override void _Process(float delta)
    {
        if(!animation.IsPlaying()){
            if(this.is_to_navigate)
                parent.GetTree().ChangeScene(url);
            else
                this.GetParent<Control>().Visible = true;
                this.QueueFree();
        } 
        
    }

    public void set_values(Node parent,string navigate_url,bool is_to_reverse = false,bool is_to_navigate = true){
        this.url = navigate_url;
        this.parent = parent;
        animation = this.GetNode<AnimationPlayer>("Up-Down");
        this.is_to_navigate = is_to_navigate;
        if(is_to_reverse){
            animation.CurrentAnimation="Closing";
        }
        else{
            animation.CurrentAnimation = "Opening";
        }
    }
    
    public void timeout(){
        this.GetParent<Control>().Visible = true;
        this.GetNode<Node2D>("Preview").Visible = false;

    }
}
