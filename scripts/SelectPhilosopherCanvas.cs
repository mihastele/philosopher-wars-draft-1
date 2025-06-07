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

	public override void _Ready()
	{
		// Load philosopher images
		philosopherTextures["Nietzsche"] = GD.Load<Texture2D>("res://images/nietzsche.png");
		philosopherTextures["Descartes"] = GD.Load<Texture2D>("res://images/descartes.png");
		philosopherTextures["Kant"] = GD.Load<Texture2D>("res://images/kant.png");
		philosopherTextures["Socrates"] = GD.Load<Texture2D>("res://images/socrates.png");

		// Assign textures to buttons
		NietzscheButton.TextureNormal = philosopherTextures["Nietzsche"];
		DescartesButton.TextureNormal = philosopherTextures["Descartes"];
		KantButton.TextureNormal = philosopherTextures["Kant"];
		SocratesButton.TextureNormal = philosopherTextures["Socrates"];

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
		GetTree().ChangeSceneToFile("res://Game.tscn");
	}
}
