using Godot;
using System.Collections;



public class Basic_Bomb:Node2D
{
    AnimatedSprite animation;

    ArrayList collision_rays;

    Basic_Func basf;

    
    public override void _Ready()
    {
        basf = new Basic_Func(this);

        animation = GetNode<AnimatedSprite>("Animation");
        animation.Play();

        collision_rays = basf.get_the_node_childrens("Collision_Rays",true);
        basf.make_all_raycast2d_enable(collision_rays);

        

    }

    public override void _Process(float delta)
    {
        if(animation.Frame >= animation.Frames.GetFrameCount("Explode")-1){
            // this.QueueFree();
        }

        foreach (RayCast2D item in collision_rays)
        {
            Basic_Character character = item.GetCollider() as Basic_Character;
            if(item.IsColliding() && !character.is_resisted){
                character.health-=2;
                // item.Enabled = false;
            }
        }

    }
}