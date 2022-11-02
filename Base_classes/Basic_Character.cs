using Godot;
using System;
using System.Collections;

public class Basic_Character : RigidBody2D
{
	public AnimatedSprite animations;

	ArrayList Basic_Movements;

	public int health;

	public void custom_constructor(){

		Basic_Movements = new ArrayList(){"Idle","Run","Walk","Jump","Dead"};
		
		// GD.Print("I am the constructor from the Basic_Characters.!!");

		animations = GetNode<AnimatedSprite>("Movements");
		animations.Animation = "Idle"; // setting the initial animation to idle
		animations.Playing = true;

	}

	public void custom_process(float delta)
	{
		

	}


	// to get the whether any movement is a basic movement or not
	public bool get_w_is_basic_movement(string move_name){
		return Basic_Movements.Contains(move_name);
	}
}
