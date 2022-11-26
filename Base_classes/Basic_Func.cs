using Godot;
using System.Collections;

public class Basic_Func
{
    public Node node;

    public Data_Manager dm;

    public Global_Variables global_Variables;

    public Basic_Func(Node node){
        this.node = node;
        dm = new Data_Manager();

        global_Variables = node.GetNode<Global_Variables>("/root/Global_Variables");
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

    
    public ArrayList get_the_node_childrens(string node_name,bool is_ray_is_raycast2d=false)
    {
        var collider_rays = new ArrayList();
        var rays = node.GetNode<Node2D>(node_name).GetChildren();
        foreach (var item in rays)
        {
            collider_rays.Add(item);
            if(is_ray_is_raycast2d){
                RayCast2D ray = item as RayCast2D;
                ray.Enabled = true;
            }
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

    public bool clear_children_nodes(Node node_name){
        foreach (Node2D item in node_name.GetChildren())
        {
            item.QueueFree();
        }
        return true;
    }

    

}