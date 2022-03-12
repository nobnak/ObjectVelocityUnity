using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

public class Test33Matrix
{
    private const float SIGMA = 2f * float.Epsilon;

    // A Test behaves as an ordinary method
    [Test]
    public void Test33MatrixSimplePasses() {
        var m = float3x3.zero;
        for (var r = 0; r < 3; r++) {
            for (var c = 0; c < 3; c++) {
                m[c][r] = (-1 * r) * (c + r * 3);
            }
        }
        m.AreEqual(new float3x3(0, 1, 2, -3, -4, -5, 6, 7, 8));

        Assert.AreEqual(3, math.csum(new float3(0, 1, 2)));
        Assert.AreEqual(m.Norm_1(), 15f);
        Assert.AreEqual(m.Norm_inf(), 21f);

        var norm = m.Norm_1_inf();
        Assert.AreEqual(norm.x, 15f);
        Assert.AreEqual(norm.y, 21f);
    }

    [Test]
    public void TestPolar() {
        var rot = float3x3.EulerXYZ(0f, 0f, 0.5f * math.PI);
        var scale = float3x3.Scale(10f, -200f, 3000f);
        var A = math.mul(rot, scale);

        var i = 0;
        var U = A;
        var closeToConvergence = false;

        for (i = 0; i < 100; i++) {
            var y = math.inverse(U);
            var r = 1f;

            if (!closeToConvergence) {
                var a = math.sqrt(U.Norm_1() * U.Norm_inf());
                var b = math.sqrt(y.Norm_1() * y.Norm_inf());
                r = math.sqrt(b / a);
            }

            var U1 = (r * U + math.transpose(y) / r) / 2;

            var diffnorm1 = (U1 - U).Norm_1();
            if (closeToConvergence) {
                if (diffnorm1 < SIGMA) break;
            } else {
                closeToConvergence = (diffnorm1 <= (10f * SIGMA) * U.Norm_1());
            }
            U = U1;
        }

        var H = math.mul(math.transpose(U), A);
        H = (H + math.transpose(H)) / 2;

        Debug.Log($"Result : loops={i}\nU: {U}\nH: {H}\nA: {A}");
        A.AreEqual(math.mul(U, H));
    }

    
}

public static class MatrixExtension {

    public static float Norm_1(this float3x3 m) {
        return math.max(math.max(
            math.csum(math.abs(m[0])), 
            math.csum(math.abs(m[1]))),
            math.csum(math.abs(m[2])));
    }
    public static float Norm_inf(this float3x3 m) {
        return math.transpose(m).Norm_1();
    }
    public static float2 Norm_1_inf(this float3x3 m) {
        float3 col = float3.zero, row = float3.zero;

        for (var r = 0; r < 3; r++) {
            for (var c = 0; c < 3; c++) {
                var v = math.abs(m[c][r]);
                col[c] += v;
                row[r] += v;
            }
        }

        return new float2(
            math.max(math.max(col[0], col[1]), col[2]),
            math.max(math.max(row[0], row[1]), row[2])
            );
    }
}

public static class TestUtils {
    public static void AreEqual(this float3 a, float3 b, float delta) {
        for (var i = 0; i < 3; i++)
            Assert.AreEqual(a[i], b[i], delta, $"at i={i}");
    }
    public static void AreEqual(this float3x3 a, float3x3 b) {
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                a[x].AreEqual(b[x], 1e-3f);
    }
}