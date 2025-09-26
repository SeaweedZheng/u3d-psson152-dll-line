using Game;
using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using SBoxApi;
using GameMaker;

namespace GameMaker
{
    public class MetaSystemManager : MonoSingleton<MetaSystemManager>
    {

        MessageDelegates onToolEventDelegates;

        private void Awake()
        {

            onToolEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                    { GlobalEvent.PageButton, OnClickPageBtn},
                    { GlobalEvent.CustomButtonClearCode, OnClickCustomButtonClearCode},
                    { GlobalEvent.CustomButtonDivicePrenter, OnClickCustomButtonDivicePrenter},
                    { GlobalEvent.CustomButtonCreditUp, OnClickCustomButtonCreditUp},
                    { GlobalEvent.CustomButtonCreditDown, OnClickCustomButtonCreditDown},
                    { GlobalEvent.CustomButtonSboxGetAccount,OnClickCustomButtonSboxGetAccount},
                    { GlobalEvent.CustomButtonCoinIn, OnClickCustomButtonCoinIn},
                    { GlobalEvent.CustomButtonTicketOut, OnClickCustomButtonTicketOut},
                    { GlobalEvent.TipPopupMsg, OnClickTipPopupMsg},
                    { GlobalEvent.IOTCoinIn, OnClickIOTCoinIn},
                    { GlobalEvent.ShowCode, OnClickShowCode},
                    { GlobalEvent.AesTest, OnClickAesTest},
                    { GlobalEvent.DeviceCounterClear, OnDeviceCounterClear},
                    { GlobalEvent.DeviceCounterAddCoinIn, OnDeviceCounterAddCoinIn},
                    { GlobalEvent.DeviceCounterAddCoinOut, OnDeviceCounterAddCoinOut},
                    { GlobalEvent.DeviceCounterAddCoinIn100, OnDeviceCounterAddCoinIn100},
                    { GlobalEvent.DeviceTestPrintQRCode,OnDeviceTestPrintQRCode},
                    { GlobalEvent.DeviceTestPrintTicket ,OnDeviceTestPrintTicket},
                    { GlobalEvent.DeviceTestPrintJCM950, OnDeviceTestPrintJCM950},
                    { GlobalEvent.DeviceTestPrintTRANSACT950, OnDeviceTestPrintTRANSACT950},
                }
             );
            // TextAsset txt =  ResourceManager.Instance.Load<TextAsset>("Assets/GameRes/_Common/Game Maker/Datas/page");  // 报错
            // TextAsset txt = ResourceManager.Instance.LoadD<TextAsset>("Assets/GameRes/_Common/Game Maker/Datas/page");  // 报错
            TextAsset txt = ResourceManager.Instance.LoadAssetAtPathOnce<TextAsset>("Assets/GameRes/_Common/Game Maker/ABs/Datas/tmg_page.json");  //
            //DebugUtil.Log( $"page = {txt.text}");
            TestManager.Instance.SetKV(TestManager.PAGES, txt.text);

            txt = ResourceManager.Instance.LoadAssetAtPathOnce<TextAsset>("Assets/GameRes/_Common/Game Maker/ABs/Datas/tmg_custom_button.json");  //
            TestManager.Instance.SetKV(TestManager.CUSTOM_BUTTON, txt.text);

            EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_TOOL_EVENT, onToolEventDelegates.Delegate);
        }


        protected override void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_TOOL_EVENT, onToolEventDelegates.Delegate);

            base.OnDestroy();
        }

        void Update()
        {
            OnToolTest();
        }

        void OnToolTest()
        {

            if (TestManager.Instance.HasKey("tool_jackpot"))
            {
                string val = TestManager.Instance.GetValueOnce("tool_jackpot");
                if (val == "jackpot_hall")
                {
                    long winCredit = long.Parse(TestManager.Instance.GetValueOnce("win_credit"));

                    WinJackpotInfo data = new WinJackpotInfo()
                    {
                        macId = 152,
                        seat = 1,
                        win = (int)winCredit,
                        jackpotId = 1,
                        orderId = 10001,
                        time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    };
                    EventCenter.Instance.EventTrigger<string>(RPCName.jackpotHall, JsonConvert.SerializeObject(data));
                }
            }

            if (TestManager.Instance.HasKeyOnce("tool_event"))
            {
                EventCenter.Instance.EventTrigger<string>(TestManager.Instance.GetValueOnce("event_name"),
                    TestManager.Instance.GetValueOnce("event_data"));
            }
        }

        public void OnClickPageBtn(EventData data)
        {

            Dictionary<string, object> res = (Dictionary<string, object>) data.value;

            string pgName = (string)res["pageName"];
            string pgData = (string)res["pageData"];

            DebugUtils.Log($" name = {pgName}   value = {JSONNodeUtil.ObjectToJsonStr(pgData)} ");

            PageName pageName = (PageName)Enum.Parse(typeof(PageName), pgName);

            if (pageName == PageName.Console001PageConsoleMain)
            {
                MachineDeviceCommonBiz.Instance.OpenConsole();
            }
            else
            {
                if (PageManager.Instance.IndexOf(pageName) != -1)
                {
                    PageManager.Instance.ClosePage(pageName);
                }
                else
                {
                    PageManager.Instance.OpenPage(pageName);
                }
            }

        }

        public void OnClickCustomButtonClearCode(EventData data)
        {
            MachineDataManager.Instance.RequestClearCodingActive(null, null);
        }

        public void OnClickCustomButtonDivicePrenter(EventData data)
        {
            GameObject.Find("INSTANCE/Machine Controller/Device Printer Out").GetComponent<DevicePrinterOut>().DoPrinterOut();
        }

        public void OnClickCustomButtonCreditUp(EventData data)
        {
            DeviceCreditUpDown.Instance.CreditUp();
        }
        public void OnClickCustomButtonCreditDown(EventData data)
        {
            DeviceCreditUpDown.Instance.CreditDown();
        }


        public void OnClickCustomButtonSboxGetAccount(EventData data)
        {
            MachineDataManager.Instance.RequestGetPlayerInfo((res) =>
            {
                SBoxAccount data = res as SBoxAccount;
                Debug.Log($" SBoxAccount: { JsonConvert.SerializeObject(data)}");
            }, (err) =>
            {

            });
        }

        public void OnClickCustomButtonCoinIn(EventData data)
        {
            EventCenter.Instance.EventTrigger<CoinInData>(SBoxSanboxEventHandle.COIN_IN, 
                new CoinInData()
                {
                    id =0,
                    coinNum = 1,
                });
        }
        public void OnClickCustomButtonTicketOut(EventData data)
        {
            MachineDeviceCommonBiz.Instance.TestTicketOut();
        }



        int idx = 0;
        public void OnClickTipPopupMsg(EventData data)
        {
            string msg = JSONNode.Parse((string)data.value)["msg"];
            TipPopupHandler.Instance.OpenPopup($"{msg} idx:{idx++}");
        }


        public void OnClickIOTCoinIn(EventData data)
        {
            DeviceIOTPayment.Instance.DoQrCoinIn();
        }


        public void OnClickShowCode(EventData data)
        {

            /*
            Type staticClassType = typeof(Code);

            // 获取静态类的所有字段
            FieldInfo[] fields = staticClassType.GetFields(BindingFlags.Public | BindingFlags.Static);

            string res = "";
            // 遍历字段并打印字段名和值
            foreach (FieldInfo field in fields)
            {
                // 获取字段的值
                object value = field.GetValue(null); // 对于静态字段，传递null作为实例参数
                // 打印字段名和值
                DebugUtil.Log($"【Code】{value}: {field.Name}");
                res += $"{value}: {field.Name}<br>";
            }
            CommonPopupHandler.Instance.OpenPopupSingle(
            new CommonPopupInfo()
            {
                isUseXButton = false,
                buttonAutoClose1 = true,
                type = CommonPopupType.OK,
                text = res,
                buttonText1 = I18nMgr.T("OK"),
            });*/

            List<string> codeInfo = new List<string>();

            codeInfo.Add($"【Software Code】");
            Type staticClassType = typeof(Code);
            // 获取静态类的所有字段
            FieldInfo[] fields = staticClassType.GetFields(BindingFlags.Public | BindingFlags.Static);
            // 遍历字段并打印字段名和值
            foreach (FieldInfo field in fields)
            {
                // 获取字段的值
                object value = field.GetValue(null); // 对于静态字段，传递null作为实例参数
                                                     // 打印字段名和值
                                                     //DebugUtil.Log($"【Code】{value}: {field.Name}");
                codeInfo.Add($"{value}: {field.Name}");
            }

            PageManager.Instance.OpenPage(PageName.Console001PopupConsoleNotice,
                new EventData<Dictionary<string, object>>("",
                    new Dictionary<string, object>()
                    {
                        ["title"] = I18nMgr.T("Error Code Query"),
                        ["content"] = codeInfo,
                        //["AutoCloseMS"] = 10000,
                    }));
        }


        public void OnClickAesTest(EventData data)
        {
            string plainText = "{\"name\":\"xiao ming\",\"age\":18,\"is_student\":true}";
            string cipherText = AesManager.Instance.TryLocalEncrypt(plainText);
            plainText = AesManager.Instance.TryLocalDecrypt(cipherText);

            DebugUtils.Log($"加密后：--{cipherText}--");
            DebugUtils.Log($"加密前：{plainText}");
        }


        public void OnDeviceCounterClear(EventData data)
        {
            MachineDataManager.Instance.RequestCounter(0, 0, 1, (res) =>
            {
                int resault = (int)res;

                DebugUtils.Log($"清除投币码表 : {resault}");

                // 这里必须嵌套，MachineDataManager的方法无法重复调用，响应回调会被覆盖
                MachineDataManager.Instance.RequestCounter(1, 0, 1, (res) =>
                {
                    int resault = (int)res;
                    DebugUtils.Log($"清除退币码表 : {resault}");
                });
            });
        }

        public void OnDeviceCounterAddCoinIn(EventData data)
        {
            MachineDataManager.Instance.RequestCounter(0, 1, 2, (res) =>
            {
                int resault = (int)res;
                if (resault < 0)
                    DebugUtils.LogError($"投币码表 : 返回状态：{resault}  投币个数：{1}");
                else
                    DebugUtils.Log($"投币码表 : 返回状态：{resault}  投币个数：{1}");
            });
        }
        public void OnDeviceCounterAddCoinOut(EventData data)
        {
            MachineDataManager.Instance.RequestCounter(1, 1, 2, (res) =>
            {
                int resault = (int)res;
                if (resault < 0)
                    DebugUtils.LogError($"退币码表 : 返回状态：{resault}  退币个数：{1}");
                else
                    DebugUtils.Log($"退币码表 : 返回状态：{resault}  退币个数：{1}");
            });
        }

        public void OnDeviceCounterAddCoinIn100(EventData data)
        {
            MachineDataManager.Instance.RequestCounter(0, 100, 2, (res) =>
            {
                int resault = (int)res;
                if (resault < 0)
                    DebugUtils.LogError($"投币码表 : 返回状态：{resault}  投币个数：{1}");
                else
                    DebugUtils.Log($"投币码表 : 返回状态：{resault}  投币个数：{1}");
            });
        }


        public void OnDeviceTestPrintQRCode(EventData data)
        {
            MachineDeviceCommonBiz.Instance.TestPrintQRCodeInfo();



/*
string jsonStr = @"
{
    ""ResentData"": ""yrheNXzCz6dMilHh95eLsay8dGLASACH3DlPGnIgDEgiM5+5PpkZlCHyQmU39FEeaWCXeEedmCbzD4c7DU67H0cItIt3II9SKBFJZFifS78VY2nxoZGq5AO3++c/c7x25t0DTcra+3Fi14ubpwnwAmOOdeD3lvtYKggaislt+dfaUCxjZWiARoXClEpKyAneDiiqE9KQSioxcycH8ZOPx2ggfVtdu6u6Eg15l56+xPXTcLKZu/1PncCKZNUq9qTzZSQtut5bLfmjysIV7P87flTS39/n3bwP4hJ9dKZinsC3Idc1WHaN7INZ8tGfh2YV/fvuPR9v4V3vjgXC43ECXCCeJgUItpPIIRTGIucsPXlZmK/L+9WgVLNe2A2R4CfS0nq/ZaoX4bEFRNBlP41Lv+ieO5sfXLNxCJuSlNtcUSAgdtp425j9pHzKFAYoMRQ0xGljioB9fqbTOXWRAag2VOMyhSJQ1og0fcoDVLkGCGUgStHTi1JJbWS2WTzh/3vnnZlMCJvEgxBRmYujJnk7CEY11HbQ55JyWoV5KhwJ70yLLO8tZVLlw2dbsneCGzpTHX6AB3/GmryXTm8R8WQTSPqjzIO+jjyuqT6GmUJOwMhAQtEJAPrPNhCpOqeJSriSU3G5mvgjDMg6rHz7O8ImAlYxoMFt4SeEfwG++sroVEoqUKtGQaPD96dVSGcZ89OGwTYR0pck52oIi5ozDYW8BgULwL5JTARe7B5aYpZQc5PpU42qCHgYtvDvfuLpJnO5k2eGMbfL+Ck5HveK72ArT2K8wGm28DiOi3YJF4xaH5UVLpcEfO8LzEYUZmzaMQqNLXpIKamcIM4eU36pdoppTxphIF7Lv1zB/0B81mVJBYjog6CEg1vWFJYyDHUNms0f"",
    ""Code"": 0,
    ""ErrMsg"": ""操作成功"",
    ""Action"": ""HD_HandleTasks"",
    ""action_type"": ""create_qrcode"",
    ""device_id"": ""11009001"",
    ""device_name"": ""GOOD FORTUNE RETURNS"",
    ""msgid"": 1,
    ""Data"": {
        ""createdAt"": ""04/19/2025 11:29:38 CST (UTC+08:00)"",
        ""createdDate"": ""04/19/2025"",
        ""createdTime"": ""11:29:38 CST (UTC+08:00)"",
        ""money"": 2,
        ""money_per_coin_in"": 0.5,
        ""qrcode"": ""bank:1745033378685027700_27"",
        ""showno"": ""show:20250419032938685027_27"",
        ""storeInfo"": {
            ""storeAddr"": ""Canton"",
            ""storeDetailAddr"": ""Panyu District, DaLongJiedao"",
            ""storeEmail"": ""02052643131@gmail.com"",
            ""storeName"": ""Xinshukeng second store"",
            ""storeTel"": ""02052643131"",
            ""tCityAddr"": ""City"",
            ""tDate"": ""Date"",
            ""tDevice"": ""Device"",
            ""tGameMachine"": ""Game Device"",
            ""tOrderNo"": ""Order"",
            ""tStoreEmail"": ""Email"",
            ""tStoreName"": ""Store"",
            ""tStoreTel"": ""Tel"",
            ""tStreetAddr"": ""Street"",
            ""tTime"": ""Time""
        },
        ""timezone"": ""Asia/Shanghai""
    }
}";



*/


        }




     
      
        public void OnDeviceTestPrintTicket(EventData data)
        {
            MachineDeviceCommonBiz.Instance.TestPrinterTicket();
        }

        public void OnDeviceTestPrintJCM950(EventData data)
        {
            MachineDeviceCommonBiz.Instance.PrinterJCM950();
        }

        public void OnDeviceTestPrintTRANSACT950(EventData data)
        {
            MachineDeviceCommonBiz.Instance.PrinterTRANSACT950();
        }
    }
}