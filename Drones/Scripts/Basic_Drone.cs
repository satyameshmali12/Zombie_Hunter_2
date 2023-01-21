#region Description
/*

Basic Drone Class 
this class will handle the basic functioning of all the drones
there may be also some ases that a drone will not follow the basic function


Fucntion of the Basic_Drone
to make it dead ones health become less than equal to zero
if wanted then moves to the target_position
falls the bomb (as per the data given) 
plays with the drone ini faller


Spawn Drone is one one of the important thing
this function will be runed right from the context where the drone is been used

*/
#endregion



using Godot;
using System;
using System.Collections;

public class Basic_Drone : Item_Using_Menu_Component, Character
{

    public _Type_of_ _node_type { get; set; }
    public int health { set; get; }

    public AnimatedSprite movements;
    Particles2D death_animation;
    bool is_death = false;
    public Data_Manager drone_data;

    bool is_bomb_restored = true;




    // setting the initial speed to zero
    public Vector2 speed = Vector2.Zero;

    // Both of this field will be taken right from the drone_data
    public int speed_x = 0; // the speed with which the drone will move horizontally
    public int speed_y = 0; // the speed with which the drone will move vertically
    public bool have_reached_target_position = false;


    #region Bomb_Ini_Faller
    // b_d stands for basic_drone
    // for drone_ini_faller
    // bomb name will only be provided only when it can fall the bomb
    public bool is_can_fall_bomb = false;
    public string bomb_name = null;
    public bool is_to_follow_basic_b_d_rules;
    public int max_travelling_distance;
    public int ini_faller_y_speed;

    public int number_of_bursting = 1;
    #endregion



    public float bomb_drop_interval = 1;
    Timer bomb_drop_timer;


    public bool task_complete, is_kill_timer_started = false;
    Timer kill_timer;


    public string death_explosion_name = "Two_Explosion";




    public ArrayList can_collide_with { get; set; }

    // the one who have spawned that will be the parent but not where the drone is been added 


    // for Movement AI
    public RayCast2D left_collision_ray, right_collision_ray, ground_collision_ray;
    public ArrayList left_movement_rays, right_movement_rays;
    public bool is_to_follow_ai_movement = true;
    public bool is_transioning_vertically = false;

    public Timer vertical_transion_timer;
    public Timer max_screen_death_remain_timer;

    // public string name = null;

    public int collision_damage = 10;

    public bool parent_leaved_scene_true = false;

    public Basic_Drone()
    {
        warning_message = "May adding this now can be fatal for others!";
    }


    public override void _Ready()
    {
        base._Ready();

        thumbnail_url = "res://.import/white_shader.png-91869615b3668e899040d115f94a0fc9.stex";

        _node_type = _Type_of_.Drone;
        can_collide_with = new ArrayList() { _Type_of_.Player, _Type_of_.Zombie, _Type_of_.Throwable_Weapon };
        health = 100;



        this.Monitoring = true;

        this.Connect("body_entered", this, "Collided_With_Object");
        movements = this.GetNode<AnimatedSprite>("Movements");

        death_animation = this.GetNode<Particles2D>("Death_Animation");

        // drone_data = new Data_Manager("data/data_fields/drone_data_fields.zhd");
        drone_data = dm;
        // drone_data.load_data(this.Name);
        // GD.Print("drone name right from the basic drone is: ", name);
        // drone_data.load_data(name);
        // setting the item_name to the drone name
        item_name = name;

        speed_x = Convert.ToInt32(drone_data.get_data("Drone_Speed_X"));
        speed_y = Convert.ToInt32(drone_data.get_data("Drone_Speed_Y"));


        number_of_bursting = Convert.ToInt32(drone_data.get_data("No_Of_Bursting"));
        max_travelling_distance = Convert.ToInt32(drone_data.get_data("Ini_Faller_Range"));
        ini_faller_y_speed = drone_data.get_integer_data("Ini_Faller_Y_Speed");

        bomb_drop_timer = basf.create_timer(bomb_drop_interval, "Bomb_Restored");
        bomb_drop_timer.Start();

        kill_timer = basf.create_timer(3, "Kill_Drone");
        max_screen_death_remain_timer = basf.create_timer(5, "Remove_Instantly");


        left_collision_ray = this.GetNode<RayCast2D>("Left_Collision_Ray");
        right_collision_ray = this.GetNode<RayCast2D>("Right_Collision_Ray");
        ground_collision_ray = this.GetNode<RayCast2D>("Ground_Collision_Rays");

        vertical_transion_timer = basf.create_timer(2, "Over_Vertical_Transion");

        collision_damage = Convert.ToInt32(drone_data.get_data("Collision_Damage"));

        parent.Connect("tree_exiting", this, "Parent_Died_Kill_Drone");

        is_to_display_position = true;


    }

