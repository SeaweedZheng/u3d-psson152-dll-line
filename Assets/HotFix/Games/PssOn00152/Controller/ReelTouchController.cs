using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ReelTouchController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void CheckTouch01()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                //画出射线，只有在scene视图中才能看到
                Debug.DrawLine(ray.origin, hit.point);
                GameObject gameObject = hit.collider.gameObject;
                if (gameObject.tag == "reel")
                {

                }
                DebugUtils.Log($"点击到了物体： {gameObject.name}");
            }
        }
    }

    void CheckTouch()
    {
        // 检测鼠标是否在滑动
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                DebugUtils.Log("Mouse hit: " + hit.transform.gameObject.name);
            }
        }

        // 检测触摸屏是否在滑动（仅限移动设备）
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Touch touch = Input.GetTouch(0);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
            {
                DebugUtils.Log("Touch hit: " + hit.transform.gameObject.name);
            }
        }
    }


    public float swipeThreshold = 50.0f; // 滑动的最小距离阈值

    private Vector2 touchStartPos;

    void Update()
    {
        //DebugUtil.Log("i am here!!!!");

        CheckTouch01();

        CheckTouch();

        // 检测鼠标滑动
        if (Input.GetMouseButtonDown(0))
        {
            // 记录鼠标按下时的位置
            touchStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            // 当鼠标抬起时计算滑动
            Vector2 touchEndPos = Input.mousePosition;
            Vector2 swipe = touchEndPos - touchStartPos;
            if (Mathf.Abs(swipe.x) > swipeThreshold)
            {
                // 水平滑动
                if (swipe.x > 0) DebugUtils.Log("Right Swipe");
                else DebugUtils.Log("Left Swipe");
            }
            else if (Mathf.Abs(swipe.y) > swipeThreshold)
            {
                // 垂直滑动
                if (swipe.y > 0) DebugUtils.Log("Down Swipe");
                else DebugUtils.Log("Up Swipe");
            }
        }

        // 检测触摸滑动
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // 记录触摸开始时的位置
                touchStartPos = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                // 当触摸结束时计算滑动
                Vector2 touchEndPos = touch.position;
                Vector2 swipe = touchEndPos - touchStartPos;
                if (Mathf.Abs(swipe.x) > swipeThreshold)
                {
                    // 水平滑动
                    if (swipe.x > 0) DebugUtils.Log("Right Swipe");
                    else DebugUtils.Log("Left Swipe");
                }
                else if (Mathf.Abs(swipe.y) > swipeThreshold)
                {
                    // 垂直滑动
                    if (swipe.y > 0) DebugUtils.Log("Down Swipe");
                    else DebugUtils.Log("Up Swipe");
                }
            }
        }
    }
}
