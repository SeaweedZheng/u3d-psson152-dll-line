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
    /// ��������
    /// </summary>
    /// <returns></returns>
    private LineRenderer CreateLine()
    {
        //ʵ��������
        LineRenderer line = Instantiate(linePre, linePre.transform.position, Quaternion.identity);



        #region ���ò㼶
        //�������
        // ��ȡUI���������ͨ��UI������Ϊ"UI"
        int uiLayer = LayerMask.NameToLayer("UI");
        // ������Ϸ����Ĳ�ΪUI��
        line.gameObject.layer = uiLayer;

        // ���ò㼶
        line.sortingLayerName = "Base";
        line.sortingOrder = 5;

        linesCacher.Add(line.gameObject);
        #endregion




        //������ʼ�ͽ�������ɫ
        line.startColor = Color.red;
        line.endColor = Color.blue;

        //������ʼ�ͽ����Ŀ��
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
            //ʵ��������
            clone = CreateLine();


            //����
            positionCount = 0;
        }
        if (Input.GetMouseButton(0))
        {
            //ÿһ֡��⣬��������ʱ��Խ��������Խ��
            positionCount++;

            //���ö�����
            clone.positionCount = positionCount;

            //���ö���λ��(����������������������Ļ����ת��Ϊ��������)
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

