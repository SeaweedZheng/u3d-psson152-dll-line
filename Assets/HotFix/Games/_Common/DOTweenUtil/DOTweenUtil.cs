using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DOTweenUtil
{
    private static DOTweenUtil instance;
    public static DOTweenUtil Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DOTweenUtil();
            }
            return instance;
        }
    }
    public void DOMove(Transform tran, Vector3 to, float duration, Ease type = Ease.Linear, TweenCallback action = null)
    {
        tran.DOMove(to, duration, false).SetEase(type).OnComplete(action);
    }
    public void DOLocalMove(Transform tran, Vector3 to, float duration, Ease type = Ease.Linear, TweenCallback action = null)
    {
        tran.DOLocalMove(to, duration, false).SetEase(type).OnComplete(action);
    }
    public void DOLocalMoveX(Transform tran, float xTo, float duration, Ease type = Ease.Linear, TweenCallback action = null)
    {
        tran.DOLocalMoveX(xTo, duration, false).SetEase(type).OnComplete(action);
    }
    public void DOLocalMoveY(Transform tran, float yTo, float duration, Ease type = Ease.Linear, TweenCallback action = null)
    {
        tran.DOLocalMoveY(yTo, duration, false).SetEase(type).OnComplete(action);
    }
    public void DOMoveX(Transform tran, float xTo, float duration, Ease type = Ease.Linear, TweenCallback action = null)
    {
        tran.DOMoveX(xTo, duration, false).SetEase(Ease.Linear).OnComplete(action);
    }
}
