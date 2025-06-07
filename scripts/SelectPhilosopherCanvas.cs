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
	private System.Collections.Generic.Dictionary<string, Texture2D> philosopherTextures = GlobalState.philosopherTextures;
	[Export] private Godot.Collections.Array<TextureRect> philosopherRects;


	public override void _Ready()
	{

		
		for (int i = 0; i < philosopherRects.Count; i++)
		{
			string philosopher = GlobalState.philosopherNames[i];
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
			// Update the selected philosopher
			selectedPhilosopher = philosopher;
			GD.Print($"Selected: {philosopher}");
		
			// Reset all philosopher rects to normal appearance
			foreach (var rect in philosopherRects)
			{
				rect.Modulate = new Color(1, 1, 1); // Reset to default color
				rect.Scale = Vector2.One; // Reset scale
			}
		
			// Highlight the selected philosopher
			int index = System.Array.IndexOf(GlobalState.philosopherNames, philosopher);
			if (index >= 0 && index < philosopherRects.Count)
			{
				// Apply a highlight effect - you can customize this
				philosopherRects[index].Modulate = new Color(1.3f, 1.3f, 1.3f); // Slight yellow tint
				// philosopherRects[index].Scale = new Vector2(1.1f, 1.1f); // Make it slightly larger
			}
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
