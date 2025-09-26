using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoroutineHelper : MonoBehaviour
{
    // Start is called before the first frame update
    static void Do(IEnumerator func)
    {
        GameObject go = new GameObject("CoroutineHelper");
        go.AddComponent<TestCoroutineHelper>();
        UnityEngine.Object.DontDestroyOnLoad(go);
        TestCoroutineHelper comp = go.GetComponent<TestCoroutineHelper>();
        comp.StartCoroutine(Do1(comp,func));
    }
    static public IEnumerator Do1(MonoBehaviour mono, IEnumerator func)
    {
        yield return func;

        Destroy(mono);
    }
}
