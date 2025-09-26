
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class ConditionalShowAttribute : PropertyAttribute
{
    public string conditionFieldName; // Ҫ�Ƚϵ��ֶ���
    public object expectedValue;      // ������ֵ���������������ͣ�
    public bool invert = false;       // �Ƿ�ת����

    public ConditionalShowAttribute(string conditionFieldName, object expectedValue)
    {
        this.conditionFieldName = conditionFieldName;
        this.expectedValue = expectedValue;
    }
}


#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(ConditionalShowAttribute))]
public class ConditionalShowDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attribute = (ConditionalShowAttribute)this.attribute;
        var targetObj = property.serializedObject.targetObject;
        var conditionField = targetObj.GetType().GetField(attribute.conditionFieldName);

        if (conditionField != null)
        {
            var conditionValue = conditionField.GetValue(targetObj);
            bool shouldShow = object.Equals(conditionValue, attribute.expectedValue);

            if (attribute.invert) shouldShow = !shouldShow;

            if (shouldShow)
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attribute = (ConditionalShowAttribute)this.attribute;
        var targetObj = property.serializedObject.targetObject;
        var conditionField = targetObj.GetType().GetField(attribute.conditionFieldName);

        if (conditionField != null)
        {
            var conditionValue = conditionField.GetValue(targetObj);
            bool shouldShow = object.Equals(conditionValue, attribute.expectedValue);

            if (attribute.invert) shouldShow = !shouldShow;

            return shouldShow ? EditorGUI.GetPropertyHeight(property) : 0f;
        }
        return EditorGUI.GetPropertyHeight(property);
    }
}


#endif

public class Example01 : MonoBehaviour
{
    public enum Mode { A, B, C }
    public Mode currentMode;

    [ConditionalShow("currentMode", Mode.B)] // ���� currentMode == Mode.B ʱ��ʾ
    public Vector3 positionWhenModeB;

    public int threshold = 5;

    [ConditionalShow("threshold", 10)] // ���� threshold == 10 ʱ��ʾ
    public string message;

    public string password = "secret";

    [ConditionalShow("password", "admin")] // ���� password == "admin" ʱ��ʾ
    public bool isAdmin;
}