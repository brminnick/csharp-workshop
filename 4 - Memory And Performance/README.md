# In, out, ref, and benchmaks

## Steps to Execute Benchmark

### 1. Publish The Project

Run the following command to create a self contained executable

- Windows 64-bit OS

```console
dotnet publish -c Release --self-contained -r win10-x64
```

- Windows 32-bit OS

```console
dotnet publish -c Release --self-contained -r win10-x86
```

- macOS

```console
dotnet publish ./Benchmark.csproj -c Release --self-contained -r osx-x64
```

### 2. Run the Benchmark

Run from the following command in the project directory

- Windows 64-bit OS

```console
./bin/Release/netcoreapp2.0/win10-x64/benchmark.exe
```

- Windows 32-bit OS

```console
./bin/Release/netcoreapp2.0/win10-x32/benchmark.exe
```

- macOS

```console
./bin/Release/netcoreapp2.0/osx-x64/Benchmark
```

## Sample Benchmark Results

| Method | Mean | Error | StdDev | Median |
|--------|------|-------|--------|--------|
| ImmutableAddByType |  0.0191 ns | 0.0224 ns | 0.0210 ns |  0.0110 ns |
| ImmutableAddByRefType |  0.3007 ns | 0.0381 ns | 0.0648 ns |  0.2939 ns |
| MutableAddByType |  0.0105 ns | 0.0198 ns | 0.0186 ns |  0.0000 ns |
| MutableAddByRefType | 17.6355 ns | 0.3840 ns | 0.4856 ns | 17.6148 ns |

## Lab Explanation

In this lab, we will explore the following:

- pass by 'ref'
- pass uninitialized struct by 'out'

Explore different sizes. For the last one, compare with
members of your group because different machines may
have different characteristics.

Another question to explore if changing between 64 bit
and 32 bit and independent makes a difference. How about
.NET Framework and .NET Core?

Consider making a ref struct.

Consider adding interface implementation to the benchmark. How does that change the performance on structs, readonly structs, and ref struct (trick question, ref structs can't add interface implementation)

## Presenter Notes

Run the benchmark program. explain how 
benchmark.net works. It's OSS, its on NuGet and it's a great
tool to measure performance on low-level algorithms.

As written, this compares four permutations:
mutable struct, passed by value, 
mutable struct, pased by readonly ref, 
immutable struct, passed by value,
immutable struct, passed by readonly ref.

There are other permutations that apply and can help understand 
the nuances of different design choices.