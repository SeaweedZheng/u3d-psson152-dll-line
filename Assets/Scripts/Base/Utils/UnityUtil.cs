using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region ¡¾Find GameObject¡¿
public partial class UnityUtil
{
    public static List<T> GetComponentsInChildrenIncludeDisactive<T>(Transform tfmParent) where T : Component
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

    public static GameObject FindParentWithComponent<T>(GameObject go, int j = 50) where T : Component
    {
        Transform tfm = go.transform;
        while (--j > 0)
        {
            tfm = tfm.parent;
            if (tfm == null)
                return null;
            T comp = tfm.GetComponent<T>();
            if (comp != null)
                return tfm.gameObject;
        }
        return null;
    }
}
#endregion

public partial class UnityUtil
{

    public static void SetRectTransformAnchors(RectTransform rtfm, float left, float right, float top, float bottom)
    {
        Debug.LogError("ÓÐbug");
        rtfm.offsetMin = new Vector2(left, bottom);
        rtfm.offsetMax = new Vector2(right, top);
    }

}
