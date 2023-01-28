using Godot;
using System;
using System.Collections;

public class Resist_Remover : Basic_Spell
{

    ArrayList resisted_characters = new ArrayList();
    Timer preview_remover;
    ArrayList can_over_resist;
    

    public override void _Ready()
    {
        base._Ready();
        // GD.Print(screen_time);
        // GD.Print((screen_time/100)*40);
        // GD.Print((screen_time/100)*40);
        // float preview_screen_time = (screen_time/100)*40;


        // GD.Print(preview_screen_time, " right from the resist_remover.cs haha.!!");
        preview_remover = basf.create_timer(screen_time,"Remove_Preview");
        preview_remover.OneShot = true;
        can_over_resist = new ArrayList(){_Type_of_.Player,_Type_of_.Zombie};

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

    }
    public override void effect()
    {
        base.effect();

        var collision_rays = this.GetNode<Node2D>("Spell_Preview/Rays").GetChildren();

        foreach (RayCast2D ray in collision_rays)
        {
            var collider = ray.GetCollider();

            if(collider!=null && collider!=parent)
            {
                Global_Variables_F_A_T type = collider as Global_Variables_F_A_T;
                if(can_over_resist.Contains(type._node_type) && !resisted_characters.Contains(collider))
                {
                    resisted_characters.Add(collider);
                }
            }            
        }


        foreach (Basic_Character character in resisted_characters)
        {
            character.is_resisted = false;
        }
    }
    public override void end_effect()
    {
        base.end_effect();
    }

    public override void use_item(Item_Using_Menu menu, Basic_Func basf)
    {
        base.use_item(menu, basf);
        menu.Visible = false;
    }

    public void Remove_Preview()
    {
        this.GetNode<Node2D>("Spell_Preview").Visible = false;
        preview_remover.Stop();
    }

}
