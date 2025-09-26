#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Animations;

[CustomEditor(typeof(ToggleAnimatorTrigger))]
public class ToggleAnimatorTriggerEditor : Editor
{
    SerializedProperty m_OnTriggerProperty;
    SerializedProperty m_OffTriggerProperty;


    bool m_IsOn = false;

    void OnEnable()
    {
        m_OnTriggerProperty = serializedObject.FindProperty("m_OnTrigger");
        m_OffTriggerProperty = serializedObject.FindProperty("m_OffTrigger");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var go = (target as ToggleAnimatorTrigger).gameObject;
        var animator = go.GetComponent<Animator>();

        EditorGUILayout.LabelField("Transition", "Animation");
        ++EditorGUI.indentLevel;
        {
            EditorGUILayout.PropertyField(m_OnTriggerProperty);
            EditorGUILayout.PropertyField(m_OffTriggerProperty);

            if (animator == null || animator.runtimeAnimatorController == null)
            {
                Rect buttonRect = EditorGUILayout.GetControlRect();
                buttonRect.xMin += EditorGUIUtility.labelWidth;
                if (GUI.Button(buttonRect, "Auto Generate Animation", EditorStyles.miniButton))
                {
                    var controller = GenerateSelectableAnimatorContoller(m_OnTriggerProperty.stringValue,
                        m_OffTriggerProperty.stringValue, go);
                    if (controller != null)
                    {
                        if (animator == null)
                            animator = go.AddComponent<Animator>();

                        AnimatorController.SetAnimatorController(animator, controller);
                    }
                }
            }

        }
        --EditorGUI.indentLevel;

        /*ToggleAnimatorTrigger script = (ToggleAnimatorTrigger)target;
        Toggle tg = go.GetComponent<Toggle>();
        if (tg != null&& m_IsOn != tg.isOn)
        {
            m_IsOn = tg.isOn;
            Animator ator = go.GetComponentInChildren<Animator>();
            ator.ResetTrigger(script.onTaigger);
            ator.ResetTrigger(script.offTrigger);
            ator.SetTrigger(m_IsOn ? script.onTaigger : script.offTrigger);

            ator.Play(m_IsOn ? script.onTaigger : script.offTrigger);
        }*/


        serializedObject.ApplyModifiedProperties();
    }

    private static AnimatorController GenerateSelectableAnimatorContoller(string onTrigger, string offTrigger, GameObject target)
    {
        if (target == null)
            return null;

        // Where should we create the controller?
        var path = GetSaveControllerPath(target);
        if (string.IsNullOrEmpty(path))
            return null;

        // Create controller and hook up transitions.
        var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
        GenerateTriggerableTransition(onTrigger, controller);
        GenerateTriggerableTransition(offTrigger, controller);

        AssetDatabase.ImportAsset(path);

        return controller;
    }

    private static string GetSaveControllerPath(GameObject target)
    {
        var defaultName = target.gameObject.name;
        var message = string.Format("Create a new animator for the game object '{0}':", defaultName);
        return EditorUtility.SaveFilePanelInProject("New Animation Contoller", defaultName, "controller", message);
    }

    private static AnimationClip GenerateTriggerableTransition(string name, AnimatorController controller)
    {
        // Create the clip
        var clip = AnimatorController.AllocateAnimatorClip(name);
        AssetDatabase.AddObjectToAsset(clip, controller);

        // Create a state in the animatior controller for this clip
        var state = controller.AddMotion(clip);

        // Add a transition property
        controller.AddParameter(name, AnimatorControllerParameterType.Trigger);

        // Add an any state transition
        var stateMachine = controller.layers[0].stateMachine;
        var transition = stateMachine.AddAnyStateTransition(state);
        transition.AddCondition(AnimatorConditionMode.If, 0, name);
        transition.exitTime = 1f;
        transition.duration = 0f;
        return clip;
    }
}
#endif