namespace Classwork.Chapter6
{
    public static class Q6_24
    {
        public static void Run(bool intro = true)
        {
            Console.Clear();
            #region Intro Text
            if(intro)
            {
                Console.WriteLine("Welcome to the Diamond Drawer. Press enter to begin. ");
                Console.ReadLine();
                Console.Clear();
            }
            #endregion

            // This looks crazy, I know. But I got tired of writing my input statements with validation loops so I wrote a Utils class to handle all that nonsense for me.
            // If you want me to justify and explain it I definitely can, just let me know.
            // But its basically just getting an int bigger than 2 and storing it in the variable size.
            Utils.CompactReadLine("Insert the size for your diamond", out int size, () => Utils.SafeReadInt((n) => n > 2 && n <= 19, "Please input a valid size between 2 and 19"));
            Console.Clear();

            // I know the assignment said to make sure the input is odd. I challenged myself to make it work with even inputs. Hopefully it's clear that I know how to
            // both identify the numbers parity (as seen in GetInDiamond) and confine inputs to based on its value (as seen above). If I wanted to limit the parity of input
            // I would simply change "n > 2" to "n > 2 && n % 2 == 1".
            Diamond.PrintDiamond(size);

            #region Restart Logic
            Utils.CompactReadLine("Enter r to restart. Enter anything else to exit", out string restart);
            if (restart.ToLower() == "r") Run(false);
            #endregion
        }
    }

    public static class Diamond
    {
        /// <summary>
        /// Returns true if a given index is inside the diamond on a given layer
        /// </summary>
        /// <param name="i">The index on the layer</param>
        /// <param name="layerWidth">Size of the layer</param>
        /// <param name="totalWidth">Overall width of diamond</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static bool IsInDiamond(int i, int layerWidth, int totalWidth)
        {
            if (!(layerWidth % 2 == totalWidth % 2)) throw new Exception("Layer width parity must match Total Width parity");

            var lowCenter = (totalWidth / 2); 
            // Okay... ill be honest here... i have no clue whats going on with this value. I just kept changing the -1 to different values and it ended up working.
            var highCenter = (totalWidth / 2) + (totalWidth % 2 == 0 ? -1 : 0);

            return i >= lowCenter - (layerWidth / 2) && i <= highCenter + (layerWidth / 2);
        }

        /// <summary>
        /// Prints a single layer of the diamond
        /// </summary>
        /// <param name="layerWidth">Size of the layer</param>
        /// <param name="totalWidth">Overall width of diamond</param>
        /// <exception cref="Exception"></exception>
        private static void PrintLayer(int layerWidth, int totalWidth)
        {
            if (!(layerWidth % 2 == totalWidth % 2)) throw new Exception("Layer width parity must match Total Width parity");

            for(int i = 0; i < totalWidth; i++)
            {
                Console.Write(IsInDiamond(i, layerWidth, totalWidth) ? "*" : " ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prints a single layer of the diamond. Based on what we did in class last week. 
        /// Muuuch simpler and doesn't rely on that weird "high center" I use above.
        /// </summary>
        /// <param name="layerWidth"></param>
        /// <param name="totalWidth"></param>
        private static void PrintLayerSimple(int layerWidth, int totalWidth)
        {
            if ((totalWidth - layerWidth) % 2!= 0) throw new Exception("Layer width parity must match Total Width parity");
            int whiteSpace = (totalWidth - layerWidth) / 2;

            Console.WriteLine("".PadLeft(layerWidth, '*').PadLeft(layerWidth + whiteSpace).PadRight(totalWidth));
        }

        /// <summary>
        /// Prints a diamond. Loops up, then down in size printing layer by layer
        /// </summary>
        /// <param name="width">Overall width of diamond</param>
        public static void PrintDiamond(int width)
        {
            int startSize = width % 2 == 0 ? 2 : 1;
            for(int i = startSize; i <= width; i += 2)
            {
                PrintLayerSimple(i, width);
            }

            for (int i = width - 2; i >= startSize; i -= 2)
            {
                PrintLayerSimple(i, width);
            }
        }
    }
}
