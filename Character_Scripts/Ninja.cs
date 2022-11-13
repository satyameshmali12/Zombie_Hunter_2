#region Description

#endregion


using Godot;
using System;
using System.Collections;


public class Ninja : Basic_Player
{

    int no_of_coll = 0;
    Kunai kunai; // this is the throw weapon of the player
    Timer Glide_Timer;
    bool is_gliding;


    public override void _Ready()
    {
        custom_constructor(700, 11000);

        is_busy = false;


        is_gliding = false;


        kunai = GetNode("Kunai") as Kunai;
        kunai.SetAsToplevel(true);
        kunai.Visible = false;


        available_moves = new ArrayList() { "attack", "climb", "death", "glide", "idle", "jump", "jump_attack", "run", "slide", "throw" };
        available_moves_consumption = new int[10] { 0, 0, 0, 2, 0, 0, 5, 0, 0, 10 };
        available_moves_damage = new int[10] { 20, 0, 0, 5, 0, 5, 50, 0, 3, 100 };

        Glide_Timer = this.create_timer(1, "Glide_Timer_Out");



    }

    public override void _Process(float delta)
    {
        basic_animation_changing_condition = !is_busy;

        custom_process(delta);

        // jump attack move of the ninja
        if (Input.IsActionJustReleased("Jump_Attack") && !is_on_ground && can_perform_move("Jump_Attack"))
        {
            perform_move("Jump_Attack");
            advanced_gravity = default_gravity;
        }

        else if (animations.Animation == "Jump_Attack" && is_on_ground)
        {
            is_busy = false;
        }

        set_animation_idle("Jump_Attack");


        // to throw the kunai
        if (Input.IsActionJustPressed("T") || animations.Animation == "Throw")
        {

            if (!is_gliding && can_perform_move("Throw", animations.Animation != "Throw"))
            {
                // GD.Print("hey I am here in the ninja's code right there..!!");
                var throw_frame_count = animations.Frame;
                var throw_total_frame = animations.Frames.GetFrameCount("Throw");


                // setting the is_busy to false ones the animation is finished
                if (throw_frame_count >= throw_total_frame - 1)
                {
                    is_busy = false;
                }

                // checking whether the kunai is visible or not
                // and if not then making it visible
                if (!kunai.Visible)
                {
                    is_busy = true;
                    if (animations.Animation != "Throw")
                    {
                        animations.Animation = "Throw";
                    }

                    if (throw_frame_count > throw_total_frame - 3)
                    {
                        kunai.Position = this.Position;
                        kunai.Visible = true;

                        // setting the direction of the kunai
                        kunai.Kunai_Texture.FlipV = animations.FlipH;
                    }
                }
            }

            is_gliding = false;
        }

        if (!is_gliding && !height_checker.IsColliding() && !is_on_ground && can_perform_move("Glide", false))
        {
            is_gliding = true;
            // is_busy = true;
            // animations.Animation = "Glide";
            perform_move("Glide");
            advanced_gravity = 50;
            Glide_Timer.Start();
        }
        if (!can_perform_move("Glide", false) && animations.Animation == "Glide")
        {
            set_animation_idle("Glide");
            set_gravity_default();
        }

        else if (is_gliding && is_on_ground || !is_gliding && advanced_gravity != default_gravity)
        {
            is_gliding = false;
            is_busy = false;
            advanced_gravity = default_gravity;
            Glide_Timer.Stop();
        }


        LinearVelocity = moving_speed;

    }

    public override void collided_with_body(Node body)
    {
        base.collided_with_body(body);
        Global_Variables_F_A_T new_node = (Global_Variables_F_A_T)body;

        if (new_node._node_type == "player")
        {
            body.QueueFree();
        }
    }

    public void Glide_Timer_Out()
    {
        if (animations.Animation == "Glide")
        {
            power_available -= 10;
        }
    }
}
