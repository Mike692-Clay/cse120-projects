using System;
using System.Collections.Generic;

class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}

class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthSeconds { get; set; }

    private List<Comment> comments = new List<Comment>();

    public Video(string title, string author, int lengthSeconds)
    {
        Title = title;
        Author = author;
        LengthSeconds = lengthSeconds;
    }

    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }

    public int GetCommentCount()
    {
        return comments.Count;
    }

    public void DisplayVideoInfo()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {LengthSeconds} seconds");
        Console.WriteLine($"Comments ({GetCommentCount()}):");

        foreach (Comment c in comments)
        {
            Console.WriteLine($"  {c.CommenterName}: {c.Text}");
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create a few videos
        Video video1 = new Video("How to Cook Pasta", "ChefMike", 600);
        video1.AddComment(new Comment("Alice", "This helped me a lot!"));
        video1.AddComment(new Comment("Bob", "Great recipe, thanks."));
        video1.AddComment(new Comment("Charlie", "Can you make a vegan version?"));

        Video video2 = new Video("Learn C# Basics", "CodeAcademy", 1200);
        video2.AddComment(new Comment("Dave", "Very clear explanation."));
        video2.AddComment(new Comment("Eva", "Can you do advanced topics next?"));
        video2.AddComment(new Comment("Frank", "I finally understand classes!"));

        Video video3 = new Video("Top 10 Travel Destinations", "GlobeTrotter", 900);
        video3.AddComment(new Comment("Grace", "Adding these to my bucket list!"));
        video3.AddComment(new Comment("Henry", "I’ve been to 3 of these already."));
        video3.AddComment(new Comment("Isabella", "Great video and editing."));

        // Put them in a list
        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Display info for each video
        foreach (Video v in videos)
        {
            v.DisplayVideoInfo();
        }
    }
}

