using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TuplesAndPatterns
{
    public class ConnectedSensor
    {
        public IEnumerable<double> ReadData(int numberDataPoints)
        {
            Random r = new Random();
            foreach(var _ in Enumerable.Range(0,numberDataPoints))
                yield return (r.NextDouble() - 0.5) * 100; // from -50 to 50
        }
    }
}
