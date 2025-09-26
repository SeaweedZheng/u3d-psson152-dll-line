using System.Collections.Generic;
using UnityEngine;

public class LineRendererDraw : MonoBehaviour
{
    private LineRenderer clone;
    public LineRenderer linePre;
    private int positionCount;
    private Material lineMaterial;

    private void Start()
    {
        lineMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        linePre.material = lineMaterial;
    }

    /// <summary>
    /// 创建线条
    /// </summary>
    /// <returns></returns>
    private LineRenderer CreateLine()
    {
        //实例化对象
        LineRenderer line = Instantiate(linePre, linePre.transform.position, Quaternion.identity);



        #region 设置层级
        //设置类别
        // 获取UI层的索引，通常UI层命名为"UI"
        int uiLayer = LayerMask.NameToLayer("UI");
        // 设置游戏对象的层为UI层
        line.gameObject.layer = uiLayer;

        // 设置层级
        line.sortingLayerName = "Base";
        line.sortingOrder = 5;

        linesCacher.Add(line.gameObject);
        #endregion




        //设置起始和结束的颜色
        line.startColor = Color.red;
        line.endColor = Color.blue;

        //设置起始和结束的宽度
        //line.startWidth = 0.4f;
        //line.endWidth = 0.35f;
        line.startWidth = 0.2f;
        line.endWidth = 0.15f;
        return line;
    }

    List<GameObject> linesCacher = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //实例化对象
            clone = CreateLine();


            //计数
            positionCount = 0;
        }
        if (Input.GetMouseButton(0))
        {
            //每一帧检测，按下鼠标的时间越长，计数越多
            positionCount++;

            //设置顶点数
            clone.positionCount = positionCount;

            //设置顶点位置(顶点的索引，将鼠标点击的屏幕坐标转换为世界坐标)
            clone.SetPosition(positionCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15)));
        }

        if (Input.GetMouseButtonUp(0))
        {
            linesCacher.Remove(clone.gameObject);
            Destroy(clone.gameObject);
        }
    }

    private void OnDestroy()
    {
        while (linesCacher.Count > 0)
        {
            GameObject line = linesCacher[0];
            linesCacher.RemoveAt(0);
            Destroy(line);
        }
    }
}