    public override void _Process(float delta)
    {

        base._Process(delta);

        this.Position += speed;

        if (health <= 0)
        {


            max_screen_death_remain_timer.Start();


            if (is_death && !death_animation.Emitting)
            {
                var death_explosion = basf.get_the_packed_scene($"res://Bomb's/Scenes/{death_explosion_name}.tscn").Instance<Basic_Bomb>();
                death_explosion.bomb_name = death_explosion_name;
                death_explosion.Position = this.Position;
                this.QueueFree();
                this.basf.global_Variables.level_scene.AddChild(death_explosion);
            }


            if (!is_death)
            {
                if (movements.Frames.Animations.Contains("Death"))
                {
                    movements.Animation = "Death";
                }
                death_animation.Emitting = true;
                is_death = true;
            }
        }

        if (task_complete && !is_kill_timer_started)
        {
            kill_timer.Start();
            is_kill_timer_started = true;
        }

        if (left_collision_ray.IsColliding() || right_collision_ray.IsColliding())
        {
            L_R_Ray_Collided();
        }

        if (is_to_follow_ai_movement)
        {
            if ((left_collision_ray.IsColliding() && speed.x < 0 || right_collision_ray.IsColliding() && speed.x > 0) && !is_transioning_vertically)
            {

                var down_speed = 0f;

                if (!ground_collision_ray.IsColliding())
                {
                    down_speed = speed_y;
                }
                else
                {
                    down_speed = -speed_y;
                }

                speed = new Vector2(speed.x, down_speed);
                is_transioning_vertically = true;
                vertical_transion_timer.Start();
            }

            // if(ground_collision_ray.IsColliding()){
            //     speed = new Vector2(speed.x,-speed_y);
            // }
        }
    }

    public virtual void Collided_With_Object(Node body)
    {
        change_health(-collision_damage);
    }




    /// <summary>This function the makes the drone only to follow the target position horizontally i.e along x-axis</summary>
    public void move_to_target_position()
    {
        var x_speed = 0f;

        if (Math.Abs(this.Position.x - target_position.x) > 10)
        {
            x_speed = (this.Position.x > target_position.x) ? -speed_x : speed_x;
            have_reached_target_position = false;
        }
        else
        {
            have_reached_target_position = true;
        }
        speed = new Vector2(x_speed, speed.y);
    }

    public virtual bool drop_bomb()
    {
        if (is_can_fall_bomb && is_bomb_restored)
        {

            var drone_ini_faller = basf.get_the_packed_scene(basf.global_paths.Drone_Ini_Faller_Url).Instance<Drone_Ini_Faller>();
            drone_ini_faller.bomb_name = bomb_name;
            drone_ini_faller.number_of_bursting = number_of_bursting;
            drone_ini_faller.max_travelling_distance = max_travelling_distance;
            // drone_ini_faller.speed = new Vector2(0, Convert.ToInt32(drone_data.get_data("Ini_Faller_Y_Speed")));
            drone_ini_faller.speed = new Vector2(0, ini_faller_y_speed);

            drone_ini_faller.Position = this.Position + new Vector2(0, 30);

            basf.global_Variables.level_scene.AddChild(drone_ini_faller);
            is_bomb_restored = false;
            return true;
        }

        return false;
    }

    public void Bomb_Restored()
    {
        is_bomb_restored = true;
    }

    public virtual void Kill_Drone()
    {
        this.health = 0;
    }


    public bool change_health(int change)
    {
        health += change;
        return true;
    }

    public void Over_Vertical_Transion()
    {
        speed = new Vector2(speed.x, 0);
        is_transioning_vertically = false;
        vertical_transion_timer.Stop();
    }



    public virtual void L_R_Ray_Collided()
    {


    }

    public void Remove_Instantly()
    {
        Kill_Drone();
        is_death = true;
        death_animation.Emitting = false;
    }

    public void Parent_Died_Kill_Drone()
    {
        parent_leaved_scene_true = true;
        this.QueueFree();
        GD.Print("hello world right from the basic drone parnet died kill drone");
    }




    // update logic for the all the drones
    public void update_logic(Data_Manager shop_data, Data_Manager user_data, Data_Manager throwable_weapon_dm)
    {

        int new_drone_speed_x = shop_data.get_integer_data("Drone_Speed_X_Increment") + shop_data.get_integer_data("Drone_Speed_X");
        int new_drone_speed_y = shop_data.get_integer_data("Drone_Speed_Y_Increment") + shop_data.get_integer_data("Drone_Speed_Y");
        int new_ini_faller_range = shop_data.get_integer_data("Ini_Faller_Range_Increment") + shop_data.get_integer_data("Ini_Faller_Range");
        int new_drone_ini_faller_speed = shop_data.get_integer_data("Ini_Faller_Y_Speed_Increment") + shop_data.get_integer_data("Ini_Faller_Y_Speed");
        int new_collision_damage = shop_data.get_integer_data("Collision_Damage_Increment") + shop_data.get_incre("Collision_Damage");

        string[] settling_field_names = new string[5] { "Drone_Speed_X", "Drone_Speed_Y", "Ini_Faller_Range", "Ini_Faller_Y_Speed", "Collision_Damage" };
        int[] settling_field_values = new int[5] { new_drone_speed_x, new_drone_speed_y, new_ini_faller_range, new_drone_ini_faller_speed, new_collision_damage };
        shop_data.set_value<int>(settling_field_names, settling_field_values);


    }

    public void perform_move(string move_name)
    {
        movements.Animation = move_name;
    }

    public override void use_item(Item_Using_Menu menu, Basic_Func basf)
    {
        base.use_item(menu, basf);
        menu.Visible = false;
    }

    public override void spawn_item(Vector2 spawn_position, Vector2 target_position, Basic_Character parent, Basic_Func basf)
    {
        base.spawn_item(spawn_position, target_position, parent, basf);

        // this.Position = spawn_position;
        // this.target_position = target_position;
        // this.parent = parent;
    }
    public override void add_to_scene(Basic_Func basf)
    {
        base.add_to_scene(basf);
        basf.global_Variables.level_scene.AddChild(this);
    }

    public override void Clear()
    {
        base.Clear();
        this.health = 0;
    }

}
