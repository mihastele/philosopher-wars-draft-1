using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

// // Judge class that evaluates plays based on card effectiveness and bias.
// public class Judge
// {
// 	public string Name { get; set; }
// 	public string Bias { get; set; } // Judge’s preferred card type

// 	public Judge(string name, string bias)
// 	{
// 		Name = name;
// 		Bias = bias;
// 	}

// 	// The JudgeRound method takes a list of tuples mapping a philosopher to their played card.
// 	public Philosopher JudgeRound(List<(Philosopher philosopher, Card card)> plays)
// 	{
// 		// Choose the play with the highest score (card effectiveness plus bonus if matching judge’s bias)
// 		(Philosopher philosopher, Card card) winningPlay = plays[0];
// 		int bestScore = winningPlay.card.Effectiveness + (winningPlay.card.Type == Bias ? 5 : 0);

// 		foreach (var play in plays)
// 		{
// 			int score = play.card.Effectiveness + (play.card.Type == Bias ? 5 : 0);
// 			if (score > bestScore)
// 			{
// 				bestScore = score;
// 				winningPlay = play;
// 			}
// 		}

// 		return winningPlay.philosopher;
// 	}
// }

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

	// private Judge _currentJudge;
	// Index to determine which philosopher is currently acting as the judge.
	private int _currentJudgeIndex = 0;

	private List<string> _questions = new List<string>()
	{
		"What is the meaning of life?",
		"Does free will truly exist?",
		"Is morality objective?"
	};

	public override void _Ready()
	{
		InitPhilosophers();
		// InitJudge();
		StartRound();
	}

	// Dynamically instance philosopher nodes
	private void InitPhilosophers()
	{
		// Load the philosopher scene (ensure the path matches your project files)
		PackedScene philosopherScene = GD.Load<PackedScene>("res://Philosopher.tscn");

		// // Define philosopher names and positions.
		// string[] philosopherNames = new string[] { "Nietzsche", "Descartes", "Kant" };
		// Vector2[] positions = new Vector2[]
		// {
		// 	new Vector2(100, 300),
		// 	new Vector2(300, 300),
		// 	new Vector2(500, 300)
		// };

		// for (int i = 0; i < philosopherNames.Length; i++)
		// {
		// 	Philosopher philosopherNode = (Philosopher)philosopherScene.Instantiate();
		// 	philosopherNode.PhilosopherName = philosopherNames[i];
		// 	philosopherNode.Position = positions[i];

		// 	// Add the dynamically created philosopher node to the container.
		// 	PhilosophersContainer.AddChild(philosopherNode);
		// 	_philosopherNodes.Add(philosopherNode);
		// }
		// Define the philosopher data: (name, position, bias).
		var philosopherData = new List<(string name, Vector2 pos, string bias)>
		{
			("Nietzsche", new Vector2(100, 300), "Emotion"),
			("Descartes", new Vector2(300, 300), "Logic"),
			("Kant",      new Vector2(500, 300), "Morality"),
			("Socrates",  new Vector2(700, 300), "Dialogue")
		};

		foreach (var data in philosopherData)
		{
			Philosopher philosopherNode = (Philosopher)philosopherScene.Instantiate();
			philosopherNode.PhilosopherName = data.name;
			philosopherNode.Position = data.pos;
			// Set each philosopher’s bias that will be used when they serve as judge.
			philosopherNode.Bias = data.bias;
			PhilosophersContainer.AddChild(philosopherNode);
			_philosopherNodes.Add(philosopherNode);
		}
	}

	// Initialize the judge with a bias (for example, favoring "Logic" cards)
	// private void InitJudge()
	// {
	// 	_currentJudge = new Judge("Plato", "Logic");
	// }

	// Simulate a game round:
	// - Display a random philosophical question.
	// - Each philosopher "plays" a random card.
	// - The judge selects the best card.
	private void StartRound()
	{
		// // --- Question Phase ---
		// Random rand = new Random();
		// string currentQuestion = _questions[rand.Next(_questions.Count)];
		// QuestionLabel.Text = currentQuestion;
		// GD.Print("Question: " + currentQuestion);

		// // --- Answer Phase ---
		// // Create a list of plays (each tuple contains a philosopher and their played card).
		// List<(Philosopher philosopher, Card card)> plays = new List<(Philosopher philosopher, Card card)>();

		// // Define a basic card deck for simulation.
		// List<Card> basicCards = new List<Card>()
		// {
		// 	new Card("Deductive Proof", "Logic", 8),
		// 	new Card("Empathy Play", "Emotion", 6),
		// 	new Card("Socratic Questioning", "Rebuttal", 7)
		// };

		// foreach (Philosopher philosopher in _philosopherNodes)
		// {
		// 	// Choose a random card from the deck.
		// 	Card playedCard = basicCards[rand.Next(basicCards.Count)];
		// 	GD.Print($"{philosopher.PhilosopherName} plays {playedCard.Name} ({playedCard.Type})");

		// 	plays.Add((philosopher, playedCard));
		// }

		// // --- Judgment Phase ---
		// Philosopher winner = _currentJudge.JudgeRound(plays);
		// GD.Print($"{winner.PhilosopherName} wins this round with their argument!");

		// // Future steps: update visual scores, rotate judge, animate card plays, etc.

		// --- Designate the Judge ---
		// Rotate judge by index. In this round, one of the philosophers will only judge.
		Philosopher currentJudge = _philosopherNodes[_currentJudgeIndex];
		GD.Print($"{currentJudge.PhilosopherName} is the judge this round!");

		// --- Question Phase ---
		Random rand = new Random();
		string currentQuestion = _questions[rand.Next(_questions.Count)];
		QuestionLabel.Text = currentQuestion;
		GD.Print("Question: " + currentQuestion);

		// --- Answer Phase ---
		// Create a list to track which philosopher plays which card.
		List<(Philosopher philosopher, Card card)> plays = new List<(Philosopher, Card)>();

		// Define a basic card deck.
		List<Card> basicCards = new List<Card>()
		{
			new Card("Deductive Proof",      "Logic", 8),
			new Card("Empathy Play",         "Emotion", 6),
			new Card("Socratic Questioning", "Rebuttal", 7)
		};

		// Each philosopher except the judge picks and “plays” a random card.
		foreach (Philosopher philosopher in _philosopherNodes)
		{
			if (philosopher == currentJudge)
				continue;

			Card playedCard = basicCards[rand.Next(basicCards.Count)];
			GD.Print($"{philosopher.PhilosopherName} plays {playedCard.Name} ({playedCard.Type})");
			plays.Add((philosopher, playedCard));
		}

		// --- Judgment Phase ---
		// The designated judge evaluates the cards using their bias.
		string judgeBias = currentJudge.Bias;
		(Philosopher philosopher, Card card) winningPlay = plays.First();
		int bestScore = winningPlay.card.Effectiveness + (winningPlay.card.Type == judgeBias ? 5 : 0);

		foreach (var play in plays)
		{
			int score = play.card.Effectiveness + (play.card.Type == judgeBias ? 5 : 0);
			if (score > bestScore)
			{
				bestScore = score;
				winningPlay = play;
			}
		}

		GD.Print($"{winningPlay.philosopher.PhilosopherName} wins this round with their {winningPlay.card.Name}!");

		// Rotate judge for the next round.
		_currentJudgeIndex = (_currentJudgeIndex + 1) % _philosopherNodes.Count;

		// Future enhancements: update visual scores, add animations, schedule the next round, etc.

	}
}
