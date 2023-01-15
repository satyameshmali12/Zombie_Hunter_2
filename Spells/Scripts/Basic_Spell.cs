using Godot;
using System;

/*

Two types of spell their

*/

public class Basic_Spell : Item_Using_Menu_Component,Global_Variables_F_A_T
{

    public _Type_of_ _node_type{get;set;}

    public bool is_to_add_to_character = false;
    public int per_damage = 1;


    public int screen_time = 2; // set this property before calling the base of the _ready function in the child classes
    Timer removal_timer;

    public Basic_Spell()
    {
        warning_message = "Some mix-up is there or may the similar exist which may don't have any effect \n \n press confirm to still add";
    }
    public override void _Ready()
    {
        base._Ready();
        _node_type = _Type_of_.Spell;
        
        

        // basf = new Basic_Func(this);

        per_damage += int.Parse(dm.get_data("Damage_Increment"));
        screen_time = int.Parse(dm.get_data("Screen_Timing"));



        removal_timer = basf.create_timer(screen_time, "Remove_Spell");
        removal_timer.Start();


    }



    public override void _Process(float delta)
    {
        base._Process(delta);
        if (!this.IsQueuedForDeletion())
        {
            effect();
        }

    }


    public override void add_to_scene(Basic_Func basf)
    {
        base.add_to_scene(basf);
        if (is_to_add_to_character)
        {
            basf.global_Variables.character_scene.AddChild(this);
        }
        else
        {
            basf.global_Variables.level_scene.AddChild(this);
        }

    }


    /// <summary>The task of the spell</summary>
    public virtual void effect()
    { }

    /// <summary>During the removal of the spell this function is been performed</summary>
    public virtual void end_effect()
    { }

    /// <summary>Whens the spell task or life span finish this function is called</summary>
    public virtual void Remove_Spell()
    {
        end_effect();
        removal_timer.Stop();
        this.QueueFree();
    }

    public void update_logic(Data_Manager shop_data, Data_Manager user_data, Data_Manager throwable_weapon_dm)
    {

        int new_screen_time = shop_data.get_integer_data("Screen_Timing")+shop_data.get_integer_data("Screen_Timing_Increment");
        shop_data.set_value("Screen_Timing",new_screen_time.ToString());
    }

    public Thing1 testing<Thing1>(Thing1 button)
    {
        return button;
    }

}
