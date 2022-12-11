using Godot;
using System.Collections;
using System;



public class Basic_Bomb:Node2D,Global_Variables_F_A_T
{
    public _Type_of_ _node_type{get;set;}
    AnimatedSprite animation;

    ArrayList collision_rays;

    Basic_Func basf;

    AudioStreamPlayer2D explosion_sound;

    int bomb_per_damage;

    
    public override void _Ready()
    {
        _node_type = _Type_of_.Bomb;
        // GD.Print()
        basf = new Basic_Func(this,"data//data_fields/bomb_data_fields.zhd");
        basf.dm.load_data(this.Name);
        var dm = basf.dm;

        bomb_per_damage = Convert.ToInt32(basf.dm.get_data("Bomb_Per_Damage"));

        animation = GetNode<AnimatedSprite>("Animation");
        animation.Play();

        collision_rays = basf.get_the_node_childrens("Collision_Rays",true);
        basf.make_all_raycast2d_enable(collision_rays);

        explosion_sound = this.GetNode<AudioStreamPlayer2D>("Explosion_Sound");
        explosion_sound.Play();


    }

    public override void _Process(float delta)
    {
        if(animation.Frame >= animation.Frames.GetFrameCount("Explode")-1){
            this.QueueFree();
            // GD.Print("hey there bomb is been quee freed..!!");
        }

        foreach (RayCast2D item in collision_rays)
        {
            if(item.IsColliding()){
                Global_Variables_F_A_T node_type = (Global_Variables_F_A_T)item.GetCollider();
                if(node_type._node_type!=_Type_of_.Block){
                    Basic_Character character = item.GetCollider() as Basic_Character;
                    if(item.IsColliding() && !character.is_resisted){
                        // character.health-=bomb_per_damage;
                        character.change_health(-bomb_per_damage);
                        // item.Enabled = false;
                    }
                }
            }
        }

    }
}