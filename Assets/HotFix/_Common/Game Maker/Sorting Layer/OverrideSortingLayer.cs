using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideSortingLayer : MonoBehaviour
{
    public virtual void UpdateSortingLayer() { }



    protected bool isUpdate = true;
    private void Start()
    {
        isUpdate = true;
    }
    private void OnEnable()
    {
        if (isUpdate)
            StartCoroutine("SortLayer");
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator SortLayer()
    {
        yield return null;

        UpdateSortingLayer();

        isUpdate = false;
    }

}
