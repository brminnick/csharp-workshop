using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var Summary = BenchmarkRunner.Run<Benchmarks>();
        }
    }

    public class Benchmarks
    {
        readonly LargeImmutableStruct immutableStruct = new LargeImmutableStruct(1.1, 2.2);
        readonly LargeMutableStruct mutableStruct = new LargeMutableStruct(1.1, 2.2);

        [Benchmark]
        public void ImmutableAddByType() => AddByType(immutableStruct);

        [Benchmark]
        public void ImmutableAddByRefType() => AddByRefType(in immutableStruct);

        [Benchmark]
        public void MutableAddByType() => AddByType(mutableStruct);

        [Benchmark]
        public void MutableAddByRefType() => AddByRefType(in mutableStruct);

        double AddByType(LargeImmutableStruct s) => s.X + s.Y;
        double AddByRefType(in LargeImmutableStruct s) => s.X + s.Y;
        double AddByType(LargeMutableStruct s) => s.X + s.Y;
        double AddByRefType(in LargeMutableStruct s) => s.X + s.Y;
    }
}