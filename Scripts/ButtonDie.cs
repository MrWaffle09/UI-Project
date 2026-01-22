using Godot;
using System;

public partial class ButtonDie : Button
{
    private void _on_pressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/menu.tscn");   
    }
}
