using UnityEngine;

public class CanvasInheritance : MonoBehaviour
{
    public Canvas canvas;

    void Start()
    {


        // Ĭ������£�Inherit���������õ�
        canvas.overrideSorting = true; // ����ȷ�����ǿ��Ը�������
        //canvas.
        //canvas.inheritSorting = false; // ���ü̳У��Ա����ǿ����Զ���������˳��

        // ����Canvas������������˳��

        canvas.sortingLayerID = LayerMask.NameToLayer("NewSortingLayer");
        //canvas.sortingLayerID = LayerMask.NameToID("NewSortingLayer"); // ʹ�ò��������������
        canvas.sortingOrder = 10; // ��������˳��
    }
}