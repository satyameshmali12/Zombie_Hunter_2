using Godot;
using System;
using System.Collections;



public class Basic_Zombie : Basic_Character
{

    public int distancing_error;

    // this will help us know all the attack a zombie have 
    // as different zombies will have their respective attack (attack_name)

    public bool is_obstruction_in_between;

    public ArrayList attack_move_names;


    // to check the left and right upward collision 
    // will help us to train our zombie AI
    RayCast2D Left_Upward_Ray, Right_Upward_Ray, Upward_Ray;

    // this two array will help us to detect the block forward or backward the character
    // will help us to train the AI of the Zombie..
    ArrayList Fall_Down_Left_Rays, Fall_Down_Right_Rays;

    public bool can_jump_over,is_attacking,is_on_edge;


    
    public override void _Ready()
    {
        base._Ready();

        _node_type = _Type_of_.Zombie;

        colliding_condition = "player";
        speed_x = 2000;

        distancing_error = 0;

        attack_move_names = new ArrayList();

        is_hitted = false;

        Left_Upward_Ray = GetNode<RayCast2D>("Left_Upward_Ray");
        Right_Upward_Ray = GetNode<RayCast2D>("Right_Upward_Ray");
        Upward_Ray = GetNode<RayCast2D>("Upward_Ray");

        is_obstruction_in_between = false;

        jump_intensity = 2000;

        colliding_condition = "all";


        // gettting all the Left and Right Fall down rays which fill help us to detect were the zombie should walk and where not
        // the stronger the zombie the stronger will be it's range of the Left and Right Fall Down Rays....
        Fall_Down_Left_Rays = get_the_collider_rays("Fall_Down_Left_Rays");
        Fall_Down_Right_Rays = get_the_collider_rays("Fall_Down_Right_Rays");


        // for powerfull zombie only
        // not every zombie can jump above a another zombie
        can_jump_over  = false;
        // is_attacking = false;

        health_bar = GetNode<ProgressBar>("Health_Bar");

    }


    public override void _Process(float delta)
    {

        base._Process(delta);


        // this is the position of the defender of the game
        // on which our zombie will attack

        var player_global_position = player_variable.player_position;

        var player_x = player_global_position.x;

        var is_left_rays_colliding = this.is_collider_ray_colliding(Left_Collision_Rays);
        var is_right_rays_colliding = this.is_collider_ray_colliding(Right_Collision_Rays);

        var player_configured_position = player_x - distancing_error;
        if (Math.Abs(player_configured_position - this.Position.x) > 10)
        {
            this.animations.FlipH = player_configured_position < this.Position.x;

            moving_speed.x = (moving_direction == Direction.Right) ? speed_x : -speed_x; // moving the zombie in the direction's
        }

        if (is_obstruction_in_between)
        {
            var is_left_upward_ray_colliding = Left_Upward_Ray.IsColliding();

            var is_right_upward_ray_colliding = Right_Upward_Ray.IsColliding();


            if (!is_left_upward_ray_colliding && moving_speed.x < 0 || !is_right_upward_ray_colliding && moving_speed.x > 0)
            {
                if (is_on_ground)
                {
                    moving_speed.y = -jump_intensity;
                }

            }
        }



        if (!L_R_Colliding && moving_speed.x < 0 || !R_R_Collding && moving_speed.x > 0)
        {
            if (is_obstruction_in_between && !is_on_ground)
            {
                is_obstruction_in_between = false;
            }
        }


        // checking whether the zombie is on the edge or not
        // this will help the zombie to make itselt safe to that of the fall damage

        var is_left_fall_ray_colliding = is_collider_ray_colliding(Fall_Down_Left_Rays);
        var is_right_fall_ray_colliding = is_collider_ray_colliding(Fall_Down_Right_Rays);
        
        is_on_edge = !is_left_fall_ray_colliding && moving_speed.x < 0 || !is_right_fall_ray_colliding && moving_speed.x > 0;

        moving_speed.x = (is_on_edge) ? 0 : moving_speed.x;



        // making the zombie to be idle if it can't move forward
        // Idle and Walk are the one which doesn't make's the is_busy to true
        if(!is_busy){
            if(moving_speed.x==0){
                perform_move("Idle");
            }
            else{
                perform_move("Walk");
            }
            is_busy = false;
        }

        
    }

    public override void collided_with_body(Node body)
    {
        base.collided_with_body(body);

        Global_Variables_F_A_T collided_body = body as Global_Variables_F_A_T;

    }


    public override void collided_with_L_R_ray(Godot.Object collided_obj)
    {
        base.collided_with_L_R_ray(collided_obj);

        Global_Variables_F_A_T collided_one = collided_obj as Global_Variables_F_A_T;

        if(is_on_ground && collided_one._node_type==_Type_of_.Zombie && can_jump_over){
            if(L_R_Colliding && moving_speed.x<0 || R_R_Collding && moving_speed.x>0){
                jump(jump_intensity-500);
            }
        }


        if (collided_one._node_type == _Type_of_.Block)
        {
            // GD.Print("hey there I am collide..!!");
            is_obstruction_in_between = true;
        }

        if (!is_busy)
        {
            if (collided_one._node_type == _Type_of_.Player && !is_on_edge) 
            {

                string random_attack = (string)attack_move_names[Convert.ToInt32(GD.RandRange(0, attack_move_names.Count - 1))];

                if (can_perform_move(random_attack, true))
                {
                    // is_attacking = true;
                    // converting the first character of the string to the upper case
                    var edited_attack_name = $"{random_attack.ToUpper()[0]}{random_attack.Substring(1, random_attack.Length - 1)}";

                    perform_move(edited_attack_name);

                }
            }
        }

        else
        {
            if (!is_hitted)
            {
                if (collided_one._node_type == _Type_of_.Player)
                {
                    Basic_Player character = collided_obj as Basic_Player;
                    character.health -= available_moves_damage[available_moves.IndexOf(this.animations.Animation.ToLower())];
                    is_hitted = true;
                }
            }

        }
    }
}
