// EternalQuest.cs
// A console program that manages Simple, Eternal, and Checklist goals.
// Save as EternalQuest.cs and compile with: csc EternalQuest.cs
// (or use dotnet new console and paste the code into Program.cs)

using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
// Abstract base class for all goals
abstract class Goal
{
// Encapsulated fields
private string _title;
private string _description;
private int _points;

    // Properties
    public string Title { get => _title; protected set => _title = value; }
    public string Description { get => _description; protected set => _description = value; }
    public int Points { get => _points; protected set => _points = value; }

    // Constructor
    public Goal(string title, string description, int points)
    {
        Title = title;
        Description = description;
        Points = points;
    }

    // Is the goal complete? Defaults false; overridden by subclasses if necessary.
    public virtual bool IsComplete() => false;

    // Record that the user completed (or recorded) the goal once.
    // Returns number of points awarded for this record.
    public abstract int RecordEvent();

    // Provide string to display in the list
    public abstract override string ToString();

    // Serialize to a line that can be saved to file
    public abstract string Serialize();

    // Deserialize factory
    public static Goal Deserialize(string line)
    {
        // Format: Type|field1|field2|...
        // We guard against malformed input.
        var parts = line.Split('|');
        if (parts.Length < 1) return null;

        var type = parts[0];
        try
        {
            switch (type)
            {
                case "Simple":
                    // Simple|title|desc|points|isComplete (bool)
                    if (parts.Length != 5) return null;
                    var sTitle = parts[1];
                    var sDesc = parts[2];
                    var sPoints = int.Parse(parts[3]);
                    var sIsComplete = bool.Parse(parts[4]);
                    var sg = new SimpleGoal(sTitle, sDesc, sPoints);
                    if (sIsComplete) sg.MarkComplete(); // restore state
                    return sg;

                case "Eternal":
                    // Eternal|title|desc|points
                    if (parts.Length != 4) return null;
                    var eTitle = parts[1];
                    var eDesc = parts[2];
                    var ePoints = int.Parse(parts[3]);
                    return new EternalGoal(eTitle, eDesc, ePoints);

                case "Checklist":
                    // Checklist|title|desc|points|requiredCount|bonus|currentCount
                    if (parts.Length != 7) return null;
                    var cTitle = parts[1];
                    var cDesc = parts[2];
                    var cPoints = int.Parse(parts[3]);
                    var required = int.Parse(parts[4]);
                    var bonus = int.Parse(parts[5]);
                    var current = int.Parse(parts[6]);
                    var cg = new ChecklistGoal(cTitle, cDesc, cPoints, required, bonus);
                    cg.SetCurrentCount(current); // restore progress (internal method)
                    return cg;

                default:
                    return null;
            }
        }
        catch
        {
            return null;
        }
    }
}

// Simple goal: complete once
class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string title, string description, int points)
        : base(title, description, points)
    {
        _isComplete = false;
    }

    public override bool IsComplete() => _isComplete;

    // Mark complete directly (used when loading)
    public void MarkComplete() => _isComplete = true;

    public override int RecordEvent()
    {
        if (_isComplete)
        {
            Console.WriteLine("This goal is already complete; no additional points awarded.");
            return 0;
        }
        _isComplete = true;
        Console.WriteLine($"You completed \"{Title}\" and earned {Points} points!");
        return Points;
    }

    public override string ToString()
    {
        var checkbox = _isComplete ? "[X]" : "[ ]";
        return $"{checkbox} {Title} ({Description})";
    }

    public override string Serialize()
    {
        // Simple|title|desc|points|isComplete
        return $"Simple|{Title}|{Description}|{Points}|{_isComplete}";
    }
}

// Eternal goal: never complete, always gives points when recorded
class EternalGoal : Goal
{
    public EternalGoal(string title, string description, int points)
        : base(title, description, points)
    {
    }

    public override int RecordEvent()
    {
        Console.WriteLine($"Recorded \"{Title}\" and earned {Points} points!");
        return Points;
    }

