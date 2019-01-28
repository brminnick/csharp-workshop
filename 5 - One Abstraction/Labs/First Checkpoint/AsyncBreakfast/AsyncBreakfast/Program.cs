using System;
using System.Threading.Tasks;

namespace AsyncBreakfast
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("Coffee is ready");

            Egg eggs = await FryEggs(2);
            Console.WriteLine("Eggs are ready");

            Bacon bacon = await FryBacon(3);
            Console.WriteLine("Bacon is ready");

            Toast toast = await ToastBread(2);

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

        private static async Task<Toast> ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
                Console.WriteLine("Putting a slice of bread in the toaster");

            Console.WriteLine("Start toasting...");
            await Task.Delay(3000);

            Console.WriteLine("Removing toast from toaster");

            return new Toast();
        }

        private static async Task<Bacon> FryBacon(int slices)
        {
            Console.WriteLine($"Putting {slices} of bacon in the pan");
            Console.WriteLine("Cooking first side of bacon...");
            await Task.Delay(3000);

            for (int slice = 0; slice < slices; slice++)
                Console.WriteLine("Flipping a slice of bacon");

            Console.WriteLine("Cooking the second side of bacon...");
            await Task.Delay(3000);

            Console.WriteLine("Putting bacon on plate");

            return new Bacon();
        }

        private static async Task<Egg> FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);

            Console.WriteLine($"Cracking {howMany} eggs");

            Console.WriteLine("Cooking the eggs ...");
            await Task.Delay(3000);

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
