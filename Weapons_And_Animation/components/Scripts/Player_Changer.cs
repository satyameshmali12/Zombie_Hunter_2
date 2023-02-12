using Godot;
using System;


/// <summary>This UI helps in changing the player during the Mutli_AI_Match</summary>
public class Player_Changer : Node2D
{

    Basic_Func basf;
    
    Node2D herosArea;
    public override void _Ready()
    {
        base._Ready();
        basf = new Basic_Func(this);

        herosArea = basf.global_Variables.herosArea;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.Visible = basf.global_Variables.current_level_type == Level_Type.Multi_AI;

        if (this.Visible)
        {

            var heros = herosArea.GetChildren();

            VBoxContainer player_changer_box = this.GetNode<VBoxContainer>("VBoxContainer");
            var pl_chang_b_chil = player_changer_box.GetChildren();
            foreach (Button item in pl_chang_b_chil)
            {
                var index = pl_chang_b_chil.IndexOf(item);

                if (index < heros.Count)
                {
                    Basic_Player hero = heros[index] as Basic_Player;
                    item.Visible = true;
                    item.Text = hero.character_name;

                    Button select_button = item.GetNode<Button>("MarginContainer/Button");
                    select_button.Text = (basf.global_Variables._main_character_name==hero.character_name)?"Selected":"Select";

                    if(select_button.Pressed && select_button.Text!="Selected")
                    {
                        foreach (Basic_Player player in heros)
                        {
                            if(player.character_name==hero.character_name)
                            {

                                Level levelScene = basf.global_Variables.level_scene as Level;
                                levelScene.setMainPlayer(player); // setting the main player
                                
                            }
                            else{
                                player.MakeAIPlayer();
                            }
                        }
                        break;
                    }
                }
                else
                {
                    item.Visible = false;
                }

            }

        }

    }

}
