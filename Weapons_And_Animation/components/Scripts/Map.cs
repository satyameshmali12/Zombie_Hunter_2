using Godot;
using System;


/// <summary>
/// To display the entire game in a small box
/// it displays the main character,enemys i.e zombie
/// </summary>
public class Map : Node2D
{
    public Basic_Func basf;

    public Vector2 start_point, end_point;
    public Node2D points;
    TileMap tileMap;
    Node2D main_parent;

    Node2D point_area;

    ReferenceRect render_offset;


    PackedScene dot_scene;

    Timer re_render_all_dots_timer;



    float point_render_area_width = 0;
    float point_render_area_height = 0;

    bool is_data_loaded_ones = false;



    Vector2 size; // the pixel size of the main level(level_area) calculate right ro the start_point and the end position2d node used in the each level

    #region Field Which will be retrived from the another classes
    public bool is_point_given = false; // to check whether has given the point or not
    public Vector2 adding_position = Vector2.Zero;

    Dot pointer_dot;

    #endregion
    public override void _Ready()
    {
        basf = new Basic_Func(this);

        // getting the level_scene
        // to toto get the data fro from the main level scene to here and rendering it on the map
        main_parent = basf.global_Variables.level_scene;

        tileMap = main_parent.GetNode<TileMap>("TileMap");
        points = main_parent.GetNode<Node2D>("Points");

        start_point = points.GetNode<Position2D>("Start_Point").GlobalPosition;
        end_point = points.GetNode<Position2D>("End_Point").GlobalPosition;

        point_area = this.GetNode<Node2D>("Point_Area"); // getting node in the map in which point are gonna get added


        // point_render_area = this.GetNode<Control>("Point_Render_Area");
        render_offset = this.GetNode<ReferenceRect>("Render_Offset");

        dot_scene = basf.get_the_packed_scene("res://Weapons_And_Animation/components/scenes/Dot.tscn");

        re_render_all_dots_timer = basf.create_timer(1, "Re_Render_All_Dots");

        re_render_all_dots_timer.Start();

        // basf.global_Variables.visibility_list.Add(this);
        pointer_dot = this.GetNode<Dot>("Pointer_Dot");
        pointer_dot.Visible = false;
    }

    public override void _Process(float delta)
    {

        // var all_points = basf.get_the_node_childrens("Point_Area");


        if (is_data_loaded_ones)
        {
            if (Input.IsActionJustPressed("Mouse_Pressed"))
            {
                var mouse_x = point_area.GetLocalMousePosition().x;
                var mouse_y = point_area.GetLocalMousePosition().y;
                if (mouse_x > 0 && mouse_x < point_render_area_width && mouse_y > 0 && mouse_y < point_render_area_height)
                {
                    is_point_given = true;

                    pointer_dot.Visible = true;
                    pointer_dot.Position = new Vector2(mouse_x, mouse_y);

                    /*
                    calculating the point adding position of the map with respect to the game scene
                    so that map drop on desired places can be performed
                    */
                    var mouse_x_percent = mouse_x / point_render_area_width * 100;
                    var mouse_y_percent = mouse_y / point_render_area_height * 100;

                    var adding_x = (mouse_x_percent * size.x) / 100;
                    var adding_y = (mouse_y_percent * size.y) / 100;

                    adding_position = new Vector2(adding_x, adding_y);
                }

            }
        }







    }


    /// <summary>To add the point on the map</summary>
    public void add_point(Vector2 map_position, Color dot_color)
    {
        size = end_point - start_point;

        var width = size.x;
        var height = size.y;

        // all the stuff for getting the percentage of the characters on the game scene
        //start

        var distance_from_origin_x = Math.Abs(start_point.x);
        var distacne_from_orgin_y = Math.Abs(start_point.y);

        var adding_position_x = distance_from_origin_x + map_position.x;
        var adding_position_y = distacne_from_orgin_y + map_position.y;

        var adding_position_x_percentage = adding_position_x / width * 100;
        var adding_position_y_percentage = adding_position_y / height * 100;

        // GD.Print(adding_position_x_percentage,adding_position_y_percentage);

        //end


        //  all the stuff for rendering the point on the screen using the percentage finded upward
        point_render_area_width = render_offset.RectSize.x;
        point_render_area_height = render_offset.RectSize.y;


        var point_render_area_height_decre = 30;


        Dot dot = dot_scene.Instance<Dot>();
        dot.dot_color = dot_color;
        var dot_x = (adding_position_x_percentage * point_render_area_width) / 100;
        var dot_y = (adding_position_y_percentage * (point_render_area_height - point_render_area_height_decre)) / 100;
        dot.Position = new Vector2(dot_x, dot_y);

        point_area.AddChild(dot);

        var dot_global_position = dot.GlobalPosition;

        var render_area_position = render_offset.RectGlobalPosition;

        dot.Visible = (dot_global_position.y > render_area_position.y);





    }


    /// <summary>
    /// This function basically makes the use of the add_point
    /// <para>it just adds all the points on the map which are need to be displayed with the help of the add_point() function</para>
    /// </summary>
    public void Re_Render_All_Dots()
    {
        basf.clear_children_nodes(point_area);
        var all_zombies = main_parent.GetNode<Node2D>("Zombie_Area").GetChildren();

        foreach (Node2D item in all_zombies)
        {
            add_point(item.GlobalPosition, new Color(200, 192, 192));
        }

        var player_position = basf.global_Variables.character_scene.Position;
        add_point(player_position, new Color(116, 0, 255, 255));


        is_data_loaded_ones = true;
    }

    /// <summary>
    /// This makes all the stuffs of the map to be restart
    /// <para>It makes the map ready for reusing it...</para>
    /// </summary>
    public bool reset()
    {
        is_point_given = false;
        pointer_dot.Visible = false;
        adding_position = Vector2.Zero;
        return true;
    }
}
