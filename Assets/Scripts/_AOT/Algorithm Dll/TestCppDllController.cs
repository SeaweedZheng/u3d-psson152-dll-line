using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

public class TestCppDllController : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        /*
        byte[] byteArray = new byte[512];
        DllInterface.DevVersion(ref byteArray[0]);
        string resultStr = Encoding.Default.GetString(byteArray, 0, byteArray.Length);

        Debug.LogError($"<color=blue>算发卡dll 版本号</color>, ver = {resultStr}");
*/


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
