using UnityEditor;
using UnityEngine;
using Game;


# if false
//[CanEditMultipleObjects] ѡ�������󣬶�������Ϲ�ͬ��������Կ��Ա༭
//[ExecuteInEditMode] �ø���ķ��������ڱ༭ģʽ��ֱ������
//[CustomEditor(typeof(BasePage))] //ֻ�Ը���������
[CustomEditor(typeof(BasePage),true)]
public class BasePageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BasePage script = target as BasePage; // ���ƹ�����

        Canvas canvas = script.transform.GetComponent<Canvas>();
        if (canvas != null && !canvas.overrideSorting)
        {
            switch (script.pageType)
            {
                case PageType.BasePage:
                case PageType.PopupPage:
                case PageType.OverlayPage:
                    canvas.overrideSorting = true;
                    canvas.sortingLayerName = "Base";
                    canvas.sortingOrder = 0;
                break;
            }
        }

        //Debug.Log($"name = {script.transform.root.name}");
    }
}

#endif