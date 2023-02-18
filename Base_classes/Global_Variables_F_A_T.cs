using Godot;
using System.Collections;


// F_A_T for all terms
public interface Global_Variables_F_A_T
{

    _Type_of_ _node_type { get; set; }
    void update_logic(Data_Manager shop_data, Data_Manager user_data, Data_Manager throwable_weapon_dm);
}


// this is the child class of the Global_Varaible_F_A_T
// this class will be inherited by all the classes which are associated with some action related stuff in the game
public interface Character : Global_Variables_F_A_T
{
    int health{get;set;} // health of the character
    int change_health(int change); // function for changing the health
    ArrayList can_collide_with{get;set;} // all the types to whom the character if collided can cause change in the health
}