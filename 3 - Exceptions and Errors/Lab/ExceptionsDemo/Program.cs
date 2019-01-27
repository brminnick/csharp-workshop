using System;
using System.Collections.Generic;

namespace ExceptionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var theBank = new Till(50, 20, 10, 5);

            var expectedTotal = 50 * 1 + 20 * 5 + 10 * 10 + 5 * 20;

            theBank.LogTillStatus();
            Console.WriteLine(theBank);
            Console.WriteLine($"Expected till value: {expectedTotal}");

            int transactions = 50;
            var ValueGenerator = new Random();

            while (transactions-- > 0)
            {
                int cost = ValueGenerator.Next(2, 50);
                int numOnes = cost % 2;
                int numFives = (cost % 10 > 7) ? 1 : 0;
                int numTens = (cost % 20 > 13) ? 1 : 0;
                int numTwenties = (cost < 20) ? 1 : 2;
                try
                {
                    Console.WriteLine($"Buying {cost} with {numTwenties} twenty dollar bills");
                    theBank.MakeChange(cost, numTwenties, numTens, numFives, numOnes);
                    expectedTotal += cost;
                } catch (InvalidOperationException e)
                {
                    Console.WriteLine($"Could not make transaction: {e.Message}");
                }
                Console.WriteLine(theBank);
                Console.WriteLine($"Expected till value: {expectedTotal}");
            }
        }
    }



    public class Till
    {
        private int OneDollarBills;
        private int FiveDollarBills;
        private int TenDollarBills;
        private int TwentyDollarBills;

        public Till(int ones, int fives, int tens = 0, int twenties = 0) =>
            (OneDollarBills, FiveDollarBills, TenDollarBills, TwentyDollarBills) =
            (ones, fives, tens, twenties);

        public void MakeChange(int cost, int twenties, int tens = 0, int fives = 0, int ones = 0)
        {
            TwentyDollarBills += twenties;
            TenDollarBills += tens;
            FiveDollarBills += fives;
            OneDollarBills += ones;
            int amountPaid = twenties * 20 + tens * 10 + fives * 5 + ones;
            int changeNeeded = amountPaid - cost;
            if (changeNeeded < 0)
                throw new InvalidOperationException("You must pay at lest the cost");

            while ((changeNeeded > 19) && (TwentyDollarBills > 0))
            {
                TwentyDollarBills--;
                changeNeeded -= 20;
                Console.WriteLine("\tHere's a twenty");
            }
            while ((changeNeeded > 9) && (TenDollarBills > 0))
            {
                TenDollarBills--;
                changeNeeded -= 10;
                Console.WriteLine("\tHere's a ten");
            }
            while ((changeNeeded > 4) && (FiveDollarBills > 0))
            {
                FiveDollarBills--;
                changeNeeded -= 5;
                Console.WriteLine("\tHere's a five");
            }
            while ((changeNeeded > 0) && (OneDollarBills > 0))
            {
                OneDollarBills--;
                changeNeeded--;
                Console.WriteLine("\tHere's a one");
            }
            if (changeNeeded > 0)
                throw new InvalidOperationException("Can't make change. Do you have anything smaller?");
        }

        public void LogTillStatus()
        {
            Console.WriteLine("The till currently has:");
            Console.WriteLine($"{TwentyDollarBills * 20} in twenties");
            Console.WriteLine($"{TenDollarBills * 10} in tens");
            Console.WriteLine($"{FiveDollarBills * 5} in fives");
            Console.WriteLine($"{OneDollarBills} in ones");
        }

        public override string ToString() =>
            $"The till has {TwentyDollarBills * 20 + TenDollarBills * 10 + FiveDollarBills * 5 + OneDollarBills} dollars";
    }
}
