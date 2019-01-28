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

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = makeToastWithButterAndJamAsync(2);

            var eggs = await eggsTask;
            Console.WriteLine("Eggs are ready");

            var bacon = await baconTask;
            Console.WriteLine("Bacon is ready");

            var toast = await toastTask;
            Console.WriteLine("Toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine("OJ is ready");

            Console.WriteLine("Breakfast is ready!");

            async Task<Toast> makeToastWithButterAndJamAsync(int number)
            {
                var plainToast = await ToastBreadAsync(number);

                ApplyButter(plainToast);
                ApplyJam(plainToast);

                return plainToast;
            }
        }

        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring Orange Juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) => Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) => Console.WriteLine("Putting butter on the toast");

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
                Console.WriteLine("Putting a slice of bread in the toaster");

            Console.WriteLine("Start toasting...");
            await Task.Delay(3000);

            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
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

        private static async Task<Egg> FryEggsAsync(int howMany)
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
