using Godot;


public class Basic_Game_View : Node2D, Global_Variables_F_A_T
{
    public _Type_of_ _node_type { get; set; }

    public Basic_Func basf;

    string level_base_url;

    public override void _Ready()
    {
        _node_type = _Type_of_.Game_View;

        basf = new Basic_Func(this);

        basf.global_Variables.current_scene = this;

        basf.create_a_sound("res://assets/audio/GUI/Scene_Enter.mp3", this, true);
    }

    public virtual void update_logic(Data_Manager shop_data, Data_Manager user_data, Data_Manager throwable_weapons_dm) { }
}
