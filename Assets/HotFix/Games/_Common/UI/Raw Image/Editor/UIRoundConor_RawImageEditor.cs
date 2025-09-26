#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UIRoundConor_RawImage), true)]
public class UIRoundConor_RawImageEditor : RawImageEditor
{
    SerializedProperty isLockCorner;
    SerializedProperty corner4;
    SerializedProperty corner;
    SerializedProperty borderWidth;
    SerializedProperty borderColor;
    SerializedProperty centerColor;
    protected virtual void OnEnable()
    {
        base.OnEnable();
        isLockCorner = serializedObject.FindProperty("m_IsLockCorner");
        corner4 = serializedObject.FindProperty("m_Corner4");
        corner = serializedObject.FindProperty("m_Corner");
        borderWidth = serializedObject.FindProperty("m_BorderWidth");
        borderColor = serializedObject.FindProperty("m_BorderColor");
        centerColor = serializedObject.FindProperty("m_CenterColor");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        isLockCorner.boolValue = EditorGUILayout.Toggle(new GUIContent("Բ����ͬ��С", "�������нǵĴ�С�Ƿ�һ��"), isLockCorner.boolValue);
        if (isLockCorner.boolValue)
        {
            corner.floatValue = EditorGUILayout.FloatField(new GUIContent("Բ�Ǵ�С", "�������нǵĴ�С"), corner.floatValue);
        }
        else
        {

            var cornerValue = corner4.vector4Value;

            var cacheLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 70;
            EditorGUILayout.BeginHorizontal();
            cornerValue.x = EditorGUILayout.FloatField("���Ͻ�", cornerValue.x);
            EditorGUILayout.Space();
            cornerValue.y = EditorGUILayout.FloatField("���Ͻ�", cornerValue.y);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            cornerValue.w = EditorGUILayout.FloatField("���½�", cornerValue.w);
            EditorGUILayout.Space();
            cornerValue.z = EditorGUILayout.FloatField("���½�", cornerValue.z);
            EditorGUILayout.EndHorizontal();

            corner4.vector4Value = cornerValue;

            EditorGUIUtility.labelWidth = cacheLabelWidth;
        }
        centerColor.colorValue = EditorGUILayout.ColorField(new GUIContent("������ɫ"), centerColor.colorValue);
        borderWidth.floatValue = EditorGUILayout.FloatField(new GUIContent("��߿��"), borderWidth.floatValue);
        borderColor.colorValue = EditorGUILayout.ColorField(new GUIContent("�����ɫ"), borderColor.colorValue);
        serializedObject.ApplyModifiedProperties();
    }
    [MenuItem("GameObject/UI/RoundCornor Raw Image", false, 2002)]//MenuOptionsPriorityOrder.RawImage
    public static void CreateGameObject()
    {
        var parent = GetCanvasGameObject();
        if (parent != null)
        {
            var go = new GameObject("UIRoundConor_RawImage");
            go.transform.SetParent(parent.transform, false);
            go.AddComponent<RectTransform>();
            go.AddComponent<CanvasRenderer>();
            go.AddComponent<UIRoundConor_RawImage>();
            Selection.activeGameObject = go;
        }
        else
        {
            EditorUtility.DisplayDialog("UIRoundConor_RawImage",
                "��ѡ��һ�������µĶ���", "Ok");
        }
    }
    public static GameObject GetCanvasGameObject()
    {
        GameObject selectedGo = Selection.activeGameObject;

        Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
        if (IsValidCanvas(canvas))
            return selectedGo.gameObject;

        Canvas[] canvasArray = StageUtility.GetCurrentStageHandle().FindComponentsOfType<Canvas>();
        for (int i = 0; i < canvasArray.Length; i++)
            if (IsValidCanvas(canvasArray[i]))
                return canvasArray[i].gameObject;

        return null;
    }
    static bool IsValidCanvas(Canvas canvas)
    {
        if (canvas == null || !canvas.gameObject.activeInHierarchy)
            return false;

        if (EditorUtility.IsPersistent(canvas) || (canvas.hideFlags & HideFlags.HideInHierarchy) != 0)
            return false;

        return StageUtility.GetStageHandle(canvas.gameObject) == StageUtility.GetCurrentStageHandle();
    }
}
#endif