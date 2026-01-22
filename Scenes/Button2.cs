using Godot;
using System;

public partial class Button2 : Button
{
    private void _on_pressed2()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Level.tscn");
    }
}
