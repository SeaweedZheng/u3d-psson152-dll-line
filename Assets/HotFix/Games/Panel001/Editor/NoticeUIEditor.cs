#if UNITY_EDITOR
using GameMaker;
using UnityEditor;
using static NoticeUI;

[CustomEditor(typeof(NoticeUI))]
public class NoticeUIEditor : Editor
{
    NoticeState state_old;
    long credit_old;
    string describe_old;
    string title_old;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        NoticeUI script = target as NoticeUI; // ���ƹ�����

        if (state_old != script.state)
        {
            script.state = script.state;
            state_old = script.state;
        }

        if (credit_old != script.credit)
        {
            script.credit = script.credit;
            credit_old = script.credit;
        }

        if (credit_old != script.credit)
        {
            script.credit = script.credit;
            credit_old = script.credit;
        }


        if (describe_old != script.describe)
        {
            script.describe = script.describe;
            describe_old = script.describe;
        }

        if (describe_old != script.describe)
        {
            script.describe = script.describe;
            describe_old = script.describe;
        }

        if (title_old != script.title)
        {
            script.title = script.title;
            title_old = script.title;
        }
        /*if (GUILayout.Button("���԰�ťA�����������"))
        {
            DebugUtil.Log("���԰�ťA �����");
        }*/
    }
}
#endif