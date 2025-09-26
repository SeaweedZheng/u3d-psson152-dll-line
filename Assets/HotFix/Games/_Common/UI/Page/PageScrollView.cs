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
/// ScorllView ����Ϊ PageScrollView
/// </summary>
public class PageScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    #region
    protected ScrollRect scrollRect;
    private int pageCount;
    /// <summary>ÿ��ҳ���horizontalNormalizedPositionֵ </summary>
    public float[] pageNormalizedPos;
    /// <summary>�Ƿ��Զ���ת </summary>
    public bool isAu = true;
    /// <summary>�Զ���ת��ʱ�������Լ������� </summary>
    public float auTime = 2;
    private float auTimer = 0;

    /// <summary>�Ƿ���ת </summary>
    private bool isJump = false;
    /// <summary>��ǰ��ҳ���±� </summary>
    private int _currentPageIndex;
    public int currentPageIndex
    {
        get => _currentPageIndex;
    }

    /// <summary>��ʼ��ҳ��λ�� </summary>
    private float startHorizontal;
    #endregion
    /// <summary>��ת��Ҫ��ʱ�� </summary>
    private float jumpTime = 0.3f;
    private float jumpTimer = 0;

    /// <summary>����ק��ʱ�򣬲������Զ����� </summary>
    private bool isDraging = false;
    /// <summary>��ת������ҳ���ʱ�򣬽�����Ӧ���¼��ص� </summary>
    public UnityEvent<int> pageHandle;


    public ScrllViewType scrllViewType = ScrllViewType.Horizontal;

    protected virtual void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        if (scrollRect == null)
        {
            throw new System.Exception("û�и����");
        }
        pageCount = transform.Find("Viewport/Content").childCount;
        pageNormalizedPos = new float[pageCount];

        switch (scrllViewType)
        {
            //����ˮƽ�ƶ���˵,����������ҳ�棬��һ����scrollRect.verticalNormalizedPosition��0���ڶ�����0.5����������1
            case ScrllViewType.Horizontal:
                for (int i = 0; i < pageCount; i++)
                {
                    pageNormalizedPos[i] = i * 1.0f / (float)(pageCount - 1);
                }
                break;
            //���ڴ�ֱ�ƶ���˵,����������ҳ�棬��һ����scrollRect.verticalNormalizedPosition��1���ڶ�����0.5����������0
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
    /// �����Զ���ת�߼�
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
            //ͨ�����Բ�ֵ�ķ�ʽ�޸�horizontalNormalizedPosition��ֵ���ﵽ�ƶ���Ч��
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
    //ͨ��������ק�¼�����ת���ɿ��ֵ�ʱ����������ҳ��
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
        //������ҷ��Ҫ��2s�Ż����ҳ�����
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
