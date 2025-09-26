#if UNITY_EDITOR
using UnityEditor;

namespace GameMaker
{
    [CustomEditor(typeof(PIDToggle), true)]
    [CanEditMultipleObjects]
    public class PIDToggleEditor : UnityEditor.UI.ToggleEditor
    {
        SerializedProperty m_onInteractableChanged;
        SerializedProperty m_onDisable;
        SerializedProperty m_disableCover;

        SerializedProperty m_OnTrigger;
        SerializedProperty m_OffTrigger;
        SerializedProperty m_goOn;
        SerializedProperty m_goOff;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_onInteractableChanged = serializedObject.FindProperty("onInteractableChanged");
            m_onDisable = serializedObject.FindProperty("onDisable");
            m_disableCover = serializedObject.FindProperty("disableCover");
            m_OnTrigger = serializedObject.FindProperty("m_OnTrigger");
            m_OffTrigger = serializedObject.FindProperty("m_OffTrigger");
            m_goOn = serializedObject.FindProperty("goOn");
            m_goOff = serializedObject.FindProperty("goOff");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_onInteractableChanged);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_onDisable);
            EditorGUILayout.PropertyField(m_disableCover);
            EditorGUILayout.PropertyField(m_OnTrigger);
            EditorGUILayout.PropertyField(m_OffTrigger);
            EditorGUILayout.PropertyField(m_goOn);
            EditorGUILayout.PropertyField(m_goOff);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif