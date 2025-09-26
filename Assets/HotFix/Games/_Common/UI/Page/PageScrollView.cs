using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
public enum ScrllViewType
{
    Horizontal,
    Vertical
}
/// <summary>
/// ScorllView 改造为 PageScrollView
/// </summary>
public class PageScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    #region
    protected ScrollRect scrollRect;
    private int pageCount;
    /// <summary>每个页面的horizontalNormalizedPosition值 </summary>
    public float[] pageNormalizedPos;
    /// <summary>是否自动跳转 </summary>
    public bool isAu = true;
    /// <summary>自动跳转的时间间隔，以及计数器 </summary>
    public float auTime = 2;
    private float auTimer = 0;

    /// <summary>是否跳转 </summary>
    private bool isJump = false;
    /// <summary>当前的页面下标 </summary>
    private int _currentPageIndex;
    public int currentPageIndex
    {
        get => _currentPageIndex;
    }

    /// <summary>开始的页面位置 </summary>
    private float startHorizontal;
    #endregion
    /// <summary>跳转需要的时间 </summary>
    private float jumpTime = 0.3f;
    private float jumpTimer = 0;

    /// <summary>在拖拽的时候，不进行自动滚动 </summary>
    private bool isDraging = false;
    /// <summary>跳转到具体页面的时候，进行相应的事件回调 </summary>
    public UnityEvent<int> pageHandle;


    public ScrllViewType scrllViewType = ScrllViewType.Horizontal;

    protected virtual void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        if (scrollRect == null)
        {
            throw new System.Exception("没有该组件");
        }
        pageCount = transform.Find("Viewport/Content").childCount;
        pageNormalizedPos = new float[pageCount];

        switch (scrllViewType)
        {
            //对于水平移动来说,假设有三个页面，第一个的scrollRect.verticalNormalizedPosition是0，第二个是0.5，第三个是1
            case ScrllViewType.Horizontal:
                for (int i = 0; i < pageCount; i++)
                {
                    pageNormalizedPos[i] = i * 1.0f / (float)(pageCount - 1);
                }
                break;
            //对于垂直移动来说,假设有三个页面，第一个的scrollRect.verticalNormalizedPosition是1，第二个是0.5，第三个是0
            case ScrllViewType.Vertical:
                for (int i = 0; i < pageCount; i++)
                {
                    pageNormalizedPos[i] = 1 - i * 1.0f / (float)(pageCount - 1);
                }
                break;
        }

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ListenJump();
        ListenAntoJump();
    }

    /// <summary>
    /// 处理自动跳转逻辑
    /// </summary>
    void ListenAntoJump()
    {
        if (isDraging == true)
            return;
        if (isAu)
        {
            auTimer += Time.deltaTime;
            if (auTimer >= auTime)
            {
                auTimer = 0;
                _currentPageIndex++;
                _currentPageIndex %= pageCount;
                PageJump(_currentPageIndex);
            }
        }
    }


    public void PageJump(int pageIndex)
    {
        isJump = true;
        jumpTimer = 0;
        this._currentPageIndex = pageIndex;
        switch (scrllViewType)
        {
            case ScrllViewType.Horizontal:
                startHorizontal = scrollRect.horizontalNormalizedPosition;
                break;
            case ScrllViewType.Vertical:
                startHorizontal = scrollRect.verticalNormalizedPosition;
                break;
        }

        if (pageHandle != null)
            pageHandle.Invoke(_currentPageIndex);
    }
    void ListenJump()
    {
        if (isJump)
        {
            jumpTimer += Time.deltaTime * (1 / jumpTime);
            //通过线性插值的方式修改horizontalNormalizedPosition的值来达到移动的效果
            switch (scrllViewType)
            {
                case ScrllViewType.Horizontal:
                    scrollRect.horizontalNormalizedPosition = Mathf.Lerp(startHorizontal, pageNormalizedPos[_currentPageIndex], jumpTimer);
                    break;
                case ScrllViewType.Vertical:
                    scrollRect.verticalNormalizedPosition = Mathf.Lerp(startHorizontal, pageNormalizedPos[_currentPageIndex], jumpTimer);
                    break;
            }
            if (jumpTimer >= 1)
            {
                isJump = false;
            }
        }
    }
    //通过监听拖拽事件，跳转到松开手的时候离得最近的页面
    public void OnEndDrag(PointerEventData eventData)
    {
        int minpageindex = 0;
        //float nowhe = scrollRect.horizontalNormalizedPosition;
        switch (scrllViewType)
        {
            case ScrllViewType.Horizontal:
                for (int i = 1; i < pageCount; i++)
                {
                    if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - pageNormalizedPos[i]) < Mathf.Abs(pageNormalizedPos[minpageindex] - scrollRect.horizontalNormalizedPosition))
                    {
                        minpageindex = i;
                    }
                }
                break;
            case ScrllViewType.Vertical:
                for (int i = 1; i < pageCount; i++)
                {
                    if (Mathf.Abs(scrollRect.verticalNormalizedPosition - pageNormalizedPos[i]) < Mathf.Abs(pageNormalizedPos[minpageindex] - scrollRect.verticalNormalizedPosition))
                    {
                        minpageindex = i;
                    }
                }
                break;
        }

        PageJump(minpageindex);
        isDraging = false;
        //结束拖曳后，要过2s才会进行页面滚动
        auTimer = 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;
    }


    public void PageSet(int pageIndex)
    {
        isJump = false;
        this._currentPageIndex = pageIndex;
        switch (scrllViewType)
        {
            case ScrllViewType.Horizontal:
                scrollRect.horizontalNormalizedPosition = pageNormalizedPos[_currentPageIndex];
                break;
            case ScrllViewType.Vertical:
                scrollRect.verticalNormalizedPosition = pageNormalizedPos[_currentPageIndex];
                break;
        }

        if (pageHandle != null)
            pageHandle.Invoke(_currentPageIndex);
    }


    [Button]
    void TestPageJump(int pageIndex)
    {
        PageJump(pageIndex);
    }

    [Button]
    void TestPageSet(int pageIndex)
    {
        PageSet(pageIndex);
    }
}
