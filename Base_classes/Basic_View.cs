using System.Collections;
using Godot;



/*
Controls for Basic View
press ctrl+p (to popup navigation bar)
*/

public class Basic_View:Control,Global_Variables_F_A_T
{
    public _Type_of_ _node_type{get;set;}
    
    public Basic_Func basf;

    string level_base_url;

    Changing_Scene changing_Scene;
    bool is_changing_scene_removed = false;

    Item_Searcher item_searcher;

    public bool is_item_searcher_available = true;

    public override void _Ready()
    {
        _node_type = _Type_of_.View;
        this.Visible = false;


        basf = new Basic_Func(this);
        changing_Scene = basf.add_changing_scene(this,"",false,false);
        
        basf.create_a_sound("res://assets/audio/GUI/Scene_Enter.mp3",this,true);

        var global = basf.global_Variables;
        global.current_scene = this;
        global.click_sound = global.navigation_sound_url;

        level_base_url = "res://Levels/Scenes/";

        item_searcher = this.GetNode<Item_Searcher>("Item_Searcher");
        item_searcher.load_data(new ArrayList(){"Home_View","Level_View","Shop","Warning_Editor","Multi_AI_Match_Gui"});
        item_searcher.render_items_copy.Remove(this.Name);
        
        
    }
    public override void _Process(float delta){

        // to popup navigation bar
        if(Input.IsActionJustPressed("Pop_Navigation_Menu"))
        {
            if(item_searcher.Visible)
            {
                item_searcher.hide();
            }
            else{
                item_searcher.pop();
            }
        }

        if(item_searcher.is_item_selected)
        {
            item_searcher.reset();
            basf.navigateTo(this,basf.global_paths.base_view_path+item_searcher.selected_item.Trim()+".tscn");
        }

    }

    public bool start_level(string level_name){
        GetTree().ChangeScene($"{level_base_url}{level_name}.tscn");
        return true;
    }

    public void initialize_view(){

    }

    public virtual void update_logic(Data_Manager shop_data,Data_Manager user_data,Data_Manager throwable_weapons_dm){
        
    }
}
