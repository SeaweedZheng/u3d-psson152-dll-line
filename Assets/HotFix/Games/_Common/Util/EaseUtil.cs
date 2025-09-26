using System;
using UnityEngine;

/// <summary>
/// ��������������
/// </summary>
public partial class EaseUtil : MonoBehaviour
{

    // easeOutQuart ��������
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

        // ������λ�ã���
    }



    /// <summary>
    /// "�����仯����"��ʱ��T�Ĺ�ϵ�������ٶ�V��ʱ��T�Ĺ�ϵ!!��
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

        // ������λ�ã���

    }


    /// <summary>
    /// * "�����仯����"��ʱ��t�ĺ�����ϵ
    /// * s�ȼ������ӣ�t�ټ�������
    /// * ��׼��ʱ�䣬t��0�仯��1.
    /// * ��׼��λ�ƣ�s��0���ӵ�1.
    /// </summary>
    /// <param name="time">��׼��ʱ�䣬0��1</param>
    /// <param name="duration">ʵ��ʱ�䣬0��duration</param>
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