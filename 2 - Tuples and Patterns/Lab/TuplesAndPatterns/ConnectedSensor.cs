using System;
using System.Collections.Generic;
using System.Linq;

namespace TuplesAndPatterns
{
    public class ConnectedSensor
    {
        public IEnumerable<double> ReadData(int numberDataPoints)
        {
            Random r = new Random((int)DateTime.Now.Ticks);

            foreach (var _ in Enumerable.Range(0, numberDataPoints))
                yield return Math.Round((r.NextDouble() - 0.5) * 100, 2); // from -50 to 50
        }
    }
}
