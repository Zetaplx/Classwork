using System.Linq;
using Classwork.Examples;

namespace Classwork.Chapter9
{
    public static class Q9_3
    {
        public static void Run(bool intro = true)
        {
            #region Default Data
            List<Invoice> invoices = new List<Invoice>();
            invoices.Add(new Invoice(83, "Electric Sander", 7  , 57.98M));
            invoices.Add(new Invoice(24, "Power Saw"      , 18 , 99.99M));
            invoices.Add(new Invoice(7 , "Sledge Hammer"  , 11 , 21.50M));
            invoices.Add(new Invoice(77, "Hammer"         , 76 , 11.99M));
            invoices.Add(new Invoice(39, "Lawn Mower"     , 3  , 79.50M));
            invoices.Add(new Invoice(68, "Screw Driver"   , 106,  6.99M));
            invoices.Add(new Invoice(56, "Jig Saw"        , 21 , 11.00M));
            invoices.Add(new Invoice(3 , "Wrench"         , 34 ,  7.50M));
            #endregion

            Console.Clear();
            #region Intro Text
            if (intro)
            {
                Console.WriteLine("Welcome to the Credit Limit Calculator. Press enter to begin.");
                Console.ReadLine();
                Console.Clear();
            }
            #endregion

            bool running = true;

            while(running)
            {
                Console.Clear();
                Utils.Menu("Select which query you would like to perform",
                    (1, "Query A: Full Stock sorted by Descriptions", () => QueryA(invoices)),
                    (2, "Query B: Full Stock sorted by Price", () => QueryB(invoices)),
                    (3, "Query C: Descriptions and Prices sorted by Price", () => QueryC(invoices)),
                    (4, "Query D: Descriptions and Values sorted by Value", () => QueryD(invoices)),
                    (5, "Query E: Descriptions and Values between $200 and $500 sorted by Value", () => QueryE(invoices)),
                    (6, "Query F: Part Numbers with Prices greater than $50 sorted by Price", () => QueryF(invoices)),
                    (7, "Query G: Description of part with maximum price\n", () => QueryG(invoices)),
                    (8, "Print default parts table", () => {
                            Console.Clear();
                            Console.WriteLine("Catalogue");
                            Utils.PrintTable(invoices,
                                ("Part Number", 20, (invoice) => "" + invoice.PartNumber),
                                ("Description", 25, (invoice) => invoice.PartDescription),
                                ("Quantity", 20, (invoice) => "" + invoice.Quantity),
                                ("Price", 10, (invoice) => $"{invoice.Price:c}")
                            );
                            Console.Write("Enter to return to menu: "); Console.ReadLine();
                        }
                    ),
                    (9, "Perform all querys in sequence", () => { QueryA(invoices); QueryB(invoices); QueryC(invoices); QueryD(invoices); QueryE(invoices); QueryF(invoices); QueryG(invoices); }),
                    (-1, "Exit Application", () => running = false)
                );
            }
        }

        private static void QueryA(List<Invoice> invoices)
        {
            Console.Clear();
            Console.WriteLine("Query A\n");
            var sortedByDescription = from i in invoices orderby i.PartDescription select i;

            Utils.PrintTable(sortedByDescription, 
                ("Part Number", 20, (invoice) => "" + invoice.PartNumber),
                ("Description", 25, (invoice) => invoice.PartDescription),
                ("Quantity"   , 20, (invoice) => "" + invoice.Quantity),
                ("Price"      , 10, (invoice) => $"{invoice.Price:c}")
            );

            Console.Write("Enter to return to menu: ");
            Console.ReadLine();
        }
        private static void QueryB(List<Invoice> invoices)
        {
            Console.Clear();
            Console.WriteLine("Query B\n");

            var sortedByPrice = from i in invoices orderby i.Price select i;

            Utils.PrintTable(sortedByPrice,
                ("Product Number", 20, (t) => "" + t.PartNumber),
                ("Description", 20, (t) => "" + t.PartDescription),
                ("Quantity", 15, (t) => "" + t.Quantity),
                ("Price", 10, (t) => $"{t.Price:c}")
            );
            Console.Write("Enter to return to menu: ");
            Console.ReadLine();
        }
        private static void QueryC(List<Invoice> invoices)
        {
            Console.Clear();
            Console.WriteLine("Query C\n");

            IEnumerable<(string desc, decimal price)> descriptionsByPrice = from i in invoices orderby i.Price select (i.PartDescription, i.Price);
            Utils.PrintTable(descriptionsByPrice,
                ("Description", 20, (t) => t.desc),
                ("Price", 10, (t) => $"{t.price:c}")
            );
            Console.Write("Enter to return to menu: ");
            Console.ReadLine();
        }
        private static void QueryD(List<Invoice> invoices)
        {
            Console.Clear();
            Console.WriteLine("Query D\n");

            IEnumerable<(string desc, decimal value)> descriptionsByValue = from i in invoices orderby (i.Price * i.Quantity) select (i.PartDescription, i.Price * i.Quantity);

            Utils.PrintTable(descriptionsByValue,
                ("Description", 20, (t) => t.desc),
                ("Invoice Total", 15, (t) => $"{t.value:c}")
            );

            Console.Write("Enter to return to menu: ");
            Console.ReadLine();
        }
        private static void QueryE(List<Invoice> invoices)
        {
            Console.Clear();
            Console.WriteLine("Query E\n");

            IEnumerable<(string desc, decimal value)> descriptionByValue = from i in invoices orderby (i.Price * i.Quantity) select (i.PartDescription, i.Price * i.Quantity);

            var limitedTotals = from i in descriptionByValue where i.value >= 200 && i.value <= 500 select i;
            Utils.PrintTable(limitedTotals,
                ("Description", 20, (t) => t.desc),
                ("Invoice Total", 15, (t) => $"{t.value:c}")
            );

            Console.Write("Enter to return to menu: ");
            Console.ReadLine();
        }
        private static void QueryF(List<Invoice> invoices)
        {
            Console.Clear();
            Console.WriteLine("Query F\n");

            var partNumbersByPrice = from i in invoices where i.Price > 50M orderby i.Price select i.PartNumber;
            Utils.PrintTable(partNumbersByPrice, ("Part Number", 20, (t) => "" + t));

            Console.Write("Enter to return to menu: ");
            Console.ReadLine();
        }
        private static void QueryG(List<Invoice> invoices)
        {
            Console.Clear();
            Console.WriteLine("Query G\n");

            Console.WriteLine($"Max value Item: {(from i in invoices orderby (i.Price * i.Quantity) select i.PartDescription).Last()}");

            Console.Write("Enter to return to menu: ");
            Console.ReadLine();
        }
    }
}
