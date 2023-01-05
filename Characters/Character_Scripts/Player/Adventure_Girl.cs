using Godot;
using System;
using System.Collections;

public class Adventure_Girl : Basic_Player
{
    public override void _Ready()
    {
        base._Ready();
        custom_constructor(700,9000,basic_attack_name : "Melee");

        is_busy = false;
        available_moves = new ArrayList(){"death","idle","jump","melee","run","shoot","slide"};
        available_moves_consumption = new int[7]{0,0,2,2,0,0,0};
        available_moves_damage = new int[7]{0,0,5,25,1,1,3};
        settle_damage_increment_possible_moves(3);

        this.load_basic_weapon("Adven_Bullet",0,-100,100);
        // can_shoot = true;
        // bullet_scene = ResourceLoader.Load<PackedScene>("res://Weapons_And_Animation/scenes/Adven_Bullet.tscn");


        // b_rightchange = 100;
        // b_leftchange = -100;
        // b_height_change = 0;
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        basic_animation_changing_condition = !is_busy;
        
        custom_process(delta);

        move();
        
    }
    public override void collided_with_body(Node body)
	{
		base.collided_with_body(body);
		
	}
}
