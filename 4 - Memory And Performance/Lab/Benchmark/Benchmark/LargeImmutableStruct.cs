namespace Benchmark
{
    readonly struct LargeImmutableStruct
    {
        private readonly double a;
        private readonly double b;
        private readonly double c;
        private readonly double d;
        private readonly double e;
        private readonly double f;
        private readonly double g;
        private readonly double h;

        public LargeImmutableStruct(double x, double y = 0, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
            a = 1;
            b = 2;
            c = 3;
            d = 4;
            e = 5;
            f = 6;
            g = 7;
            h = 8;
        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }
    }
}
