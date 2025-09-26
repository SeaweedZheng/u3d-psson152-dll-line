using Game;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLayerController : MonoBehaviour
{
    List<Renderer> SpineOrParticleSystemLst = new List<Renderer>();
    private void OnEnable()
    {
        if (SpineOrParticleSystemLst.Count <0)
        {
            Renderer red = transform.GetComponent<Renderer>();
            if (red != null)
            {
                SpineOrParticleSystemLst.Add(red);
            }
            List<Renderer> redLst = UnityUtil.GetComponentsInChildrenIncludeDisactive<Renderer>(transform);
            SpineOrParticleSystemLst.AddRange(redLst);
        }
        GetPageNode();
        if (basePage != null 
            && PageManager.Instance.IndexOf(basePage) != 0
            && SpineOrParticleSystemLst[0].sortingLayerName != LayerController.STACK_PAGE)
        {

        }
    }

    private void OnDisable()
    {
        
    }


    PageBase basePage = null;
    void GetPageNode()
    {
        Transform tfm = transform;
        int j = 50;
        while(--j > 0){

            tfm = tfm.parent;
            if (tfm == null)
                break;

            basePage = tfm.GetComponent<PageBase>();
            if (basePage != null)
                break;
        }
    }
}
