using UnityEngine;


/// <summary>
/// ƽ��滭�߶�
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class MouseLineRenderer : MonoBehaviour
{
    /*
    [MenuItem("Window/Seaweed/Mouse Line Renderer")]
    public static void Init()
    {
        // �򿪴���
        DragMiniBeizerListener window = (DragMiniBeizerListener)EditorWindow.GetWindow(typeof(DragMiniBeizerListener));
        window.Show();
    }*/


    //LineRenderer
    private LineRenderer lineRenderer;
    //����һ��Vector3,�����洢�������λ��
    private Vector3 position;
    //���������˵�
    private int index = 0;
    //�˵���
    private int LengthOfLineRenderer = 0;

    void Start()
    {
        //���LineRenderer���
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        //���ò���
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        //������ɫ
        lineRenderer.SetColors(Color.red, Color.yellow);
        //���ÿ��
        lineRenderer.SetWidth(0.02f, 0.02f);

    }

    // Update is called once per frame
    void Update()
    {
        //��ȡLineRenderer���
        lineRenderer = GetComponent<LineRenderer>();
        //������
        if (Input.GetMouseButtonDown(0))
        {
            //�����������Ļ����ת��Ϊ�������꣬Ȼ��洢��position��
            position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
            //�˵���+1
            LengthOfLineRenderer++;
            //�����߶εĶ˵���
            lineRenderer.SetVertexCount(LengthOfLineRenderer);

        }
        //���������߶�
        while (index < LengthOfLineRenderer)
        {
            //����ȷ��һ��ֱ�ߣ������������λ��Ƶ�Ϳ����γ��߶���
            lineRenderer.SetPosition(index, position);
            index++;
        }
    }

    void OnGUI()
    {
        GUILayout.Label("��ǰ���X��λ�ã�" + Input.mousePosition.x);
        GUILayout.Label("��ǰ���Y��λ�ã�" + Input.mousePosition.y);
    }
}
