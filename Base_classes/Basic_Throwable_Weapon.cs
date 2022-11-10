#region Description

//for using this class refer the bullet component of the game
// all the things should like the bullet class as well as the bullet scene respectively
#endregion


using Godot;

public class Basic_Throwable_Weapon:Area2D,Global_Variables_F_A_T
{
    public string _node_type{get;set;}

    [Export]
    public int damage=0; // the damage to given after hitting a enemy black or a particular in the game

    [Export]
    public int weapon_speed = 0; // change this speed to make the move faster

    Vector2 moving_speed=Vector2.Zero; // setting the initial speed to zero
    // this is concurrently depended on the moving_speed

    bool is_collided;



    Particles2D collision_animation;
    AnimatedSprite animation;


    public override void _Ready()
    {
        this.SetAsToplevel(true);// setting the weapon to inherit the character from his parent
        
        _node_type = "throwable_weapon";

        is_collided = false;

        animation = GetNode<AnimatedSprite>("Animation");

        
        var new_timer = create_timer(10,"Life_Finished");

        this.Connect("body_entered",this,"Collided_With_An_Obj");

        collision_animation = GetNode<Particles2D>("Collision_Animation");


    }


    public  override void _Process(float delta)
    {

        if (!is_collided)
            this.Position += moving_speed;

        if(is_collided && !collision_animation.Emitting){
            Life_Finished();
        }

    }



    // once the object life time is finished
    public virtual void Life_Finished(){
        this.QueueFree();
    }

    
    public virtual void Collided_With_An_Obj(Node2D body)
    {

        is_collided = true;
        this.animation.Visible = false;
        collision_animation.Emitting = true;
        
    }


    // this function will help us to set the position of the weapon
    public virtual void spawn_weapon(Vector2 position,Direction dir,bool is_to_adjust=false){
        if(is_to_adjust){
            this.Position = position + new Vector2((dir==Direction.Right)?-50:-300,-50);
        }
        else{
            this.Position = position;
        }

        var bullet_animation = this.GetNode<AnimatedSprite>("Animation");
        bullet_animation.FlipH = (dir==Direction.Right)?false:true;
        moving_speed = (dir==Direction.Right)?new Vector2(weapon_speed,0):new Vector2(-weapon_speed,0);   

    }

    public Timer create_timer(int wait_time, string signal_func_name)
    {
        var new_timer = new Timer();
        new_timer.WaitTime = 1;
        this.AddChild(new_timer);
        new_timer.Stop();
        new_timer.Connect("timeout", this, signal_func_name);
        return new_timer;
    }
}
