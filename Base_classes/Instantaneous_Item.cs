// using Godot;
// using System;
// using System.Collections;
// using Godot.Collections;

// public class Instantaneous_Item : Area2D
// {

//     Basic_Func basf;

//     ArrayList all_items; 

//     Item_Searcher item_searcher;  

//     public override void _Ready()
//     {
//         basf = new Basic_Func(this);

//         item_searcher = this.GetNode<Item_Searcher>("Item_Searcher");

//         ArrayList all_item_names = new ArrayList();
//         foreach (string data_url in basf.global_Variables.item_data_field_urls)
//         {

//             Data_Manager manager = new Data_Manager(data_url);
//             // we have taken here the data_field identifier as the first value in the field names as first field in the data_field is the name only in all the cases
//             ArrayList all_items_names = manager.get_set_of_field_values(manager.all_field_names[0].ToString());

//             foreach (string item in all_items_names)
//             {
//                 all_items.Add(basf.get_dictionary<string>(new string[2] { "name", "data_field_url" }, new string[2] { item, data_url }));
//                 all_item_names.Add(item);
//             }
//         }

//         item_searcher.load_data(all_item_names);
//         GD.Print(all_item_names.Count);
//     }

//     public override void _Process(float delta)
//     {
        
//         var buying_button_containers = this.GetNode<VBoxContainer>("Buying_Buttons_Container").GetChildren();
//         for (int i = 0; i < item_searcher.item_boxes.Count; i++)
//         {
//             GD.Print("hey I am here from the instantanouse_item.cs haha..!!");
//             GD.Print("First stage");
//             Container box = item_searcher.item_boxes[i] as Container;
//             Container button_box = buying_button_containers[i] as Container;

//             if(box.Visible)
//             {
//                 GD.Print("Second_Stage");
//                 string Select_Button_Text = box.GetNode<Button>("Select_Button").Text;
//                 int item_price = 0;
//                 foreach (Dictionary<string,string> item in all_items)
//                 {
//                     if(item["name"]==Select_Button_Text)
//                     {

//                         Data_Manager dm = new Data_Manager(item["data_field_url"]);
//                         dm.load_data(item["name"]);
//                         item_price = int.Parse(dm.get_data("Per_Price"));
//                         button_box.GetNode<Button>("Button").Text = item_price.ToString();
//                     }
//                 }

//             }
//             else{
//                 button_box.Visible = false;
//             }

            
//         }


        
//     }

//     public virtual void start_processing(){

//     }

//     public virtual void end_processing(){

//     }
// }
