#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(PssOn00152.ContentBlackboard))]
public class ContentBlackboardEditor : Editor
{
    long m_TotalBet01 = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PssOn00152.ContentBlackboard script = target as PssOn00152.ContentBlackboard; // 绘制滚动条

        /*将U3d 编辑器的修改广播出去*/
        if (m_TotalBet01 != script.totalBet)
        {
            script.observable.SetProperty(ref m_TotalBet01, script.totalBet, "totalBet");
        }

        var check = script.uiGrandJP; //（调用属性时，会延时发送事件）
        check = script.uiMajorJP; //（调用属性时，会延时发送事件）
        check = script.uiMinorJP; //（调用属性时，会延时发送事件）
        check = script.uiMiniJP; //（调用属性时，会延时发送事件）
                               //DebugUtil.Log("只有在碰触属性面板时，会轮询打印");

    }
}


#endif