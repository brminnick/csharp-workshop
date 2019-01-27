using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace TuplesAndPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            var pts = GetPoints(50);

            var current = (X: 0.0, Y: 0.0);

            foreach (var pt in pts)
            {
                var xyDistance = (x: pt.X - current.X, y: pt.Y - current.Y);
                var distance = Math.Sqrt(xyDistance.x * xyDistance.x + xyDistance.y * xyDistance.y);
                Console.WriteLine($"The distance from {current} to {pt} is {distance}");
                if (SameQuadrant(current, pt))
                    Console.WriteLine("In the same quadrant");
                else
                    Console.WriteLine("different quadrants");
                current = pt;
            }

            pts = parsePoints(jsonText);
        }

        // Use a simple JSON object that cotnains points (x,y) or distance (dX, dY),
        // Use pattern matching to build the IEnumerable
        // It's simple to explain and simple to make it work.

        private const string jsonText =
@"{
path: [
  {
    X: 20,
    Y: 12
  },
  {
    dx: -23,
    dy: -5
  },
  {
    x: 5,
    y: 12
  },
  {
    deltaX: 10,
    deltaY: 5
  }
]
}";

        private static IEnumerable<(double X, double Y)> parsePoints(string jsonText)
        {
            JObject data = JObject.Parse(jsonText);

            JArray trip = (JArray)data["path"];

            foreach(JObject obj in trip)
            {
                // TODO: Extend this to use pattern matching to return the next point.
                Console.WriteLine(obj);
            }
            return default;
        }

        public static bool SameQuadrant((double X, double Y) left, (double X, double Y) right) =>
            (Math.Sign(left.X), Math.Sign(left.Y)) == (Math.Sign(right.X), Math.Sign(right.Y));

        static IEnumerable<(double X, double Y)> GetPoints(int numberPoints)
        {
            var generator = new ConnectedSensor();

            var enumerator = generator.ReadData(numberPoints * 2).GetEnumerator();
            while (enumerator.MoveNext())
            {
               var X = enumerator.Current;
               enumerator.MoveNext();
               var Y = enumerator.Current;
               yield return (X, Y);
            }
        }

    }
}
