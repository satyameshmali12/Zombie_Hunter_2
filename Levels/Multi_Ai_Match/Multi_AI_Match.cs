using Godot;
using Godot.Collections;
using System;

public class Multi_AI_Match : Arcade
{

    public override void _Ready()
    {
        base._Ready();

        setLevelType(Level_Type.Multi_AI);
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

    }

    public override void addCharacters()
    {
        foreach (Dictionary<string,string> item in basf.global_Variables.multi_ai_character_details)
        {
            string name = item["name"];
            bool is_main_character = bool.Parse(item["is_main_character"]);

            Basic_Player hero_scene = basf.get_the_packed_scene(basf.global_paths.Character_Scenes_Base_Url+$"/Player/{name}.tscn").Instance<Basic_Player>();

            hero_scene.GlobalPosition = Vector2.Zero;
            heros_area.AddChild(hero_scene);

            if(is_main_character)
            {
                hero_scene.MakeConPlayer();
                setMainPlayer(hero_scene);
            }
            else{
                hero_scene.MakeAIPlayer();
            }
            
        }
    }
}
