# Afternoon Async / Await lab

## Add error reporting.

The first lab did nothing with errors. Let's fix that in two ways:

1. Validate args and state with a synchronous Task-returning method.
1. Report errors correctly with proper await  or processing async errors.

THat means adding the API errors collection to the returned data.
Throw when that data comes back bad. Each error has a message, locations array.

## Interface / API critique

How would you create a class hierarchy for these APIs? What operations should be synchronous? What should be asynchronous?

How does that shape construction, properties, and other elements of APIs?

Some thoughts:
Are async APIs factories that *produce* results as new objects?
Should async APIs perform *actions* that modifying existing objects?

## Progress and cancellation.

As a demo, show the page cursor features in the GitHub explorer web page. Once you've done that, introduce the lab:

Now, we want all the open issues (newest first) in those respositories. Get them 25 per page.

Add a progress report for "retrieved M of N issues".
Support cancellation at the end of each response.

If short of time, just add one overload that supports both cancellation and progress.
If enough time, add all permutations.
