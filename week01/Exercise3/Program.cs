using System;

class Program
{
    static void Main()
    {
        Random random = new Random();
        int magicNumber = random.Next(1, 101); // Random number between 1 and 100
        int guess = 0;

        Console.WriteLine("Welcome to the Magic Number game!");
        Console.WriteLine("I have chosen a number between 1 and 100. Try to guess it!");

        // Loop until the user guesses correctly
        while (guess != magicNumber)
        {
            Console.Write("What is your guess? ");
            string input = Console.ReadLine();
            guess = int.Parse(input);

            if (guess < magicNumber)
            {
                Console.WriteLine("Higher");
            }
            else if (guess > magicNumber)
            {
                Console.WriteLine("Lower");
            }
            else
            {
                Console.WriteLine("You guessed it!");
            }
        }
    }
}
