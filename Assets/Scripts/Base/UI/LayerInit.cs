using Game;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LayerInit : MonoBehaviour
{

    bool isInit = false;
    void Start()
    {
        if (isInit)
            return;
        isInit = true;

        GameObject goBasePage = UnityUtil.FindParentWithComponent<PageBase>(gameObject);

        if (goBasePage == null)
            return;
        Canvas canvas = goBasePage.transform.GetComponent<Canvas>();
        PageBase compBasePage = goBasePage.transform.GetComponent<PageBase>();

        //Debug.Log($"Layer Iinit Info :{transform.name} -- {goBasePage.name} -- {compBasePage.pageNumb} --  {compBasePage.pageType.ToString()}");
        if (canvas != null
            //&& canvas.sortingLayerName == LayerController.STACK_PAGE
            && PageManager.Instance.IndexOf(compBasePage)!= 0
           )
        {
            List<LayerController> lst = LayerController.GreatLayerInfos(transform);


            // 还没start前，可以已经被设置为“STACK PAGE”
            lst.ForEach(lyc =>
            {
                string info = lyc.ToDefaultLayer(compBasePage.pageNumb, compBasePage.pageType);
                //Debug.Log(info);
            });

            compBasePage.layerInfos.AddRange(lst);
        }        
    }

    public void SetInited()
    {
        isInit = true;
    }
}
