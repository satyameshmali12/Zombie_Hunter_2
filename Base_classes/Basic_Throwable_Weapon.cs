#region Description

//for using this class refer the bullet component of the game
// all the things should like the bullet class as well as the bullet scene respectively
#endregion


using Godot;

public class Basic_Throwable_Weapon : Area2D, Global_Variables_F_A_T
{
    public _Type_of_ _node_type { get; set; }

    [Export]
    public int damage = 0; // the damage to given after hitting a enemy black or a particular in the game

    [Export]
    public int weapon_speed = 0; // change this speed to make the move faster

    Vector2 moving_speed = Vector2.Zero; // setting the initial speed to zero
    // this is concurrently depended on the moving_speed

    bool is_collided;

    Basic_Func basf;



    Particles2D collision_animation;
    AnimatedSprite animation;

    AudioStreamPlayer2D collision_sound, shoot_sound;


    public int shoot_sound_duration = 1;
    public float collision_sound_start_point = .5f;
    public int max_screen_time = 10;


    public override void _Ready()
    {
        this.SetAsToplevel(true);// setting the weapon to inherit the character from his parent

        _node_type = _Type_of_.Throwable_Weapon;

        basf = new Basic_Func(this);

        is_collided = false;

        animation = GetNode<AnimatedSprite>("Animation");

        // the max time a weapon can stay on the screen
        var new_timer = basf.create_timer(max_screen_time, "Life_Finished");
        new_timer.Start();

        this.Connect("body_entered", this, "Collided_With_An_Obj");

        collision_animation = GetNode<Particles2D>("Collision_Animation");


        collision_sound = this.GetNode<AudioStreamPlayer2D>("Bullet_Collision_Sound");
        shoot_sound = this.GetNode<AudioStreamPlayer2D>("Shoot_Sound");


        // playing the shooting sound with the help of the basf function
        basf.create_a_sound(shoot_sound.Stream.ResourcePath, this, true, shoot_sound_duration);

    }


    public override void _Process(float delta)
    {

        if (!is_collided)
            this.Position += moving_speed;

        if (is_collided && !collision_animation.Emitting)
        {
            Life_Finished();
        }

    }



    // once the object life time is finished
    public virtual void Life_Finished()
    {
        this.QueueFree();
    }


    public virtual void Collided_With_An_Obj(Node2D body)
    {

        Global_Variables_F_A_T collided_body = body as Global_Variables_F_A_T;
        if (this.animation.Visible)
        {
            if (collided_body._node_type == _Type_of_.Zombie)
            {
                Basic_Character collided_player = collided_body as Basic_Character;

                collided_player.change_health(-(damage + (int)(weapon_speed / 100 * 20)));
            }
            else if (collided_body._node_type == _Type_of_.Block)
            {
                TileMap tileMap = body as TileMap;
                var tiles_position = tileMap.GetUsedCells();
                // foreach (Vector2 item in tiles_position)
                // {
                //     // GD.Print($"x:- {item.x}, y:- {item.y}");
                // }
                // var tile_position = tileMap.MapToWorld(this.Position);
                // tileMap.TileSet.RemoveTile(tileMap.GetCell((int)tile_position.x,(int)tile_position.y));
            }
        }
        is_collided = true;
        this.animation.Visible = false;
        collision_animation.Emitting = true;


        // playing the collision sound from a specific point of it
        collision_sound.Play(collision_sound_start_point);




    }


    // this function will help us to set the position of the weapon
    // the arguments i.e. left , right and the height change are the one which helps to adjust the postion of the bullet as per per the character settings 3
    public virtual void spawn_weapon(Vector2 position, Direction dir, int left_change, int right_change, int height_change, bool is_to_adjust = true)
    {
        if (is_to_adjust)
        {
            // this.Position = position + new Vector2((dir==Direction.Right)?-50:-300,-50);
            this.Position = position + new Vector2((dir == Direction.Right) ? left_change : right_change, height_change);
        }
        else
        {
            this.Position = position;
        }

        var bullet_animation = this.GetNode<AnimatedSprite>("Animation");
        bullet_animation.FlipH = (dir == Direction.Right) ? false : true;
        moving_speed = (dir == Direction.Right) ? new Vector2(weapon_speed, 0) : new Vector2(-weapon_speed, 0);

    }
    
}
