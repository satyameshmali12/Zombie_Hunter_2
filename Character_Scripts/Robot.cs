using Godot;
using System;
using System.Collections;

public class Robot : Basic_Character
{

    PackedScene bullet_scene;

    Timer bullet_timer;

    bool is_can_shoot;

    public override void _Ready()
    {
        custom_constructor(700, 9000, basic_attack_name: "Melee");
        is_busy = false;
        available_moves = new ArrayList() { "death", "idle", "jump", "jump_melee", "jump_shoot", "melee", "run", "run_shoot", "shoot", "slide" };
        available_moves_consumption = new int[10] { 0, 0, 0, 10, 15, 10, 0, 10, 8, 0};

        bullet_scene = ResourceLoader.Load<PackedScene>("res://Weapons_And_Animation/scenes/Bullet.tscn");

        bullet_timer = GetNode<Timer>("Bullet_Timer");


    }


    public override void _Process(float delta)
    {
        basic_animation_changing_condition = !is_busy;

        custom_process(delta);

        if (Input.IsActionPressed("F") && !is_on_ground && can_perform_move("Jump_Melee".ToLower()))
        {
            perform_move("Jump_Melee");
        }
        // else if (!Input.IsActionPressed("F") && animations.Animation == "Jump_Melee")
        // {
        //     set_animation_idle("Jump_Melee");
        // }

        var shoot_pressed = Input.IsActionPressed("S");
        var shooting_condition = animations.Animation!="Shoot" && animations.Animation!="Jump_Shoot" && animations.Animation!="Run_Shoot";

        if (Input.IsActionPressed("Shift") && shoot_pressed && shooting_condition)
        {
            add_new_bullet_action(is_on_ground,"Shoot");
            add_new_bullet_action(!is_on_ground,"Jump_Shoot");
        }

        if(moving_speed.x!=0 && is_on_ground && shoot_pressed && shooting_condition && can_perform_move("Run_Shoot".ToLower())){
            perform_move("Run_Shoot");
            add_new_bullet(20);
        }

        // to set the animation idle in the the following animation ends
        // description of the this method can be founded in the basic_character class
        set_animation_idle("Shoot");
        set_animation_idle("Jump_Shoot");
        set_animation_idle("Run_Shoot");
        set_animation_idle("Jump_Melee");

        LinearVelocity = moving_speed;

    }
    public override void collided_with_body(Node body)
    {
        base.collided_with_body(body);

        Global_Variables_F_A_T old_tile = (Global_Variables_F_A_T)body;

        if (old_tile._node_type == "block")
        {


        }

    }

    void add_new_bullet(int speed_increment=0)
    {
        if(true){
            var bullet = (Bullet)bullet_scene.Instance();
            bullet.weapon_speed = (is_on_ground)?15:30;
            bullet.weapon_speed+=speed_increment;
            bullet.spawn_weapon(this.Position, moving_direction, true);
            this.AddChild(bullet);
        }
    }


    public void add_new_bullet_action(bool is_on_or_not_on_ground,string move_name){
        if(is_on_or_not_on_ground && can_perform_move(move_name.ToLower())){
            perform_move(move_name);
            add_new_bullet();
        }
    }
}



