using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalState : Node
{
    public static string SelectedPhilosopher = ""; // Use a static variable for easy access
    public static Dictionary<string, Texture2D> philosopherTextures = new Dictionary<string, Texture2D>(); // Create a dictionary to store philosopher textures

    public static void initPhilosopherTexturesDictionary()
    {
        philosopherTextures["Nietzsche"] = GD.Load<Texture2D>("res://philosophers/Nietzsche.png");
        philosopherTextures["Descartes"] = GD.Load<Texture2D>("res://philosophers/Descartes.png");
        philosopherTextures["Kant"] = GD.Load<Texture2D>("res://philosophers/Kant.png");
        philosopherTextures["Socrates"] = GD.Load<Texture2D>("res://philosophers/Socrates.png");
    }

    public static Dictionary<string, Texture2D> getPhilosopherTexturesDictionary()
    {
        return philosopherTextures;
    }
}