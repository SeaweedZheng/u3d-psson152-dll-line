using UnityEngine;

public class TestDll : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        TestHotfitCppDllHelper.Instance.DoCappDll();
    }


    /// <summary>
    /// * ������Ҫ��̬����dll ������
    /// * cpp dll ֱ�ӷ��뵽��Ŀ�� �������һ����
    /// * cpp dll ��.dll������Ŀ�У����Զ�����
    /// * cpp dll �� xxx.dll.bytes ����StreamingAssets �У���Ҫ��̬���أ���
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
