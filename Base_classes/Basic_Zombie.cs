using Godot;
using System;
using System.Collections;



public class Basic_Zombie : Basic_Character
{

    public int distancing_error;

    // this will help us know all the attack a zombie have 
    // as different zombies will have their respective attack (attack_name)
    public ArrayList attack_move_names;

    public bool is_hitted; // to hit only once in a attack





    public override void _Ready()
    {
        base._Ready();

        _node_type = "zombie";

        colliding_condition = "player";
        speed_x = 2000;

        distancing_error = 0;

        attack_move_names = new ArrayList();

        is_hitted = false;

    }


    public override void _Process(float delta)
    {

        base._Process(delta);


        // this is the position of the defender of the game
        // on which our zombie will attack

        var player_global_position = player_variable.player_position;

        var player_x = player_global_position.x;

        this.animations.FlipH = player_x - distancing_error < this.Position.x;

        moving_speed.x = (moving_direction == Direction.Right) ? speed_x : -speed_x; // moving the zombie in the direction's

        moving_speed.x = (L_R_Colliding && moving_speed.x < 0 || R_R_Collding && moving_speed.x > 0) ? 0 : moving_speed.x;

        
        // setting the attack to the idle 
        // complete description can be founded in the respective class i.e set_animation_idle()
        if(attack_move_names.Contains(this.animations.Animation.ToLower())){
            var is_settled = set_animation_idle(this.animations.Animation);
            if(is_settled){
                is_hitted = false;
            }

        }


        if(!is_busy){

            if(moving_speed.x!=0 && animations.Animation!="Walk"){
                perform_move("Walk");
                is_busy = false;
            }
        }

        // GD.Print(this.power_available);


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
        if (!is_busy)
        {
            if (collided_one._node_type == "player")
            {

                string random_attack = (string)attack_move_names[Convert.ToInt32(GD.RandRange(0, attack_move_names.Count - 1))];

                if (can_perform_move(random_attack, true))
                {
                    // converting the first character of the string to the upper case
                    var edited_attack_name = $"{random_attack.ToUpper()[0]}{random_attack.Substring(1,random_attack.Length-1)}";

                    perform_move(edited_attack_name);

                }
            }
        }

        else{
            if(!is_hitted){
                Basic_Player character = collided_obj as Basic_Player;
                character.health-=available_moves_damage[available_moves.IndexOf(this.animations.Animation.ToLower())];
                is_hitted = true;
            }

        }


    }
}
