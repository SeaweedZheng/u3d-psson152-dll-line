using System;
using UnityEngine;

/// <summary>
/// 变化函数
/// </summary>
public static class TweenUtils
{
    #region Tween Vector

    public static Vector3 VectorTweenLinear(Vector3 from, Vector3 to, float t)
    {
        return Vector3.Lerp(from, to, t);
    }

    public static Vector3 VectorTweenCollectMove(Vector3 from, Vector3 to, float t)
    {
        return Vector3.Lerp(from, to, EaseCollectMove(t));
    }

    public static Vector3 VectorTweenInSine(Vector3 from, Vector3 to, float t)
    {
        return Vector3.Lerp(from, to, EaseInSine(t));
    }

    public static Vector3 VectorTweenOutSine(Vector3 from, Vector3 to, float t)
    {
        return Vector3.Lerp(from, to, EaseOutSine(t));
    }

    public static Vector3 VectorTweenInCubic(Vector3 from, Vector3 to, float t)
    {
        return Vector3.Lerp(from, to, EaseInCubic(t));
    }

    public static Vector3 VectorTweenOutCubic(Vector3 from, Vector3 to, float t)
    {
        return Vector3.Lerp(from, to, EaseOutCubic(t));
    }

    public static Vector3 VectorTweenInQuint(Vector3 from, Vector3 to, float t)
    {
        return Vector3.Lerp(from, to, EaseInQuint(t));
    }

    public static Color ColorTweenInSine(Color from, Color to, float t)
    {
        return Color.Lerp(from, to, EaseInSine(t));
    }

    public static Color ColorTweenInQuad(Color from, Color to, float t)
    {
        return Color.Lerp(from, to, EaseInQuad(t));
    }


    public static Vector3 VectorDownUp(Vector3 from, Vector3 to, float t)
    {
        //y = 4x^2 - 4x + 1
        float func(float t) => (float)(4 * t * t - 4 * t + 1);
        return Vector3.Lerp(from, to, func(t));
    }



    #endregion

    #region Ease Function
    // preview https://easings.net/zh-cn

    //public static float 

    public static float EaseInQuad(float t)
    {
        return t * t;
    }

    public static float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }

    public static float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : 1 - Mathf.Pow(-2f * t + 2f, 2f) * 0.5f;
    }

    public static float EaseInSine(float t)
    {
        return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
    }

    public static float EaseOutSine(float t)
    {
        return Mathf.Sin(t * Mathf.PI * 0.5f);
    }

    public static float EaseInOutSine(float t)
    {
        return -(Mathf.Cos(t * Mathf.PI) - 1f) * 0.5f;
    }

    public static float EaseCollectMove(float t)
    {
        return -t * t + 2f * t;
    }

    public static float EaseInCubic(float t)
    {
        return t * t * t;
    }

    public static float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }

    public static float EaseInQuint(float t)
    {
        return t * t * t * t * t;
    }

    public static float EaseOutQuint(float t)
    {
        return 1 - Mathf.Pow(1 - t, 5);
    }

    public static float EaseInCirc(float t)
    {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));
    }

    public static float EaseOutQuart(float t)
    {
        return 1 - Mathf.Pow(1 - t, 4);
    }

    public static float EaseOutCirc(float t)
    {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));
    }


    #endregion
}
