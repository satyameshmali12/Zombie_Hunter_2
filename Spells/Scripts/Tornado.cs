using Godot;
using Godot.Collections;
using System;
using System.Collections;

public class Tornado : Basic_Spell
{
    ArrayList captured_troops;
    ArrayList captured_troops_property;
    Godot.Collections.Array collision_rays;

    ArrayList can_capture_to;

    int moving_speed = 10; // for x axis
    int bit_up_movement = 100; // for y axis
    int damage = 10;

    ArrayList damaged_troops = new ArrayList();
    Timer damage_troop_recovery_timer;




    public override void _Ready()
    {
        base._Ready();
        captured_troops = new ArrayList();
        captured_troops_property = new ArrayList();
        collision_rays = this.GetNode<Node2D>("Rays").GetChildren();
        can_capture_to = new ArrayList() { _Type_of_.Player, _Type_of_.Zombie };

        damage_troop_recovery_timer = basf.create_timer(2f, "Damaged_Troops_Recovery");
        damage_troop_recovery_timer.Start();

        damage += int.Parse(dm.get_data("Damage_Increment"));


    }

    public override void _Process(float delta)
    {
        base._Process(delta);



    }
    public override void effect()
    {
        base.effect();


        foreach (Basic_Character character in captured_troops)
        {

            // Task
            // if not any ray is colliding to character so just remove it from the list and make to fall under the influence of gravity
            if (!damaged_troops.Contains(character))
            {
                character.change_health((character != parent) ? -damage : -(int)damage / 2);
                damaged_troops.Add(character);
            }


            var is_any_ray_colliding = is_any_ray_colliding_to_given_char(character);
            if (!is_any_ray_colliding)
            {
                captured_troops_property.Remove(captured_troops_property[captured_troops.IndexOf(character)]);
                captured_troops.Remove(character);
                character.is_to_fall_in_gravity_influence = true;
                break;
            }

            if (!character.IsConnected("tree_exiting", this, "Captured_Troops_Exiting"))
            {
                character.Connect("tree_exiting", this, "Captured_Troops_Exiting");
            }
            if (character.IsQueuedForDeletion())
            {
                captured_troops_property.Remove(captured_troops_property[captured_troops.IndexOf(character)]);
                captured_troops.Remove(character);
                break;
            }
            // try without adding generic to the dictionary in the given context
            Dictionary<string, string> details = captured_troops_property[captured_troops.IndexOf(character)] as Dictionary<string, string>;

            bool is_given_distance_covered = bool.Parse(details["is_given_distance_covered"]);
            if (!is_given_distance_covered)
            {
                int speed_x = int.Parse(details["speed_x"]);
                int distance_convered = int.Parse(details["convered_distance"]);
                int distance_to_be_covered = int.Parse(details["distance_to_be_covered"]);

                if (distance_convered + Math.Abs(speed_x) < distance_to_be_covered)
                {
                    character.Position += new Vector2(speed_x, 0);
                    distance_convered += Math.Abs(speed_x);
                    details["convered_distance"] = distance_convered.ToString();
                }
                else
                {
                    var remaining_movement = Convert.ToInt32(distance_to_be_covered % distance_convered);
                    character.Position += new Vector2(remaining_movement, 0);
                    details["is_given_distance_covered"] = true.ToString();
                }
            }

        }





        /*
        getting the collided characters and adding them in the captured troops list 
        and 
        simultaneouly alotting new movement to the movement convered one
        */
        foreach (RayCast2D item in collision_rays)
        {
            if (item.IsColliding())
            {

                Global_Variables_F_A_T type = item.GetCollider() as Global_Variables_F_A_T;
                if (can_capture_to.Contains(type._node_type))
                {
                    Basic_Character character = item.GetCollider() as Basic_Character;
                    character.is_to_fall_in_gravity_influence = false;
                    if (!captured_troops.Contains(character))
                    {

                        Vector2 ray_start_point = item.Position;
                        Vector2 ray_final_point = ray_start_point + item.CastTo;
                        int speed_x = (ray_start_point.x > ray_final_point.x) ? moving_speed : -moving_speed;
                        int distance_to_be_covered = Math.Abs(Convert.ToInt32(item.CastTo.x) * 2);
                        captured_troops.Add(character);
                        captured_troops_property.Add(
                            new Dictionary<string, string>() {
                                { "speed_x", speed_x.ToString() },
                                { "no_of_collision", "0" },
                                { "is_given_distance_covered", "false" },
                                { "convered_distance", "0" },
                                { "distance_to_be_covered", distance_to_be_covered.ToString() } }
                            );
                    }
                    else
                    {
                        Dictionary<string, string> collided_character_details = captured_troops_property[captured_troops.IndexOf(character)] as Dictionary<string, string>;
                        bool is_given_distance_covered = bool.Parse(collided_character_details["is_given_distance_covered"]);
                        if (is_given_distance_covered)
                        {
                            character.Position -= new Vector2(0, bit_up_movement);
                            var speed_x = (-int.Parse(collided_character_details["speed_x"])).ToString();
                            var distance_to_be_covered = (int.Parse(collided_character_details["distance_to_be_covered"])).ToString();
                            var no_of_collision = (int.Parse(collided_character_details["no_of_collision"]) + 1);

                            captured_troops_property[captured_troops_property.IndexOf(collided_character_details)]
                                    = new Dictionary<string, string>() {
                                        { "speed_x", speed_x.ToString() },
                                        { "no_of_collision", no_of_collision.ToString() },
                                        { "is_given_distance_covered", "false" },
                                        { "convered_distance", "0" },
                                        { "distance_to_be_covered", distance_to_be_covered.ToString() }
                                    };

                            if (no_of_collision > 10)
                            {
                                character.Position -= new Vector2(0, 1000);
                            }

                            break;
                        }
                    }
                }
            }
        }
    }

    /*
    this funciton is called when any captured troop removes the scene tree
    */
    public void Captured_Troops_Exiting()
    {
        // removing the troops which are queue freed
        foreach (Basic_Character character in captured_troops)
        {
            if (character.IsQueuedForDeletion())
            {
                captured_troops_property.Remove(captured_troops_property[captured_troops.IndexOf(character)]);
                captured_troops.Remove(character);
                break;
            }
        }

    }

    public override void end_effect()
    {
        base.end_effect();

        // making all the characters gravity normal once the spell is being removed
        foreach (Basic_Character item in captured_troops)
        {
            item.is_to_fall_in_gravity_influence = true;
        }
    }

    public bool is_any_ray_colliding_to_given_char(Basic_Character character)
    {
        foreach (RayCast2D item in collision_rays)
        {
            if (item.GetCollider() == character)
            {
                return true;
            }
        }
        return false;
    }

    // hiding the menu once the 
    public override void use_item(Item_Using_Menu menu, Basic_Func basf)
    {
        base.use_item(menu, basf);
        menu.Visible = false;
    }

    public void Damaged_Troops_Recovery()
    {
        damaged_troops.Clear();
    }

}
