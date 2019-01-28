# Exceptions Demo

First, walk through the MakeChange routine at a conceptual level. Do not show the code. It sounds simple: look at the idea where you make change from the largest bills (since those are the ones people likely give you.) When you run out of larger bills, start using smaller ones. The functionality is simple and clearly defined.

There are two error conditions: One is when the amount paid doesn't cover the cost of the items. The other is when the till doesn't have enough small bills to make change. Once again, stress the simplicity.

It looks simple.  Now, without looking at the implementation of the till class, show the test code in program.cs. 

Run the simulation with a small set of data, like you would in a unit test. It looks great. Now, change the number of iterations and try again. Point out the runtime problem: State has been mutated when there have been error conditions. If you run this over a longer period of time, it will always fail eventually.

Why did this happen? Well, the code mutated state before it encountered an error condition.

Key point: This is a small simulation that demonstrates a common problem in large code bases: Unless unit tests are carefully constructed, exceptions may not always occur in your test environments. It's only under production stress that these appear. Then, data errors happen with live customer data. It gets very expensive and becomes a data recovery issue.

## The lab

Now, show the code for the make change method. The lab is to fix the code *so that it still throws exceptions in these error conditions*, but recovery is safe. That means the state of the till cannot change when the purchase can't be completed.

There are a number of ways to implement the change, but all share some common themes discussed in the "practices" slide.
