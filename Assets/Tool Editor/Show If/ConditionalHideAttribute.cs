#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


/// <summary>
/// 
/// </summary>
/// <remarks>
/// * �������������������(�� Sirenix.Odinlnspector.ShowlfAttribute)�������Զ��� PropertyDrawer ���������Ե���ʾ�߼���
/// </remarks>
public class ConditionalHideAttribute : PropertyAttribute
{
    public string conditionName;
    public bool invert = false; // ��ѡ����ת����

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
        return true; // ��������ֶβ����ڣ�Ĭ����ʾ
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
/// ʹ�ð���
/// </summary>
public class Example : MonoBehaviour
{
    public bool showField;

    [ConditionalHide("showField")] // ���� showField Ϊ true ʱ��ʾ
    public Vector3 position;
}