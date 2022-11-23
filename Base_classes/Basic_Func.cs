using Godot;
using System.Collections;

public class Basic_Func
{
    public Node node;

    public Basic_Func(Node node){
        this.node = node;
    }

    public Timer create_timer(int wait_time, string signal_func_name)
    {
        var new_timer = new Timer();
        new_timer.WaitTime = 1;
        node.AddChild(new_timer);
        new_timer.Stop();
        new_timer.Connect("timeout", node, signal_func_name);
        return new_timer;
    }

    
    public ArrayList get_the_collider_rays(string node_name)
    {
        var collider_rays = new ArrayList();
        var rays = node.GetNode<Node2D>(node_name).GetChildren();
        foreach (var item in rays)
        {
            collider_rays.Add(item);
        }
        return collider_rays;
    }

    public bool make_all_raycast2d_enable(ArrayList arr)
    {
        try
        {
            foreach (RayCast2D item in arr)
            {
                item.Enabled = true;
            }
            return true;
        }
        catch (System.Exception)
        {
            throw new System.InvalidCastException("Check for you node type may it differs..!!");
        }
    }

}