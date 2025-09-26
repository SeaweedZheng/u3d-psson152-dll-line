using Game;
using System.Collections.Generic;

public class UIConst
{
    private static UIConst _instance;
    public static UIConst Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIConst();
            }
            return _instance;
        }
    }

    //public Dictionary<string, string> pathDict;


    public Dictionary<PageName, string> pathDict;
    public UIConst()
    {
        pathDict = new Dictionary<PageName, string>()
        {
            [PageName.PopupSystemTip] = "Assets/GameRes/_Common/Game Maker/Prefabs/System Popup/Popup System Tip 001.prefab",
            [PageName.PopupSystemCommon] = "Assets/GameRes/_Common/Game Maker/Prefabs/System Popup/Popup System Common 001.prefab",
            [PageName.PageSystemMask] = "Assets/GameRes/_Common/Game Maker/Prefabs/System Mask/Page System Mask 002.prefab",
            [PageName.PageSysMessage] = "Assets/GameRes/_Common/Game Maker/Prefabs/System Popup/Page System Message.prefab",
            //[PageName.PO152PopupBigWin] = "Assets/GameRes/Games/PssOn00152/Prefabs/Popup/Popup Big Win.prefab",



            [PageName.PO152PopupFreeSpinResult] = "Assets/GameRes/Games/PssOn00152/Prefabs/Popup/Popup Free Spin Result.prefab",
            [PageName.PO152PopupFreeSpinTrigger] = "Assets/GameRes/Games/PssOn00152/Prefabs/Popup/PopupFreeSpinTrigger.prefab",
            [PageName.PO152PopupGameLoading] = "Assets/GameRes/Games/PssOn00152/Prefabs/Popup/Popup Game Loading.prefab",
            [PageName.PO152PageGameMainTestNew] = "Assets/GameRes/Games/PssOn00152/Prefabs/Page System God New.prefab",
            [PageName.PO152PageGameMain] = "Assets/GameRes/Games/PssOn00152/Prefabs/Page Game Main.prefab",


            // 联网彩金
            [PageName.PopupJackpotOnLine] = "Assets/GameRes/Games/Jackpot On Line Bundle 001/Prefabs/PopupJackpotTrigger.prefab",
            [PageName.PopupJackpotOnLine002] = "Assets/GameRes/Games/Jackpot On Line Bundle 002/Prefabs/PopupJackpotTrigger.prefab",



            // 游戏152
            [PageName.P015PopupQrCoinIn] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Prefabs/PopupQrCoinIn.prefab",
            [PageName.PO152PageGameMain1080] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Prefabs/PageGameMain.prefab",
            [PageName.PO152PopupGameLoading1080] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Prefabs/PopupGameLoading.prefab",
            [PageName.PO152PopupFreeSpinTrigger1080] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Prefabs/PopupFreeSpinTrigger 01.prefab",
            [PageName.PO152PopupBigWin1080] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Prefabs/PopupBigWin 01.prefab", //Popup Big Win 01
            [PageName.PO152PopupFreeSpinResult1080] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Prefabs/PopupFreeSpinResult 01.prefab",
            [PageName.PO152PopupJackpot1080] = "Assets/GameRes/Games/PssOn00152 (1080x1920)/Prefabs/PopupJackpotGame.prefab",


            // 后台
            [PageName.Console001PopupConsoleChooseDevice] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleChooseDevice.prefab",
            [PageName.Console001PopupConsoleSetPassword001] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleSetPassword 001.prefab",
            [PageName.Console001PageConsoleLogRecord] = "Assets/GameRes/Games/Console001/Prefabs/PageConsoleLogRecord.prefab",
            [PageName.Console001PopupConsoleChooseLanguage] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleChooseLanguage.prefab",
            [PageName.Console001PageConsoleBusinessRecord] = "Assets/GameRes/Games/Console001/Prefabs/PageConsoleBusinessRecord.prefab",
            [PageName.Console001PageConsoleDrawLine] = "Assets/GameRes/Games/Console001/Prefabs/PageConsoleDrawLine.prefab",
            //[PageName.Console001PopupConsoleSetPassword] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleSetPassword.prefab",
            [PageName.Console001PopupConsoleSlideSetting001] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleSlideSetting 001.prefab",
            [PageName.Console001PopupConsoleSlideSetting002] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleSlideSetting 002.prefab",
            [PageName.Console001PopupConsoleCoder] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleCoder.prefab",
            [PageName.Console001PageConsoleMain] = "Assets/GameRes/Games/Console001/Prefabs/PageConsoleMain.prefab",
            [PageName.Console001PageConsoleMachineSettings] = "Assets/GameRes/Games/Console001/Prefabs/PageConsoleMachineSettings.prefab",
            [PageName.Console001PopupConsoleCommon] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleCommon.prefab",
            [PageName.Console001PopupConsoleSound] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleSound.prefab",
            [PageName.Console001PopupConsoleCalendar] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleCalendar.prefab",
            [PageName.Console001PopupConsoleKeyboard001] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleKeyboard 001.prefab",
            [PageName.Console001PopupConsoleKeyboard002] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleKeyboard 002.prefab",
            [PageName.Console001PopupConsoleKeyboard003] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleKeyboard 003.prefab",
            [PageName.Console001PageConsoleEventRecord] = "Assets/GameRes/Games/Console001/Prefabs/PageConsoleEventRecord.prefab",
            [PageName.Console001PageConsoleGameHistory] = "Assets/GameRes/Games/Console001/Prefabs/PageConsoleGameHistory.prefab",
            [PageName.Console001PageConsoleMachineTest] = "Assets/GameRes/Games/Console001/Prefabs/PageConsoleMachineTest.prefab",
            [PageName.Console001PageConsoleGameInformation] = "Assets/GameRes/Games/Console001/Prefabs/PageConsoleGameInformation.prefab",
            [PageName.Console001PageConsoleAdmin] = "Assets/GameRes/Games/Console001/Prefabs/PageConsoleAdmin.prefab",
            [PageName.Console001PopupConsoleNotice] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleNotice.prefab",
            [PageName.Console001PopuoConsoleMoneyBoxRedeem] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleMoneyBoxRedeem.prefab",
            [PageName.Console001PopupConsoleSetMachineID] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleMachineID.prefab",
            [PageName.Console001PopupConsoleSetServerConnect] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleSetServerConnect.prefab",
            [PageName.Console001PopupConsoleSetServerConnect001] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleSetServerConnect 001.prefab",
            [PageName.Console001PopupConsoleSetParameter001] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleSetParameter 001.prefab",
            [PageName.Console001PopupConsoleSetParameter002] = "Assets/GameRes/Games/Console001/Prefabs/PopupConsoleSetParameter 002.prefab",


            [PageName.PageTestGodNew] = "Assets/GameRes/Games/PssOn00152/Prefabs/Page System God New.prefab",


        };
    }
}

