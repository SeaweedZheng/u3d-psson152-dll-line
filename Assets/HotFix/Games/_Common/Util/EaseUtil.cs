using System;
using UnityEngine;

/// <summary>
/// 缓动函数化函数
/// </summary>
public partial class EaseUtil : MonoBehaviour
{

    // easeOutQuart 缓动函数
    public static float EaseOutQuart(float t)
    {
        return 1f - (float)Math.Pow(1 - t, 4);
    }

}

#region DoTween
public partial class EaseUtil : MonoBehaviour
{

    public static float DoTweenEaseFuncLinear(float time, float duration, float overshootOrAmplitude, float period)
    {
        //DebugUtil.Log($"time ={time} duration={duration} overshootOrAmplitude={overshootOrAmplitude} period={period}");
        float t = time / duration;
        return t;

        // 返回是位置！！
    }



    /// <summary>
    /// "动画变化速率"和时间T的关系（不是速度V和时间T的关系!!）
    /// </summary>
    /// <param name="time"></param>
    /// <param name="duration"></param>
    /// <param name="overshootOrAmplitude"></param>
    /// <param name="period"></param>
    /// <returns></returns>
    public static float DoTweenEaseOutQuart(float time, float duration, float overshootOrAmplitude, float period)
    {
        float t = time / duration;

        float res = (1f - (float)Math.Pow(1 - t, 4));

        DebugUtils.Log($"time ={time} duration={duration} res = {res} overshootOrAmplitude={overshootOrAmplitude} period={period}");
        //return (float)time / duration;

        return res;

        // 返回是位置！！

    }


    /// <summary>
    /// * "动画变化速率"和时间t的函数关系
    /// * s先减速增加，t再加速增加
    /// * 标准化时间，t从0变化到1.
    /// * 标准化位移，s从0增加到1.
    /// </summary>
    /// <param name="time">标准化时间，0到1</param>
    /// <param name="duration">实际时间，0到duration</param>
    /// <param name="overshootOrAmplitude"></param>
    /// <param name="period"></param>
    /// <returns></returns>
    public static float DoTweenEaseOutInQuart4(float time, float duration, float overshootOrAmplitude, float period)
    {
        float t = time / duration;

        t = t - 0.5f;
        if (t < 0)
        {
            return -0.5f * ((float)Math.Pow(-2 * t, 4) - 1);
        }
        else
        {
            return 0.5f * ((float)Math.Pow(2 * t, 4) + 1);
        }
    }

    public static float DoTweenEaseOutInQuart2(float time, float duration, float overshootOrAmplitude, float period)
    {
        float t = time / duration;

        t = t - 0.5f;
        if (t < 0)
        {
            return -0.5f * ((float)Math.Pow(-2 * t, 2) - 1);
        }
        else
        {
            return 0.5f * ((float)Math.Pow(2 * t, 2) + 1);
        }
    }

    //  => (float)(4 * time * time - 4 * time + 1);
    //public delegate float EaseFunction(float time, float duration, float overshootOrAmplitude, float period);

    public static float DoTweenEaseOutInQuart2(float time)
    {
        return DoTweenEaseOutInQuart2(time, 1, 0, 0);
    }

}

#endregion