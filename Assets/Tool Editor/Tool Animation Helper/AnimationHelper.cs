#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// 在animator下添加animationclip作为子节点
/// </summary>
public class AnimationHelper : EditorWindow
{
    private enum WindowsType
    {
        Create,
        Rename
    }

    private static WindowsType k_WindowsType;

    //[MenuItem("Assets/Seaweed/Animation/CreateSubAnimationClip")]
    [MenuItem("Assets/Seaweed/Animation/在Animator下添加Animationclip作为子节点")]
    private static void CreateSubAnimationClip()
    {
        k_WindowsType = WindowsType.Create;

        Rect wr = new Rect(0, 0, 300, 148);
        AnimationHelper window = (AnimationHelper)EditorWindow.GetWindowWithRect(typeof(AnimationHelper), wr, true, "CreateSubAnimationClip");

        Object[] objs = Selection.objects;
        if (objs.Length == 1 && objs[0] is RuntimeAnimatorController) window.m_Controller = (RuntimeAnimatorController)objs[0];

        window.Show();
    }

    [MenuItem("Assets/Animation/RenameSubAnimationClip")]
    private static void RenameSubAnimationClip()
    {
        k_WindowsType = WindowsType.Rename;

        Rect wr = new Rect(0, 0, 300, 148);
        AnimationHelper window = (AnimationHelper)EditorWindow.GetWindowWithRect(typeof(AnimationHelper), wr, true, "RenameSubAnimationClip");

        Object[] objs = Selection.objects;
        if (objs.Length == 1 && objs[0] is AnimationClip)
        {
            window.m_Clip = (AnimationClip)objs[0];
            window.m_AnimClipName = objs[0].name;
        }

        window.Show();
    }

    [MenuItem("Assets/Animation/DeleteSubAnimationClip")]
    private static void DeleteSubAnimationClip()
    {
        Object[] objs = Selection.objects;
        if (objs.Length == 1 && objs[0] is AnimationClip)
        {
            GameObject.DestroyImmediate(objs[0], true);
            AssetDatabase.SaveAssets();
        }
    }

    private string m_AnimClipName;
    private RuntimeAnimatorController m_Controller;
    private AnimationClip m_Clip;

    private void OnGUI()
    {
        switch (k_WindowsType)
        {
            case WindowsType.Create:
                m_Controller = EditorGUILayout.ObjectField("Animator Controller", m_Controller, typeof(RuntimeAnimatorController), true) as RuntimeAnimatorController;
                m_AnimClipName = EditorGUILayout.TextField("Animation Clip Name", m_AnimClipName);
                if (GUILayout.Button("Create", GUILayout.Height(35)))
                {
                    AnimationClip animationClip = new AnimationClip();
                    animationClip.name = m_AnimClipName;
                    AssetDatabase.AddObjectToAsset(animationClip, m_Controller);
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animationClip));
                }
                break;
            case WindowsType.Rename:
                m_Clip = EditorGUILayout.ObjectField("Animator Clip", m_Clip, typeof(AnimationClip), true) as AnimationClip;
                m_AnimClipName = EditorGUILayout.TextField("Animation Clip Name", m_AnimClipName);
                if (GUILayout.Button("Rename", GUILayout.Height(35)))
                {
                    m_Clip.name = m_AnimClipName;
                    AssetDatabase.SaveAssets();
                }
                break;
        }

    }
}
#endif