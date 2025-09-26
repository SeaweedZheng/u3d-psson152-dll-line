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
/// ����ű���ʱ���ã�
/// </summary>
public class DeviceScanQRCode : MonoBehaviour
{
    /// <summary> ��̨��Ǯ�� ��ӡ��ά���ǰ׺</summary>
    string patternBank = @"bank:(.*?)&QRCodeEnd&";
    /// <summary> �ֻ����ɶ�ά�루�ֱ�Ǯ�� </summary>
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
    /// ���յ�����̨��bank��ά����Ϣ�Ĵ���
    /// </summary>
    /// <param name="message">��Ϣ����(�Ѿ�ȥ����ǰ׺�ͺ�׺)</param>
    public void CheckBankOrderInputHandle(string message)
    {
        Debug.Log("ɨ���ά�����ݣ� " + message);
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
                Debug.Log("��ά�����ݣ� bank:" + temp);
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
    /// ���յ����ֻ���qrcode��ά����Ϣ�Ĵ���
    /// </summary>
    /// <param name="message">��Ϣ����(�Ѿ�ȥ����ǰ׺�ͺ�׺)</param>
    public void CheckQRCodeOrderInputHandle(string message)
    {
        Debug.Log("ɨ���ά�����ݣ� " + message);
        /**/
        //if (CheckCanShowUsePop())
        if (true)
        {
#if UNITY_ANDROID
            string temp = MoneyBoxUtils.DecryptQRCode(message);
            Debug.Log("��ά�����ݣ� qr_code:" + temp);
            //CheckQRCode("qr_code:" + temp);
#endif
        }
        else
        {
            inputValue = "";
        }
        
    }


}
