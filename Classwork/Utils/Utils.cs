using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classwork
{
    public static class Utils
    {
        #region Console ReadLine Shortcuts
        /// <summary>
        /// Reads line from user. Will repeat prompt until input meets the condition.
        /// </summary>
        /// <param name="condition">Condition that user input must meet.</param>
        /// <param name="errMessage">Text that prompts the user on an invalid input.</param>
        /// <returns></returns>
        public static string SafeReadLine(Func<string, bool> condition, string errMessage = "Invalid Input. Try again")
        {
            string output = "";
            output = Console.ReadLine() ?? "";
            while (!condition(output))
            {
                Console.Write(errMessage + ": ");
                output = Console.ReadLine() ?? "";
            }
            return output;
        }

        /// <summary>
        /// Reads line from user as an integer. Will repeat promt until the input is a valid integer and the condition is met.
        /// </summary>
        /// <param name="condition">Condition that user input must meet.</param>
        /// <param name="errMessage">Text that prompts the user on an invalid input.</param>
        /// <returns></returns>
        public static int SafeReadInt(Func<int, bool> condition, string errMessage = "Invalid Input. Please enter a valid integer")
        {
            var number = SafeReadLine((s) => int.TryParse(s, out var num) && condition(num), errMessage);
            return int.Parse(number);
        }

        /// <summary>
        /// Reads line from user as an integer. Will repeat promt until the input is a valid integer.
        /// </summary>
        /// <param name="errMessage">Text that prompts the user on an invalid input.</param>
        /// <returns></returns>
        public static int SafeReadInt(string errMessage = "Invalid Input. Please enter a valid integer") => SafeReadInt((n) => true, errMessage);

        /// <summary>
        /// Reads line from user as a float. Will repeat promt until the input is a valid float and the condition is met.
        /// </summary>
        /// <param name="condition">Condition that user input must meet.</param>
        /// <param name="errMessage">Text that prompts the user on an invalid input.</param>
        /// <returns></returns>
        public static float SafeReadFloat(Func<float, bool> condition, string errMessage = "Invalid Input. Please enter a valid floating point number")
        {
            var number = SafeReadLine((s) => float.TryParse(s, out var num) && condition(num), errMessage);
            return float.Parse(number);
        }
        /// <summary>
        /// Reads line from user as a float. Will repeat promt until the input is a valid float .
        /// </summary>
        /// <param name="errMessage">Text that prompts the user on an invalid input.</param>
        /// <returns></returns>
        public static float SafeReadFloat(string errMessage = "Invalid Input. Please enter a valid floating point number") => SafeReadFloat((n) => true, errMessage);

        /// <summary>
        /// Reads line from user as a double. Will repeat promt until the input is a valid double and the condition is met.
        /// </summary>
        /// <param name="condition">Condition that user input must meet.</param>
        /// <param name="errMessage">Text that prompts the user on an invalid input.</param>
        /// <returns></returns>
        public static double SafeReadDouble(Func<double, bool> condition, string errMessage = "Invalid Input. Please enter a valid floating point number")
        {
            var number = SafeReadLine((s) => double.TryParse(s, out var num) && condition(num), errMessage);
            return double.Parse(number);
        }
        /// <summary>
        /// Reads line from user as a double. Will repeat promt until the input is a valid double
        /// </summary>
        /// <param name="errMessage">Text that prompts the user on an invalid input.</param>
        /// <returns></returns>
        public static double SafeReadDouble(string errMessage = "Invalid Input. Please enter a valid floating point number") => SafeReadDouble((n) => true, errMessage);

        /// <summary>
        /// Prompts the user to input some data of type <typeparamref name="T"/>. Inputs that data using the <paramref name="readLineFunction"/> and stores it in <paramref name="output"/>
        /// </summary>
        /// <param name="msg">Message that prompts the user for input.</param>
        /// <param name="output">Reference that recieves the user's input.</param>
        /// <param name="readLineFunction">Function which aquires user's input.</param>
        public static void CompactReadLine<T>(string msg, out T output, Func<T> readLineFunction)
        {
            Console.Write(msg + ": ");
            output = readLineFunction();
        }
        /// <summary>
        /// Prompts the user to input some a string and stores it in <paramref name="output"/>
        /// </summary>
        /// <param name="msg">Message that prompts the user for input.</param>
        /// <param name="output">Reference that recieves the user's input.</param>
        /// <param name="readLineFunction">Function which aquires user's input.</param>
        public static void CompactReadLine(string msg, out string output) => CompactReadLine(msg, out output, () => Console.ReadLine() ?? "");
        #endregion
    }
}
