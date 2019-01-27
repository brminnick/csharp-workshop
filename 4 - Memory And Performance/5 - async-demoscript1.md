# Async Breakfast demo

Open the code from the asyncbreakfast-starter.zip

Run the program. Discuss what's happening. Point out that each long-running task runs to completion before starting the next long running task. That means a cold breakfast, and inefficient use of resources.

## Incorporate basic async / await

Open Program.cs.  In turn, make the following to each long-running task:

1. Append `Async` to the method signature.
1. Change the `.Wait()` calls to `await` expressions in each call to `Task.Delay`. Point out that `Task.Delay` simulates some operation that takes time.
1. Add the `async` modifier to the method.
1. Change the return type to `Task<T>` where `T` is the previous return type.
1. Note the new warning in the `Main` method, and add an `await` expression.

After making all the changes in the first method, note that the `Main` method now has a warning. Take the suggestion to upgrade the project so that `async Task Main` is a valid entry point signature. The program should look like this:

```csharp
    class Program
    {
        static async Task Main(string[] args)
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");
            Egg eggs = await FryEggs(2);
            Console.WriteLine("eggs are ready");
            Bacon bacon = await FryBacon(3);
            Console.WriteLine("bacon is ready");
            Toast toast = await ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");
            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");

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
            Console.WriteLine("Remove toast from toaster");
            return new Toast();
        }

        private static async Task<Bacon> FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000);
            for (int slice = 0; slice < slices; slice++)
                Console.WriteLine("flipping a slice of bacon");
            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("Put bacon on plate");
            return new Bacon();
        }

        private static async Task<Egg> FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("Put eggs on plate");
            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
    }
```

Run the program again. Notice that nothing really changes.  Explain that this is because each task is `await`-ed before the the next task completes. In cloud scenarios, that might be fine. The CPU resource could now start tasks for other web requests while awaiting. In this scenario, there is other work to do, so let's continue refactoring a bit.

## Start multiple tasks as soon as possible

The next set of changes is to start subsequent tasks before the preceding task has finished, when possible. Change each await as in the following example:

```csharp
Egg eggs = await FryEggs(2);
```

Should change to:

```csharp
Task<Egg> eggTask = FryEggs(2);
Egg eggs = await eggTask;
```

Then, move all the `await` statements below the lines that start each task.  Run it. Now, it finishes faster, because you're starting tasks while awaiting asynchronous work to finish. POint out that this is more like the way people actually make breakfast.

## Compose tasks

The toast illustrates a common problem developers have composing task-based work: There is an asynchronous part, followed by a synchronous part. You want to add butter and jam as soon as the toast finishes, without waiting for the bacon and eggs to be done.

This could be done with `Task.ContinueWith()`, but there's a better way: refactor it to take advantage of new C# language features, like local functions. Move the toast making code to a local function in `Main`:

```csharp
async Task<Toast> makeToastWithButterAndJamAsync(int number)
{
    var plainToast = await ToastBreadAsync(number);
    ApplyButter(plainToast);
    ApplyJam(plainToast);
    return plainToast;
}
```

Then, change `Main` to use the new function:

```csharp
var toastTask = makeToastWithButterAndJamAsync(2);
// ...
var toast = await toastTask;
Console.WriteLine("toast is ready");
```

That's done.  Call it good.
