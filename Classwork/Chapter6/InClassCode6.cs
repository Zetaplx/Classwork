using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classwork.Chapter6.Classtime
{
    public class InClassCode6
    {
        public static void Run()
        {
            int productNumber;
            float profit = 0;

            List<int> products = new List<int>(new int[]{ 1, 2, 3 });

            do
            {
                Utils.CompactReadLine("Which product? (-1 to exit)", out productNumber, () => Utils.SafeReadInt((n) => n == -1 || products.Contains(n)));
                if (productNumber != -1)
                {
                    Utils.CompactReadLine("How many?", out int qty, () => Utils.SafeReadInt((n) => n > 0));
                    switch (productNumber)
                    {
                        case 1:
                            profit += qty * 2.98f;
                            break;
                        case 2:
                            profit += qty * 4.50f;
                            break;
                        case 3:
                            profit += qty * 9.98f;
                            break;
                    }
                    
                    Console.WriteLine($"Running Total: {profit:c}\n");
                }
            } while (productNumber != -1);

            Console.WriteLine($"Total: {profit:c}");
        }
    }

    public static class Triangle
    {
        public static void D(int n)
        {
            for(int r = 1; r <= n; r++)
            {
                Console.WriteLine("".PadLeft(r, '*').PadLeft(n, ' '));
            }
        }
    }
}
