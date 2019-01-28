using System;
using System.Threading.Tasks;

namespace AsyncBreakfast
{
    class Program
    {
        static void Main(string[] args)
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("Coffee is ready");

            Egg eggs = FryEggs(2);
            Console.WriteLine("Eggs are ready");

            Bacon bacon = FryBacon(3);
            Console.WriteLine("Bacon is ready");

            Toast toast = ToastBread(2);

            ApplyButter(toast);
            ApplyJam(toast);

            Console.WriteLine("Toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine("OJ is ready");

            Console.WriteLine("Breakfast is ready!");
        }

        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring Orange Juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) => Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) => Console.WriteLine("Putting butter on the toast");

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
                Console.WriteLine("Putting a slice of bread in the toaster");

            Console.WriteLine("Start toasting...");

            Task.Delay(3000).Wait();

            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"Putting {slices} of bacon in the pan");

            Console.WriteLine("Cooking first side of bacon...");
            Task.Delay(3000).Wait();

            for (int slice = 0; slice < slices; slice++)
                Console.WriteLine("Flipping a slice of bacon");

            Console.WriteLine("Cooking the second side of bacon...");
            Task.Delay(3000).Wait();

            Console.WriteLine("Putting bacon on plate");

            return new Bacon();
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            Task.Delay(3000).Wait();

            Console.WriteLine($"Cracking {howMany} eggs");

            Console.WriteLine("Cooking the eggs ...");
            Task.Delay(3000).Wait();

            Console.WriteLine("Putting eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
    }
}
