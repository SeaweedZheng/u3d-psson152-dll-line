using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BzLine : MonoBehaviour
{
    [Header("移动的物体")]
    public Transform target;  // 目标物体
    [Header("飞行时间控制")]
    public float line_time = 5f;
    [Header("重新开始按钮")]
    public Button _button;
    private LineRenderer lineRenderer;
    [Header("起始点,n个拐点,终点")]
    public List<Vector3> points = new List<Vector3>();
    private List<Vector3> points1;
    [Header("是否启用世界坐标")]
    public bool pos_type = true;


    public Tween tween;
    void Start()
    {
        _button.onClick.AddListener(tryAgain);
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        points1 = new List<Vector3>();
        //MoveToBeizer(target, points.ToArray(), line_time, null, true);
    }

    void tryAgain()
    {
        MoveToBeizer(target, points.ToArray(), line_time, null, pos_type);
    }

    public void MoveToBeizer(Transform _transform, Vector3[] bzizerVec, float time, Action _Compelte, bool Isword)
    {
        StartCoroutine(BeizerTween(_transform, bzizerVec, time, _Compelte, Isword));
    }

    IEnumerator BeizerTween(Transform _transform, Vector3[] bzizerVec, float time, Action _Compelte, bool Isword)
    {
        target.transform.position = points[0];
        lineRenderer.positionCount = 0;
        Destroy(lineRenderer);
        points1.Clear();
        yield return new WaitForSeconds(0.1f);
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        Vector3[] CurrArray = BeizerTool.GetBeizerList_n(bzizerVec, (int)(time * 50));
        int index = 0;
        float counter = 0;
        while (counter < time)
        {
            counter += Time.fixedDeltaTime;
            if (Isword is true)
            {
                _transform.DOMove(CurrArray[index], 0.01f);
            }
            else
                _transform.DOLocalMove(CurrArray[index], 0.01f);

            index += 1;
            if (index > CurrArray.Length - 1)
            {
                index = CurrArray.Length - 1;
            }
            yield return new WaitForFixedUpdate();
        }
        if (_Compelte != null)
            _Compelte.Invoke();
        //_transform.gameObject.AddComponent<CanvasGroup>().DOFade(0, 1).OnComplete(delegate() {
        //    Destroy(_transform.gameObject);
        //});
    }

    void Update()
    {
        // 获取目标物体的位置，并添加到路径点列表

        points1.Add(target.position);
        // 更新LineRenderer的点
        if (lineRenderer)
        {
            lineRenderer.positionCount = points1.Count;
            lineRenderer.SetPositions(points1.ToArray());
        }
    }
}