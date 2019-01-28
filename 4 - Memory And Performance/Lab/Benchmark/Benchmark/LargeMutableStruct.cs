namespace Benchmark
{
    struct LargeMutableStruct
    {
        private double a;
        private double b;
        private double c;
        private double d;
        private double e;
        private double f;
        private double g;
        private double h;

        public LargeMutableStruct(double x, double y = 0, double z = 0)
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
