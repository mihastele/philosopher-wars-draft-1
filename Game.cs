using Godot;
using System;
using System.Collections.Generic;

// Simple Card class for simulation.
public class Card
{
	public string Name { get; set; }
	public string Type { get; set; } // e.g., "Logic", "Emotion", "Rebuttal"
	public int Effectiveness { get; set; } // Value for judging

	public Card(string name, string type, int effectiveness)
	{
		Name = name;
		Type = type;
		Effectiveness = effectiveness;
	}
}

// Judge class that evaluates plays based on card effectiveness and bias.
public class Judge
{
	public string Name { get; set; }
	public string Bias { get; set; } // Judge’s preferred card type

	public Judge(string name, string bias)
	{
		Name = name;
		Bias = bias;
	}

	// The JudgeRound method takes a list of tuples mapping a philosopher to their played card.
	public Philosopher JudgeRound(List<(Philosopher philosopher, Card card)> plays)
	{
		// Choose the play with the highest score (card effectiveness plus bonus if matching judge’s bias)
		(Philosopher philosopher, Card card) winningPlay = plays[0];
		int bestScore = winningPlay.card.Effectiveness + (winningPlay.card.Type == Bias ? 5 : 0);

		foreach (var play in plays)
		{
			int score = play.card.Effectiveness + (play.card.Type == Bias ? 5 : 0);
			if (score > bestScore)
			{
				bestScore = score;
				winningPlay = play;
			}
		}

		return winningPlay.philosopher;
	}
}

public partial class Game : Node2D
{
	// Reference to the UI Label that displays the current question.
	[Export]
	public Label QuestionLabel;

	// Container node in which philosopher nodes will be dynamically instanced.
	[Export]
	public Node2D PhilosophersContainer;

	// Keep track of the instantiated philosopher nodes.
	private List<Philosopher> _philosopherNodes = new List<Philosopher>();

	private Judge _currentJudge;
	private List<string> _questions = new List<string>()
	{
		"What is the meaning of life?",
		"Does free will truly exist?",
		"Is morality objective?"
	};

	public override void _Ready()
	{
		InitPhilosophers();
		InitJudge();
		StartRound();
	}

	// Dynamically instance philosopher nodes
	private void InitPhilosophers()
	{
		// Load the philosopher scene (ensure the path matches your project files)
		PackedScene philosopherScene = GD.Load<PackedScene>("res://Philosopher.tscn");

		// Define philosopher names and positions.
		string[] philosopherNames = new string[] { "Nietzsche", "Descartes", "Kant" };
		Vector2[] positions = new Vector2[]
		{
			new Vector2(100, 300),
			new Vector2(300, 300),
			new Vector2(500, 300)
		};

		for (int i = 0; i < philosopherNames.Length; i++)
		{
			Philosopher philosopherNode = (Philosopher)philosopherScene.Instantiate();
			philosopherNode.PhilosopherName = philosopherNames[i];
			philosopherNode.Position = positions[i];

			// Add the dynamically created philosopher node to the container.
			PhilosophersContainer.AddChild(philosopherNode);
			_philosopherNodes.Add(philosopherNode);
		}
	}

	// Initialize the judge with a bias (for example, favoring "Logic" cards)
	private void InitJudge()
	{
		_currentJudge = new Judge("Plato", "Logic");
	}

	// Simulate a game round:
	// - Display a random philosophical question.
	// - Each philosopher "plays" a random card.
	// - The judge selects the best card.
	private void StartRound()
	{
		// --- Question Phase ---
		Random rand = new Random();
		string currentQuestion = _questions[rand.Next(_questions.Count)];
		QuestionLabel.Text = currentQuestion;
		GD.Print("Question: " + currentQuestion);

		// --- Answer Phase ---
		// Create a list of plays (each tuple contains a philosopher and their played card).
		List<(Philosopher philosopher, Card card)> plays = new List<(Philosopher philosopher, Card card)>();

		// Define a basic card deck for simulation.
		List<Card> basicCards = new List<Card>()
		{
			new Card("Deductive Proof", "Logic", 8),
			new Card("Empathy Play", "Emotion", 6),
			new Card("Socratic Questioning", "Rebuttal", 7)
		};

		foreach (Philosopher philosopher in _philosopherNodes)
		{
			// Choose a random card from the deck.
			Card playedCard = basicCards[rand.Next(basicCards.Count)];
			GD.Print($"{philosopher.PhilosopherName} plays {playedCard.Name} ({playedCard.Type})");

			plays.Add((philosopher, playedCard));
		}

		// --- Judgment Phase ---
		Philosopher winner = _currentJudge.JudgeRound(plays);
		GD.Print($"{winner.PhilosopherName} wins this round with their argument!");

		// Future steps: update visual scores, rotate judge, animate card plays, etc.
	}
}
