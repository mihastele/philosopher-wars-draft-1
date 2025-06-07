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

		// // Define a basic card deck.
		// List<Card> basicCards = new List<Card>()
		// {
		// 	new Card("Deductive Proof",      "Logic", 8),
		// 	new Card("Empathy Play",         "Emotion", 6),
		// 	new Card("Socratic Questioning", "Rebuttal", 7)
		// };

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
			new Card("If a tree falls in a forest and no one is around to hear it, does it make a sound?", "Paradox", 7, "Philosophical Dilemma"),
			new Card("Can God create a stone so heavy that He cannot lift it?", "Paradox", 9, "Omnipotence Paradox"),

			// Rebuttal Cards
			new Card("Extraordinary claims require extraordinary evidence.", "Rebuttal", 8, "Carl Sagan"),
			new Card("A wise man can learn more from a foolish question than a fool can learn from a wise answer.", "Rebuttal", 7, "Bruce Lee"),
			new Card("There is no such thing as society, only individuals and families.", "Rebuttal", 6, "Margaret Thatcher"),

			// Dialogue Cards
			new Card("What is justice?", "Dialogue", 9, "Socrates"),
			new Card("Does a thing become good simply because the gods command it?", "Dialogue", 8, "Euthyphro Dilemma"),
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
