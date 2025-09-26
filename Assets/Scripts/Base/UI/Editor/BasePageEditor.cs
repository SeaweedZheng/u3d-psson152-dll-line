using UnityEditor;
using UnityEngine;
using Game;


# if false
//[CanEditMultipleObjects] 选择多个对象，多个对象上共同的组件属性可以编辑
//[ExecuteInEditMode] 让该类的方法可以在编辑模式下直接运行
//[CustomEditor(typeof(BasePage))] //只对父类起作用
[CustomEditor(typeof(BasePage),true)]
public class BasePageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BasePage script = target as BasePage; // 绘制滚动条

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