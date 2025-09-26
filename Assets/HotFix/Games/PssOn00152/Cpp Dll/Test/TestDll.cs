using UnityEngine;

public class TestDll : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        TestHotfitCppDllHelper.Instance.DoCappDll();
    }


    /// <summary>
    /// * 这里需要动态集成dll 跟包走
    /// * cpp dll 直接放入到项目中 打包跟包一起走
    /// * cpp dll 以.dll放入项目中，将自动加载
    /// * cpp dll 以 xxx.dll.bytes 放入StreamingAssets 中，需要动态加载！！
    /// </summary>
    void DllInAot()
    {
        // Assembly.Load();
        TestApkCppDllManager.Instance.TestGetStructData();

        TestApkCppDllManager.Instance.TestGetSlotResult(11);
            
    }


    void Test1()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
