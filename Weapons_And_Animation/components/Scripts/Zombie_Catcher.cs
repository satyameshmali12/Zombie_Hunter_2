using Godot;
using Godot.Collections;
using System.Collections;

public class Zombie_Catcher : Node2D
{

    ArrayList catchedZombie = new ArrayList();
    ArrayList speedGivenCatchedZombie = new ArrayList();

    [Export]
    int speed_x = 5;
    [Export]
    int speed_y = 20;

    RayCast2D l_C_R, r_C_R, d_C_R, u_C_R;

    Vector2 mSpeed = Vector2.Zero;
    bool isCatchingZombie = false;

    bool isRandomSpeedGiven = false;


    Basic_Func basf;

    Vector2 startPoint, endPoint;

    Timer catchZombieTimer;

    ArrayList catchedZombieData = new ArrayList();

    Data_Manager zombieDataManager;

    bool isDataSaved = false;

    public override void _Ready()
    {
        l_C_R = this.GetNode<RayCast2D>("Movement_Rays/Left_Ray");
        r_C_R = this.GetNode<RayCast2D>("Movement_Rays/Right_Ray");
        d_C_R = this.GetNode<RayCast2D>("Movement_Rays/Down_Ray");
        u_C_R = this.GetNode<RayCast2D>("Movement_Rays/Up_Ray");

        basf = new Basic_Func(this);

        zombieDataManager = new Data_Manager(basf.global_paths.Zombie_Data_Field_Url);

        var scene = basf.global_Variables.level_scene;
        startPoint = scene.GetNode<Position2D>("Points/Start_Point").GlobalPosition;
        endPoint = scene.GetNode<Position2D>("Points/End_Point").GlobalPosition;

        catchZombieTimer = basf.create_timer(1, "CatchZombie");
    }

    public override void _Process(float delta)
    {

        var catcher_rays = this.GetNode<Node2D>("Catcher_Rays").GetChildren();


        // for catching the zombies
        foreach (RayCast2D catcher_ray in catcher_rays)
        {
            Godot.Object collider = catcher_ray.GetCollider();

            if (collider != null)
            {
                Global_Variables_F_A_T type = collider as Global_Variables_F_A_T;

                if (type._node_type == _Type_of_.Zombie)
                {
                    Basic_Zombie zombie = collider as Basic_Zombie;
                    if (zombie.is_death && !catchedZombie.Contains(zombie))
                    {
                        catchedZombie.Add(zombie);

                        // this makes the ship to stop for catching a zombie after a interval of one second
                        if (catchZombieTimer.IsStopped())
                        {
                            catchZombieTimer.Start();
                        }

                    }
                }
            }
        }

        // for giving a random movement and for determinig whether the drone is picking the zombie or not
        if (catchedZombie.Count == 0)
        {
            isCatchingZombie = false;
            if (!isRandomSpeedGiven)
            {
                // getting the zombie area
                Node2D zombie_area = basf.global_Variables.level_scene.GetNode<Node2D>("Zombie_Area");

                // getting the number of diedzombie in the zombie area
                int diedZombieCount = 0;
                foreach (Basic_Zombie zombie in zombie_area.GetChildren())
                {
                    diedZombieCount += (zombie.is_death)?1:0;
                }

                // giving a random speed if diedZombieCount is zero
                if (diedZombieCount==0)
                {
                    int randDir = (int)GD.RandRange(0, 2);
                    mSpeed.x = (randDir == 0) ? speed_x : -speed_x;
                    isRandomSpeedGiven = true;
                }
                // if died zombie count is not equal to zero then moving in the direction where more zombie are died
                else
                {
                    int leftDiedZombieCount = 0;

                    foreach (Basic_Zombie zombie in zombie_area.GetChildren())
                    {
                        if (zombie.is_death)
                        {
                            if (this.GlobalPosition.x > zombie.GlobalPosition.x)
                            {
                                leftDiedZombieCount++;
                            }
                        }
                    }
                    int rightDiedZombieCount = diedZombieCount-leftDiedZombieCount;

                    mSpeed.x = (leftDiedZombieCount>rightDiedZombieCount)?-speed_x:speed_x;
                }
            }
        }
        else
        {
            isRandomSpeedGiven = false;
        }


        if (!isCatchingZombie)
        {
            // changing the moving directionAsPer collision
            var leftCollider = l_C_R.GetCollider();
            var rightCollider = l_C_R.GetCollider();

            float distanceFromStartPoint = basf.abs_subtraction(this.GlobalPosition.x, startPoint.x);
            float distanceFromEndPoint = basf.abs_subtraction(this.GlobalPosition.x, endPoint.x);



            if (leftCollider != null)
            {
                Global_Variables_F_A_T type = leftCollider as Global_Variables_F_A_T;
                if (type._node_type == _Type_of_.Block)
                {
                    if (distanceFromStartPoint < 150 || this.GlobalPosition.x < startPoint.x)
                    {
                        mSpeed.x = speed_x;
                    }
                }
            }

            if (rightCollider != null)
            {

                Global_Variables_F_A_T type = leftCollider as Global_Variables_F_A_T;
                if (type._node_type == _Type_of_.Block)
                {
                    if (distanceFromEndPoint < 150 || this.GlobalPosition.x > endPoint.x - 200)
                    {
                        mSpeed.x = -speed_x;
                    }
                }
            }
            this.Position += mSpeed;
        }
        else
        {

            foreach (Basic_Zombie zombie in catchedZombie)
            {
                if (!speedGivenCatchedZombie.Contains(zombie))
                {
                    speedGivenCatchedZombie.Add(zombie);
                    zombie.isToAvoidBaseLogic = true;
                    float zomXSep = this.GlobalPosition.x - zombie.GlobalPosition.x;
                    float zomYSep = this.GlobalPosition.y - zombie.GlobalPosition.y;

                    int noOFMoveComponents = 10;
                    zombie.moving_speed.x = (zomXSep / noOFMoveComponents) * 10;
                    zombie.moving_speed.y = (zomYSep / noOFMoveComponents) * 10;
                    zombie.CollisionMask = 2;
                }
                if (zombie.GlobalPosition.y < this.GlobalPosition.y)
                {
                    removeZombie(zombie);
                    break;
                }
            }

            var killerRays = this.GetNode<Node2D>("Killer_Rays").GetChildren();

            foreach (RayCast2D killerRay in killerRays)
            {
                var collider = killerRay.GetCollider();
                if (catchedZombie.Contains(collider))
                {
                    Basic_Zombie zombie = collider as Basic_Zombie;
                    removeZombie(zombie);
                    break;
                }

            }

        }
    }

