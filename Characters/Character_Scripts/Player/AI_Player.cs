using Godot;
using Godot.Collections;
using System.Collections;
using System;

public class AI_Player : Node2D
{
    Basic_Player parent_char;

    ProgressBar health_bar;

    Basic_Character target_enemy = null;

    ArrayList can_collide_to;

    Basic_Func basf;

    Vector2 last_position = Vector2.Zero;

    int rand_move_x_speed = 0;





    public override void _Ready()
    {
        basf = new Basic_Func(this);
        parent_char = this.GetParent<Basic_Player>();
        health_bar = this.GetNode<ProgressBar>("Health_Bar");

        can_collide_to = new ArrayList() { _Type_of_.Zombie };



    }

    public override void _Process(float delta)
    {

        this.Visible = parent_char.is_AI;

    }

    public void AI_FUNC()
    {

        if (this.Visible)
        {
            health_bar.Value = parent_char.health;
            Godot.Collections.Array rays = this.GetNode<Node2D>("Zombie_Detection_Rays").GetChildren();
            foreach (RayCast2D item in rays)
            {
                Godot.Object collider = item.GetCollider();

                if (collider != null)
                {
                    Global_Variables_F_A_T type = collider as Global_Variables_F_A_T;
                    if (can_collide_to.Contains(type._node_type))
                    {
                        Basic_Character collided_character = (Basic_Character)type;
                        if (target_enemy != null)
                        {

                            var target_enemy_sep = basf.abs_subtraction(this.Position.x, target_enemy.Position.x);

                            var new_target_enemy_sep = basf.abs_subtraction(this.Position.x, collided_character.Position.x);

                            if (target_enemy_sep > 100 && new_target_enemy_sep < target_enemy_sep)
                            {
                                target_enemy = collided_character;
                            }

                        }
                        else if (collided_character != parent_char)
                        {
                            target_enemy = (Basic_Character)type;
                        }
                    }
                }
            }

            if (target_enemy != null)
            {
                if (!target_enemy.IsConnected("tree_exiting", this, "Leaving_Tree"))
                {
                    target_enemy.Connect("tree_exiting", this, "Leaving_Tree");
                }
                Vector2 e_pos = target_enemy.GlobalPosition;
                Vector2 p_pos = parent_char.GlobalPosition;
                float x_speed = (e_pos.x > p_pos.x) ? parent_char.speed_x : -parent_char.speed_x;
                parent_char.moving_speed = new Vector2(x_speed, parent_char.moving_speed.y);
                rand_move_x_speed = 0;

            }
            else{
                if(rand_move_x_speed==0)
                {
                    int dir = (int)GD.RandRange(0,2);
                    rand_move_x_speed = (dir==0)?parent_char.speed_x:-parent_char.speed_x;
                }
                parent_char.moving_speed.x = rand_move_x_speed;
            }
            
        }
    }



    public void Leaving_Tree()
    {
        target_enemy = null;
    }
}
