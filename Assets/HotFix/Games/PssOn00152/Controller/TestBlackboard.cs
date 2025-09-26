using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMaker;
public class TestBlackboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [Button]
    void SetBB(long credit = -999)
    {
        Variable<List<long>> vBetList =  BlackboardUtils.FindVariable<List<long>>(null, "./betList");
        vBetList.value[0] = credit;
    }


    [Button]
    void SetTotalBet(long credit = 0)
    {
        BlackboardUtils.SetValue<long>(null, "./totalBet", credit);

        DebugUtils.Log($"totalBet = {BlackboardUtils.GetValue<long>(null, "./totalBet")}");

    }

    [Button]
    void ShowTotalBet()
    {
        DebugUtils.Log($"totalBet = {BlackboardUtils.GetValue<long>(null, "./totalBet")}");
    }

    [Button]
    void SetSelectLine(int line = 0)
    {
        BlackboardUtils.SetValue<int>(null, "./selectLine", line);
        //DebugUtil.Log($"totalBet = {BlackboardUtils.GetValue<long>(null, "./totalBet")}");
    }

    [Button]
    void SetTestBB01_BetList(long credit = 0)
    {
        Variable<List<long>> vBetList = BlackboardUtils.FindVariable<List<long>>(null, "./testBB01/betList");
        vBetList.value[1] = credit;

        //DebugUtil.Log($"totalBet = {BlackboardUtils.GetValue<long>(null, "./totalBet")}");
    }





    [Button]
    void TestFind(string pth = "Reporter")
    {
        GameObject go = GOFind.FindObjectIncludeInactive(pth);
        if (go != null)
        {
            Debug.Log($"找到对象 ： {go.name}");
            go.SetActive(!go.active);
        }
        else
        {
            Debug.Log($"未找到对象");
        }

    }

    [Button]
    void TestShowDontDestroy()
    {
        GOFind.Test11();
    }

}
