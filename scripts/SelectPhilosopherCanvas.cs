using Godot;
using System;
//using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public partial class SelectPhilosopherCanvas : CanvasLayer
{
	private string selectedPhilosopher = "";

	[Export] public TextureButton NietzscheButton;
	[Export] public TextureButton DescartesButton;
	[Export] public TextureButton KantButton;
	[Export] public TextureButton SocratesButton;
	[Export] public Button ConfirmButton;

	// Dictionary storing philosopher images
	private Dictionary<string, Texture2D> philosopherTextures = new Dictionary<string, Texture2D>();
	private Dictionary<string, TextureRect> philosopherTextureRects = new Dictionary<string, TextureRect>();
	
	[Export]
	private Godot.Collections.Array<TextureRect> philosopherRects;
	

	public override void _Ready()
	{
		// Load philosopher images
		philosopherTextures["Nietzsche"] = GD.Load<Texture2D>("res://philosophers/Nietzsche.png");
		philosopherTextures["Descartes"] = GD.Load<Texture2D>("res://philosophers/Descartes.png");
		philosopherTextures["Kant"] = GD.Load<Texture2D>("res://philosophers/Kant.png");
		philosopherTextures["Socrates"] = GD.Load<Texture2D>("res://philosophers/Socrates.png");

		// philosopherSprites["Nietzsche"] = new Sprite2D();
		// philosopherSprites["Descartes"] = new Sprite2D();
		// philosopherSprites["Kant"] = new Sprite2D();
		// philosopherSprites["Socrates"] = new Sprite2D();
		// philosopherSprites["Nietzsche"].Texture = philosopherTextures["Nietzsche"];
		// philosopherSprites["Descartes"].Texture = philosopherTextures["Descartes"];
		// philosopherSprites["Kant"].Texture = philosopherTextures["Kant"];
		// philosopherSprites["Socrates"].Texture = philosopherTextures["Socrates"];

		//// Assign textures to buttons
		//NietzscheButton.TextureNormal = philosopherTextures["Nietzsche"];
		//DescartesButton.TextureNormal = philosopherTextures["Descartes"];
		//KantButton.TextureNormal = philosopherTextures["Kant"];
		//SocratesButton.TextureNormal = philosopherTextures["Socrates"];
//
		//// Scale the buttons dynamically
		//NietzscheButton.RectScale = new Vector2(0.15, 0.15);
		//DescartesButton.RectScale = new Vector2(0.15, 0.15);
		//KantButton.RectScale = new Vector2(0.15, 0.15);
		//SocratesButton.RectScale = new Vector2(0.15, 0.15);
		//// // Scale the TextureRect
		//// NietzscheButton.TextureNormal.scale = new Vector2(0.15, 0.15);
		//// DescartesButton.TextureNormal.scale = new Vector2(0.15, 0.15);
		//// KantButton.TextureNormal.scale = new Vector2(0.15, 0.15);
		//// SocratesButton.TextureNormal.scale = new Vector2(0.15, 0.15);

		// // Connect button signals
		// NietzscheButton.Pressed += OnNietzschePressed;
		// DescartesButton.Pressed += OnDescartesPressed;
		// KantButton.Pressed += OnKantPressed;
		// SocratesButton.Pressed += OnSocratesPressed;
		// ConfirmButton.Pressed += OnConfirmPressed;

		 // Create TextureRects for each philosopher
		
		var philosopherNames = new[] {"Socrates" , "Nietzsche", "Kant", "Descartes" };
		for (int i = 0; i < philosopherRects.Count; i++)
		{
			var philosopher = philosopherNames[i];
			var textureRect = philosopherRects[i];	
			textureRect.StretchMode = TextureRect.StretchModeEnum.KeepCentered;
			textureRect.Scale = new Vector2(0.15f, 0.15f);
			textureRect.Texture = philosopherTextures[philosopher];

			// Add click detection
			textureRect.MouseFilter = Control.MouseFilterEnum.Pass;
			textureRect.MouseEntered += () => OnPhilosopherHovered(philosopher);
			textureRect.MouseExited += () => OnPhilosopherHovered(philosopher);
			textureRect.GuiInput += (InputEvent @event) => OnPhilosopherSelected(this, philosopher, @event);

			// Add to dictionary and scene
			philosopherTextureRects[philosopher] = textureRect;
			AddChild(textureRect);

			// count++;
		}

		// Position the TextureRects in a grid
		PositionPhilosophers();

		// Connect confirm button
		ConfirmButton.Pressed += OnConfirmPressed;
	}

	// private void OnNietzschePressed() { selectedPhilosopher = "Nietzsche"; }
	// private void OnDescartesPressed() { selectedPhilosopher = "Descartes"; }
	// private void OnKantPressed() { selectedPhilosopher = "Kant"; }
	// private void OnSocratesPressed() { selectedPhilosopher = "Socrates"; }

	// private void OnConfirmPressed()
	// {
	// 	// Store selection globally
	// 	var globalState = GetNode("/root/GlobalState") as GlobalState;
	// 	globalState.SelectedPhilosopher = selectedPhilosopher;
	// 	GD.Print($"Selected Philosopher: {selectedPhilosopher}");

	// 	// Transition to the game scene
	// 	GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
	// }

	 private void PositionPhilosophers()
	{
		var position = new Vector2(100, 100);
		var spacing = 200;

		philosopherTextureRects["Nietzsche"].Position = position;
		philosopherTextureRects["Descartes"].Position = position + new Vector2(spacing, 0);
		philosopherTextureRects["Kant"].Position = position + new Vector2(0, spacing);
		philosopherTextureRects["Socrates"].Position = position + new Vector2(spacing, spacing);
	}

	private void OnPhilosopherHovered(string philosopher)
	{
		// Add hover effect if needed
		GD.Print($"Hovered over {philosopher}");
	}

	private void OnPhilosopherSelected(SelectPhilosopherCanvas canvas, string philosopher, InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
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
		var globalState = GetNode("/root/GlobalState") as GlobalState;
		globalState.SelectedPhilosopher = selectedPhilosopher;
		GD.Print($"Selected Philosopher: {selectedPhilosopher}");

		// Transition to the game scene
		GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
	}
}
