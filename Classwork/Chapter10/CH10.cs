using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classwork.Chapter10
{
    public static class Q10_6
    {
        public static void Run(bool intro = true)
        {
            Console.Clear();
            if(intro)
            {
                Console.WriteLine("Welcome to the Date Class Tester! Press enter to continue");
                Console.ReadLine();
                Console.Clear();
            }

            string line = "";

            Date date = new Date(DateTime.Now.Day, (Month)DateTime.Now.Month, DateTime.Now.Year);

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine($"Current Date: {date}");
                Console.WriteLine("Enter a date (as shown above) to skip to date.");
                Console.WriteLine("Enter a number to skip that many days (enter nothing or 0 to step 1 day.");
                Console.WriteLine("Enter e to exit");
                Console.Write("Enter: ");
                line = Console.ReadLine() ?? "";

                if (line.ToLower() == "e") running = false;
                else if((int.TryParse(line, out var count) && count > 0) || line == "")
                {
                    if (line == "") count = 1;
                    date.Increment(count);
                }
                else
                {
                    var split = line.Split('/');
                    if(split.Length == 3)
                    {
                        if(int.TryParse(split[0], out int month) && int.TryParse(split[1], out int day) && int.TryParse(split[2], out int year))
                        {
                            if (Date.ValidDate(day, month, year)) date.SetDate(day, (Month)month, year);
                            else
                            {
                                Console.WriteLine("Invalid date. Enter to continue: ");
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid date. Enter to continue: ");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Enter to continue: ");
                        Console.ReadLine();
                    }
                }
            }

        }
    }

    public enum Month
    {
        JAN = 1,
        FEB,
        MAR,
        APR,
        MAY,
        JUN,
        JUL,
        AUG,
        SEP,
        OCT,
        NOV,
        DEC
    }
    public class Date
    {

        public static int[] DaysPerMonth(int year) => new int[] { 31, 28 + (year % 4 == 0 ? 1 : 0), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        Month currMonth;
        int currDay;

        public static bool ValidDate(int day, int month, int year)
        {
            if(!(month > 0 && month <= 12)) return false;
            return day > 0 && day <= DaysPerMonth(year)[month - 1];
        }

        public Date(int iDay, Month iMonth, int iYear)
        {
            try
            {
                Month = iMonth;
            }
            catch
            {
                Month = (Month)1;
            }
            try
            {
                Day = iDay;
            }
            catch
            {
                Day = 1;
            }
            Year = iYear;
        }

        public int Day
        {
            get => currDay;
            set
            {
                if (value <= 0 || value > DaysPerMonth(Year)[(int)Month - 1]) throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(Day)} out of range for current month/year");
                currDay = value;
            }
        }
        public Month Month
        {
            get => currMonth;
            set
            {
                if ((int)value <= 0 || (int)value > 12) throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(Month)} out of range for current month/year");
                currMonth = value;
            }
        }
        public int Year { get; private set; }

        public void NextDay(int stepCount)
        {
            try
            {
                Day += stepCount;
            }
            catch (ArgumentOutOfRangeException)
            {
                try
                {
                    Month++;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Month = (Month)1;
                    Year++;
                }
            }
            catch
            {
                Console.WriteLine("Something else went wrong.....");
            }
        }


        public void Increment(int dayIncrement = 1, int monthIncrement = 0, int yearIncrement = 0)
        {
            Year += yearIncrement;

            if ((int)Month + monthIncrement > 12)
            {
                Year++;
                int mTemp = (int)Month;
                Month = Month.JAN;
                Increment(0, monthIncrement - (12 - mTemp + 1), 0);
            } else Month += monthIncrement;

            int dpm = DaysPerMonth(Year)[(int)Month - 1];
            if (Day + dayIncrement > dpm)
            {
                int dTemp = Day;
                Day = 1;
                Increment(0, 1, 0);
                Increment(dayIncrement - (dpm - dTemp + 1), 0, 0);
            }
            else Day += dayIncrement;
        }


        public override string ToString()
        {
            return $"{(int)Month}/{Day}/{Year}";
        }

        public void SetDate(int day, Month month, int year)
        {
            if (!ValidDate(day, (int)month, year)) return;

            Day = day;
            Month = month;
            Year = year;
        }
    }
}
