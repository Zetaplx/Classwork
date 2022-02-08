namespace Classwork.Chapter4
{
    public static class Q4_14
    {
        /// <summary>
        /// Performs all the basic functionality of question 4.14. Has togglable introduction for replayability.
        /// </summary>
        /// <param name="intro">Set to false to skip the intro text and immediately start user input.</param>
        public static void Run(bool intro = true)
        {
            Console.Clear();

            var currentYear = DateTime.Now.Year;

            #region Intro Text

            if (intro)
            {
                Console.WriteLine("Welcome to the Target Heart Rate Calculator. Press enter to begin.");
                Console.ReadLine();
                Console.Clear();
            }

            #endregion

            #region Input
            Console.Write("Enter your first name: ");
            string firstName = Console.ReadLine() ?? "";
            Console.Write("\nEnter your last name: ");
            string lastName = Console.ReadLine() ?? "";
            Console.Write($"\nEnter your year of birth: ");
            int yob;
            while (!int.TryParse(Console.ReadLine(), out yob) || yob >= currentYear)
            {
                Console.Write("Please enter a valid year: ");
            }

            Console.Write("\nEnter your height in inches: ");
            float height;
            while (!float.TryParse(Console.ReadLine(), out height) || height < 0)
            {
                Console.Write("Please enter a valid height: ");
            }

            Console.Write("\nEnter your weight in pounds: ");
            float weight;
            while (!float.TryParse(Console.ReadLine(), out weight) || weight < 0)
            {
                Console.Write("Please enter a valid weight: ");
            }
            #endregion

            // Construct HeartRate
            HeartRates hr = new HeartRates(firstName, lastName, yob, height, weight);

            #region Output
            Console.Clear();
            Console.WriteLine($"{lastName}, {firstName} -- Born {yob} (Age {hr.Age})");
            Console.WriteLine($"Height: {height} inches\tWeight: {weight} lbs");
            Console.WriteLine($"Maximum Heart Rate: {hr.MaxHeartRate}");
            var tRate = hr.TargetHeartRateRange;
            Console.WriteLine($"Target Heart Rate: {hr.TargetHeartRateRange.Min} - {hr.TargetHeartRateRange.Max}");
            var bmi = BMICalculator.CalculateBMISI(weight, height);
            Console.WriteLine($"With a BMI of {bmi:0.00}, you are {BMICalculator.GetBMIState(bmi)}.");
            #endregion

            Console.WriteLine("\nEnter r to restart. Enter anything else to exit");
            var line = Console.ReadLine() ?? "";
            if (line.ToLower() == "r") Run(false);
        }
    }

    /// <summary>
    /// Used to calculate and store information about the users heartrate including their maximum healthy heartrate
    /// and target heartrate range.
    /// </summary>
    public class HeartRates
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int YearOfBirth { get; private set; }
        /// <summary>
        /// Gets the current year from DateTime
        /// </summary>
        public int CurrentYear => DateTime.Now.Year;

        public float Height { get; private set; }
        public float Weight { get; private set; }
        /// <summary>
        /// User's Body-Mass Index based on their Height and Weight;
        /// </summary>
        public float BMI => BMICalculator.CalculateBMISI(Weight, Height);

        public HeartRates(string firstName, string lastName, int yob, float height, float weight)
        {
            FirstName = firstName;
            LastName = lastName;
            YearOfBirth = yob;

            Height = height;
            Weight = weight;
        }

        public int Age => CurrentYear - YearOfBirth;
        public int MaxHeartRate => 220 - Age;
        /// <summary>
        /// Ideal range for user's heartrate. A tuple conatining the minimum (min) heartrate and maximum (max) heartrate.
        /// </summary>
        public (int Min, int Max) TargetHeartRateRange => ((int)(MaxHeartRate * 0.5f), (int)(MaxHeartRate * 0.85f));
    }

    /// <summary>
    /// Contains the necessary functions and classifications for calculating BMI
    /// </summary>
    public static class BMICalculator
    {
        public enum BMIState
        {
            Underweight,
            Normal,
            Overweight,
            Obese
        }
        public enum UnitSystem
        {
            SI,
            Metric
        }

        /// <summary>
        /// Takes in a weight (in kilograms) and a height (in meters) to calculate a BMI
        /// </summary>
        /// <param name="w">Weight in Kilograms</param>
        /// <param name="h">Height in Meters</param>
        /// <returns></returns>
        public static float CalculateBMIMetric(float w, float h) => w / (h * h);
        /// <summary>
        /// Takes in a weight (in pounds) and a height (in inches) to calculate a BMI
        /// </summary>
        /// <param name="w">Weight in Pounds</param>
        /// <param name="h">Height in Inches</param>
        /// <returns></returns>
        public static float CalculateBMISI(float w, float h) => (w * 703) / (h * h);
        /// <summary>
        /// Calculates if the user is Underweight, Normal, Overweight, or Obese based on their BMI.
        /// </summary>
        /// <param name="bmi">User's BMI</param>
        /// <returns></returns>
        public static BMIState GetBMIState(float bmi)
        {
            if (bmi < 18.5f) return BMIState.Underweight;
            if (bmi >= 25f) return BMIState.Overweight;
            if (bmi >= 30f) return BMIState.Obese;
            return BMIState.Normal;
        }
    }
}
