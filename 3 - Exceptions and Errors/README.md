# Exceptions lab

First, walk through the MakeChange routine at a conceptual level. Do not show the code. It sounds simple: look at the idea where you make change from the largest bills (since those are the ones people likely give you.) When you run out of larger bills, start using smaller ones. 

There are two error conditions: One is when the amount paid doesn't cover the cost of the items. The other is when the till doesn't have enough small bills to make change. 

It looks simple.  Now, without looking at the code above, 

Show the test code in program.cs. Run the simulation and point out the runtime problem: State has been mutated when there have been error conditions.

Why did this happen? WEll, the code mutated state before it encountered an error condition.

Now, show the code for hte make change method. The lab is to fix the code *so that it still throws exceptions in these error conditions*, but recover is safe. That means the state of the till cannot change when the purchase can't be completed.
