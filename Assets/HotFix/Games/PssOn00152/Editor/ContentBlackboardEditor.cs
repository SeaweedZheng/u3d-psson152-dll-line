#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(PssOn00152.ContentBlackboard))]
public class ContentBlackboardEditor : Editor
{
    long m_TotalBet01 = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PssOn00152.ContentBlackboard script = target as PssOn00152.ContentBlackboard; // ���ƹ�����

        /*��U3d �༭�����޸Ĺ㲥��ȥ*/
        if (m_TotalBet01 != script.totalBet)
        {
            script.observable.SetProperty(ref m_TotalBet01, script.totalBet, "totalBet");
        }

        var check = script.uiGrandJP; //����������ʱ������ʱ�����¼���
        check = script.uiMajorJP; //����������ʱ������ʱ�����¼���
        check = script.uiMinorJP; //����������ʱ������ʱ�����¼���
        check = script.uiMiniJP; //����������ʱ������ʱ�����¼���
                               //DebugUtil.Log("ֻ���������������ʱ������ѯ��ӡ");

    }
}


#endif