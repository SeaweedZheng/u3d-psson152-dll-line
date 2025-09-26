using UnityEngine;
using Game;
using System.Collections.Generic;
using GameMaker;
using System.Timers;
using System.Collections;

public class Main
{

    static PreloadAssetBundlesHelper preloadAB = new PreloadAssetBundlesHelper()
    {
        markBundle = "MARK_BUNDLE_MAIN",
        preloadBundleNames = new List<string>()
        {
            // 财神爷游戏
            UIConst.Instance.pathDict[PageName.PO152PageGameMain1080],
        },

        preloadAssetAtPath = new List<string>()
        {
            // 通用弹窗
            UIConst.Instance.pathDict[PageName.PopupSystemTip],
            UIConst.Instance.pathDict[PageName.PageSystemMask],
           
            // 财神爷游戏
            UIConst.Instance.pathDict[PageName.PO152PageGameMain1080],
            UIConst.Instance.pathDict[PageName.PO152PopupGameLoading1080],

            // 控台
            UIConst.Instance.pathDict[PageName.Console001PageConsoleMain],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleChooseDevice],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleChooseLanguage],
            UIConst.Instance.pathDict[PageName.Console001PageConsoleBusinessRecord],
            UIConst.Instance.pathDict[PageName.Console001PageConsoleMachineSettings],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleCommon],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleKeyboard001],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleKeyboard002],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleSlideSetting001],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleSlideSetting002],
            UIConst.Instance.pathDict[PageName.Console001PageConsoleGameHistory],
            UIConst.Instance.pathDict[PageName.Console001PageConsoleMachineTest],
            UIConst.Instance.pathDict[PageName.Console001PageConsoleGameInformation],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleSetMachineID],
        },

    };

    static PreloadAssetBundlesHelper preloadABBackground = new PreloadAssetBundlesHelper()
    {
        markBundle = "MARK_BUNDLE_MAIN",
        preloadBundleNames = new List<string>()
        {
        },
        preloadAssetAtPath = new List<string>()
        {
            // 通用弹窗
            UIConst.Instance.pathDict[PageName.PopupJackpotOnLine002],
            UIConst.Instance.pathDict[PageName.PageSysMessage],
            //UIConst.Instance.pathDict[PageName.PopupSystemTip],
            //UIConst.Instance.pathDict[PageName.PageSystemMask],

           
            // 财神爷游戏
            //UIConst.Instance.pathDict[PageName.PO152PageGameMain1080],
            //UIConst.Instance.pathDict[PageName.PO152PopupGameLoading1080],
            UIConst.Instance.pathDict[PageName.P015PopupQrCoinIn],
            UIConst.Instance.pathDict[PageName.PO152PopupFreeSpinTrigger1080],
            UIConst.Instance.pathDict[PageName.PO152PopupBigWin1080],
            UIConst.Instance.pathDict[PageName.PO152PopupFreeSpinResult1080],
            UIConst.Instance.pathDict[PageName.PO152PopupJackpot1080],

            // 控台
            //UIConst.Instance.pathDict[PageName.Console001PageConsoleMain],
            //UIConst.Instance.pathDict[PageName.Console001PopupConsoleChooseDevice],
            //UIConst.Instance.pathDict[PageName.Console001PopupConsoleChooseLanguage],
            //UIConst.Instance.pathDict[PageName.Console001PageConsoleBusinessRecord],
            //UIConst.Instance.pathDict[PageName.Console001PageConsoleMachineSettings],
            //UIConst.Instance.pathDict[PageName.Console001PopupConsoleCommon],
            //UIConst.Instance.pathDict[PageName.Console001PopupConsoleKeyboard001],
            //UIConst.Instance.pathDict[PageName.Console001PopupConsoleKeyboard002],
            //UIConst.Instance.pathDict[PageName.Console001PopupConsoleSlideSetting001],
            //UIConst.Instance.pathDict[PageName.Console001PopupConsoleSlideSetting002],
            //UIConst.Instance.pathDict[PageName.Console001PageConsoleGameHistory],
            //UIConst.Instance.pathDict[PageName.Console001PageConsoleMachineTest],
            //UIConst.Instance.pathDict[PageName.Console001PageConsoleGameInformation],
            //UIConst.Instance.pathDict[PageName.Console001PopupConsoleSetMachineID],

            UIConst.Instance.pathDict[PageName.Console001PopupConsoleKeyboard003],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleSetPassword001],
            UIConst.Instance.pathDict[PageName.Console001PageConsoleLogRecord],
            UIConst.Instance.pathDict[PageName.Console001PageConsoleDrawLine],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleCoder],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleSound],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleCalendar],
            UIConst.Instance.pathDict[PageName.Console001PageConsoleEventRecord],
            UIConst.Instance.pathDict[PageName.Console001PageConsoleAdmin],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleNotice],
            UIConst.Instance.pathDict[PageName.Console001PopuoConsoleMoneyBoxRedeem],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleSetServerConnect],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleSetServerConnect001],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleSetParameter001],
            UIConst.Instance.pathDict[PageName.Console001PopupConsoleSetParameter002],
        },

    };


    public static void MainStart()
    {
        CoroutineAssistant.Instance.DoCor("COR_ON_BEFORE_PRELOAD", OnBeforePreLoadBundle());

        //PreLoadBundle();
    }

    static IEnumerator OnBeforePreLoadBundle()
    {
        // 预加载前:
        LoadingPage.Instance.RefreshProgressUIMsg("on before preload bundle");

        while (!SQLitePlayerPrefs03.Instance.isInit)
        {
            //yield return new WaitForSeconds(10f);

            yield return null;
        }

        while (!SQLiteAsyncHelper.Instance.isInit)
        {
            yield return null;
        }

        yield return null;

        PreLoadBundle();
    }

    /// <summary>
    /// 预加载
    /// </summary>
    private static void PreLoadBundle()
    {
        int preloadCount = preloadAB.preloadBundleNames.Count;
        LoadingPage.Instance.AddProgressCount(LoadingProgress.PRELOAD_ASSET_BUNDLE, preloadCount);
        DebugUtils.Log($"【内存预加载】ab包个数： {preloadCount}");

        if (!Application.isEditor) // 【？】这里要换成 ApplicationSettings.Instance.IsUseHotfix()
        {
            preloadAB.LoadPreloadBundleAsync((msg) =>
            {
                LoadingPage.Instance.Next(LoadingProgress.PRELOAD_ASSET_BUNDLE, msg);
            },
            () =>
            {
                PreLoadAsset();
            });
        }
        else
        {
            PreLoadAsset();
        }
    }
    /// <summary>
    /// 预加载资源，并等待完成
    /// </summary>
    private static void PreLoadAsset()
    {
        LoadingPage.Instance.RemoveProgress(LoadingProgress.PRELOAD_ASSET_BUNDLE);

        int preloadCount = preloadAB.preloadAssetAtPath.Count;
        LoadingPage.Instance.AddProgressCount(LoadingProgress.PRELOAD_ASSET, preloadCount);
        DebugUtils.Log($"【内存预加载】资源个数： {preloadCount}");

        if (!Application.isEditor) // 【？】这里要换成 ApplicationSettings.Instance.IsUseHotfix()
        {
            preloadAB.LoadPreloadAssetAsync((msg) =>
            {
                LoadingPage.Instance.Next(LoadingProgress.PRELOAD_ASSET, msg);
            },
            () =>
            {
                PreLoadBackboard();
                ConnectHardward();
            });
        }
        else
        {
            ConnectHardward();
        }
    }

    /// <summary>
    /// 后台预加载
    /// </summary>
    private static void PreLoadBackboard()
    {
        
        if (!Application.isEditor) // 【？】这里要换成 ApplicationSettings.Instance.IsUseHotfix()
        {
            preloadABBackground.LoadPreloadBundleAsync(null,
            () =>
            {
                preloadABBackground.LoadPreloadAssetAsync(null,
                () =>
                {
                    Debug.Log("【PreLoad】： 后台加载ab包、资源，完成");
                });
            });
        }
    }
    private static void ShowPlamtInfo()
    {
        DebugUtils.LogWarning($"平台:{ApplicationSettings.Instance.platformName}; 版本:{ApplicationSettings.Instance.appVersion}; 是否机台包:{ApplicationSettings.Instance.isMachine}; 热更新版本:{"--"}");
    }

    private static void ConnectHardward()
    {

        LoadingPage.Instance.RemoveProgress(LoadingProgress.PRELOAD_ASSET_BUNDLE);
        LoadingPage.Instance.RemoveProgress(LoadingProgress.PRELOAD_ASSET);


        ShowPlamtInfo();
        //DebugUtil.Log( "<color=red>" + "检查是否是机台..." +"</color>");

        if (ApplicationSettings.Instance.isMachine) {

            LoadingPage.Instance.AddProgressCount(LoadingProgress.CONNECT_MACHINE,2);
            LoadingPage.Instance.Next(LoadingProgress.CONNECT_MACHINE, $"connect machine: {ApplicationSettings.Instance.machineDebugUrl} ...");
            DebugUtils.LogWarning($"链接机台({ApplicationSettings.Instance.machineDebugUrl}), 初始化硬件...");
            /*
            if (Application.isEditor) {
                string IP = matchIp;
                if (matchIp.Contains(":"))
                {
                    IP = matchIp.Split(':')[0];
                    string Port = matchIp.Split(':')[1];
                    MatchDebugManager.Instance.port = int.Parse(Port);
                }
                MatchDebugManager.Instance.InitUdpNet(IP);
            }
            */
            //SBoxSandboxInit.Instance.Init(ApplicationSettings.Instance.machineDebugUrl, () =>
            SBoxInit.Instance.Init(ApplicationSettings.Instance.machineDebugUrl, () =>
            {
                DebugUtils.LogWarning("机台 链接成功...");
                
                InitSettings();//OpenGame();
            });
        }
        else {

            InitSettings();//OpenGame();
        }

    }




    #region 初始化参数
    static System.Timers.Timer checkTimer;

    static void ClearTimerInitSettings()
    {
        if (checkTimer != null)
        {
            checkTimer.Stop();
            checkTimer.Dispose();
            checkTimer = null;
        }
    }
    static void DelayCheckSettings()
    {
        ClearTimerInitSettings();

        float ms = 2000f;
        checkTimer = new System.Timers.Timer(ms);
        checkTimer.AutoReset = false; // 是否重复执行
        checkTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
        {
            Loom.QueueOnMainThread((data) =>
            {
                OnInitSettingFinish();
            }, null);
        };
        checkTimer.Start();
    }

    static void InitSettings()
    {
        LoadingPage.Instance.RemoveProgress(LoadingProgress.CONNECT_MACHINE);

        LoadingPage.Instance.AddProgressCount(LoadingProgress.INIT_SETTINGS,0);
        //LoadingPage.Instance.AddCountToProgress(LoadingProgress.INIT_SETTINGS,1);
        //LoadingPage.Instance.Next(LoadingProgress.INIT_SETTINGS, "msg");

        totalInitCount = 0;
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_INIT_SETTINGS_EVENT, OnInitSettingsEvent);
        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_INIT_SETTINGS_EVENT, OnInitSettingsEvent);
        // 获取设置参数配置
        GameObject pagePrefab =
            ResourceManager.Instance.LoadAssetAtPathOnce<GameObject>("Assets/GameRes/_Common/Game Maker/Prefabs/INSTANCE.prefab");
        pagePrefab.name = "INSTANCE";

        DelayCheckSettings();
    }

    //list - number - msg
    static void OnInitSettingsEvent(EventData res)
    {
        if (res.name == GlobalEvent.AddSettingsCount) {
            int count = (int)res.value;
            totalInitCount += count;
            LoadingPage.Instance.AddProgressCount(LoadingProgress.INIT_SETTINGS, count);
        }
        else if (res.name == GlobalEvent.InitSettings)
        {
            totalInitCount--;
            LoadingPage.Instance.Next(LoadingProgress.INIT_SETTINGS, (string)res.value);
        }
        else if (res.name == GlobalEvent.RefreshProgressMsg)
        {
            LoadingPage.Instance.RefreshProgressUIMsg((string)res.value);
        }

        DelayCheckSettings();
    }

    /// <summary> 初始化总个数 </summary>
    static int totalInitCount;

    static void OnInitSettingFinish()
    {
       
        if (totalInitCount > 0)
        {
            //LoadingPage.Instance.Next(LoadingProgress.INIT_SETTINGS,"init settings error!");
            DebugUtils.LogError("参数初始化失败！！！");
            return;
        }
        Debug.LogWarning("参数初始化成功！！！");
        //Debug.LogError("参数初始化成功！！！");

        #region 参数获取成功后
        TestManager.Instance.Init($"Ver {ApplicationSettings.Instance.appVersion}/{GlobalData.hotfixVersion}");
        TestUtils.CheckTestManager();
        TestUtils.CheckReporter();
        /*
         if (ApplicationSettings.Instance.isRelease)
        {
            TestManager.Instance.SetToolActive(false);
        }
        else
        {
            GameObject goReporter = GOFind.FindObjectIncludeInactive("Reporter");
            if (goReporter != null)
            {
                goReporter.SetActive(_consoleBB.Instance.enableReporterPage);
            }

            TestManager.Instance.SetToolActive(_consoleBB.Instance.enableTestTool);
        }
        */


        MachineDeviceCommonBiz.Instance.CheckSas();

        MachineDeviceCommonBiz.Instance.CheckMoneyBox();

        MachineDeviceCommonBiz.Instance.CheckIOT();

        MachineDeviceCommonBiz.Instance.CheckMqttRemoteButtonController();

        MachineDeviceCommonBiz.Instance.CheckBonusReport();


        MachineDeviceCommonBiz.Instance.SetLevel();

        #endregion
        EventCenter.Instance.EventTrigger(GlobalEvent.ON_INIT_SETTINGS_FINISH_EVENT);



        // 初始化成功后的操作:
        // * XXX
        // * XXX
        // 正式包删除调试工具

        OpenGame();
    }

    static void OpenGame()
    {

        LoadingPage.Instance.RemoveProgress(LoadingProgress.INIT_SETTINGS);

        //LoadingPage.Instance.AddProgress(LoadingProgress.ENTER_GAME,0);
        //LoadingPage.Instance.Next(LoadingProgress.ENTER_GAME,"enter game");
        LoadingPage.Instance.RemoveProgress(LoadingProgress.ENTER_GAME);
        LoadingPage.Instance.Finish("enter game");
        // 预加载 login 页 ？？
        LoadingPage.Instance.Close(2f);

        // 游戏加载页面：
        //TestManager.Instance.SetServer(ApplicationSettings.Instance.machineDebugUrl);
        PageManager.Instance.OpenPage(PageName.PO152PopupGameLoading1080);
        //UIManager.Instance.OpenPanel(PageName.MainGameView);
    }

    #endregion




    #region 测试代码
    /*CorBehaviour _ctrlInitSetting;
    CorBehaviour ctrlInitSetting
    {
        get
        {
            if (_ctrlInitSetting == null)
            {
                GameObject singleton = new GameObject();
                CorBehaviour ctrl = singleton.AddComponent<CorBehaviour>();
                singleton.name = "Init Setting ";
                Object.DontDestroyOnLoad(singleton);
            }
            return _ctrlInitSetting;
        }
        set { _ctrlInitSetting = value; }
    }
    void TestInit()
    {
        ctrlInitSetting.DoCor("INIT_SETTING_FINISH", ctrlInitSetting.DoTask(() =>
        {
            Debug.Log("i am finish");
            GameObject.DestroyImmediate(ctrlInitSetting.gameObject);
            ctrlInitSetting = null;
        },1000));
    }*/
        #endregion
    }
