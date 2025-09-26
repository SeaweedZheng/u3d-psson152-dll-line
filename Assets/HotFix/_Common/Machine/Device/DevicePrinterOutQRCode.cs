using CryPrinter;
using GameMaker;
using MoneyBox;
using SBoxApi;
using SkiaSharp;
using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

public class TicketInfo
{
    public string storeName;
    public long Money;
    public string TicketType;
    public string MoneyAmountEn;

    /// <summary> 二维码内容 </summary>
    public string BankInfo;
    public string OrderText;
    public string StoreAddr;
    public string StoreEmail;
    public string StoreTel;
}

public partial class DevicePrinterOutQRCode : MonoBehaviour
{

    /// <summary>
    /// 调用打印机进行打印
    /// </summary>
    /// <param name="qrcode">打印的信息</param>
    /// <param name="showNo">订单</param>
    /// <param name="isCheck">是否检测打印完成</param>
    public void PrintQRCodeInfo(string qrcode = "DD:12345678", string showNo = "DD:87654321", bool isCheck = false)
    {
        //string temp = value_txt.text.Replace(",", "");
        //long result = long.Parse(temp);
        //long outCredite = result / outCreditRate;

        // 1000积分 => 多少币 => 多少钱

        /*
        long money = 88;
        TicketInfo ticketInfo = new TicketInfo()
        {
            BankInfo = qrcode,
            Money = money,
            OrderText = showNo,
            TicketType = "Money Type",
        };

        PrintTicket(ticketInfo, isCheck);
        */
        PrintTicket(qrcode, showNo, "Money Type",88,"Store Name","City01", "Store  Address", "store@qq.com","123456","Machine Name", (issuccess,msg)=>{ });
    }





}
public partial class DevicePrinterOutQRCode : MonoBehaviour
{

    private const string TEST_PORT = "COM3";
    private const string PORT = "/dev/ttyS1";
#if UNITY_ANDROID
    PhoenixPrinter printer = null;
#endif
    void Awake()
    {
#if UNITY_ANDROID

        printer = new PhoenixPrinter(Application.isEditor ? TEST_PORT : PORT);

        /*if (Application.isEditor)
        {
            printer = new PhoenixPrinter(TEST_PORT);
        }
        else
        {
            printer = new PhoenixPrinter(PORT);
        }*/
        //companyName = BlackboardUtils.GetOrCreateVariable<string>(MainBlackboard.Get(), "CompanyName").value;
        //companyAddress = BlackboardUtils.GetOrCreateVariable<string>(MainBlackboard.Get(), "CompanyAddress").value;
        //companyEmail = BlackboardUtils.GetOrCreateVariable<string>(MainBlackboard.Get(), "CompanyEmail").value;
        //telephone = BlackboardUtils.GetOrCreateVariable<string>(MainBlackboard.Get(), "telephone").value;
        //deviceName = BlackboardUtils.GetOrCreateVariable<string>(MainBlackboard.Get(), "DeviceName").value;
        //errorHandleActionList = new List<Action>();

        //EventCenter.Instance.AddEventListener<string>(SBoxEventHandle.SBOX_SANDBOX_VERSION, CheckSboxSandVersion);
#endif
    }


    /// <summary>
    /// 返回值为true代表未连接打印机
    /// </summary>
    /// <returns></returns>
    public bool GetIsConnectPrinter()
    {
        bool result = false;
#if UNITY_ANDROID
        StatusReport isOnline = printer.GetStatus(StatusTypes.OfflineStatus);
        result = isOnline.HasError;
        bool parper = GetIsPaperStatus();
#endif
        return result;
    }

    private bool GetIsConnectPrinterStatus()
    {
        bool result = false;
#if UNITY_ANDROID
        StatusReport isOnline = printer.GetStatus(StatusTypes.OfflineStatus);
        result = isOnline.HasError;
#endif
        return result;
    }


    public bool GetIsPaperStatus()
    {
#if UNITY_ANDROID
        var parperStatus = printer.GetStatus(StatusTypes.PaperStatus);
        bool isParper = parperStatus.IsPaperPresent;
        return isParper;
#else
        return false;
#endif
    }


    public StatusReport GetOfflineStatusCode()
    {
#if UNITY_ANDROID
        return printer.GetStatus(StatusTypes.OfflineStatus);
#else
        return null;
#endif
    }

    public bool GetPrinterStatus()
    {
#if UNITY_ANDROID
        var status = printer.GetStatus(StatusTypes.PrinterStatus);
        return status.IsOnline;
#else
        return false;
#endif
    }


