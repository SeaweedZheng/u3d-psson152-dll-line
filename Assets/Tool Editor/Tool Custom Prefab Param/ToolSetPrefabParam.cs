using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToolSetPrefabParam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// 一键修改节点下所有符合条件的Text的参数VerticalOverflow = Overflow
    /// </summary>
    public void ChangeTextVerticalOverflow()
    {
#if UNITY_EDITOR
        /*LocalText[] comps = transform.GetComponentsInChildren<LocalText>();

        foreach (LocalText compItem in comps)
        {
            Transform tfm = compItem.transform.Find("Text");

            if (tfm != null )
            {
                Text txtComp = tfm.GetComponent<Text>();
                txtComp.verticalOverflow = VerticalWrapMode.Overflow;
            }
        }*/
#endif
    }
    /// <summary>
    /// 一键修改节点下所Text的参数VerticalOverflow = Overflow
    /// </summary>
    public void ChangeAllTextVerticalOverflow()
    {

        Text[] comps = transform.GetComponentsInChildren<Text>();  //只能是active=true的后代节点

        foreach (Text compItem in comps)
        {
            compItem.verticalOverflow = VerticalWrapMode.Overflow;
        }
    }

    public void ChangeAllTextVerticalOverflowIncludeDisactive()
    {

        List<Text> comps = GetComponentsInChildrenIncludeDisactive<Text>(transform);  //只能是active=true的后代节点

        foreach (Text compItem in comps)
        {
            compItem.verticalOverflow = VerticalWrapMode.Overflow;
        }
    }

    private List<T> GetComponentsInChildrenIncludeDisactive<T>(Transform tfmParent) where T : Component
    {
        List<T> list = new List<T>();
        foreach (Transform tfm in tfmParent)
        {
            T comp = tfm.GetComponent<T>();
            if (comp != null)
            {
                list.Add(comp);
            }
            list.AddRange(GetComponentsInChildrenIncludeDisactive<T>(tfm));
        }
        return list;
    }
}
