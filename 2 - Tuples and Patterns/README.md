# Using Tuples

Show the generator class. Explain that this simulates and IoT sensor that returns
pairs of numbers that should be formed into (x,y) bits. 

First part: create GetPoints() to return named tuples for X, Y

Next, compote the distance between the points using tuples for dx, dy

Add a SameQuadrant checking the sign of both points.

For the patterns lab:

Extend this to create a JSON that could be points, or could be (dx, dy) directions.

Then, start computing the new location based on reading that kind of information.

At each step: explain that creating a Type (struct or class) implies more significance to
the data points. Instead, as just tuples, they only need to carry the information, not create behavior or significant stuff.

On the patterns: Point out that JSON data means it doesn't always have a structure.
Deserialization could fail if properties are not prese3nt. By using *patterns* instead
of a type, you continue to have the flexibility you need for extending the behavior without doing as much type plumbing when fields change (possibly not under your control.)

Show a pattern where a switch looks at some data in an object to determine what to do.
Explain that you could extend that so this code that retrieves points or deltas could generate a sequence of points. Have them do that as a lab. (Point out that the JSON is as well formed as we'd like. Sometimes it's "X", "Y". Sometimes it "x", "y", and so on. Patterns can manage that too.)

Drive a discussion from this:

When do the fields of a type change during development?

Will patterns make it more resilient.

Lab for this module: extend this demo with more types of path data. From that, write guidelines for when to use patterns.
