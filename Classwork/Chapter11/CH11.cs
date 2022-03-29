using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classwork.Chapter11
{
    public static class Q11_9
    {
        public static void Run(bool intro = true)
        {
            Console.Clear();

            if(intro)
            {
                Console.WriteLine("Welcome to the Account Tester! Press enter to continue: ");
                Console.ReadLine();
                Console.Clear();
            }

            bool mainLoop = true;
            while(mainLoop)
            {
                int m = Utils.MenuSimple("What would you like to do?", false, "Checking Account Test", "Savings Account Test", "Exit");

                switch(m)
                {
                    case 1:
                        RunCAcc();
                        break;
                    case 2:
                        RunSAcc();
                        break;
                    case 3:
                        mainLoop = false;
                        break;
                }

            }
        }

        private static void RunCAcc()
        {
            Console.WriteLine("Now creating a Checking Account!");
            Console.Write("Enter the initial balance: ");
            decimal initBalance = Utils.SafeReadDecimal((n) => n >= 0, "Invalid balance. Try again!");
            Console.Write("Enter the transaction fee: ");
            decimal transFee = Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount. Try again!");

            CheckingAccount acc = new CheckingAccount(initBalance, transFee);

            bool checkLoop = true;
            while(checkLoop)
            {
                Console.Clear();

                int m = Utils.MenuSimple($"Balance: {acc.Balance:c} -- What would you like do to?", false, "Deposit", "Withdrawl", "Return to main menu", "Restart Account");
                decimal amt = 0;
                switch (m)
                {
                    case 1:
                        Console.Write("How much would you like to deposit?: ");
                        amt = Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount! Try again!");
                        acc.Credit(amt);
                        break;
                    case 2:
                        Console.Write("How much would you like to withdrawl?: ");
                        amt = Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount! Try again!");
                        acc.Debit(amt);
                        break;
                    case 3:
                        checkLoop = false;
                        break;
                    case 4:
                        RunSAcc();
                        checkLoop = false;
                        break;
                }
            }
        }
        private static void RunSAcc()
        {
            Console.WriteLine("Now creating a Savings Account!");
            Console.Write("Enter the initial balance: ");
            decimal initBalance = Utils.SafeReadDecimal((n) => n >= 0, "Invalid balance. Try again!");
            Console.Write("Enter the interest rate: ");
            decimal transFee = Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount. Try again!");

            SavingsAccount acc = new SavingsAccount(initBalance, transFee);

            bool checkLoop = true;
            while (checkLoop)
            {
                Console.Clear();

                int m = Utils.MenuSimple($"Balance: {acc.Balance:c} -- What would you like do to?", false, "Deposit", "Withdrawl", "Apply Interest", "Return to main menu", "Restart Account");
                decimal amt = 0;
                switch (m)
                {
                    case 1:
                        Console.Write("How much would you like to deposit?: ");
                        amt = Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount! Try again!");
                        acc.Credit(amt);
                        break;
                    case 2:
                        Console.Write("How much would you like to withdrawl?: ");
                        amt = Utils.SafeReadDecimal((n) => n >= 0, "Invalid amount! Try again!");
                        acc.Debit(amt);
                        break;
                    case 3:
                        decimal interest = acc.CalculateInterest();
                        Console.WriteLine($"Calculated interest: {interest:c}. Enter to add.");
                        Console.ReadLine();
                        acc.Credit(interest);
                        break;
                    case 4:
                        checkLoop = false;
                        break;
                    case 5:
                        RunCAcc();
                        checkLoop = false;
                        break;
                }
            }
        }
    }

    public abstract class Account
    {
        private decimal balance;
        public decimal Balance { 
            get => balance;
            protected set => balance = value >= 0 ? value : throw new Exception("Balance must be at least $0.00!");
        }

        public Account(decimal initBalance)
        {
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
        public CheckingAccount(decimal initBalance, decimal transactionFee) : base(initBalance)
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
            if(!base.Debit(transactionFee)) 
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
        public SavingsAccount(decimal initBalance, decimal interestRate) : base(initBalance)
        {
            this.interestRate = interestRate;
        }

        /// <summary>
        /// Calculates the account interest based on current balance and interest rate.
        /// </summary>
        public decimal CalculateInterest() => interestRate * Balance;
    }
}
