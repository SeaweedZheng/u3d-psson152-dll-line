#if UNITY_EDITOR
using UnityEditor;

namespace GameMaker
{
    public class EditorWindowBaseInterface : EditorWindow
    {
        public virtual string GetEditorName()
        {
            return string.Empty;
        }
    }
}
#endif