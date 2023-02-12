using Godot;

public class Dot : Node2D
{

    Sprite dot_image;

    public Color dot_color = new Color(12, 199, 24);

    public override void _Ready()
    {
        dot_image = this.GetNode<Sprite>("Dot");
        dot_image.Modulate = dot_color;
    }

    public override void _Process(float delta)
    {

    }
}
