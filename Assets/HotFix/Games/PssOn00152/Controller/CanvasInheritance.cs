using UnityEngine;

public class CanvasInheritance : MonoBehaviour
{
    public Canvas canvas;

    void Start()
    {


        // 默认情况下，Inherit属性是启用的
        canvas.overrideSorting = true; // 首先确保我们可以覆盖排序
        //canvas.
        //canvas.inheritSorting = false; // 禁用继承，以便我们可以自定义排序层和顺序

        // 设置Canvas的排序层和排序顺序

        canvas.sortingLayerID = LayerMask.NameToLayer("NewSortingLayer");
        //canvas.sortingLayerID = LayerMask.NameToID("NewSortingLayer"); // 使用层名称设置排序层
        canvas.sortingOrder = 10; // 设置排序顺序
    }
}