using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EasingFunctions
{
    /// <summary> 빠르다가 느려짐 </summary>
    public static Func<float, float, float> easeOutCubic = (float x, float a) => 1 - Mathf.Pow(1 - x, a);
    /// <summary> 느리다가 빨라짐 </summary>
    public static Func<float, float, float> easeInCubic = (float x, float a) => Mathf.Pow(x, a);
    /// <summary> 느리다가 빨라짐 버젼 B </summary>
    public static Func<float, float, float> easeInCubicB = (float x, float a) => x * (Mathf.Pow(x, a) + 1);
    /// <summary> 느리다가 빨라지다가 느려짐 </summary>
    public static Func<float, float, float> easeInOutCirc = (float x, float a) => Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
}
