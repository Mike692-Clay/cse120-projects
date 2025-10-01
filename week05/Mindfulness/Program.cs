using System;
using System.Collections.Generic;
using System.Threading;

abstract class Activity
{
    private string _name;
    private string _description;
    private int _duration;

    public Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void Start()
    {
        Console.Clear();
        Console.WriteLine($"--- {_name} ---");
        Console.WriteLine(_description);
        Console.Write("\nEnter duration in seconds: ");
        _duration = int.Parse(Console.ReadLine() ?? "30");

        Console.WriteLine("\nGet ready to begin...");
        ShowSpinner(3);

        RunActivity(_duration);

        Console.WriteLine("\nWell done!!");
        ShowSpinner(2);
        Console.WriteLine($"You completed the {_name} for {_duration} seconds.");
        ShowSpinner(3);
    }

    protected abstract void RunActivity(int duration);

    protected void ShowSpinner(int seconds)
    {
        string[] spinner = { "|", "/", "-", "\\" };
        DateTime end = DateTime.Now.AddSeconds(seconds);
        int i = 0;

        while (DateTime.Now < end)
        {
            Console.Write(spinner[i % spinner.Length]);
            Thread.Sleep(200);
            Console.Write("\b \b");
            i++;
        }
    }

    protected void ShowCountdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i);
            Thread.Sleep(1000);
            Console.Write("\b \b");
        }
    }
}

class BreathingActivity : Activity
{
    public BreathingActivity() 
        : base("Breathing Activity", 
               "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.") { }

    protected override void RunActivity(int duration)
    {
        DateTime end = DateTime.Now.AddSeconds(duration);

        while (DateTime.Now < end)
        {
            Console.Write("\nBreathe in... ");
            ShowCountdown(3);

            Console.Write("\nBreathe out... ");
            ShowCountdown(3);
        }
    }
}

class ReflectionActivity : Activity
{
    private static string[] _prompts =
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private static string[] _questions =
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience?",
        "What did you learn about yourself?",
        "How can you keep this in mind in the future?"
    };

    public ReflectionActivity() 
        : base("Reflection Activity",
               "This activity will help you reflect on times in your life when you have shown strength and resilience.") { }

    protected override void RunActivity(int duration)
    {
        Random rand = new Random();
        Console.WriteLine("\n" + _prompts[rand.Next(_prompts.Length)]);
        ShowSpinner(5);

        DateTime end = DateTime.Now.AddSeconds(duration);

        while (DateTime.Now < end)
        {
            string question = _questions[rand.Next(_questions.Length)];
            Console.WriteLine("\n" + question);
            ShowSpinner(5);
        }
    }
}

class ListingActivity : Activity
{
    private static string[] _prompts =
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity()
        : base("Listing Activity",
               "This activity will help you reflect on the good things in your life by having you list as many things as you can.") { }

    protected override void RunActivity(int duration)
    {
        Random rand = new Random();
        Console.WriteLine("\n" + _prompts[rand.Next(_prompts.Length)]);
        Console.WriteLine("You will have a few seconds to think...");
        ShowCountdown(5);

        Console.WriteLine("\nStart listing items (press Enter after each):");

        List<string> items = new List<string>();
        DateTime end = DateTime.Now.AddSeconds(duration);

        while (DateTime.Now < end)
        {
            if (Console.KeyAvailable)
            {
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    items.Add(input);
            }
        }

        Console.WriteLine($"\nYou listed {items.Count} items!");
    }
}

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Menu Options:");
            Console.WriteLine("1. Start Breathing Activity");
            Console.WriteLine("2. Start Reflection Activity");
            Console.WriteLine("3. Start Listing Activity");
            Console.WriteLine("4. Quit");
            Console.Write("Select a choice from the menu: ");

            string choice = Console.ReadLine();

            Activity activity = null;
            switch (choice)
            {
                case "1": activity = new BreathingActivity(); break;
                case "2": activity = new ReflectionActivity(); break;
                case "3": activity = new ListingActivity(); break;
                case "4": return;
                default: Console.WriteLine("Invalid choice. Press any key to try again."); Console.ReadKey(); continue;
            }

            activity.Start();
        }
    }
}
