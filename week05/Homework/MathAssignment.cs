using System;

public class MathAssignment : Assignment
{
    // Private member variables specific to MathAssignment
    private string _textbookSection;
    private string _problems;

    // Constructor: takes all four parameters
    public MathAssignment(string studentName, string topic, string textbookSection, string problems)
        : base(studentName, topic)  // call base constructor for inherited attributes
    {
        _textbookSection = textbookSection;
        _problems = problems;
    }

    // Method specific to MathAssignment
    public string GetHomeworkList()
    {
        return $"Section {_textbookSection} Problems {_problems}";
    }
}
