using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMessagePopup : PageCorBase
{
    public Text txtMessage;

    const string COR_AUTO_CLOSE = "COR_AUTO_CLOSE";

    string message;
    public override void OnOpen(PageName name, EventData data)
    {
        base.OnOpen(name, data);
        message = "<color=red>【系统消息】</color>系统消息提示...";  //即将进入设置界面。。。

        if (data != null)
        {
            Dictionary<string, object> res = data.value as Dictionary<string, object>;

            if (res.ContainsKey("autoCloseTimeS"))
            {
                DoCor(COR_AUTO_CLOSE, AutoClose((float)res["autoCloseTimeS"]));
            }
            message = (string)res["message"];
        }

        txtMessage.text = message;
    }


    IEnumerator AutoClose(float timeS = 15f)
    {
        yield return new WaitForSeconds(timeS);

        ClosePage();
    }
    void ClosePage()=> PageManager.Instance.ClosePage(this, new EventData<string>("PopupClose", ""));
    
}
