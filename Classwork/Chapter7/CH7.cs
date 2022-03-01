namespace Classwork.Chapter7
{
    public static class Q7_30
    {
        public static void Run(bool intro = true)
        {
            Console.Clear();
            #region Intro Text
            if (intro)
            {
                Console.WriteLine("Welcome to the Number Guesser. Press enter to begin. ");
                Console.ReadLine();
                Console.Clear();
            }
            #endregion

            Random rand = new Random();
            int number = rand.Next(Guesser.MinValue, Guesser.MaxValue + 1);

            bool correct = false;
            do
            {
                Utils.CompactReadLine("Enter your guess", out int guess, () => Utils.SafeReadInt((n) => n >= Guesser.MinValue && n <= Guesser.MaxValue, $"Enter a value between {Guesser.MinValue} and {Guesser.MaxValue}"));
                var evaluate = Guesser.EvaluateGuess(guess, number);

                switch (evaluate)
                {
                    case Guesser.Guess.TOO_LOW:
                        Console.WriteLine("Too Low. Try again.");
                        break;
                    case Guesser.Guess.TOO_HIGH:
                        Console.WriteLine("Too High. Try again.");
                        break;
                    // If it's not too low and not too high, it must be correct.
                    default:
                        Console.WriteLine("Congratulations! You guessed the number!");
                        correct = true;
                        break;
                }

            } while (!correct);


            #region Restart Logic
            Utils.CompactReadLine("Enter r to restart. Enter anything else to exit", out string restart);
            if (restart.ToLower() == "r") Run(false);
            #endregion
        }
    }

    public static class Guesser
    {
        public const int MinValue = 1;
        public const int MaxValue = 1000;

        public enum Guess
        {
            TOO_LOW,
            CORRECT,
            TOO_HIGH
        }

        public static Guess EvaluateGuess(int guess, int correct)
        {
            return guess == correct ? Guess.CORRECT : guess < correct ? Guess.TOO_LOW : Guess.TOO_HIGH;
        }
    }
}
