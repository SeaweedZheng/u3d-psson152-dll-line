using UnityEngine.UI;
using UnityEditor;
using TMPro;

/*
TMP_BaseEditorPanel.cs
TMP_EditorPanel.cs
TMP_EditorPanelUI.cs

In version 2.0.1, it is TMP_UIEditorPanel.cs.
In the next release is had been renamed TMP_EditorPanelUI.cs.
*/

/*
namespace GameMaker
{
    [CustomEditor(typeof(PIDTextMeshProUGUI), true)]
    [CanEditMultipleObjects]
    public class PIDTextMeshProUGUIEditor : TMPro.TextMeshProUGUIEditor
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
*/