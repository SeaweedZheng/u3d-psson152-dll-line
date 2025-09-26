using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
public class MaskIconUI : MonoBehaviour
{
    public GameObject goMask;

    public bool isClockWise = true;
    /// <summary> 大于多少ms刷新一次 </summary>
    public float gapTimeMs = 50f;
    /// <summary> 实际用了多少ms </summary>
    private float dif = 0f;
    /// <summary> 移动翻倍 </summary>
    public float multiplier = 0.2f;
    private float angle = 0;
    private long lastTime = 0;

    /// <summary> 需要跳转时，一圈跳转的次数(取<=0时，不跳跃) </summary>
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

            if (time <= 0)  // 连续式转动
                angle += (isClockWise ? -1 : 1) * dif * multiplier;
            else // 跳跃式转动
                angle += (isClockWise ? -1 : 1)
                * (float)Math.Round(dif / gapTimeMs, 0)
                * (float)Math.Ceiling(multiplier)
                * (360f / time);

            // 控制在[-360,0],[0,360]这两个区间内
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
