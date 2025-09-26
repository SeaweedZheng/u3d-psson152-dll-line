#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


/// <summary>
/// 
/// </summary>
/// <remarks>
/// * 如果不想依赖第三方库(如 Sirenix.Odinlnspector.ShowlfAttribute)，可以自定义 PropertyDrawer 来控制属性的显示逻辑。
/// </remarks>
public class ConditionalHideAttribute : PropertyAttribute
{
    public string conditionName;
    public bool invert = false; // 可选：反转条件

    public ConditionalHideAttribute(string conditionName)
    {
        this.conditionName = conditionName;
    }
}


#if UNITY_EDITOR

 
[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHideDrawer : PropertyDrawer
{
    private bool ShouldShow(SerializedProperty property)
    {
        var conditionName = (attribute as ConditionalHideAttribute).conditionName;
        var targetObj = property.serializedObject.targetObject;
        var field = targetObj.GetType().GetField(conditionName);

        if (field != null)
        {
            bool conditionValue = (bool)field.GetValue(targetObj);
            return (attribute as ConditionalHideAttribute).invert ? !conditionValue : conditionValue;
        }
        return true; // 如果条件字段不存在，默认显示
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property))
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ShouldShow(property) ? EditorGUI.GetPropertyHeight(property) : 0f;
    }
}

#endif

/// <summary>
/// 使用案例
/// </summary>
public class Example : MonoBehaviour
{
    public bool showField;

    [ConditionalHide("showField")] // 仅当 showField 为 true 时显示
    public Vector3 position;
}