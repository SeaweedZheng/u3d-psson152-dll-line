using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using System;
using SlotMaker;
using TMPro;

[Serializable]
public class Test09
{
    /// <summary>单圈转动时间 </summary>
    [SerializeField]
    public float time = 0;

}
public class TestDotween : MonoBehaviour
{
    private TextMeshProUGUI tmpSymbol;

    public int idx;
    public ReelSetting defaultReelSetting;
    public Test09 tesclass;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    void Test(float timeS = 1f)
    {
        DOTweenUtil.Instance.DOLocalMoveY(transform, 0, timeS, Ease.Linear, () => {});
    }
}
