using Godot;

public partial class GlobalState : Node
{
    public string SelectedPhilosopher { get; set; }

    public override void _Ready()
    {
        // Initialize with default value
        SelectedPhilosopher = "";
    }
}
