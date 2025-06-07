using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
	// private Dictionary<string, Sprite2D> philosopherSprites = new Dictionary<string, Sprite2D>();
	
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

		// Assign textures to buttons
		NietzscheButton.TextureNormal = philosopherTextures["Nietzsche"];
		DescartesButton.TextureNormal = philosopherTextures["Descartes"];
		KantButton.TextureNormal = philosopherTextures["Kant"];
		SocratesButton.TextureNormal = philosopherTextures["Socrates"];

		// Scale the buttons dynamically
		NietzscheButton.RectScale = new Vector2(0.15, 0.15);
		DescartesButton.RectScale = new Vector2(0.15, 0.15);
		KantButton.RectScale = new Vector2(0.15, 0.15);
		SocratesButton.RectScale = new Vector2(0.15, 0.15);
		// // Scale the TextureRect
		// NietzscheButton.TextureNormal.scale = new Vector2(0.15, 0.15);
		// DescartesButton.TextureNormal.scale = new Vector2(0.15, 0.15);
		// KantButton.TextureNormal.scale = new Vector2(0.15, 0.15);
		// SocratesButton.TextureNormal.scale = new Vector2(0.15, 0.15);

		// Connect button signals
		NietzscheButton.Pressed += OnNietzschePressed;
		DescartesButton.Pressed += OnDescartesPressed;
		KantButton.Pressed += OnKantPressed;
		SocratesButton.Pressed += OnSocratesPressed;
		ConfirmButton.Pressed += OnConfirmPressed;
	}

	private void OnNietzschePressed() { selectedPhilosopher = "Nietzsche"; }
	private void OnDescartesPressed() { selectedPhilosopher = "Descartes"; }
	private void OnKantPressed() { selectedPhilosopher = "Kant"; }
	private void OnSocratesPressed() { selectedPhilosopher = "Socrates"; }

	private void OnConfirmPressed()
	{
		// Store selection globally
		var globalState = GetNode("/root/GlobalState") as GlobalState;
		globalState.SelectedPhilosopher = selectedPhilosopher;
		GD.Print($"Selected Philosopher: {selectedPhilosopher}");

		// Transition to the game scene
		GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
	}
}
