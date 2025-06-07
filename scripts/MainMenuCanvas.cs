using Godot;
using System;

public partial class MainMenuCanvas : CanvasLayer
{
	[Export] public Button StartButton;
	[Export] public Button ExitButton;

	public override void _Ready()
	{
		StartButton.Pressed += OnStartPressed;
		ExitButton.Pressed += OnExitPressed;
	}

	private void OnStartPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/SelectPhilosopher.tscn");
	}

	private void OnExitPressed()
	{
		GetTree().Quit();
	}
}
