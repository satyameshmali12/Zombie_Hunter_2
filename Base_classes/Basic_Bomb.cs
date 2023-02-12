using Godot;
using System.Collections;
using System;


/*
This class contains all the stuffs that a basic bom should have
*/
public class Basic_Bomb : Node2D, Global_Variables_F_A_T
{
    public _Type_of_ _node_type { get; set; }
    AnimatedSprite animation;

    ArrayList collision_rays;

    Basic_Func basf;

    AudioStreamPlayer2D explosion_sound;

    int bomb_per_damage;

    public string bomb_name = null; // this will be setted from right initial_faller

    ArrayList can_damage_to;
    // Vector2 throwing_power = new Vector2(100,100);

    // throwing power
    int x_t_p = 1000;
    int y_t_p = 1000;


    public override void _Ready()
    {
        _node_type = _Type_of_.Bomb;

        basf = new Basic_Func(this, "data//data_fields/bomb_data_fields.zhd");
        // basf.dm.load_data(this.Name);
        // here the node name is not used there can be multiple same bomb so there will be in change in the node name
        // since no node can have two children with same name
        basf.dm.load_data(bomb_name);
        var dm = basf.dm;

        x_t_p = basf.dm.get_integer_data("X_Throw_Intensity");
        y_t_p = basf.dm.get_integer_data("Y_Throw_Intensity");

        bomb_per_damage = Convert.ToInt32(basf.dm.get_data("Damage_Increment"));

        animation = GetNode<AnimatedSprite>("Movements");
        animation.Play();

        collision_rays = basf.get_the_node_childrens("Collision_Rays", true);
        basf.make_all_raycast2d_enable(collision_rays);

        explosion_sound = this.GetNode<AudioStreamPlayer2D>("Explosion_Sound");
        explosion_sound.Play();


        // It contains all the stuff which inherits the Character interface
        can_damage_to = new ArrayList() { _Type_of_.Player, _Type_of_.Zombie, _Type_of_.Drone };

    }

    public override void _Process(float delta)
    {
        if (animation.Frame >= animation.Frames.GetFrameCount("Explode") - 1)
        {
            this.QueueFree();
        }

        foreach (RayCast2D item in collision_rays)
        {
            if (item.IsColliding())
            {
                Global_Variables_F_A_T type = (Global_Variables_F_A_T)item.GetCollider();
                if (can_damage_to.Contains(type._node_type))
                {
                    if (type._node_type != _Type_of_.Drone)
                    {
                        // breaking the resistance of the character if can_be_breaken
                        Basic_Character basic_Character = type as Basic_Character;
                        if (basic_Character.can_be_resistance_breaken)
                        {
                            basic_Character.is_resisted = false;
                        }

                        /* throwing the character when it comes in the contact with the bomb */

                        var edited_x_throw = (item.GlobalPosition.x < item.GetCollisionPoint().x) ? x_t_p : -x_t_p;
                        basic_Character.moving_speed = new Vector2(edited_x_throw, -y_t_p);
                        basic_Character.move(true);
                    }
                    Character character = item.GetCollider() as Character;
                    character.change_health(-bomb_per_damage);
                    basf.global_Variables.score += bomb_per_damage;
                }
            }
        }
    }

    public virtual void update_logic(Data_Manager shop_data, Data_Manager user_data, Data_Manager throwable_weapons_dm)
    {


        var old_x_t_p = basf.dm.get_integer_data("X_Throw_Intensity");
        var old_y_t_p = basf.dm.get_integer_data("Y_Throw_Intensity");
        var x_t_p_increment = basf.dm.get_integer_data("X_Intensity_Increment_Per_Update");
        var y_t_p_increment = basf.dm.get_integer_data("Y_Intensity_Increment_Per_Update");

        var new_x_t_p = old_x_t_p + x_t_p_increment;
        var new_y_t_p = old_y_t_p + y_t_p_increment;

        shop_data.set_value("X_Throw_Intensity", new_x_t_p.ToString());
        shop_data.set_value("Y_Throw_Intensity", new_y_t_p.ToString());


    }
}