using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classwork.Chapter12
{
    public static class Q12_13
    {
        public static void Run(bool intro = true)
        {
            Console.Clear();

            if (intro)
            {
                Console.WriteLine("Welcome to the Account Tester 2! Press enter to continue: ");
                Console.ReadLine();
                Console.Clear();
            }

            Dictionary<int, Account> accounts = new Dictionary<int, Account>();

            bool mainLoop = true;

            int id = 0;

            while(mainLoop)
            {
                Console.Clear();

                int m = Utils.MenuSimple("What would you like to do?", false, "Create Account", "Remove Account", "Modify Account", "Run basic assignment loop", "Exit");

                switch(m)
                {
                    case 1:
                        Console.Write("Enter new account ID: ");
                        id = Utils.SafeReadInt((n) => n > 0 && !accounts.ContainsKey(n), "Please enter a new positive ID");
                        accounts.Add(id, CreateAccount(id));
                        break;
                    case 2:
                        Console.Write("Enter account ID: ");
                        id = Utils.SafeReadInt((n) => n > 0 && accounts.ContainsKey(n), "Please enter an existing ID");
                        accounts.Remove(id);
                        break;
                    case 3:
                        Console.Write("Enter account ID: ");
                        id = Utils.SafeReadInt((n) => n > 0 && accounts.ContainsKey(n), "Please enter an existing ID");
                        ModifyAccount(accounts[id]);
                        break;
                    case 4:
                        Assignment(accounts.Values.ToList());
                        break;
                    case 5:
                        mainLoop = false;
                        break;
                }
            }
        }

        public static Account CreateAccount(int id)
        {
            Console.Clear();
            Console.WriteLine($"Creating new account #{id}");

            Account acc;

            int m = Utils.MenuSimple("What kind of account would you like to create?", false, "Checking Account", "Savings Account");
            Console.Write("Enter initial balance: ");
            decimal initBalance = Utils.SafeReadDecimal((n) => n >= 0);

            switch (m)
            {
                case 1:
                    Console.Write("Enter transaction fee: ");
                    decimal fee = Utils.SafeReadDecimal((n) => n >= 0);
                    acc = new CheckingAccount(id, initBalance, fee);
                    break;
                default: // Only other choice is 2
                    Console.Write("Enter interest rate: ");
                    decimal ir = Utils.SafeReadDecimal((n) => n >= 0);
                    acc = new SavingsAccount(id, initBalance, ir);
                    break;
            }

            return acc;
        }

        public static void ModifyAccount(Account acc)
        {
            if (acc is SavingsAccount sc) ModifyAccount(sc);
            else if (acc is CheckingAccount cc) ModifyAccount(cc);
            else throw new Exception("Account must be either a savings or checking account... ");
        }
        public static void ModifySavingsAccount(SavingsAccount acc)
        {
            Console.Clear();

            int m = Utils.MenuSimple($"Account #{acc.ID}: {acc.Balance:c} -- How would you like to modify?", false, "Deposit", "Withdrawl", "Apply Interest", "Return to main menu");

            switch(m)
            {
                case 1:
                    Console.WriteLine("How much would you like to Deposit?");
                    acc.Credit(Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount. Try again!"));
                    break;
                case 2:
                    Console.WriteLine("How much would you like to Withdrawl?");
                    acc.Debit(Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount. Try again!"));
                    break;
                case 3:
                    var interest = acc.CalculateInterest();
                    Console.WriteLine($"Interest calculated to {interest:c}. Enter to apply.");
                    Console.ReadLine();
                    acc.Credit(interest);
                    break;
                case 4:
                    return;
            }
            ModifySavingsAccount(acc);
        }
        public static void ModifyCheckingAccount(CheckingAccount acc)
        {
            Console.Clear();

            int m = Utils.MenuSimple($"Account #{acc.ID}: {acc.Balance:c} -- How would you like to modify?", false, "Deposit", "Withdrawl", "Return to main menu");

            switch (m)
            {
                case 1:
                    Console.WriteLine("How much would you like to Deposit?");
                    acc.Credit(Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount. Try again!"));
                    break;
                case 2:
                    Console.WriteLine("How much would you like to Withdrawl?");
                    acc.Debit(Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount. Try again!"));
                    break;
                case 3:
                    return;
            }
            ModifyCheckingAccount(acc);
        }

        public static void Assignment(List<Account> accounts)
        {
            Console.Clear();
            foreach(var acc in accounts)
            {
                Console.WriteLine($"Account #{acc.ID}: {acc.Balance:c}");
                Console.Write($"Enter amount to deposit: ");
                decimal credit = Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount. Try again!");

                Console.Write($"Enter amount to withdrawl: ");
                decimal debit  = Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount. Try again!");


                if(acc is SavingsAccount sa)
                {
                    decimal interest = sa.CalculateInterest();
                    Console.WriteLine($"Applying interest: {sa.CalculateInterest:c}");
                }

                Console.WriteLine($"Final balance: {acc.Balance}");
            }
        }
    }

    public abstract class Account
    {
        public int ID { get; private set; }
        private decimal balance;
        public decimal Balance
        {
            get => balance;
            protected set => balance = value >= 0 ? value : throw new Exception("Balance must be at least $0.00!");
        }

        public Account(int id, decimal initBalance)
        {
            this.ID = id;
            this.Balance = initBalance;
        }

        /// <summary>
        /// Adds funds to the account.
        /// </summary>
        /// <param name="amt">The amount to add to the account. Must be positive.</param>
        public virtual void Credit(decimal amt)
        {
            if (amt < 0) throw new Exception("Credited amount must be positive!");
            Balance += amt;
        }

        /// <summary>
        /// Charges the account with the specified amount. If unable, gives an error message and returns false.
        /// </summary>
        /// <param name="amt">The amount to charge the account. Must be positive.</param>
        /// <returns>True if amount was able to be withdrawn, otherwise false.</returns>
        public virtual bool Debit(decimal amt)
        {
            if (amt < 0) throw new Exception("Charged amount must be positive!");
            if (Balance >= amt)
            {
                Balance -= amt;
                return true;
            }

            Console.WriteLine("Debit amount exceeded account balance!");
            return false;
        }
    }
    public class CheckingAccount : Account
    {
        private decimal transactionFee;
        public CheckingAccount(int id, decimal initBalance, decimal transactionFee) : base(id, initBalance)
        {
            this.transactionFee = transactionFee;
        }

        /// <summary>
        /// Adds funds to the account then charges a transaction fee. If unable to pay fee, funds are not added.
        /// </summary>
        /// <param name="amt">The amount to add to the account. Must be positive.</param>
        public override void Credit(decimal amt)
        {
            base.Credit(amt);

            // Withdraw the transaction fee, if unable, remove credit added. NOTE: uses base.Debit so the transaction fee is not charges on these charges.
            if (!base.Debit(transactionFee))
                base.Debit(amt);
        }

        /// <summary>
        /// Charges the account with the specified amount plus the transaction fee. If unable, gives an error message and returns false.
        /// </summary>
        /// <param name="amt">The amount to charge the account prior to the transaction fee. Must be positive.</param>
        /// <returns>True if amount was able to be withdrawn, otherwise false.</returns>
        public override bool Debit(decimal amt)
        {
            return base.Debit(amt + transactionFee);
        }
    }
    public class SavingsAccount : Account
    {
        private decimal interestRate;
        public SavingsAccount(int id, decimal initBalance, decimal interestRate) : base(id, initBalance)
        {
            this.interestRate = interestRate;
        }

        /// <summary>
        /// Calculates the account interest based on current balance and interest rate.
        /// </summary>
        public decimal CalculateInterest() => interestRate * Balance;
    }
}
