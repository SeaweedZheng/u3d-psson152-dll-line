using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DefaultBasePopup
{

    public static Dictionary<GameObject, DefaultBasePopup> pageCtrlLst = new Dictionary<GameObject, DefaultBasePopup>();


    protected GameObject goPopup;

    public virtual bool IsOpen() => goPopup != null && goPopup.active;

    public virtual bool IsTop()
    {
        return goPopup != null && goPopup.transform.parent != null
            && goPopup.transform.GetSiblingIndex() == goPopup.transform.parent.childCount - 1;
    }

    public GameObject CreatPop(string path = "Common/Prefabs/Popup Json Editor")
    {
            GameObject root = GameObject.Find("Canvas Overlay");
            GameObject goClone = Resources.Load<GameObject>(path);
            GameObject goPop = GameObject.Instantiate(goClone);
            goPop.transform.SetParent(root.transform, false);

            goPop.transform.localPosition = Vector3.zero;
            goPop.transform.localScale = Vector3.one;

            RectTransform rectTransform = goPop.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;       // 左下锚点 (0, 0)
            rectTransform.anchorMax = Vector2.one;        // 右上锚点 (1, 1)
                                                          // 设置 Offsets 为 0（与边缘对齐）
            rectTransform.offsetMin = Vector2.zero;       // left = 0, bottom = 0
            rectTransform.offsetMax = Vector2.zero;       // right = 0, top = 0

        return goPop;
    }

    public void OnOpen(string path = "Common/Prefabs/Popup Json Editor")
    {
        if (goPopup == null)
        {
            goPopup = CreatPop(path);
            pageCtrlLst.Add(goPopup,this);

            InitParam();
        }

        if (!IsTop())
            OnTop();

        goPopup.transform.SetSiblingIndex(goPopup.transform.parent.childCount - 1);
        goPopup.SetActive(true);
       
    }


    public virtual void OnTop()
    {
        Debug.Log($"i am {this.GetType().Name}");
    }

    protected virtual void InitParam()
    {

    }

    protected virtual void OnClose()
    {
        if (goPopup != null)
        {
            goPopup.transform.SetSiblingIndex(0);
            goPopup.SetActive(false);

            int count = goPopup.transform.parent.childCount;
            for (int i = count-1; i>=0; i--)
            {
                Transform tfm = goPopup.transform.parent.GetChild(i);
                if (tfm.gameObject.active == true)
                {
                    if (pageCtrlLst.ContainsKey(tfm.gameObject))
                        pageCtrlLst[tfm.gameObject].OnTop();
                    /*DefaultBasePopup pop = tfm.GetComponent<DefaultBasePopup>();
                    if (pop != null)
                        pop.OnTop();
                    */
                    break;
                }
            }
        }
    }

    public virtual void OpenPopup(string path = "Common/Prefabs/Popup Json Editor")
    {
        OnOpen(path);
    }
    public virtual void ClosePopup()
    {
        OnClose();
    }
}
