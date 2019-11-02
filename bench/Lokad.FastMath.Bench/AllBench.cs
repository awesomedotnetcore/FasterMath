﻿using BenchmarkDotNet.Attributes;
using Lokad.FastMath.Alt;
using System;
using System.Runtime.Intrinsics;

namespace Lokad.FastMath.Bench
{
    [RPlotExporter]
    public class AllBench
    {
        public float DiGX = 4f;

        public Vector256<float> DigGX8 = Vector256.Create(4f);

        [Benchmark]
        public float Digamma_FastMath() => FastMath.Digamma(DiGX);

        [Benchmark]
        public Vector256<float> Digamma_FastMath_F8() => FastMath.Digamma(DigGX8);

        [Benchmark]
        public double Digamma_AltMath() => AltMath.Digamma(DiGX);


        public float ExpX = 0.1f;

        public Vector256<float> X8 = Vector256.Create(0.1f);

        [Benchmark]
        public float Exp_System_MathF() => MathF.Exp(ExpX);

        [Benchmark]
        public float Exp_System_Math() => (float)Math.Exp(ExpX);

        [Benchmark]
        public float Exp_FastMath() => FastMath.Exp(ExpX);

        [Benchmark]
        public Vector256<float> Exp_FastMath_F8() => FastMath.Exp(X8);


        public float LogX = 0.1f;

        public Vector256<float> LogX8 = Vector256.Create(0.1f);

        [Benchmark]
        public float Log_System_MathF() => MathF.Log(LogX);

        [Benchmark]
        public float Log_System_Math() => (float)Math.Log(LogX);

        [Benchmark]
        public float Log_FastMath() => FastMath.Log(LogX);

        [Benchmark]
        public Vector256<float> Log_FastMath_F8() => FastMath.Log(LogX8);


        public float Log2X = 123;

        [Benchmark]
        public uint Log2_System_MathF() => (uint)MathF.Log(Log2X, 2.0f);

        [Benchmark]
        public uint Log2_System_Math() => (uint)Math.Log(Log2X, 2.0);

        [Benchmark]
        public uint Log2_FastMath_Uint() => FastMath.Log2(123);

        [Benchmark]
        public uint Log2_AltMath_WithLookup() => AltMath.Log2(123);


        public float LogGX = 0.1f;

        public Vector256<float> LogGX8 = Vector256.Create(0.1f);

        [Benchmark]
        public float LogGamma_FastMath() => FastMath.LogGamma(LogGX);

        [Benchmark]
        public Vector256<float> LogGamma_FastMath_F8() => FastMath.LogGamma(LogGX8);

        [Benchmark]
        public double LogGamma_AltMath() => AltMath.LogGamma(LogGX);
    }
}
