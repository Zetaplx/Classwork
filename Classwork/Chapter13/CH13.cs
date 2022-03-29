using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classwork.Chapter13
{
    public static class Q13_10
    {
        public static void Run(bool intro = true)
        {
            Console.Clear();
            double miles = 0;
            double gallons = 0;

            Console.Write("Enter Miles Driven: ");
            try
            {
                miles = GatherInput((n) => n > 0, "Input must be greater than 0");
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
                Console.WriteLine("Enter to try again");
                Console.ReadLine();
                Run(false);
            }

            Console.Write("Enter Gallons Used: ");
            try
            {
                gallons = GatherInput((n) => n > 0, "Input must be greater than 0");
            }
            catch(FormatException fe)
            {
                Console.WriteLine(fe.Message);
                Console.WriteLine("Enter to try again");
                Console.ReadLine();
                Run(false);
            }

            Console.WriteLine($"MPG = {miles / gallons}");

            Console.WriteLine("Enter r to replay. Enter anything else to exit.");
            if((Console.ReadLine() ?? "").ToLower() == "r") Run(false);
        }

        public static double GatherInput(Func<double, bool> condition, string conditionFailedMessage = "")
        {
            string str = Console.ReadLine() ?? "";
            if (!double.TryParse(str, out var input)) throw new FormatException($"Cannot convert from \"{str}\" to double.");
            if (!condition(input)) throw new FormatException($"{input} does not meet given condition. " + condition);
            return input;
        }
    }
}
