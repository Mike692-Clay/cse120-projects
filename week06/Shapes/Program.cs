using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Test individual shapes
        Square square = new Square("Red", 5);
        Console.WriteLine($"Square - Color: {square.GetColor()}, Area: {square.GetArea()}");

        Rectangle rectangle = new Rectangle("Blue", 4, 6);
        Console.WriteLine($"Rectangle - Color: {rectangle.GetColor()}, Area: {rectangle.GetArea()}");

        Circle circle = new Circle("Green", 3);
        Console.WriteLine($"Circle - Color: {circle.GetColor()}, Area: {circle.GetArea()}");

        Console.WriteLine("\n--- Using Polymorphism with a List ---");

        // Create a List of shapes
        List<Shape> shapes = new List<Shape>
        {
            square,
            rectangle,
            circle
        };

        // Iterate and display polymorphic results
        foreach (Shape shape in shapes)
        {
            Console.WriteLine($"{shape.GetType().Name} - Color: {shape.GetColor()}, Area: {shape.GetArea()}");
        }
    }
}
