# In, out, ref, and benchmaks

Presenter: Run the benchmark program. explain how 
benchmark.net works. It's OSS, its on NuGet and it's a great
tool to measure performance on low-level algorithms.

As written, this compares four permutations:
mutable struct, passed by value, 
mutable struct, pased by readonly ref, 
immutable struct, passed by value,
immutable struct, passed by readonly ref.

There are other permutations that apply and can help understand 
the nuances of different design choices.

The code for the lab is here: https://github.com/dotnet/samples/tree/master/csharp/safe-efficient-code/benchmark

In this lab, explore the following:

. pass by 'ref'
. pass uninitialized struct by 'out'

Explore different sizes. For the last one, compare with
members of your group because different machines may
have different characteristics.

Another question to explore if changing between 64 bit
and 32 bit and independent makes a difference. How about
.NET Framework and .NET Core?

Consider making a ref struct.

Consider adding interface implementation to the benchmark. How does that change the performance on structs, readonly structs, and ref struct (trick question, ref structs can't add interface implementation)