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
                //�������ߣ�ֻ����scene��ͼ�в��ܿ���
                Debug.DrawLine(ray.origin, hit.point);
                GameObject gameObject = hit.collider.gameObject;
                if (gameObject.tag == "reel")
                {

                }
                DebugUtils.Log($"����������壺 {gameObject.name}");
            }
        }
    }

    void CheckTouch()
    {
        // �������Ƿ��ڻ���
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                DebugUtils.Log("Mouse hit: " + hit.transform.gameObject.name);
            }
        }

        // ��ⴥ�����Ƿ��ڻ����������ƶ��豸��
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


    public float swipeThreshold = 50.0f; // ��������С������ֵ

    private Vector2 touchStartPos;

    void Update()
    {
        //DebugUtil.Log("i am here!!!!");

        CheckTouch01();

        CheckTouch();

        // �����껬��
        if (Input.GetMouseButtonDown(0))
        {
            // ��¼��갴��ʱ��λ��
            touchStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            // �����̧��ʱ���㻬��
            Vector2 touchEndPos = Input.mousePosition;
            Vector2 swipe = touchEndPos - touchStartPos;
            if (Mathf.Abs(swipe.x) > swipeThreshold)
            {
                // ˮƽ����
                if (swipe.x > 0) DebugUtils.Log("Right Swipe");
                else DebugUtils.Log("Left Swipe");
            }
            else if (Mathf.Abs(swipe.y) > swipeThreshold)
            {
                // ��ֱ����
                if (swipe.y > 0) DebugUtils.Log("Down Swipe");
                else DebugUtils.Log("Up Swipe");
            }
        }

        // ��ⴥ������
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // ��¼������ʼʱ��λ��
                touchStartPos = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                // ����������ʱ���㻬��
                Vector2 touchEndPos = touch.position;
                Vector2 swipe = touchEndPos - touchStartPos;
                if (Mathf.Abs(swipe.x) > swipeThreshold)
                {
                    // ˮƽ����
                    if (swipe.x > 0) DebugUtils.Log("Right Swipe");
                    else DebugUtils.Log("Left Swipe");
                }
                else if (Mathf.Abs(swipe.y) > swipeThreshold)
                {
                    // ��ֱ����
                    if (swipe.y > 0) DebugUtils.Log("Down Swipe");
                    else DebugUtils.Log("Up Swipe");
                }
            }
        }
    }
}
