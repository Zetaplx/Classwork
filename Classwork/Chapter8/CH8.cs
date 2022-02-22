namespace Classwork.Chapter8
{
    public class Q8_17
    {
        public static void Run(bool intro = true)
        {
            Console.Clear();

            #region Intro Text
            if (intro)
            {
                Console.WriteLine("Welcome to the Dice Roller. Press enter to begin. ");
                Console.ReadLine();
                Console.Clear();
            }
            #endregion

            Utils.CompactReadLine("How many dice will you be rolling?",       out int numDice,  () => Utils.SafeReadInt((n) => n > 0,  "Please enter a positive number"));
            Utils.CompactReadLine("How many sides are on each die?",          out int numSides, () => Utils.SafeReadInt((n) => n >= 3, "Please enter a positive number (at least 3)"));
            Utils.CompactReadLine("How many times will you roll these dice?", out int numRolls, () => Utils.SafeReadInt((n) => n > 0,  "Please enter a positive number"));

            int minRoll = numDice;
            int maxRoll = numDice * numSides;

            int[] counter = new int[maxRoll - minRoll + 1];
            Random rand = new Random();

            for (int i = 0; i < numRolls; i++)
            {
                int sum = 0;
                for(int d = 0; d < numDice; d++)
                {
                    sum += rand.Next(1, numSides + 1);
                }

                counter[sum - minRoll]++;
            }

            for(int i = 0; i < counter.Length; i++)
            {
                string str = $"{minRoll + i}:\t({counter[i]})\t-> ";
                Console.WriteLine(str.PadRight(str.Length + counter[i], '*'));
            }

            #region Restart Logic
            Utils.CompactReadLine("Enter r to restart. Enter anything else to exit", out string restart);
            if (restart.ToLower() == "r") Run(false);
            #endregion
        }
    }
}
