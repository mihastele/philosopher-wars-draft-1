using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Simple Card class for simulation.
public class Card
{
	public string Name { get; set; }
	public string Type { get; set; } // Logic, Emotion, etc.
	public int Effectiveness { get; set; }
	public string OriginalAuthor { get; set; } // New field for attribution

	public Card(string name, string type, int effectiveness, string originalAuthor)
	{
		Name = name;
		Type = type;
		Effectiveness = effectiveness;
		OriginalAuthor = originalAuthor;
	}
}


public partial class Game : Node2D
{
	// Reference to the UI Label that displays the current question.
	[Export] public Label QuestionLabel;

	// Container node in which philosopher nodes will be dynamically instanced.
	[Export] public Node2D PhilosophersContainer;

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
		PackedScene philosopherScene = GD.Load<PackedScene>("res://scenes/Philosopher.tscn");

		// Get the viewport size to calculate top right corner
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;

		// // Load the player texture sprite
		// Sprite2D player = GetNode<Sprite2D>("Player"); // Adjust the path if needed


		// Define philosopher data without positions
		var philosopherData = new List<(string name, string bias)>
		{
			("Nietzsche", "Emotion"),
			("Descartes", "Logic"),
			("Kant", "Morality"),
			("Socrates", "Dialogue")
		};

		// // Calculate positions dynamically
		// int philosopherCount = philosopherData.Count;
		// float spacing = 200f; // Space between philosophers
		// float startX = 100f; // Start position from left
		// float yPosition = 300f; // Fixed Y position

		// Calculate positions dynamically
		int philosopherCount = philosopherData.Count - 1;
		float philosopherWidth = 1024f * 0.15f; // 0.15 is the scale factor for the philosopher texture
		float spacing = 100; // Space between philosophers

// Calculate total width of all philosophers and spacing
		float totalWidth = (philosopherCount * philosopherWidth) + ((philosopherCount - 1) * spacing);


// Calculate start X to center horizontally
		float startX = (viewportSize.X - totalWidth) / 2f;

// Calculate Y position 50 units from the bottom
		float yPosition = viewportSize.Y - 120f;


		// Create a dictionary to store positions
		Dictionary<string, Vector2> philosopherPositions = new Dictionary<string, Vector2>();

		// Find the selected philosopher from GlobalState
		string selectedPhilosopherName = GlobalState.SelectedPhilosopher;

		// Calculate positions for each philosopher

		var filteredPhilosophers = philosopherData.Where(x => x.name != selectedPhilosopherName).ToList();
		// foreach (var philosoperTuple in filteredPhilosophers)
		// {
		// GD.Print($"filtered: {philosoperTuple}");
		// }
		for (int i = 0; i < filteredPhilosophers.Count; i++)
		{
			string name = filteredPhilosophers[i].name;
			// GD.Print($"name: {name}");
			philosopherPositions[name] = new Vector2(startX + (i * (spacing + philosopherWidth)), yPosition);
		}


		foreach (var data in philosopherData)
		{
			Philosopher philosopherNode = (Philosopher)philosopherScene.Instantiate();
			philosopherNode.PhilosopherName = data.name;

			// Position the selected philosopher in the top right corner
			if (data.name == selectedPhilosopherName)
			{
				philosopherNode.Position = new Vector2(viewportSize.X - 100, 100); // Adjusted for some padding
			}
			else
			{
				philosopherNode.Position = philosopherPositions[data.name];
			}

			PhilosophersContainer.AddChild(philosopherNode);

			// Set each philosopher's bias that will be used when they serve as judge.
			philosopherNode.Bias = data.bias;
			_philosopherNodes.Add(philosopherNode);
		}
	}

	private void StartRound()
	{
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


		List<Card> deck = new List<Card>()
		{
			// Logic Cards
			new Card("Cogito, ergo sum.", "Logic", 8, "René Descartes"),
			new Card("A statement is true if it corresponds to reality.", "Logic", 7, "Correspondence Theory"),
			new Card("Mathematics is the language of the universe.", "Logic", 6, "Pythagoras"),

			// Rhetoric Cards
			new Card("Give me liberty, or give me death!", "Rhetoric", 9, "Patrick Henry"),
			new Card("The unexamined life is not worth living.", "Rhetoric", 8, "Socrates"),
			new Card("He who opens a school door, closes a prison.", "Rhetoric", 7, "Victor Hugo"),

			// Emotion Cards
			new Card("The heart has its reasons which reason knows nothing of.", "Emotion", 7, "Blaise Pascal"),
			new Card("I think, therefore I suffer.", "Emotion", 6, "Fyodor Dostoevsky"),
			new Card("One death is a tragedy, a million deaths is a statistic.", "Emotion", 5, "Joseph Stalin"),

			// Paradox Cards
			new Card("This sentence is false.", "Paradox", 8, "The Liar Paradox"),
			new Card("If a tree falls in a forest and no one is around to hear it, does it make a sound?", "Paradox", 7,
				"Philosophical Dilemma"),
			new Card("Can God create a stone so heavy that He cannot lift it?", "Paradox", 9, "Omnipotence Paradox"),

			// Rebuttal Cards
			new Card("Extraordinary claims require extraordinary evidence.", "Rebuttal", 8, "Carl Sagan"),
			new Card("A wise man can learn more from a foolish question than a fool can learn from a wise answer.",
				"Rebuttal", 7, "Bruce Lee"),
			new Card("There is no such thing as society, only individuals and families.", "Rebuttal", 6,
				"Margaret Thatcher"),

			// Dialogue Cards
			new Card("What is justice?", "Dialogue", 9, "Socrates"),
			new Card("Does a thing become good simply because the gods command it?", "Dialogue", 8,
				"Euthyphro Dilemma"),
			new Card("Can you truly know anything with certainty?", "Dialogue", 7, "Epistemological Skepticism")
		};


		// Each philosopher except the judge picks and “plays” a random card.
		foreach (Philosopher philosopher in _philosopherNodes)
		{
			if (philosopher == currentJudge)
				continue;

			Card playedCard = deck[rand.Next(deck.Count)];
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
