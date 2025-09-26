
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
public static class TestUtils  
{
    public static void CheckReporter()
    {
        GameObject goReporter = GOFind.FindObjectIncludeInactive("Reporter");
        if (ApplicationSettings.Instance.isRelease)
        {
            if (goReporter != null)
                GameObject.DestroyImmediate(goReporter);
        }
        else
        {

            if (_consoleBB.Instance.enableReporterPage)
            {
                if (goReporter == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("Common/Prefabs/Reporter");
                    // ʵ����Ԥ���嵽��ǰ����
                    GameObject go = GameObject.Instantiate(prefab);
                    // ����ʵ���������Ϸ��������
                    go.name = "Reporter";
                    if (go.GetComponent<GOFindTag>() == null)
                        go.AddComponent<GOFindTag>();

                    go.SetActive(true);
                }
            }
            else
            {
                if (goReporter != null)
                {
                    Debug.LogError("ɾ�� Reporter");
                    GameObject.DestroyImmediate(goReporter);
                }
            }
        }
    }


    public static void CheckTestManager()
    {
        if (ApplicationSettings.Instance.isRelease)
        {
            TestManager.Instance.SetToolActive(false);
        }
        else
        {
            TestManager.Instance.SetToolActive(_consoleBB.Instance.enableTestTool);
        }
    }
}
