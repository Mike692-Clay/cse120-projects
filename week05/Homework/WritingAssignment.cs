using System;

public class WritingAssignment : Assignment
{
    // Private member variable specific to WritingAssignment
    private string _title;

    // Constructor: accepts studentName, topic, and title
    public WritingAssignment(string studentName, string topic, string title)
        : base(studentName, topic)  // call base constructor
    {
        _title = title;
    }

    // Method specific to WritingAssignment
    public string GetWritingInformation()
    {
        return $"{_title} by {GetStudentName()}";
    }
}
