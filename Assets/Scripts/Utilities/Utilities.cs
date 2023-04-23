using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static float EaseInOut(float t, float easingFactor)
    {
        // Apply ease-in-out function to t
        return (3f - 2f * Mathf.Pow(1f - t, easingFactor)) * t * t;
    }
}
