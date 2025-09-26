using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;


// (【*****】【Unity3D BezierCurve绘制曲线】使用贝塞尔曲线公式与LineRenderer绘制曲线形成路径)[https://blog.csdn.net/f_957995490/article/details/105917091]


#region Bezier

[RequireComponent(typeof(LineRenderer))]
public partial class MiniBezier : MonoBehaviour
{
    private Tween tween;

    public float _times = 6f;
    public float _pointCount = 10f;
    public GameObject goTarget;


    public void DoAnim(Vector3 startPos, Vector3 controlPos, Vector3 endPos, Ease ease, TweenCallback cb = null)
    {
        Kill();

        goTarget.transform.position = startPos;
        Vector3[] pathvec = Bezier2Path(startPos, controlPos, endPos);
        //goTarget.transform.DOPath(pathvec, _times);

        tween = goTarget.transform.DOPath(pathvec, _times) // 设置路劲
        .SetEase(ease)  // 设置缓动函数(移动曲线)  //EaseUtil
        .OnComplete(cb)
        ;
    }

    public void DoAnim(Vector3 startPos, Vector3 controlPos, Vector3 endPos, EaseFunction easeFunc, TweenCallback cb = null)
    {
        Kill();

        goTarget.transform.position = startPos;
        Vector3[] pathvec = Bezier2Path(startPos, controlPos, endPos);

        tween = goTarget.transform.DOPath(pathvec, _times)
        .SetEase(easeFunc)
        //.SetLoops(-1, LoopType.Restart)// 设置为无限循环
        .OnComplete(cb)
        ;
    }

  
    //获取二阶贝塞尔曲线路径数组
    private Vector3[] Bezier2Path(Vector3 startPos, Vector3 controlPos, Vector3 endPos)
    {
        Vector3[] path = new Vector3[(int)_pointCount];
        for (int i = 1; i <= _pointCount; i++)
        {
            float t = i / _pointCount;
            path[i - 1] = Bezier2(startPos, controlPos, endPos, t);
        }
        return path;
    }
    // 2阶贝塞尔曲线（分别是起始点 P0、结束点 P2 和一个控制点 P1。）
    public static Vector3 Bezier2(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float t)
    {
        return (1 - t) * (1 - t) * startPos + 2 * t * (1 - t) * controlPos + t * t * endPos;
    }

    // 3阶贝塞尔曲线（分别是起始点 P0、结束点 P3 和两个控制点 P1和P2。）
    public static Vector3 Bezier3(Vector3 startPos, Vector3 controlPos1, Vector3 controlPos2, Vector3 endPos, float t)
    {
        float t2 = 1 - t;
        return t2 * t2 * t2 * startPos
            + 3 * t * t2 * t2 * controlPos1
            + 3 * t * t * t2 * controlPos2
            + t * t * t * endPos;
    }
}
#endregion

#region DoTween封装

[RequireComponent(typeof(LineRenderer))]
public partial class MiniBezier : MonoBehaviour
{

    [Button]
    void TestDoAnim()
    {
        DoAnim(v3StartPos, v3ControlPos, v3EndPos, EaseUtil.DoTweenEaseOutInQuart2);
    }


    /*<summary> 倒放 （不起作用！)</summary>
    public void PlayBackwards(Vector3 startPos, Vector3 controlPos, Vector3 endPos, EaseFunction easeFunc, TweenCallback cb = null)
    {
        Kill();

        goTarget.transform.position = startPos;
        Vector3[] pathvec = Bezier2Path(startPos, controlPos, endPos);

        tween = goTarget.transform.DOPath(pathvec, _times)
        .SetEase(easeFunc)
        //.SetLoops(-1, LoopType.Restart)// 设置为无限循环
        .OnComplete(cb)
        ;

        if (tween != null && tween.IsActive())
            tween.PlayBackwards(); //倒放
    }*/


