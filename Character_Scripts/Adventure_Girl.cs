using Godot;
using System;
using System.Collections;

public class Adventure_Girl : Basic_Player
{
    public override void _Ready()
    {
        base._Ready();
        custom_constructor(700,9000);
        is_busy = false;
        available_moves = new ArrayList(){};
        available_moves_consumption = new int[0]{};
        
    }


    public override void _Process(float delta)
    {
        base._Process(delta);
        basic_animation_changing_condition = !is_busy;
        
        custom_process(delta);

        LinearVelocity = moving_speed;
        
    }
    public override void collided_with_body(Node body)
	{
		base.collided_with_body(body);
		
	}
}
