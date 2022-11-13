using Godot;
using System;
using System.Collections.Generic;

public class Laser_Bean : Node2D
{
    List<RayCast2D> collider_rays;
    RayCast2D testing_ray;
    public Particles2D bean_animation;

    public bool is_animation_performing;
    bool is_timer_started;

    // bool isis_first_time;

    Direction direction = Direction.Right;

    Timer emitter_setter_timer;
    public override void _Ready()
    {
        bean_animation = GetNode<Particles2D>("Bean_Animation");
        bean_animation.OneShot = true;

        // making the array of the ray
        collider_rays = new List<RayCast2D>();

        var ray_node = GetNode<Node2D>("Collision_Rays"); // parent node of all the collider rays



        for (int i = 0; i < ray_node.GetChildCount(); i++)
        {
            var count = i+1;
            var selected_ray = ray_node.GetNode<RayCast2D>($"Collision_Ray_{count}"); 
            collider_rays.Add(selected_ray);

        }



        testing_ray = GetNode<RayCast2D>("Testing_Ray");

        is_animation_performing = false;
        hide_beam(); // hiding the beam initially

        // perform here
        emitter_setter_timer = create_timer(3,"hide_beam");
        emitter_setter_timer.OneShot = true;
        emitter_setter_timer.Stop();

        is_timer_started = false;


    }

    public override void _Process(float delta)
    {
        if(!bean_animation.Emitting && !is_timer_started && is_animation_performing){
            emitter_setter_timer.WaitTime = 3;
            emitter_setter_timer.Start();
            is_timer_started = true;
        }

        // foreach (RayCast2D item in collider_rays)
        // {
        //     if(item.IsColliding()){

        //         var max_time = 7;
        //         var original_length = item.CastTo.x;
        //         var beam_pos_x = Math.Abs(bean_animation.Position.x);
        //         var collision_x = Math.Abs(item.GetCollisionPoint().x);

        //         var distance_from_point = Math.Abs(beam_pos_x-collision_x);

        //         var new_timing = max_time*distance_from_point/original_length;

        //         bean_animation.Lifetime = (new_timing>=1)?new_timing:1;
        //         GD.Print(new_timing);


        //     }
        // }


        // all the collision logic for the damge
        foreach (var item in collider_rays)
        {
            if(item.IsColliding()){
                Global_Variables_F_A_T collided_obj = (Global_Variables_F_A_T)item.GetCollider();
                GD.Print(collided_obj._node_type);
            }
        }
        
    }

    public void hide_beam(){
        bean_animation.Visible = false;
        make_rays_ena_or_disa(collider_rays,false);
        is_timer_started = false;
        is_animation_performing = false;
        GD.Print("hey there the beam is been hided");
    }

    public void perform_bean(Direction dir,Vector2 pos){
        bean_animation.Position = pos + new Vector2((dir==Direction.Right)?150:30,65);
        bean_animation.Visible = true;
        make_rays_ena_or_disa(collider_rays,true);
        // bean_animation.Amount = 800;
        is_animation_performing = true;

        bean_animation.Emitting = true;
        
        // making the bean and all the rays to revert in the direction of the player

        bean_animation.ProcessMaterial.Set("direction",new Vector3((dir==Direction.Right)?1:-1,0,0));
        foreach (var item in collider_rays)
        {
            var norm_val = Math.Abs(item.CastTo.x);
            
            // change the below value i.e 160 if any collision related error occurs
            // here the value 160 is founded experimental
            item.CastTo = new Vector2((dir==Direction.Right)?norm_val:-norm_val,item.CastTo.y);//changing the direction of the collider_rays
            item.Position = new Vector2((dir==Direction.Left)?pos.x:pos.x+160,item.Position.y);//changing the position of the collider_rays
        }

        

    }

    // void restart_setting(){
    //     // hide_beam()
    // }


    // ena_disa_stands for whether to disable the ray's or enable it
    // this function can also be called as the preprocessing before the beam in started
    // without making the beam collider_rays enable the beam will seem to be useless
    void make_rays_ena_or_disa(List<RayCast2D> collider_rays,bool ena_or_disa=true){
        foreach (var item in collider_rays)
        {
            item.Enabled = ena_or_disa;
        }
    }

    public Timer create_timer(int wait_time, string signal_func_name)
    {
        var new_timer = new Timer();
        new_timer.WaitTime = wait_time;
        this.AddChild(new_timer);
        new_timer.Stop();
        new_timer.Connect("timeout", this, signal_func_name);
        return new_timer;
    }
}