    public override string ToString()
    {
        // No [X] as it is never complete
        return $"[∞] {Title} ({Description}) - Each time: {Points} pts";
    }

    public override string Serialize()
    {
        // Eternal|title|desc|points
        return $"Eternal|{Title}|{Description}|{Points}";
    }
}

// Checklist goal: needs to be completed a number of times to finish, awards bonus at completion
class ChecklistGoal : Goal
{
    private int _requiredCount;
    private int _currentCount;
    private int _bonus;

    public ChecklistGoal(string title, string description, int pointsPerCompletion, int requiredCount, int bonus)
        : base(title, description, pointsPerCompletion)
    {
        _requiredCount = Math.Max(1, requiredCount);
        _currentCount = 0;
        _bonus = Math.Max(0, bonus);
    }

    // Allow restoring current count when loading
    public void SetCurrentCount(int c)
    {
        _currentCount = Math.Max(0, Math.Min(c, _requiredCount));
    }

    public override bool IsComplete() => _currentCount >= _requiredCount;

    public override int RecordEvent()
    {
        if (IsComplete())
        {
            Console.WriteLine("This checklist goal is already completed; no additional points awarded.");
            return 0;
        }

        _currentCount++;
        int awarded = Points;
        Console.WriteLine($"Recorded \"{Title}\" ({_currentCount}/{_requiredCount}) and earned {Points} points!");

        if (_currentCount >= _requiredCount)
        {
            awarded += _bonus;
            Console.WriteLine($"Congratulations! You completed the checklist and earned a bonus of {_bonus} points!");
        }

        return awarded;
    }

    public override string ToString()
    {
        var checkbox = IsComplete() ? "[X]" : "[ ]";
        return $"{checkbox} {Title} ({Description}) -- Completed {_currentCount}/{_requiredCount} times. Each: {Points} pts, Bonus: {_bonus} pts";
    }

    public override string Serialize()
    {
        // Checklist|title|desc|points|requiredCount|bonus|currentCount
        return $"Checklist|{Title}|{Description}|{Points}|{_requiredCount}|{_bonus}|{_currentCount}";
    }
}

class Program
{
    static List<Goal> goals = new List<Goal>();
    static int totalScore = 0;

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Eternal Quest!");
        LoadIfAutoExists();
        bool running = true;
        while (running)
        {
            ShowMenu();
            var input = Console.ReadLine();
            Console.WriteLine();
            switch (input)
            {
                case "1":
                    CreateGoal();
                    break;
                case "2":
                    ListGoals();
                    break;
                case "3":
                    RecordEvent();
                    break;
                case "4":
                    Console.WriteLine($"Your current score is: {totalScore} points.");
                    break;
                case "5":
                    SaveGoals();
                    break;
                case "6":
                    LoadGoals();
                    break;
                case "7":
                    Console.WriteLine("Goodbye, adventurer!");
                    running = false;
                    break;
                default:
                    Console.WriteLine("Please enter a number between 1 and 7.");
                    break;
            }
            Console.WriteLine();
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Create New Goal");
        Console.WriteLine("2. List Goals");
        Console.WriteLine("3. Record Event (mark a goal as done)");
        Console.WriteLine("4. Display Score");
        Console.WriteLine("5. Save Goals");
        Console.WriteLine("6. Load Goals");
        Console.WriteLine("7. Exit");
        Console.Write("Choose an option (1-7): ");
    }

    static void CreateGoal()
    {
        Console.WriteLine("Choose goal type:");
        Console.WriteLine("1. Simple Goal (complete once)");
        Console.WriteLine("2. Eternal Goal (repeatable)");
        Console.WriteLine("3. Checklist Goal (complete N times for bonus)");
        Console.Write("Choice (1-3): ");
        var choice = Console.ReadLine();
        Console.Write("Title: ");
        var title = Console.ReadLine();
        Console.Write("Description: ");
        var desc = Console.ReadLine();
        int points = PromptForInt("Points awarded per completion: ");

        switch (choice)
        {
            case "1":
                goals.Add(new SimpleGoal(title, desc, points));
                Console.WriteLine("Simple goal created.");
                break;
            case "2":
                goals.Add(new EternalGoal(title, desc, points));
                Console.WriteLine("Eternal goal created.");
                break;
            case "3":
                int required = PromptForInt("How many times required to complete: ");
                int bonus = PromptForInt("Bonus points awarded when completed: ");
                goals.Add(new ChecklistGoal(title, desc, points, required, bonus));
                Console.WriteLine("Checklist goal created.");
                break;
            default:
                Console.WriteLine("Invalid choice, aborting creation.");
                break;
        }
    }

