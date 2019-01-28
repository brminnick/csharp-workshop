# Morning Async / Await lab

## Get Issues

Start with the skeleton, and get the most recent 50 issues created for

-dotnet/docs
-dotnet/dotnet-api-docs

Then, get the most recent merged 50 PRs for:

-dotnet/docs
-dotnet/dotnet-api-docs
-dotnet/samples

Print out pertitent information for each.

Once you've got the basics working, try to control order of output in the following ways:

1. Print all issues first, then PRs
1. Print all PRs first, then issues.
1. First print whatever task finished first.

## Interface / API critique

How would you create a class hierarchy for these APIs? What operations should be synchronous? What should be asynchronous?

How does that shape construction, properties, and other elements of APIs?

Some thoughts:
Are async APIs factories that *produce* results as new objects?
Should async APIs perform *actions* that modifying existing objects?
