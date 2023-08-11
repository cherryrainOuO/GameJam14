using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class YieldFunctions
{
    public static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!_timeInterval.TryGetValue(seconds, out wfs))
            _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }

    internal class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y) => x == y;
        int IEqualityComparer<float>.GetHashCode(float obj) => obj.GetHashCode();
    }

    private static readonly Dictionary<Func<bool>, WaitUntil> _timeInterval2 = new Dictionary<Func<bool>, WaitUntil>(new FuncComparer());
    public static WaitUntil WaitUntil(Func<bool> predicate)
    {
        WaitUntil wu;
        if (!_timeInterval2.TryGetValue(predicate, out wu))
            _timeInterval2.Add(predicate, wu = new WaitUntil(predicate));
        return wu;
    }

    internal class FuncComparer : IEqualityComparer<Func<bool>>
    {
        bool IEqualityComparer<Func<bool>>.Equals(Func<bool> x, Func<bool> y) => x == y;
        int IEqualityComparer<Func<bool>>.GetHashCode(Func<bool> obj) => obj.GetHashCode();
    }
}
