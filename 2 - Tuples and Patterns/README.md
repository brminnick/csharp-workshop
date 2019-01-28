# Using Tuples

## Tuple Demo

Show the generator class. Explain that this simulates and IoT sensor that returns
pairs of numbers that should be formed into (x,y) bits. 

First part: create GetPoints() to return named tuples for X, Y

Next, compote the distance between the points using tuples for dx, dy

Add a SameQuadrant checking the sign of both points.

## Discussion: Rules for classes, structs, Tuples and anonymous types

Ask the question: When should you pick tuples as your design choice?

prediction that will lead to a bit fo a loss, without good answers. That's because tuples don't solve an *obvious* problem. 

Turn the discussion around: Ask when should you choose a class? When should you choose a struct? What features of classes or structs make them suitable for your designs? 

prediction: this gets to classes solving the problem of behavior + data. They include inheritance, interface implementation, member methods etc.
Structs include member methods, can implement interfaces (yes, boxing), but no inheritance.

Tuples have *none of those features*. They are a 'better' property bag. Let's compare that with our demo: So fare, the point has no behavior. Stated another way, that means you don't need member methods. They have no inheritance, they don't implement arbitrary interfaces. 

Consider features and patterns that would not be there if Tuples had existed since C# 1.0:
- The try / out pattern
- Others? (I don't know, but maybe we get good suggestions).

## Patterns bal

Extend the point sample to read JSON that could be points, or could be (dx, dy) directions.

Then, start computing the new location based on reading that kind of information.

On the patterns: Point out that JSON data means it doesn't always have a structure.
Deserialization could fail if properties are not prese3nt. By using *patterns* instead
of a type, you continue to have the flexibility you need for extending the behavior without doing as much type plumbing when fields change (possibly not under your control.)

Show a pattern where a switch looks at some data in an object to determine what to do.
Explain that you could extend that so this code that retrieves points or deltas could generate a sequence of points. Have them do that as a lab. (Point out that the JSON is as well formed as we'd like. Sometimes it's "X", "Y". Sometimes it "x", "y", and so on. Patterns can manage that too.)

Drive a discussion from this:

When do the fields of a type change during development?

Will patterns make it more resilient? Or is it lazy? What are the tradeoffs?