    public void removeZombie(Basic_Zombie zombie,bool isToQueueFreeZombie = true)
    {
        if(catchedZombie.Contains(zombie))
        {
            catchedZombie.Remove(zombie);
            speedGivenCatchedZombie.Remove(zombie);
        }

        if(!isDataSaved)
        {
            Dictionary<string, string> zombieData = null;
            string zombieName = zombie.character_name;

            foreach (Dictionary<string, string> data in catchedZombieData)
            {
                if (data["name"].ToString() == zombieName)
                {
                    zombieData = data;
                    break;
                }
            }

            if (zombieData == null)
            {
                zombieData = new Dictionary<string, string>() { { "name", zombieName }, { "count", "1" } };
                catchedZombieData.Add(zombieData);
            }
            else
            {
                zombieData["count"] = (int.Parse(zombieData["count"]) + 1).ToString();
            }
        }
        if(isToQueueFreeZombie)
        {
            zombie.QueueFree();
        }
    }

    public void CatchZombie()
    {
        if (catchedZombie.Count > 0)
        {
            isCatchingZombie = true;
        }
        catchZombieTimer.Stop();
    }

    public void saveData()
    {

        int totalCatchedZombieCount = 0;

        foreach (Basic_Zombie zombie in basf.global_Variables.level_scene.GetNode<Node2D>("Zombie_Area").GetChildren())
        {
            if(zombie.is_death)
            {
                removeZombie(zombie,false);
            }
        }   

        foreach (Dictionary<string,string> zombie in catchedZombieData)
        {
            string zombieName = zombie["name"];
            zombieDataManager.load_data(zombieName);
            int Num_Of_Catched_Zombies = int.Parse(zombie["count"])+zombieDataManager.get_integer_data("Num_Of_Catched_Zombies");
            zombieDataManager.set_value("Num_Of_Catched_Zombies",(Num_Of_Catched_Zombies).ToString());
            totalCatchedZombieCount+=int.Parse(zombie["count"]);
        }
        zombieDataManager.save_data();
        zombieDataManager.load_previous_data_again();

        isDataSaved = true;

        basf.global_Variables.catchZombieCount = totalCatchedZombieCount;
    }




}
