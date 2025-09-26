using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCustomDic : MonoBehaviour
{
    public CustomDicStringSprit dic01;

    public CustomDicStringString dico2;
    void Start()
    {
        foreach (KeyValuePair<string, string> item in dico2)
        {
            DebugUtils.Log($"[KV] {item.Key} = {item.Value}");
        }
    }
}
