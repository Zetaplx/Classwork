using System.Linq;

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

        public static T SafeReadLine<T>(Func<T, bool> condition, Func<string, T> parse, Func<string, bool> tryParse, string errMessage = $"Invalid input. Try again")
        {
            var value = SafeReadLine((v) => tryParse(v) && condition(parse(v)), errMessage);
            return parse(value);
        }

        public static T SafeReadLine<T>(Func<string, T> parse, Func<string, bool> tryParse, string errMessage = $"Invalid input. Try again")
        {
            var value = SafeReadLine((v) => tryParse(v), errMessage);
            return parse(value);
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

        #region Menu
        public static void Menu<T>(string headline, Action defaultAction, string defaultMessage, Func<string, T> parse, Func<string, bool> tryParse, params (T key, string message, Action response)[] choices)
        {
            Console.WriteLine(headline + ": ");
            for(int i = 0; i < choices.Length; i++)
            {
                Console.WriteLine($"{choices[i].key?.ToString() ?? $"Err ({i})"}: {choices[i].message}");
            }
            Console.WriteLine($"Default: {defaultMessage}");

            var convList = ConvertList(choices, (c) => c.key);
            CompactReadLine("Enter", out T output, () => SafeReadLine(parse, tryParse, "Invalid Choice. Try again"));
            int index = convList.IndexOf(output);

            if (index == -1) { defaultAction(); return; }

            choices[index].response();
        }
        public static void Menu<T>(string headline, Func<string, T> parse, Func<string, bool> tryParse, params (T key, string message, Action response)[] choices)
        {
            Console.WriteLine(headline + ": ");
            for (int i = 0; i < choices.Length; i++)
            {
                Console.WriteLine($"{choices[i].key?.ToString() ?? $"Err ({i})"}: {choices[i].message}");
            }
            var convList = ConvertList(choices, (c) => c.key);
            CompactReadLine("Enter", out T output, () => SafeReadLine((v) => convList.Contains(v), parse, tryParse, "Invalid Choice. Try again"));
            int index = convList.IndexOf(output);

            choices[index].response();
        }
        
        public static void Menu(string headline, Action defaultAction, string defaultMessage, params (string message, Action response)[] choices)
        {
            Console.WriteLine(headline + ": ");
            for (int i = 0; i < choices.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {choices[i].message}");
            }
            Console.WriteLine($"Default: {defaultMessage}");

            CompactReadLine("Enter", out int output, () => SafeReadInt((n) => n > 0, "Invalid Choice. Try again"));
            int index = output - 1;

            if(index >= choices.Length) { defaultAction(); return; }

            choices[index].response();
        }
        public static void Menu(string headline, params (string message, Action response)[] choices)
        {
            Console.WriteLine(headline + ": ");
            for (int i = 0; i < choices.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {choices[i].message}");
            }

            CompactReadLine("Enter", out int output, () => SafeReadInt((n) => n > 0 && n <= choices.Length, "Invalid Choice. Try again"));
            int index = output - 1;

            choices[index].response();
        }
        public static void Menu(string headline, params (int key, string message, Action response)[] choices)
        {
            Console.WriteLine(headline + ": ");
            for (int i = 0; i < choices.Length; i++)
            {
                Console.WriteLine($"{choices[i].key}: {choices[i].message}");
            }
            var keys = (from c in choices select c.key).ToList();
            CompactReadLine("Enter", out int output, () => SafeReadInt((n) => keys.Contains(n), "Invalid Choice. Try again"));
            choices[keys.IndexOf(output)].response();
        }

        public static int MenuSimple(string headline, bool simpleAbort = true, params string[] messages)
        {
            Console.WriteLine(headline + ": ");
            for (int i = 0; i < messages.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {messages[i]}");
            }
            if(simpleAbort) { Console.WriteLine("-1 to exit"); }

            CompactReadLine("Enter", out int val, () => SafeReadInt((n) => (n > 0 && n <= messages.Length) || (simpleAbort && n == -1)));
            return val;
        }
        #endregion

        public static List<T> ConvertList<T, S>(IEnumerable<S> arr, Func<S, T> convert)
        {
            List<T> list = new List<T>();
            foreach(var v in arr)
            {
                list.Add(convert(v));
            }
            return list;
        }

        public static void PrintTable<T>(IEnumerable<T> arr, params (string title, int width, Func<T, string> val)[] columns)
        {
            foreach(var col in columns)
            {
                Console.Write(col.title.PadRight(col.width));
            }

            Console.WriteLine();

            foreach(var row in arr)
            {
                foreach(var col in columns)
                {
                    Console.Write(col.val(row).PadRight(col.width));
                }
                Console.WriteLine();
            }
        }
    }
}
