``` ini

BenchmarkDotNet=v0.11.3, OS=macOS Mojave 10.14.3 (18D42) [Darwin 18.2.0]
Intel Core i7-7920HQ CPU 3.10GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview-010177
  [Host]     : .NET Core ? (CoreCLR 4.6.0.0, CoreFX 4.6.26614.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.6 (CoreCLR 4.6.0.0, CoreFX 4.6.26212.01), 64bit RyuJIT


```
|                Method |       Mean |     Error |    StdDev |     Median |
|---------------------- |-----------:|----------:|----------:|-----------:|
|    ImmutableAddByType |  0.0191 ns | 0.0224 ns | 0.0210 ns |  0.0110 ns |
| ImmutableAddByRefType |  0.3007 ns | 0.0381 ns | 0.0648 ns |  0.2939 ns |
|      MutableAddByType |  0.0105 ns | 0.0198 ns | 0.0186 ns |  0.0000 ns |
|   MutableAddByRefType | 17.6355 ns | 0.3840 ns | 0.4856 ns | 17.6148 ns |
