using Godot;
using System;

public partial class PauseMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void resume()
	{
		Get_Tree().paused=false;
	}
	
	private void pause()
	{
		Get_Tree().paused=true;
	}
	
}
