using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Dictionary<int, CreditLimitCalculator.Account> accounts = new Dictionary<int, CreditLimitCalculator.Account>();
            string line;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter an account number. New numbers will create a new account. " +
                    "Existing numbers will allow you to update the account's balance. \nEnter e to exit the application.");
                Console.Write("Account Number: ");
                int accountNumber;
                line = Console.ReadLine() ?? "";
                if (line.ToLower() != "e")
                {
                    while (!int.TryParse(line, out accountNumber))
                    {
                        Console.Write("Please enter a valid number: ");
                        line = Console.ReadLine() ?? "";
                    }

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
                            newLine = Console.ReadLine();
                            y = newLine.ToLower() == "y";
                            n = newLine.ToLower() == "n";
                        }
                        if (y) CreditLimitCalculator.ModifyAccount(newAccount);
                    }
                }
            } while (line.ToLower() != "e");
        }
    }


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
}
