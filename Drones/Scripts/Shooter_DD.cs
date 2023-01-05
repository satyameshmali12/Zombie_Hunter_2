using Godot;
using System;
using System.Collections;

/*

Until target is not given this drone just basically follows the player with a specific offset

*/


public class Shooter_DD : Basic_Drone
{

    Vector2 enemy_target_position;
    bool is_enemy_target_position_given;

    ArrayList shooting_rays;

    PackedScene bullet_scene;


    // this help us in tracking the number of bullet.
    // with this we can give the drone the reloading feature
    int number_of_bullet_used = 0;
    int max_number_of_bullet = 100;

    Timer shoot_bullet_timer;
    float shoot_bullet_interval;

    bool can_shoot = true;
    bool is_reloading = false;

    int speed_components = 15;

    ArrayList ray_used = new ArrayList();

    Timer ray_used_cleaning_timer;
    float ray_used_cleaning_interval = .5f;

    Timer reloading_timer;
    float reloading_interval = 6;

    bool is_y_speed_changed = false;

    RayCast2D height_checker_ray;
    

    public override void _Ready()
    {
        bomb_drop_interval = 1;
        drone_name = "Shooter_DD";

        base._Ready();

        enemy_target_position = Vector2.Zero;
        is_enemy_target_position_given = false;

        shooting_rays = basf.get_the_node_childrens("Shooting_Rays");

        bullet_scene = basf.get_the_packed_scene(basf.global_paths.Shooter_DD_Bullet_Url);

        shoot_bullet_interval = .1f;
        shoot_bullet_timer = basf.create_timer(shoot_bullet_interval, "Retrive_Bullet");

        ray_used_cleaning_timer = basf.create_timer(ray_used_cleaning_interval, "Clean_Used_Rays");
        ray_used_cleaning_timer.Start();

        reloading_timer = basf.create_timer(reloading_interval, "Reload_Bullets");

        height_checker_ray = this.GetNode<RayCast2D>("Height_Checker_Ray");

        

        // temporary code 
        // parent = basf.global_Variables.character_scene as Basic_Character;
        // GD.Print("Parent from the shooter_dd is :- ",parent);

    }


    public override void _Process(float delta)
    {
        base._Process(delta);

        // temporary code just for testing purpose
        if (parent == null && basf.global_Variables.character_scene != null)
        {
            parent = basf.global_Variables.character_scene as Basic_Character;
        }


        if (!is_enemy_target_position_given)
        {

            target_position = basf.global_Variables.character_scene.Position;
            move_to_target_position();

        }

        if(!is_transioning_vertically){

            if(!height_checker_ray.IsColliding()){
                speed = new Vector2(speed.x,speed_y);
                is_y_speed_changed = true;
            }
            else if(is_y_speed_changed){
                is_y_speed_changed = false;
                speed = new Vector2(speed.x,0);
            }

            



        }

        if (!is_reloading)
        {
            foreach (RayCast2D item in shooting_rays)
            {
                var collider = item.GetCollider();
                if (item.IsColliding() && !ray_used.Contains(item))
                {
                    shoot_bullet(item);
                }
            }
        }




    }

    public void shoot_bullet(RayCast2D item)
    {

        var start_position = item.GlobalPosition;
        var bullet_target_position = item.GetCollisionPoint();
        Global_Variables_F_A_T type = (Global_Variables_F_A_T)item.GetCollider();

        if (number_of_bullet_used < max_number_of_bullet && can_shoot && item.IsColliding() && type._node_type != _Type_of_.Block && type != parent)
        {
            ray_used.Add(item);
            var bullet = bullet_scene.Instance<Shooter_DD_Bullet>();

            var mid_point = new Vector2(start_position.x, bullet_target_position.y);

            var y_side_size = mid_point.y - start_position.y;
            var x_side_size = bullet_target_position.x - mid_point.x;


            var x_speed = x_side_size / speed_components;
            var y_speed = y_side_size / speed_components;

            GD.Print(x_speed, " ", y_speed);

            if (Math.Abs(x_speed) < 5 || Math.Abs(y_speed) < 5)
            {
                x_speed *= 2;
                y_speed *= 2;
            }

            bullet.moving_speed = new Vector2(x_speed, y_speed);
            bullet.Position = start_position;


            this.AddChild(bullet);
            number_of_bullet_used++;
            can_shoot = false;
            shoot_bullet_timer.Start();
        }

        else if (number_of_bullet_used >= max_number_of_bullet && !is_reloading)
        {
            reloading_timer.Start();
            is_reloading = true;
        }




    }

    public void Retrive_Bullet()
    {
        can_shoot = true;
        shoot_bullet_timer.Stop();
    }

    public void Clean_Used_Rays()
    {
        ray_used.Clear();
    }

    public void Reload_Bullets()
    {
        is_reloading = false;
        number_of_bullet_used = 0;
        Clean_Used_Rays();
        reloading_timer.Stop();
    }

}
