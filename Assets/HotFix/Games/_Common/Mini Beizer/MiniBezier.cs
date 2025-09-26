using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;


// (��*****����Unity3D BezierCurve�������ߡ�ʹ�ñ��������߹�ʽ��LineRenderer���������γ�·��)[https://blog.csdn.net/f_957995490/article/details/105917091]


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

        tween = goTarget.transform.DOPath(pathvec, _times) // ����·��
        .SetEase(ease)  // ���û�������(�ƶ�����)  //EaseUtil
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
        //.SetLoops(-1, LoopType.Restart)// ����Ϊ����ѭ��
        .OnComplete(cb)
        ;
    }

  
    //��ȡ���ױ���������·������
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
    // 2�ױ��������ߣ��ֱ�����ʼ�� P0�������� P2 ��һ�����Ƶ� P1����
    public static Vector3 Bezier2(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float t)
    {
        return (1 - t) * (1 - t) * startPos + 2 * t * (1 - t) * controlPos + t * t * endPos;
    }

    // 3�ױ��������ߣ��ֱ�����ʼ�� P0�������� P3 ���������Ƶ� P1��P2����
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

#region DoTween��װ

[RequireComponent(typeof(LineRenderer))]
public partial class MiniBezier : MonoBehaviour
{

    [Button]
    void TestDoAnim()
    {
        DoAnim(v3StartPos, v3ControlPos, v3EndPos, EaseUtil.DoTweenEaseOutInQuart2);
    }


    /*<summary> ���� ���������ã�)</summary>
    public void PlayBackwards(Vector3 startPos, Vector3 controlPos, Vector3 endPos, EaseFunction easeFunc, TweenCallback cb = null)
    {
        Kill();

        goTarget.transform.position = startPos;
        Vector3[] pathvec = Bezier2Path(startPos, controlPos, endPos);

        tween = goTarget.transform.DOPath(pathvec, _times)
        .SetEase(easeFunc)
        //.SetLoops(-1, LoopType.Restart)// ����Ϊ����ѭ��
        .OnComplete(cb)
        ;

        if (tween != null && tween.IsActive())
            tween.PlayBackwards(); //����
    }*/


    /// <summary> ֹͣ���� </summary>
    [Button]
    public void Pause()
    {
        if (tween != null && tween.IsPlaying())
            tween.Pause();  
    }

    /// <summary> ɱ������ </summary>
    [Button]
    public void Kill()
    {
        if (tween != null && tween.IsActive())
            tween.Kill();  
    }

    /// <summary> �������� </summary>
    [Button]
    public void Play()
    {
        if (tween != null && tween.IsActive())
            tween.Play(); 
    }

    /// <summary> 
    /// ���� 
    /// 
    /// * Ĭ������
    /// * �ظ����ţ�û���κ�Ч��
    /// * ���ź����ţ�ֻ�Ǹı䲥�ŷ��򣬲��ı���㡣
    /// </summary>
    [Button]
    public void PlayForward()
    {
        if (tween != null && tween.IsActive())
            tween.PlayForward(); 
    }


    /// <summary>
    /// ����
    /// 
    /// * ֻ�������ţ����ܵ��š�
    /// * �޷�һ��ʼ�͵��š�
    /// </summary>
    [Button]
    public void PlayBackwards()
    {
        if (tween != null && tween.IsActive())
            tween.PlayBackwards();
    }  

}
#endregion

#region �༭��������ʾ Bezier ����

/// <summary>
/// �༭��������ʾ Bezier ����
/// </summary>
/// <remarks>
/// ����1��ʹ��./Editor/MiniBeizerDragListenerEditor.cs�ű�<br/>
/// ����2: ʹ��[ExecuteInEditMode] + Update����
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
        if (Application.isEditor && !Application.isPlaying) // ��������Ϸʱ��ʾ
        {
            // Debug.Log("i am here 123456");
            DrawCurve(); // �ڱ༭���н��л���
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


    private int SEGMENT_COUNT = 60;//����ȡ�������ȡ��Խ���������Խ�����ھ�ȷ��
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
    public void DrawCurve()//������
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
    public void ClearCurve()//������
    {
        isDrawCurve = false;
        lineRenderer.positionCount = 0;
    }
}
#endregion