public enum PageName
{
    PageTestGodNew,

    Console001PopupConsoleChooseDevice,
    Console001PopupConsoleSetPassword001,
    Console001PageConsoleLogRecord,
    Console001PopupConsoleChooseLanguage,
    Console001PageConsoleBusinessRecord, 
    Console001PageConsoleDrawLine,
   // Console001PopupConsoleSetPassword,
    Console001PopupConsoleSlideSetting001,
    Console001PopupConsoleSlideSetting002,
    Console001PopupConsoleCoder,
    Console001PageConsoleGameInformation,
    Console001PageConsoleMachineTest,
    Console001PageConsoleGameHistory,
    Console001PageConsoleEventRecord,
    Console001PopupConsoleKeyboard003,
    Console001PopupConsoleKeyboard002,
    Console001PopupConsoleKeyboard001,
    Console001PopupConsoleCalendar,
    Console001PopupConsoleSound,
    Console001PopupConsoleCommon,
    Console001PageConsoleMachineSettings,
    Console001PageConsoleMain,
    Console001PageConsoleAdmin,
    Console001PopupConsoleNotice,
    Console001PopuoConsoleMoneyBoxRedeem,
    Console001PopupConsoleSetMachineID,
    Console001PopupConsoleSetServerConnect,
    Console001PopupConsoleSetServerConnect001,
    Console001PopupConsoleSetParameter001,
    Console001PopupConsoleSetParameter002,

    P015PopupQrCoinIn,
    //PssOn00152
    PO152PageGameMain,
    PO152PageGameMain1080,
    PO152PageGameMainTestNew,
    PO152PopupGameLoading,
    PO152PopupGameLoading1080,
    PO152PopupFreeSpinTrigger,
    PO152PopupFreeSpinTrigger1080,
    //Popup Free Spin Trigger 01
    //PO152PopupBigWin,
    PO152PopupBigWin1080, //Popup Big Win 01
    PO152PopupFreeSpinResult,

    PO152PopupFreeSpinResult1080,
    //Popup Free Spin Result 01
    PO152PopupJackpot1080,


    // 系統弹窗
    PopupJackpotOnLine,
    PopupJackpotOnLine002,
    PopupSystemTip,
    PopupSystemCommon,
    PageSystemMask,
    PageSysMessage,



}
