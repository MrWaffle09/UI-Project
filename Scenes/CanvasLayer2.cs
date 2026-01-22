using Godot;
using System;

public partial class CanvasLayer2 : CanvasLayer
{
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);


        {
            GD.Print("pranab is not co0lz");
            if (Input.IsActionJustPressed("Escape"))
            {
                GD.Print("Escape");
                GetTree().Paused = !GetTree().Paused;
                this.Visible = !this.Visible;
            }

        }
    }
}