    /// <summary> 停止动画 </summary>
    [Button]
    public void Pause()
    {
        if (tween != null && tween.IsPlaying())
            tween.Pause();  
    }

    /// <summary> 杀死动画 </summary>
    [Button]
    public void Kill()
    {
        if (tween != null && tween.IsActive())
            tween.Kill();  
    }

    /// <summary> 继续播放 </summary>
    [Button]
    public void Play()
    {
        if (tween != null && tween.IsActive())
            tween.Play(); 
    }

    /// <summary> 
    /// 正放 
    /// 
    /// * 默认正放
    /// * 重复正放，没有任何效果
    /// * 倒放和正放，只是改变播放方向，不改变起点。
    /// </summary>
    [Button]
    public void PlayForward()
    {
        if (tween != null && tween.IsActive())
            tween.PlayForward(); 
    }


    /// <summary>
    /// 倒放
    /// 
    /// * 只有先正放，才能倒放。
    /// * 无法一开始就倒放。
    /// </summary>
    [Button]
    public void PlayBackwards()
    {
        if (tween != null && tween.IsActive())
            tween.PlayBackwards();
    }  

}
#endregion

#region 编辑器划线显示 Bezier 函数

/// <summary>
/// 编辑器划线显示 Bezier 函数
/// </summary>
/// <remarks>
/// 方法1：使用./Editor/MiniBeizerDragListenerEditor.cs脚本<br/>
/// 方法2: 使用[ExecuteInEditMode] + Update方法
/// </remarks>
//[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public partial class MiniBezier : MonoBehaviour
{

    public GameObject goStart;
    public GameObject goControl;
    public GameObject goEnd;

    protected virtual void Update()
    {

#if UNITY_EDITOR && false
        if (Application.isEditor && !Application.isPlaying) // 不允许游戏时显示
        {
            // Debug.Log("i am here 123456");
            DrawCurve(); // 在编辑器中进行划线
        }
#endif


#if UNITY_EDITOR


        //OutQuart

        if (Application.isEditor && Application.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                DoAnim(v3StartPos, v3ControlPos, v3EndPos, EaseUtil.DoTweenEaseOutInQuart2);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                DoAnim(v3StartPos, v3ControlPos, v3EndPos, Ease.OutQuart);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                DoAnim(v3StartPos, v3ControlPos, v3EndPos, EaseUtil.DoTweenEaseOutInQuart2);
            }
        }
#endif
    }

    public Vector3 v3StartPos
    {
        get
        {
            if (goStart != null)
            {
                return goStart.transform.position;
            }
            return Vector3.zero;
        }
    }

    public Vector3 v3ControlPos
    {
        get
        {
            if (goControl != null)
            {
                return goControl.transform.position;
            }
            return Vector3.zero;
        }
    }

    public Vector3 v3EndPos
    {
        get
        {
            if (goEnd != null)
            {
                return goEnd.transform.position;
            }
            return Vector3.zero;
        }
    }

    private LineRenderer m_lineRenderer = null;


    private int SEGMENT_COUNT = 60;//曲线取点个数（取点越多这个长度越趋向于精确）
    LineRenderer lineRenderer
    {
        get
        {
            if (m_lineRenderer == null)
            {
                m_lineRenderer = transform.GetComponent<LineRenderer>();
            }
            return m_lineRenderer;
        }
    }


    bool isDrawCurve = false;
    [Button]
    public void DrawCurve()//画曲线
    {
        isDrawCurve = true;
        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            float t = (float)i / (float)SEGMENT_COUNT;
            Vector3 pixel = Bezier2(v3StartPos, v3ControlPos, v3EndPos, t);
            lineRenderer.positionCount = SEGMENT_COUNT;
            lineRenderer.SetPosition(i, pixel);
        }

    }
    [Button]
    public void ClearCurve()//画曲线
    {
        isDrawCurve = false;
        lineRenderer.positionCount = 0;
    }
}
#endregion