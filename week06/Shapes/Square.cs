using System;

public class Square : Shape
{
    private double _side;

    // Constructor calls base constructor
    public Square(string color, double side) : base(color)
    {
        _side = side;
    }

    // Override GetArea
    public override double GetArea()
    {
        return _side * _side;
    }
}
