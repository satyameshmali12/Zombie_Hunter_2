using Godot;
using System;
using System.Collections;

public class Adventure_Girl : Basic_Player
{
    public override void _Ready()
    {
        this.character_name = "Adventure_Girl";

        settle_fields(700,9000,basic_attack_name : "Melee");

        base._Ready();


        is_busy = false;
        available_moves = new ArrayList(){"damaged","death","hurt","idle","jump","melee","run","shoot","slide"};
        available_moves_consumption = new int[9]{0,0,0,0,2,2,0,10,0};
        available_moves_damage = new int[9]{0,0,0,0,5,25,1,1,3};
        settle_damage_increment_possible_moves(3);
        attack_move_names = new ArrayList(){"melee"};

        this.load_basic_weapon("Adven_Bullet",0,-150,150);
        
    }

    public override void _Process(float delta)
    {
        basic_animation_changing_condition = !is_busy;
        
        base._Process(delta);
        
    }
    public override void collided_with_body(Node body)
	{
		base.collided_with_body(body);
		
	}
}
