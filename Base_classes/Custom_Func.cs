using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class Custom_Func:Node2D
{

    public ArrayList get_the_collider_rays(string node_name)
    {
        var collider_rays = new ArrayList();
        var rays = GetNode<Node2D>(node_name).GetChildren();
        foreach (RayCast2D item in rays)
        {
            item.Enabled = true;
            collider_rays.Add(item);
        }
        return collider_rays;
    }

    public Timer create_timer(int wait_time, string signal_func_name)
    {
        var new_timer = new Timer();
        new_timer.WaitTime = 1;
        this.AddChild(new_timer);
        new_timer.Stop();
        new_timer.Connect("timeout", this, signal_func_name);
        return new_timer;
    }

    

}