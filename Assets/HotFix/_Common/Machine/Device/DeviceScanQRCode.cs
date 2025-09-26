using GameMaker;
using MoneyBox;
using System.Text.RegularExpressions;
using UnityEngine;



public enum CheckInputStatus
{
    None,
    Idel,
    Using,
}

/// <summary>
/// 这个脚本暂时不用！
/// </summary>
public class DeviceScanQRCode : MonoBehaviour
{
    /// <summary> 机台或钱箱 打印二维码的前缀</summary>
    string patternBank = @"bank:(.*?)&QRCodeEnd&";
    /// <summary> 手机生成二维码（分变钱） </summary>
    string patternQRCode = @"qr_code:(.*?)&QRCodeEnd&";
    private CheckInputStatus _CheckInputStatus = CheckInputStatus.None;
    private string inputValue = "";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        return;

        if (ApplicationSettings.Instance.isMachine || Application.isEditor)
        {
            if (Input.anyKeyDown)
            {
                if (_CheckInputStatus == CheckInputStatus.Using)
                {
                    inputValue = "";
                    return;
                }
                string temp = Input.inputString;
                if (!string.IsNullOrEmpty(temp))
                {
                    inputValue += temp;
                    Match match = Regex.Match(inputValue, patternQRCode);
                    if (match.Success)
                    {
                        string qrValue = match.Groups[1].Value;
                        inputValue = "";
                        CheckQRCodeOrderInputHandle(qrValue);
                    }
                    match = Regex.Match(inputValue, patternBank);
                    if (match.Success)
                    {
                        string bankValue = match.Groups[1].Value;
                        inputValue = "";
                        CheckBankOrderInputHandle(bankValue);
                    }
                }
            }
        }
    }


    /// <summary>   
    /// 当收到（机台）bank二维码消息的处理
    /// </summary>
    /// <param name="message">消息内容(已经去除了前缀和后缀)</param>
    public void CheckBankOrderInputHandle(string message)
    {
        Debug.Log("扫描二维码内容： " + message);
        /**/
        //if (CheckCanShowUsePop())
        if(true)
        {
#if UNITY_ANDROID
            //string temp = PrinterController.Decrypt(message);
            //ShowBankPopup("bank:" + temp);


            try
            {
                string temp = MoneyBoxUtils.DecryptQRCode(message);
                Debug.Log("二维码内容： bank:" + temp);
                //ShowBankPopup("bank:" + temp);
                //EventCenter.Instance.EventTrigger<string>(GlobalEvent.ON_DEVICE_EVENT);
            }
            catch 
            {

            }
#endif
        }
        else
        {
            inputValue = "";
        }
    }


    /// <summary>
    /// 当收到（手机）qrcode二维码消息的处理
    /// </summary>
    /// <param name="message">消息内容(已经去除了前缀和后缀)</param>
    public void CheckQRCodeOrderInputHandle(string message)
    {
        Debug.Log("扫描二维码内容： " + message);
        /**/
        //if (CheckCanShowUsePop())
        if (true)
        {
#if UNITY_ANDROID
            string temp = MoneyBoxUtils.DecryptQRCode(message);
            Debug.Log("二维码内容： qr_code:" + temp);
            //CheckQRCode("qr_code:" + temp);
#endif
        }
        else
        {
            inputValue = "";
        }
        
    }


}
