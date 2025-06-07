using Godot;
using System;
//using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public partial class SelectPhilosopherCanvas : CanvasLayer
{
	private string selectedPhilosopher = "";

	[Export] public Button ConfirmButton;

	// Dictionary storing philosopher images
	private Dictionary<string, Texture2D> philosopherTextures = new Dictionary<string, Texture2D>();
	[Export] private Godot.Collections.Array<TextureRect> philosopherRects;


	public override void _Ready()
	{
		// Load philosopher images
		philosopherTextures["Nietzsche"] = GD.Load<Texture2D>("res://philosophers/Nietzsche.png");
		philosopherTextures["Descartes"] = GD.Load<Texture2D>("res://philosophers/Descartes.png");
		philosopherTextures["Kant"] = GD.Load<Texture2D>("res://philosophers/Kant.png");
		philosopherTextures["Socrates"] = GD.Load<Texture2D>("res://philosophers/Socrates.png");

		var philosopherNames = new[] { "Socrates", "Nietzsche", "Kant", "Descartes" };
		for (int i = 0; i < philosopherRects.Count; i++)
		{
			string philosopher = philosopherNames[i];
			var textureRect = philosopherRects[i];
			textureRect.ExpandMode = TextureRect.ExpandModeEnum.FitWidth;
			// Set a custom size that's 15% of the original texture size
			textureRect.Texture = philosopherTextures[philosopher];
			Vector2 originalSize = textureRect.Texture.GetSize();
			textureRect.CustomMinimumSize = originalSize * 0.15f;

			// Add click detection
			textureRect.MouseFilter = Control.MouseFilterEnum.Pass;

			// // Capture the philosopher variable in a local copy to avoid closure issues
			// string capturedPhilosopher = philosopher;
			textureRect.MouseEntered += () => OnPhilosopherHovered(philosopher);
			textureRect.MouseExited += () => OnPhilosopherHovered(philosopher);
			textureRect.GuiInput += (InputEvent @event) => OnPhilosopherSelected(this, philosopher, @event);

			// count++;
		}

		// Connect confirm button
		ConfirmButton.Pressed += OnConfirmPressed;
	}

	private void OnPhilosopherHovered(string philosopher)
	{
		// Add hover effect if needed
		GD.Print($"Hovered over {philosopher}");
	}

	private void OnPhilosopherSelected(SelectPhilosopherCanvas canvas, string philosopher, InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left &&
			mouseEvent.Pressed)
		{
			selectedPhilosopher = philosopher;
			GD.Print($"Selected: {philosopher}");
		}
	}

	private void OnConfirmPressed()
	{
		if (string.IsNullOrEmpty(selectedPhilosopher))
		{
			GD.Print("No philosopher selected!");
			return;
		}

		// Store selection globally
		// var globalState = GetNode("/root/GlobalState") as GlobalState;
		GlobalState.SelectedPhilosopher = selectedPhilosopher;
		GD.Print($"Selected Philosopher: {selectedPhilosopher}");

		// Transition to the game scene
		GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
	}
}
