using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace TuplesAndPatterns
{
    class Program
    {
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

        static void Main(string[] args)
        {
            var points = GeneratePoints(50);

            var currentPoint = (X: 0.0, Y: 0.0);

            foreach (var point in points)
            {
                var xyDistance = (x: point.X - currentPoint.X, y: point.Y - currentPoint.Y);

                var distance = Math.Sqrt(xyDistance.x * xyDistance.x + xyDistance.y * xyDistance.y);

                Console.Write($"The distance from {currentPoint} to {point} is approximately {Math.Round(distance, 2)}");

                if (ArePointsInSameQuadrant(currentPoint, point))
                    Console.WriteLine(" and they are in the same quadrant.");
                else
                    Console.WriteLine(" and they are in different quadrants.");

                currentPoint = point;

                Console.WriteLine();
            }

            points = GetParsedPoints(jsonText);

            Console.WriteLine("Parsed JSON Output");
            foreach (var point in points)
                Console.WriteLine(point);
        }

        static bool ArePointsInSameQuadrant((double X, double Y) left, (double X, double Y) right) =>
            (Math.Sign(left.X), Math.Sign(left.Y)) == (Math.Sign(right.X), Math.Sign(right.Y));

        static IEnumerable<(double X, double Y)> GetParsedPoints(string json)
        {
            JObject data = JObject.Parse(json);

            JArray trip = (JArray)data["path"];

            foreach (JObject obj in trip)
            {
                yield return ((double)obj.Values().First(), (double)obj.Values().Skip(1).First());
            }
        }

        static IEnumerable<(double X, double Y)> GeneratePoints(int numberOfPointsToGenerate)
        {
            var generator = new ConnectedSensor();

            var pointsEnumerator = generator.ReadData(numberOfPointsToGenerate * 2).GetEnumerator();

            while (pointsEnumerator.MoveNext())
            {
                var x = pointsEnumerator.Current;

                pointsEnumerator.MoveNext();

                var y = pointsEnumerator.Current;

                yield return (x, y);
            }
        }
    }
}
