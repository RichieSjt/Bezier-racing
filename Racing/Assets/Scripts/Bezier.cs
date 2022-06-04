using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier
{
    public static Vector3 EvalBezier(List<Vector3> P, float t) 
    {
        int n = P.Count;
        Vector3 p = Vector3.zero;

        for(int i = 0; i < n; i++)
        {
            p += Combination(n-1, i) * Mathf.Pow(1.0f - t, n-1 - i) * Mathf.Pow(t, i) * P[i];
        }
        
        return p;
    }

    static int Factorial(int n)
    {
        if (n == 0) return 1;
        else return n * Factorial(n - 1);
    }

    static float Combination(int n, int i)
    {
        return Factorial(n) / (Factorial(i) * Factorial(n - i));
    }
}
