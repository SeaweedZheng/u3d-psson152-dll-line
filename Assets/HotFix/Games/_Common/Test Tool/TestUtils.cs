
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
                    // 实例化预制体到当前场景
                    GameObject go = GameObject.Instantiate(prefab);
                    // 设置实例化后的游戏对象名称
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
                    Debug.LogError("删除 Reporter");
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
