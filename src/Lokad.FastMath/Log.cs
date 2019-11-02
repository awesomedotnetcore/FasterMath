﻿using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Lokad.FastMath
{
    public partial class FastMath
    {
        // https://github.com/jhjourdan/SIMD-math-prims/blob/master/simd_math_prims.h

        ///* Absolute error bounded by 1e-6 for normalized inputs
        //    Returns a finite number for +inf input
        //    Returns -inf for nan and <= 0 inputs.
        //    Continuous error. */
        //inline float logapprox(float val) {
        //    union { float f; int32_t i; } valu;
        //    float exp, addcst, x;
        //    valu.f = val;
        //    exp = valu.i >> 23;
        //    /* -89.970756366f = -127 * log(2) + constant term of polynomial below. */
        //    addcst = val > 0 ? -89.970756366f : -(float)INFINITY;
        //    valu.i = (valu.i & 0x7FFFFF) | 0x3F800000;
        //    x = valu.f;


        //    /* Generated in Sollya using:
        //    > f = remez(log(x)-(x-1)*log(2),
        //            [|1,(x-1)*(x-2), (x-1)*(x-2)*x, (x-1)*(x-2)*x*x,
        //                (x-1)*(x-2)*x*x*x|], [1,2], 1, 1e-8);
        //    > plot(f+(x-1)*log(2)-log(x), [1,2]);
        //    > f+(x-1)*log(2)
        //    */
        //    return
        //    x * (3.529304993f + x * (-2.461222105f + x * (1.130626167f +
        //        x * (-0.288739945f + x * 3.110401639e-2f))))
        //    + (addcst + 0.6931471805f*exp);
        //}

        /// <summary>
        /// Absolute error bounded by 1e-4.
        /// </summary>
        /// <remarks>
        /// 2x slower than MathF.Log(), 0.7x faster than Math.Log(). 
        /// Aka 4x faster per-value then MathF.Log, and 12x faster than Math.Log.
        /// </remarks>
        public unsafe static Vector256<float> Log(Vector256<float> val)
        {
            Vector256<float> exp, addcst, x;

            exp = Avx2.ConvertToVector256Single(Avx2.ShiftRightArithmetic(val.As<float, int>(), 23));

            // According to BenchmarkDotNet, isolating all the constants up-front
            // yield nearly 10% speed-up.

            const float bf0 = -89.970756366f;
            const float bf1 = float.NaN; // behavior of MathF.Log() on negative numbers
            const float bf2 = 3.529304993f;
            const float bf3 = -2.461222105f;
            const float bf4 = 1.130626167f;
            const float bf5 = -0.288739945f;
            const float bf6 = 3.110401639e-2f;
            const float bf7 = 0.6931471805f;

            const int bi0 = 0x7FFFFF;
            const int bi1 = 0x3F800000;

            //addcst = val > 0 ? -89.970756366f : -(float)INFINITY;

            addcst = Avx.BlendVariable(Vector256.Create(bf0),
                Vector256.Create(bf1),
                Avx.Compare(val, Vector256<float>.Zero, FloatComparisonMode.OrderedLessThanNonSignaling));

            x = Avx2.Or(Avx2.And(
                    val.As<float, int>(),
                    Vector256.Create(bi0)),
                    Vector256.Create(bi1)).As<int, float>();

            /*    x * (3.529304993f + 
                    x * (-2.461222105f + 
                      x * (1.130626167f +
                        x * (-0.288739945f + 
                          x * 3.110401639e-2f))))
                + (addcst + 0.6931471805f*exp); */

            return Avx2.Add(
                   Avx2.Multiply(x, Avx2.Add(Vector256.Create(bf2),
                     Avx2.Multiply(x, Avx2.Add(Vector256.Create(bf3),
                       Avx2.Multiply(x, Avx2.Add(Vector256.Create(bf4),
                         Avx2.Multiply(x, Avx2.Add(Vector256.Create(bf5),
                           Avx2.Multiply(x, Vector256.Create(bf6)))))))))),
                   Avx.Add(addcst,
                     Avx2.Multiply(Vector256.Create(bf7), exp)));
        }
    }
}