    private IEnumerator CheckPrinterIsBusy(Action<int,string> onCallBack)
    {

#if UNITY_ANDROID

        while (true)
        {
            yield return new WaitForSecondsRealtime(4.0f);
            var offlineCode = printer.GetStatus(StatusTypes.OfflineStatus);// GetOfflineStatusCode();

            if (!offlineCode.IsBusy) // 不忙
            {
                yield return new WaitForSecondsRealtime(1.0f);
                object[] res = GetPrintIsSuccess();
                bool isSuccess = (bool)res[0];
                if (isSuccess)
                {
                    //Debug.LogWarning("【printer qr】success: " + (string)res[1]);
                    //发送打印完成事件 - 或弹窗
                    //MessageDispatcher.Dispatch(EVTType.ON_CONTENT_EVENT, new ParadoxNotion.EventData("PrintFinish"));


                    Debug.LogWarning($"二维码打印机最终返回：{(string)res[1]}");
                    onCallBack?.Invoke(isSuccess? 0 : 1, (string)res[1]);
                }
                else
                {
                    //Debug.LogWarning("【printer qr】error: " + (string)res[1]);
                    //发送失败事件 - 或弹窗
                    Debug.LogWarning($"二维码打印机最终返回：{(string)res[1]}");

                    onCallBack?.Invoke(isSuccess ? 0 : 1, (string)res[1]);
                }
                break;
            }
        }
#else
        yield return null;
#endif
    }



    public object[] GetPrintIsSuccess()
    {
#if UNITY_ANDROID
        int state = SBoxSandbox.PrinterState();///先检测有无连接打印机
        //Debug.Log("printer state.............." + state.ToString());
        StatusReport offlineCode = printer.GetStatus(StatusTypes.OfflineStatus);  //GetOfflineStatusCode();
        //Debug.Log("新款打印机状态 IsOnline:" + offlineCode.IsOnline.ToString() + " HasError: " + offlineCode.HasError.ToString() + " IsPaperPresent: " + offlineCode.IsPaperPresent.ToString() + "IsPaperLevelOkay:" + offlineCode.IsPaperLevelOkay);
        if (offlineCode.HasError)///打印机有错误
        {
            if (!offlineCode.IsPaperPresent)
            {
                return new object[] { false, "Printer Out Of Paper" };
            }
            if (!offlineCode.IsOnline)
            {
                return new object[] { false, "Disconnected Printer" };
            }
            return new object[] { false, "Printer Exception, please contact the administrator" };
        }
        if (!offlineCode.IsPaperLevelOkay)///为fasle，代表少纸
        {
            return new object[] { false, "Printer Out Of Paper" };
        }
        if (!offlineCode.IsPaperPresent)
        {
            return new object[] { false, "Printer Out Of Paper" };
        }
        if (!offlineCode.IsOnline)
        {
            return new object[] { false, "Disconnected Printer" };
        }
        if (state == -2)
        {
            return new object[] { false, "Printer Exception, please contact the administrator" };
        }
        return new object[] { true, "" };
#else
        return new object[] { false, "" };
#endif
    }


    /// <summary>
    /// 打印普通的无二维码的凭证
    /// </summary>
    public void PrintNormalTicket(long credit, string bankOrder, string OrderText,
        string companyName,
        string companyAddress,
        string companyEmail,
        string telephone,
        string deviceName
        )
    {
        string testMsg = companyName + "\r\n" +
             "Money Type \r\n" +
             "$" + credit + "\r\n" +
             //NumberToEnglishString(credit) + "\r\n" +
             //bankOrder + "\r\n" +
             DateTime.Now.ToLongTimeString() + "\r\n" +
             DateTime.Now.ToString("dd MMM yyyy dddd", CultureInfo.CreateSpecificCulture("en-GB")) + "\r\n" +
             OrderText + "\r\n" +
             companyAddress + "\r\n" +
             companyEmail + "\r\n" +
             telephone;

        SBoxSandbox.PrinterMessage(testMsg);
    }



