using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class Basic_View:Control,Global_Variables_F_A_T
{
    public _Type_of_ _node_type{get;set;}
    
    public Basic_Func all_func;

    public override void _Ready()
    {
        _node_type = _Type_of_.View;
        
        all_func = new Basic_Func(this);

    }
        
}
