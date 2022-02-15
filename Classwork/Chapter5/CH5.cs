namespace Classwork.Chapter5
{
    public static class Q5_18
    {
        public static void Run(bool intro = true)
        {
            Console.Clear();
            #region Intro Text
            if (intro)
            {
                Console.WriteLine("Welcome to the Credit Limit Calculator. Press enter to begin.");
                Console.ReadLine();
                Console.Clear();
            }
            #endregion

            // Stores all customer accounts for the final display.
            List<Customer> customers = new List<Customer>();

            #region Account ID Input
            Console.Write("Enter an account number to add an account. Enter \"e\" to exit: ");
            string line = Console.ReadLine() ?? "";
            int accountID = 0;
            while(line.ToLower() != "e" && (!int.TryParse(line, out accountID) || accountID < 0))
            {
                Console.Write("Please enter a valid account number: ");
                line = Console.ReadLine() ?? "";
            }
            #endregion

            while (line.ToLower() != "e")
            {
                // Okay. Utils.SafeReadInt is a method I wrote to make my life a bit easier when it comes to reading ints based off of some condition
                // If you'd like me to full justify/explain this code I can, just let me know. The code for it all can be found in the Utils folder.
                // I do believe I have shown that I know how to perform the sort of condition checking asked in the assignment with the initial line above.
                // Additionally, that initial input is not something my methods are designed to handle since it works with both the integer and the string versions of the input.

                #region Create account
                Console.Write("Enter the initial balance: ");
                int init = Utils.SafeReadInt((n) => n >= 0, "Please enter a valid balance");

                Console.Write("Enter the total account charges for the month: ");
                int charges = Utils.SafeReadInt((n) => n >= 0, "Please enter a valid amount");

                Console.Write("Enter the total account credits for the month: ");
                int credits = Utils.SafeReadInt((n) => n >= 0, "Please enter a valid amount");

                Console.Write("Enter the credit limit: ");
                int creditLimit = Utils.SafeReadInt((n) => n >= 0, "Please enter a valid credit limit");

                Customer customer = new Customer(accountID, init, charges, credits, creditLimit);
                Console.WriteLine(customer);

                customers.Add(customer);
                #endregion

                #region Account ID Input
                Console.Write("\n\nEnter an account number to add an account. Enter \"e\" to exit: ");
                line = Console.ReadLine() ?? "";
                while (line.ToLower() != "e" && (!int.TryParse(line, out accountID) || accountID < 0))
                {
                    Console.Write("Please enter a valid account number or \"e\" to exit: ");
                    line = Console.ReadLine() ?? "";
                }
                #endregion
            }

            int exceeded = 0;
            Console.WriteLine("\n\nAll created accounts: ");
            foreach (var acc in customers)
            {
                Console.WriteLine(acc);
                if (acc.ExceedsCreditLimit(out var amt)) exceeded++;
            }

            Console.WriteLine($"{exceeded} accounts exceeded their credit limit. {customers.Count - exceeded} did not.");

            Console.Write("\nEnter anything to exit: ");
            Console.ReadLine();
        }

        // Not the real version
        public static void RunIncorrect(bool intro = true)
        {
            Console.Clear();
            #region Intro Text
            if (intro)
            {
                Console.WriteLine("Welcome to the Credit Limit Calculator. Press enter to begin.");
                Console.ReadLine();
                Console.Clear();
            }
            #endregion

            
            Dictionary<int, CreditLimitCalculator.Account> accounts = new Dictionary<int, CreditLimitCalculator.Account>(); // Stores all accounts
            string line;

            do
            {
                #region Account Input
                Console.Clear();
                Console.WriteLine("Enter an account number. New numbers will create a new account. " +
                    "Existing numbers will allow you to update the account's balance. \nEnter e to exit the application.");
                Console.Write("Account Number: ");
                int accountNumber;
                line = Console.ReadLine() ?? "";
                while (!int.TryParse(line, out accountNumber) && line.ToLower() != "e")
                {
                    Console.Write("Please enter a valid number: ");
                    line = Console.ReadLine() ?? "";
                }
                #endregion

                if (line.ToLower() != "e")
                {
                    // Check if account already exists
                    if (accounts.TryGetValue(accountNumber, out var account))
                    {
                        CreditLimitCalculator.ModifyAccount(account);
                    }
                    else
                    {
                        CreditLimitCalculator.Account newAccount = CreditLimitCalculator.CreateAccount(accountNumber);
                        accounts.Add(accountNumber, newAccount);

                        Console.WriteLine("\nWould you like to modify new account?");
                        Console.Write("Enter y or n: ");
                        string newLine = Console.ReadLine() ?? "";
                        bool y = newLine.ToLower() == "y";
                        bool n = newLine.ToLower() == "n";
                        while (!(y || n))
                        {
                            Console.Write("Please enter y or n: ");
                            newLine = Console.ReadLine() ?? "";
                            y = newLine.ToLower() == "y";
                            n = newLine.ToLower() == "n";
                        }
                        if (y) CreditLimitCalculator.ModifyAccount(newAccount);
                    }
                }
            } while (line.ToLower() != "e");
        }
    }

    #region Not the real version
    public class CreditLimitCalculator
    {
        /// <summary>
        /// Creates a new account based one a given account number and user input.
        /// </summary>
        public static Account CreateAccount(int accountNumber)
        {
            Console.Clear();
            Console.WriteLine($"Creating account: {accountNumber}");
            Console.Write("Enter initial balance: ");
            int balance;
            while (!int.TryParse(Console.ReadLine(), out balance) || balance < 0)
            {
                Console.Write("Please enter a valid balance: ");
            }

            Console.Write("Enter credit limit: ");
            int creditLimit;
            while (!int.TryParse(Console.ReadLine(), out creditLimit) || creditLimit < 0)
            {
                Console.Write("Please enter a valid creditLimit: ");
            }

            return new Account(accountNumber, balance, creditLimit);
        }
        /// <summary>
        /// Modifies a given account based on user input.
        /// </summary>
        /// <param name="account"></param>
        public static void ModifyAccount(Account account)
        {
            Console.Clear();
            Console.WriteLine($"Modifying account {account.AccountNumber}. Current balance: {account.CurrentBalance}");
            Console.Write("Enter new charges: ");
            int charges;
            while (!int.TryParse(Console.ReadLine(), out charges))
            {
                Console.Write("Please enter a valid charge: ");
            }
            int credits;
            Console.Write("Enter new credits: ");
            while (!int.TryParse(Console.ReadLine(), out credits))
            {
                Console.Write("Please enter a valid credit: ");
            }

            if (!account.TryUpdateBalance(charges, credits))
            {
                Console.WriteLine("\nCREDIT LIMIT EXCEEDED. UNABLE TO MODIFY ACCOUNT.");
            }
            else
            {
                Console.Write($"\nNew Balance: {account.CurrentBalance}");
            }

            Console.WriteLine("\nEnter r to continue modifying. Enter anything else to return to account menu.");
            if (Console.ReadLine().ToLower() == "r") ModifyAccount(account);
        }

        /// <summary>
        /// Contains all information relavent to a single account.
        /// </summary>
        public class Account
        {
            public readonly int AccountNumber;

            int initialBalance;
            int totalCharges;
            int totalCredit;
            int creditLimit;

            

            public int CurrentBalance => initialBalance + totalCharges - totalCredit;

            public Account(int accountNumber, int balance, int creditLimit, int initialCharges = 0, int initialCredit = 0)
            {
                AccountNumber = accountNumber;
                initialBalance = balance;
                this.creditLimit = creditLimit;
                totalCharges = initialCharges;
                totalCredit = initialCredit;
            }

            public bool TryUpdateBalance(int charges, int credit)
            {
                int updatedCharges = totalCharges + charges;
                int updatedCredit = totalCredit + credit;

                int currentBalance = initialBalance + updatedCharges - updatedCredit;
                if (currentBalance < -creditLimit) return false;
                totalCharges = updatedCharges;
                totalCredit = updatedCredit;
                return true;
            }
        }
    }
    #endregion

    public class Customer
    {
        public int AccountID { get; private set; }
        public int InitalBalance { get; private set; }
        public int TotalCharges { get; private set; }
        public int TotalCredits { get; private set; }
        public int CreditLimit { get; private set; }

        public int CalculateBalance() => InitalBalance + TotalCharges - TotalCredits;
        public bool ExceedsCreditLimit(out int amt)
        {
            var balance = CalculateBalance();

            if(balance < -CreditLimit)
            {
                amt = -balance - CreditLimit;
                return true;
            }
            amt = 0;
            return false;
        }

        public Customer(int id, int balance, int charges, int credits, int limit)
        {
            AccountID = id;
            InitalBalance = balance;
            TotalCharges = charges;
            TotalCredits = credits;
            CreditLimit = limit;
        }


        public override string ToString()
        {
            return $"{AccountID}: ${CalculateBalance()} (IB + Charges - Credits => {InitalBalance} + {TotalCharges} + {TotalCredits}) {(ExceedsCreditLimit(out var amt) ? "Exceeded credit limit by $" + amt : "" )}";
        }
    }
}
