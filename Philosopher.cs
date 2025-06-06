using Godot;
using System;
using System.Collections.Generic;

public partial class Philosopher : Node2D
{
	// This exported property lets you choose the philosopher in the Godot editor.
	[Export]
	public string PhilosopherName { get; set; } = "Nietzsche";
	
	// Dictionary mapping philosopher names to their Texture2D image resources.
	private Dictionary<string, Texture2D> philosopherTextures = new Dictionary<string, Texture2D>();

	public override void _Ready()
	{
		// Load the images into the dictionary.
		// Ensure these paths correspond to where your image files are stored.
		philosopherTextures["Nietzsche"] = GD.Load<Texture2D>("res://philosophers/Nietzsche.png");
		philosopherTextures["Descartes"] = GD.Load<Texture2D>("res://philosophers/Descartes.png");
		philosopherTextures["Kant"] = GD.Load<Texture2D>("res://philosophers/Kant.png");
		philosopherTextures["Socrates"] = GD.Load<Texture2D>("res://philosophers/Socrates.png");

		// Check if the selected philosopher exists in our dictionary.
		if (philosopherTextures.ContainsKey(PhilosopherName))
		{
			// Find the first Sprite2D child node
			if (GetNode("Sprite2D") is Sprite2D sprite)
			{
				// Assign the chosen texture to the Sprite2D node.
				Texture2D tex = philosopherTextures[PhilosopherName];
				sprite.Texture = tex;
			}
			else
			{
				GD.PrintErr("No Sprite2D node found as child of Philosopher");
			}
		}
		else
		{
			GD.PrintErr($"No texture found for philosopher: {PhilosopherName}");
		}
	}
	
}
