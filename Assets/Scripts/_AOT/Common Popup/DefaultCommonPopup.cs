using UnityEngine;
using UnityEngine.UI;

public interface ICommonPopupHandel
{

    void OpenPopup(CommonPopupInfo data);
    void SetContent(CommonPopupInfo data);

    void ClosePopup();
}

public class DefaultCommonPopup : DefaultBasePopup, ICommonPopupHandel
{
    static DefaultCommonPopup _instance;

    public static DefaultCommonPopup Instance
    {
        get
        {
            if (_instance == null)
            {
                 _instance = new DefaultCommonPopup(); 
            }
            return _instance;
        }
    }


    public  void OpenPopup(CommonPopupInfo info)
    {

        base.OnOpen("Common/Prefabs/Popup Default Common");

        SetContent(info);
    }

    Text txtTitle => goPopup.transform.Find("Anchor/Title").GetComponent<Text>();
    Text txtContent => goPopup.transform.Find("Anchor/Content").GetComponent<Text>();

    Button btn1 => goPopup.transform.Find("Anchor/Footer/Button (1)").GetComponent<Button>();
    Button btn2 => goPopup.transform.Find("Anchor/Footer/Button (2)").GetComponent<Button>();
    Button btnX => goPopup.transform.Find("Anchor/Button Close").GetComponent<Button>();

    CommonPopupInfo curInfo;
    public void SetContent(CommonPopupInfo info)
    {
        curInfo = info;



        Transform tfmAnchor = goPopup.transform.Find("Anchor");

        txtTitle.gameObject.SetActive(false);
        btn1.gameObject.SetActive(false);
        btn2.gameObject.SetActive(false);
        btnX.gameObject.SetActive(false);

        tfmAnchor.Find("Footer").gameObject.SetActive(true);
        txtContent.gameObject.SetActive(true);


        switch (curInfo.type)
        {
            case CommonPopupType.SystemReset:
            case CommonPopupType.OK:
                btn1.gameObject.SetActive(true);
                break;
            case CommonPopupType.YesNo:
                btn1.gameObject.SetActive(true);
                btn2.gameObject.SetActive(true);
                break;
            case CommonPopupType.SystemTextOnly:
            case CommonPopupType.TextOnly:
                break;
        }

        txtContent.text = curInfo.text;

        if (!string.IsNullOrEmpty(curInfo.title))
        {
            txtTitle.text = curInfo.title;
            txtTitle.gameObject.SetActive(true);
        }

        if (!curInfo.isUseXButton)
        {
            btnX.onClick.RemoveAllListeners();
            btnX.gameObject.SetActive(false);
        }
        else
        {
            btnX.onClick.RemoveAllListeners();
            btnX.onClick.AddListener(() => {
                ClosePopup();
                curInfo.callbackX?.Invoke();
            });
            btnX.gameObject.SetActive(true);
        }

        if (btn1.gameObject.active)
        {
            btn1.transform.Find("Text").GetComponent<Text>().text = curInfo.buttonText1;
            btn1.onClick.RemoveAllListeners();
            btn1.onClick.AddListener(() => {
                if (curInfo.buttonAutoClose1) {
                    ClosePopup();
                }
                curInfo.callback1?.Invoke();
            });
        }

        if (btn2.gameObject.active)
        {
            btn2.transform.Find("Text").GetComponent<Text>().text = curInfo.buttonText2;
            btn2.onClick.RemoveAllListeners();
            btn2.onClick.AddListener(() => {
                if (curInfo.buttonAutoClose2) {
                    ClosePopup();
                }
                curInfo.callback2?.Invoke();
            });
        }
    }

}
