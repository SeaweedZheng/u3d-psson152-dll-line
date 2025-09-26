using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FlyCoinUI : MiniBezier
{

    public bool isClockWise = true;
    public float gapTimeMs = 80;

    /// <summary> һȦ360�ȣ���Ծ��ת���ٴ���ת�ꡣ</summary>
    public float time = -1;
    public float multiplier = 0.1f;

    float lastTimeS = 0;
    float angle = 0;
    protected override void Update()
    {
        base.Update();

        float nowTimeS = UnityEngine.Time.time;
        float dif = (nowTimeS - lastTimeS) * 1000f;
        if (dif > gapTimeMs )
        {
            lastTimeS = nowTimeS;

            if (time <= 0)  // ����ʽת��
                angle += (isClockWise ? -1 : 1) * dif * multiplier;
            else // ��Ծʽת��
                angle += (isClockWise ? -1 : 1)
                * (float)Math.Round(dif / gapTimeMs, 0)
                * (float)Math.Ceiling(multiplier)
                * (360f / time);

            // ������[-360,0],[0,360]������������
            angle = angle % 360;
            goTarget.transform.rotation = Quaternion.Euler(goTarget.transform.rotation.x, goTarget.transform.rotation.y, angle);
        }
    }


    public void SetSprite(Sprite sp)
    {
        goTarget.GetComponent<Image>().sprite = sp;
    }
    public void DoAnim(bool isLeft, TweenCallback cb = null)
    {
        Vector3 _starPos, _endPos;
        if (isLeft)
        {
            _starPos = v3StartPos;
            _endPos = v3EndPos;
        }
        else
        {
            _starPos = v3EndPos;
            _endPos = v3StartPos;
        }
  
        DoAnim(_starPos, v3ControlPos, _endPos, EaseUtil.DoTweenEaseOutInQuart2, cb);
    }
}
