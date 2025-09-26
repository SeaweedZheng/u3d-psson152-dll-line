using Game;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerController
{



    public const string Default = "Default";

    public const string STACK_PAGE = "STACK PAGE";

    public const string Background = "Background";
    public const string Base = "Base";
    public const string Midground = "Midground";
    public const string Foreground = "Foreground";
    public const string EffectMidground = "Effect Midground";
    public const string EffectForeground = "Effect Foreground";
    public const string PanelBase = "Panel Base";
    public const string PanelMidground = "Panel Midground";
    public const string PanelForeground = "Panel Foreground";
    public const string Popup = "Popup";
    public const string Interaction = "Interaction";
    public const string Overlay = "Overlay";

    /// <summary>
    /// OrderInLayer取值范围 ： -32768 ~ 32767    （65,535）
    /// 
    /// PageType - PageIndex - SortingLayer - OrderInLayer ：（00000 - 64999）
    /// [1 - 3, 第5位][0 - 9, 第4位][00 - 99，第3、2位][0 - 9, 第1位]
    /// 
    /// </summary>
    public static readonly Dictionary<string, int> layerValue = new Dictionary<string, int>
    {
        [Default] = 0,

        [Background] = 0, //pageType = Base
        [Base] = 10,
        [Midground] = 20,
        [Foreground] = 30,
        [EffectMidground] = 40,
        [EffectForeground] = 50,
        [PanelBase] = 60,
        [PanelMidground] = 70,
        [PanelForeground] = 80,
        [Popup] = 90,
        [Interaction] = 100,
        [Overlay] = 110,
    };


    public string sortingLayerName;
    public int sortingOrder;
    //public int sortingLayerID;

    public Canvas canvas;

    public SpriteRenderer spriteRenderer;

    public MeshRenderer meshRenderer; 
    //public SkeletonMecanim skeletonMecanim;
    //public SkeletonAnimation skeletonAnimation;

    public Renderer renderer;
    //public ParticleSystem particleSystem;
   // public LineRenderer lineRenderer;

    private void SetLayer(string sortingLayerName , int sortingOrder)
    {
        if (canvas != null)
        {
            canvas.sortingLayerName = sortingLayerName;
            canvas.sortingOrder = sortingOrder;
        }
        else if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = sortingLayerName;
            spriteRenderer.sortingOrder = sortingOrder;
        }
        else if (meshRenderer != null)
        {
            meshRenderer.sortingLayerName = sortingLayerName;
            meshRenderer.sortingOrder = sortingOrder;
        }
        else if (renderer != null)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = sortingOrder;
        }

        //	static readonly GUIContent SortingLayerLabel = new GUIContent("Sorting Layer", "MeshRenderer.sortingLayerID");
        // static readonly GUIContent OrderInLayerLabel = new GUIContent("Order in Layer", "MeshRenderer.sortingOrder");
    }

    public int GetLayerID()
    {
        return SortingLayer.NameToID(sortingLayerName);
    }


    private string GetName()
    {
        Transform tfm = null;
        if (canvas != null)
        {
            tfm = canvas.transform;
        }
        else if (spriteRenderer != null)
        {
            tfm = spriteRenderer.transform;
        }
        else if (meshRenderer != null)
        {
            tfm = meshRenderer.transform;
        }

        if (tfm == null)
            return "no find !";

        string name = tfm.name;
        int i = 3;
        while (--i>0)
        {
            if(tfm.parent != null)
            {
                tfm = tfm.parent;
                name = $"{tfm.name}/{name}";
            }
            else
            {
                break;
            }
        }
        return name;
    }

    /*static SetLayer(Action<string,int> callBack)
    {
        int pageVal = GetPageValue(pageType, pageNumb);
        int order = pageVal + layerValue[sortingLayerName] + sortingOrder - 32768;

    }*/
    public string ToDefaultLayer(int pageNumb,PageType pageType)
    {
        //int pageIndex = transform.GetSiblingIndex();

        string res = "";
#if !USE_STACK_PAGE

        if (sortingLayerName == Default)
        {
            return res;
        }
        else
        {

            if (!layerValue.ContainsKey(sortingLayerName)) {  //STACK PAGE
                Debug.LogWarning($"Error : GameObject({GetName()})  LayerName({sortingLayerName}) not in dict ");
                return "";
            }

            if (sortingOrder < 0 || sortingOrder > 9)
                Debug.LogError($"Error : GameObject({GetName()})  sortingOrder must between 0 - 9");

            if (pageNumb < 0 || pageNumb > 9)
                Debug.LogError($"Error : GameObject({GetName()})  pageIndex must between 0 - 9");

            int pageVal = GetPageValue(pageType, pageNumb);
            int order = pageVal + layerValue[sortingLayerName] + sortingOrder - 32768;
            res = $"pageType:{pageType}#pageNumb:{pageNumb}#pageVal:{pageVal}#sortingLayerName:{sortingLayerName}#layerValue:{layerValue[sortingLayerName]}#sortingOrder:{sortingOrder}#targetOrder:{order}#targetLayer:{STACK_PAGE}";
            SetLayer(STACK_PAGE, order);
            return res;
        }
#else
        if (sortingLayerName == Default)
        {
            int order = sortingOrder - 32768;
            res = $"pageType:{pageType}#pageNumb:{pageNumb}#sortingLayerName:{Default}#sortingOrder:{sortingOrder}#targetOrder:{order}#targetLayer:{Default}";
            SetLayer(Default, order);
            return res;
        }
        else
        {
            if (sortingOrder < 0 || sortingOrder > 9)
                Debug.LogError($"Error : GameObject({GetName()})  sortingOrder must between 0 - 9");

            if (pageNumb < 0 || pageNumb > 9)
                Debug.LogError($"Error : GameObject({GetName()})  pageIndex must between 0 - 9");

            int pageVal = GetPageValue(pageType, pageNumb);
            int order = pageVal + layerValue[sortingLayerName] + sortingOrder - 32768;

            res = $"pageType:{pageType}#pageNumb:{pageNumb}#pageVal:{pageVal}#sortingLayerName:{sortingLayerName}#layerValue:{layerValue[sortingLayerName]}#sortingOrder:{sortingOrder}#targetOrder:{order}#targetLayer:{STACK_PAGE}";
            SetLayer(Default, order);
            return res;
        }

#endif

    }

    public static int GetPageValue(PageType pageType , int pageNumb)
    {
        int pageTypeVal = 0;
        switch (pageType)
        {
            case PageType.BasePage:
                pageTypeVal =  10000;
                break;
            case PageType.PopupPage:
                pageTypeVal =  20000;
                break;
            case PageType.OverlayPage:
                pageTypeVal = 30000;
                break;
        }
        return pageTypeVal + pageNumb * 1000;
    }

    public void ToOwnerLayer()
    {
        SetLayer(sortingLayerName, sortingOrder);
    }

    public static List<LayerController> GreatLayerInfos(Transform parent)
    {

        List<LayerController> layerInfos = new List<LayerController>();


        #region owner
        // 自己的相机
        Canvas canvas = parent.GetComponent<Canvas>();
        if (canvas != null)
        {
            layerInfos.Add(new LayerController
            {
                canvas = canvas,
                sortingLayerName = canvas.sortingLayerName,
                sortingOrder = canvas.sortingOrder,
            });
        }

        SpriteRenderer sr = parent.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            layerInfos.Add(new LayerController
            {
                spriteRenderer = sr,
                sortingLayerName = canvas.sortingLayerName,
                sortingOrder = canvas.sortingOrder,
            });
        }

        Renderer rd = parent.GetComponent<Renderer>();
        if (rd != null)
        {
            layerInfos.Add(new LayerController
            {
                renderer = rd,
                sortingLayerName = canvas.sortingLayerName,
                sortingOrder = canvas.sortingOrder,
            });
        }

        #endregion





        #region 后代-Canvas
        List<Canvas> canvasLst = UnityUtil.GetComponentsInChildrenIncludeDisactive<Canvas>(parent);
        //Canvas[] canvasLst = toStack.gameObject.GetComponentsInChildren<Canvas>();
        foreach (Canvas comp in canvasLst)
        {
            layerInfos.Add(new LayerController
            {
                canvas = comp,
                sortingLayerName = comp.sortingLayerName,
                sortingOrder = comp.sortingOrder,
            });
        }
        #endregion

        #region 后代-SpriteRenderer
        List<SpriteRenderer> srLst = UnityUtil.GetComponentsInChildrenIncludeDisactive<SpriteRenderer>(parent);
        //SpriteRenderer[] srLst = toStack.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer comp in srLst)
        {
            layerInfos.Add(new LayerController
            {
                spriteRenderer = comp,
                sortingLayerName = comp.sortingLayerName,
                sortingOrder = comp.sortingOrder,
            });
        }
        #endregion

        #region 后代-Spine
        List<MeshRenderer> mrLst = UnityUtil.GetComponentsInChildrenIncludeDisactive<MeshRenderer>(parent);
        //MeshRenderer[] mrLst = toStack.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer comp in mrLst)
        {
            layerInfos.Add(new LayerController
            {
                meshRenderer = comp,
                sortingLayerName = comp.sortingLayerName,
                sortingOrder = comp.sortingOrder,
            });
        }
        #endregion

        #region 后代-ParticleSystem
        List<ParticleSystem> psysLst = UnityUtil.GetComponentsInChildrenIncludeDisactive<ParticleSystem>(parent);

        foreach (ParticleSystem comp in psysLst)
        {
            Renderer red = comp.transform.GetComponent<Renderer>();
           layerInfos.Add(new LayerController
            {
                renderer = red,
                sortingLayerName = red.sortingLayerName,
                sortingOrder = red.sortingOrder,
            });
        }
        #endregion

        #region LineRenderer
        List<LineRenderer> lrLst = UnityUtil.GetComponentsInChildrenIncludeDisactive<LineRenderer>(parent);

        foreach (LineRenderer comp in lrLst)
        {
            layerInfos.Add(new LayerController
            {
                renderer = comp,
                sortingLayerName = comp.sortingLayerName,
                sortingOrder = comp.sortingOrder,
            });
        }
        #endregion

        #region 后代-Renderer (Spine Mecanim、ParticleSystem、 Line Renderer)

        /*
        List<Renderer> redLst = UnityUtil.GetComponentsInChildrenIncludeDisactive<Renderer>(parent);
        foreach (Renderer comp in redLst)
        {
            Debug.Log($" Renderer name = {comp.name}");
        }*/

        #endregion


        List<LayerInit>  lyInfoLst = UnityUtil.GetComponentsInChildrenIncludeDisactive<LayerInit>(parent);
        foreach (LayerInit comp in lyInfoLst)
        {
            comp.SetInited();
        }


        return layerInfos;
    }
}