    /// <summary>
    /// 打印二维码的凭证
    /// </summary>
    public void PrintTicket(
        string qrcode,
        string showNo,
        string ticketType,
        long money,
        string companyName,
        string companyCity,
        string companyAddress,
        string companyEmail,
        string telephone,
        string deviceName,
        Action<int, string> onCallBack)
    {
        if (!DeviceUtils.IsCurQRCodePrinter())
        {
            //TipPopupHandler.Instance.OpenPopupOnce( I18nMgr.T("Please link the QR code printer first.") );
            //请先选择二维码打印机
            onCallBack.Invoke(1, "Please link the QR code printer first.");
            return;
        }


        Debug.Log($"开始打印二维码数据  qrcode：{qrcode}  showNo：{showNo} ");



#if UNITY_ANDROID

        printer.Reinitialize();
        var document = new StandardDocument
        {
            CodePage = CodePages.CPSPACE,
        };

        // 店名
        var StoreNameSection = new StandardSection
        {
            Content = companyName,//ticketInfo.storeName,
            Justification = FontJustification.JustifyLeft,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.None,
            Font = ThermalFonts.C,
            AutoNewline = true,
        };

        // 票类型 Money Type  Recharge Ticket 
        var TicketTypeSection = new StandardSection
        {
            Content =   ticketType, //ticketInfo.TicketType,
            Justification = FontJustification.JustifyCenter,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.Bold,
            Font = ThermalFonts.B,
            AutoNewline = true,
        };

        // 金额
        var MoneyAmountSection = new StandardSection
        {
            Content = "$" +  money.ToString("N0"),   //ticketInfo.Money.ToString("N0"),
            Justification = FontJustification.JustifyCenter,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.Bold,
            Font = ThermalFonts.B,
            AutoNewline = true,
        };

        // 金额
        var MoneyAmountEnSection = new StandardSection
        {
            Content = NumberToEnglishString(money).ToUpper(),// NumberToEnglishString(ticketInfo.Money).ToUpper(),
            Justification = FontJustification.JustifyCenter,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.None,
            Font = ThermalFonts.C,
            AutoNewline = true,
        };

        //DateTime dateTimeUtc = GetLocalDate();

        long timeMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        DateTime localDateTime01 = DateTimeOffset.UtcNow.LocalDateTime;
        ///string yyyyMMddHHmmss = localDateTime01.ToString("yyyyMMddHHmmss");

        // 时间
        var PrintTimeSection = new StandardSection
        {
            Content = localDateTime01.ToString("HH:mm:ss"), //dateTimeUtc.ToString("HH:mm:ss"),
            Justification = FontJustification.JustifyLeft,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.None,
            Font = ThermalFonts.C,
            AutoNewline = true,
        };

        // 日期
        var PrintDateSection = new StandardSection
        {
            Content =  localDateTime01.ToString("dd MMM yyyy dddd"), //dateTimeUtc.ToString("dd MMM yyyy dddd", CultureInfo.CreateSpecificCulture("en-GB")),
            Justification = FontJustification.JustifyLeft,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.None,
            Font = ThermalFonts.C,
            AutoNewline = true,
        };

        // 可见订单号
        var OrderTextSection = new StandardSection
        {
            Content = showNo,  //ticketInfo.OrderText,
            Justification = FontJustification.JustifyLeft,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.None,
            Font = ThermalFonts.C,
            AutoNewline = true,
        };

        // 店铺地址
        var StoreAddressSection = new StandardSection
        {
            Content = companyAddress,//ticketInfo.StoreAddr,
            Justification = FontJustification.JustifyLeft,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.None,
            Font = ThermalFonts.C,
            AutoNewline = true,
        };

        // 店铺邮箱
        var StoreEmailSection = new StandardSection
        {
            Content = companyEmail,//ticketInfo.StoreEmail,
            Justification = FontJustification.JustifyLeft,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.None,
            Font = ThermalFonts.C,
            AutoNewline = true,
        };

        // 店铺电话
        var StoreTelephoneSection = new StandardSection
        {
            Content = telephone,//ticketInfo.StoreTel,
            Justification = FontJustification.JustifyLeft,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.None,
            Font = ThermalFonts.C,
            AutoNewline = true,
        };

        // 设备名称
        var deviceNameSection = new StandardSection
        {
            Content = deviceName,
            Justification = FontJustification.JustifyLeft,
            HeightScalar = FontHeighScalar.h1,
            WidthScalar = FontWidthScalar.w1,
            Effects = FontEffects.None,
            Font = ThermalFonts.C,
            AutoNewline = true,
        };

        document.Sections.Add(StoreNameSection);
        document.Sections.Add(TicketTypeSection);
        document.Sections.Add(MoneyAmountSection);
        document.Sections.Add(MoneyAmountEnSection);
        //生成二维码
        //string temp = MoneyBoxUtils.EncryptQRCode(ticketInfo.BankInfo, "bank:");

        string temp = MoneyBoxUtils.EncryptQRCode(qrcode, "bank:");

        Texture2D tex = ZXingQrCode.GenerateQRImageWithColor("bank:" + temp + "&QRCodeEnd&", 256, 256, Color.black);
        // ExecutionEngineException: Method body is null. SkiaSharp.SkiaApi::sk_data_new_empty
        // 原因是插件SkiaSharp被拖动到热更代码中，插件SkiaSharp文件夹中的.dll不会打包到热更程序集中，导致函数体的缺失！
        using var qrCodeBitmap = SKBitmap.Decode(tex.EncodeToPNG());
        using var printerImage = new PrinterImage(qrCodeBitmap);
        printer.SetImage(printerImage, document, 4);

        document.Sections.Add(PrintTimeSection);
        document.Sections.Add(PrintDateSection);
        document.Sections.Add(OrderTextSection);
        document.Sections.Add(StoreAddressSection);
        document.Sections.Add(StoreEmailSection);
        document.Sections.Add(StoreTelephoneSection);
        document.Sections.Add(deviceNameSection);

        printer.PrintDocument(document);
        printer.FormFeed();

        if (onCallBack != null)
        {
            /*
            if (_cor != null)
            {
                StopCoroutine(_cor);
            }
            _cor = StartCoroutine(DelayTask(  ()=> { onCallBack(true, ""); },15f));*/
           StartCoroutine(CheckPrinterIsBusy(onCallBack));
        }
#endif
    }