    static void ListGoals()
    {
        if (goals.Count == 0)
        {
            Console.WriteLine("No goals yet.");
            return;
        }

        Console.WriteLine("Goals:");
        for (int i = 0; i < goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {goals[i].ToString()}");
        }
    }

    static void RecordEvent()
    {
        if (goals.Count == 0)
        {
            Console.WriteLine("No goals to record. Create one first.");
            return;
        }

        ListGoals();
        int choice = PromptForInt($"Choose goal number to record (1-{goals.Count}): ", 1, goals.Count);
        var goal = goals[choice - 1];
        int awarded = goal.RecordEvent();
        totalScore += awarded;
        Console.WriteLine($"Total score is now: {totalScore} pts.");
    }

    static void SaveGoals()
    {
        Console.Write("Enter filename to save to (default: goals.txt): ");
        var filename = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(filename)) filename = "goals.txt";

        try
        {
            using (var writer = new StreamWriter(filename))
            {
                // Save totalScore on first line
                writer.WriteLine(totalScore);
                foreach (var g in goals)
                {
                    writer.WriteLine(g.Serialize());
                }
            }
            Console.WriteLine($"Saved {goals.Count} goals and score ({totalScore}) to {filename}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save: {ex.Message}");
        }
    }

    static void LoadGoals()
    {
        Console.Write("Enter filename to load from (default: goals.txt): ");
        var filename = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(filename)) filename = "goals.txt";

        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.");
            return;
        }

        try
        {
            var lines = File.ReadAllLines(filename);
            if (lines.Length == 0)
            {
                Console.WriteLine("File empty.");
                return;
            }

            // First line is totalScore
            var newScore = int.Parse(lines[0]);
            var newGoals = new List<Goal>();
            for (int i = 1; i < lines.Length; i++)
            {
                var g = Goal.Deserialize(lines[i]);
                if (g != null) newGoals.Add(g);
            }

            goals = newGoals;
            totalScore = newScore;
            Console.WriteLine($"Loaded {goals.Count} goals and score ({totalScore}) from {filename}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load: {ex.Message}");
        }
    }

    static int PromptForInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (int.TryParse(s, out int val))
            {
                if (val < min || val > max)
                {
                    Console.WriteLine($"Enter a number between {min} and {max}.");
                    continue;
                }
                return val;
            }
            Console.WriteLine("Please enter a valid integer.");
        }
    }

    // If a default save file exists, ask whether to auto-load it at program start
    static void LoadIfAutoExists()
    {
        var defaultFile = "goals.txt";
        if (File.Exists(defaultFile))
        {
            Console.Write($"Found existing {defaultFile}. Load it? (y/n): ");
            var ans = Console.ReadLine();
            if (ans.Trim().ToLower().StartsWith("y"))
            {
                try
                {
                    var lines = File.ReadAllLines(defaultFile);
                    if (lines.Length > 0)
                    {
                        var newScore = int.Parse(lines[0]);
                        var newGoals = new List<Goal>();
                        for (int i = 1; i < lines.Length; i++)
                        {
                            var g = Goal.Deserialize(lines[i]);
                            if (g != null) newGoals.Add(g);
                        }
                        goals = newGoals;
                        totalScore = newScore;
                        Console.WriteLine($"Loaded {goals.Count} goals and score ({totalScore}) from {defaultFile}.");
                    }
                }
                catch
                {
                    Console.WriteLine("Failed to auto-load existing file.");
                }
            }
        }
    }
}


}
