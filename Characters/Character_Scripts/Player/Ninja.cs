#region Description

#endregion


using Godot;
using System;
using System.Collections;

/*

Movements
press f to fight
jump and press f to jump_attack
press s to throw weapon
press t to open parachute (only when player is above of the height checker range) 

*/


public class Ninja : Basic_Player
{

    int no_of_coll = 0;
    Kunai kunai; // this is the throw weapon of the player
    Timer Glide_Timer;
    bool is_gliding,is_gliding_availed,is_to_set_glide_to_idle = false;



    public override void _Ready()
    {
        base._Ready();

        custom_constructor(700, 11000);

        is_busy = false;


        is_gliding = false;

        available_moves = new ArrayList() { "attack", "climb", "death", "glide", "idle", "jump", "jump_attack", "run", "slide", "shoot" };
        available_moves_consumption = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
        available_moves_damage = new int[10] { 20, 0, 0, 5, 0, 5, 50, 0, 3, 100 };
        settle_damage_increment_possible_moves(2);


        Glide_Timer  = basf.create_timer(1, "Glide_Timer_Out");


        // can_shoot = true;
        // bullet_scene = ResourceLoader.Load<PackedScene>("res://Weapons_And_Animation/scenes/Kunai.tscn");
        // b_rightchange = 100;
        // b_leftchange = -100;
        // b_height_change = 0;
        this.load_basic_weapon("Kunai",0,-100,100);


    }

    public override void _Process(float delta)
    {
        base._Process(delta);


        custom_process(delta);

        basic_animation_changing_condition = !is_busy;


        // jump attack move of the ninja
        if (Input.IsActionJustReleased("Jump_Attack") && !is_on_ground)
        {
            if(perform_move("Jump_Attack")){
                reset_para();
            }
        }

        else if (animations.Animation == "Jump_Attack" && is_on_ground)
        {
            set_animation_idle("Jump_Attack",is_to_immediately:true);
        }

        if(animations.Animation == "Shoot"){
            reset_para();
        }


        if(!is_gliding && !height_checker.IsColliding()){
            if(!is_gliding_availed || Input.IsActionPressed("T")){
                is_gliding_availed = true;
                is_gliding = true;
            }
        }

        else if(is_gliding){
            if(perform_move("Glide")){
                advanced_gravity = 50;
                if(Glide_Timer.IsStopped()){
                    Glide_Timer.Start();
                }
            }
        }
        if(height_checker.IsColliding()){
            reset_para();
            is_gliding_availed = false;
        }


        if(!can_perform_move("Glide",true) && current_move=="Glide" || is_to_set_glide_to_idle){
            is_to_set_glide_to_idle = true;
            if(set_animation_idle("Glide")){
                set_gravity_default();
                is_to_set_glide_to_idle = false;
            }
        }

        // player_variable.player_position = this.animations.Position;
        set_animation_idle("Jump_Attack");
        set_animation_idle("Run");
        set_animation_idle("Jump");
        set_animation_idle("Shoot");

        move();
    }

    public override void collided_with_body(Node body)
    {
        base.collided_with_body(body);
                
        Global_Variables_F_A_T new_node = (Global_Variables_F_A_T)body;

        if (new_node._node_type == _Type_of_.Player)
        {
            body.QueueFree();
        }
    }

    public void Glide_Timer_Out()
    {
        if (animations.Animation == "Glide")
        {
            power_available -= 4;
        }
    }

    public override bool disconnect_all_signals()
    {
        base.disconnect_all_signals();
        Glide_Timer.Disconnect("timeout",this,"Glide_Timer_Out");
        return true;
    }

    void reset_para(){
        is_gliding = false;
        advanced_gravity = default_gravity;
        Glide_Timer.Stop();
    }
}