    Coroutine _cor;
    private IEnumerator DelayTask(Action onCallBack, float timeS)
    {
        yield return new WaitForSecondsRealtime(timeS);
        onCallBack.Invoke();
    }





    /// <summary>
    /// 数字转为英文字符串 不考虑负数
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public string NumberToEnglishString(long number)
    {
        if (number < 0)
        {
            return "";
        }
        if (number < 20) //0到19
        {
            switch (number)
            {
                case 0:
                    return "zero";
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                case 10:
                    return "ten";
                case 11:
                    return "eleven";
                case 12:
                    return "twelve";
                case 13:
                    return "thirteen";
                case 14:
                    return "fourteen";
                case 15:
                    return "fifteen";
                case 16:
                    return "sixteen";
                case 17:
                    return "seventeen";
                case 18:
                    return "eighteen";
                case 19:
                    return "nineteen";
                default:
                    return "";
            }
        }
        if (number < 100) //20到99
        {
            if (number % 10 == 0) //20,30,40,...90的输出
            {
                switch (number)
                {
                    case 20:
                        return "twenty";
                    case 30:
                        return "thirty";
                    case 40:
                        return "forty";
                    case 50:
                        return "fifty";
                    case 60:
                        return "sixty";
                    case 70:
                        return "seventy";
                    case 80:
                        return "eighty";
                    case 90:
                        return "ninety";
                    default:
                        return "";
                }
            }
            else //21.22,....99 思路：26=20+6
            {
                return string.Format("{0} {1}", NumberToEnglishString(10 * (number / 10)),
                    NumberToEnglishString(number % 10));
            }
        }
        if (number < 1000) //100到999  百级
        {
            if (number % 100 == 0)
            {
                return string.Format("{0} hundred", NumberToEnglishString(number / 100));
            }
            else
            {
                return string.Format("{0} hundred and {1}", NumberToEnglishString(number / 100),
                    NumberToEnglishString(number % 100));
            }
        }
        if (number < 1000000) //1000到999999 千级
        {
            if (number % 1000 == 0)
            {
                return string.Format("{0} thousand", NumberToEnglishString(number / 1000));
            }
            else
            {
                return string.Format("{0} thousand and {1}", NumberToEnglishString(number / 1000),
                    NumberToEnglishString(number % 1000));
            }
        }
        if (number < 1000000000) //1000 000到999 999 999 百万级
        {
            if (number % 1000 == 0)
            {
                return string.Format("{0} million", NumberToEnglishString(number / 1000000));
            }
            else
            {
                return string.Format("{0} million and {1}", NumberToEnglishString(number / 1000000),
                    NumberToEnglishString(number % 1000000));
            }
        }
        if (number <= int.MaxValue) //十亿 级
        {
            if (number % 1000000000 == 0)
            {
                return string.Format("{0} billion", NumberToEnglishString(number / 1000000000));
            }
            else
            {
                return string.Format("{0} billion and {1}", NumberToEnglishString(number / 1000000000),
                    NumberToEnglishString(number % 1000000000));
            }
        }
        return "";
    }

}
