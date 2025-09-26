using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
public class MaskIconUI : MonoBehaviour
{
    public GameObject goMask;

    public bool isClockWise = true;
    /// <summary> ���ڶ���msˢ��һ�� </summary>
    public float gapTimeMs = 50f;
    /// <summary> ʵ�����˶���ms </summary>
    private float dif = 0f;
    /// <summary> �ƶ����� </summary>
    public float multiplier = 0.2f;
    private float angle = 0;
    private long lastTime = 0;

    /// <summary> ��Ҫ��תʱ��һȦ��ת�Ĵ���(ȡ<=0ʱ������Ծ) </summary>
    public float time = 0;


    protected void Awake()
    {
        if (goMask == null)
            goMask = gameObject;
    }

    void FixedUpdate()
    {
        long nowTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        if (nowTime - lastTime > gapTimeMs)
        {
            float dif = nowTime - lastTime;
            lastTime = nowTime;
            //DebugUtil.Log($"dif = {dif}");
            //DebugUtil.Log($"dif * multiplier = {dif * multiplier}");

            if (time <= 0)  // ����ʽת��
                angle += (isClockWise ? -1 : 1) * dif * multiplier;
            else // ��Ծʽת��
                angle += (isClockWise ? -1 : 1)
                * (float)Math.Round(dif / gapTimeMs, 0)
                * (float)Math.Ceiling(multiplier)
                * (360f / time);

            // ������[-360,0],[0,360]������������
            angle = angle % 360;
            goMask.transform.rotation = Quaternion.Euler(goMask.transform.rotation.x, goMask.transform.rotation.y, angle);
        }
    }

    [Button]
    void TestAngle()
    {
        DebugUtils.Log($" 1 = {-30 % 360}");//-30
        DebugUtils.Log($" 2 = {-(720 + 56) % 360}");//-56

        DebugUtils.Log($" 2 = {-(720 + 56.66) % 360}");//-56.66

        DebugUtils.Log($" 3 = {(720 + 88) % 360}");//88
        DebugUtils.Log($" 3 = {(66) % 360}");//66
        DebugUtils.Log($" 3 = {(55.55) % 360}");//55.55
    }

}
