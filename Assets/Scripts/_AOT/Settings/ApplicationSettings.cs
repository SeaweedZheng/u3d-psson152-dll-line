#define TEST_USE_REMOTE_AB
using UnityEngine;
using System;
using System.IO;
using Sirenix.OdinInspector;
using Newtonsoft.Json.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif


[Flags]
public enum LogFilter
{
    System 		= (1 << 0),
    Unity		= (1 << 1),
    NodeCanvas	= (1 << 2),
    Bundle		= (1 << 3),
    Scene		= (1 << 4),
    Network		= (1 << 5),
    Analytics	= (1 << 6),
    Performance	= (1 << 7),
    TestSuite	= (1 << 8),
    Test		= (1 << 9)
};

/*[Serializable]
public class DesignResolutionInfo
{
    public float height = 1280f;
    public float width = 720f;
}*/


[Serializable]
//u3d�༭�����Ҽ����Create/SlotMaker/ScriptableObject/ApplicationSettings ���� ApplicationSettings.asset�ļ�
[CreateAssetMenu(fileName = "ApplicationSettings", menuName = "GameMaker/ScriptableObject/ApplicationSettings")]
public partial class ApplicationSettings : ScriptableObjectSingleton<ApplicationSettings>//public class ApplicationSettings : ScriptableObject//
{



    [Space]
    [Title("�ͻ�������")]


    [Tooltip("�Ƿ��ǻ�̨��")]
    public bool isMachine;


    [Tooltip("�Ƿ�����ʽ��")]
    public bool isRelease = false;

    /*
     * ��Build Settings �ﶨ��һ����RELEASE �������Ƿ���Release�����ǲ���ȡ�ģ�
     * RELEASE �����ڰ����ֻ���ڴ������ʱ��ȷ���ų���һ����룡
     * ����ȸ��������У�#if RELEASE ...  #else ... #endif �� �����ʱ ��RELEASE�� �ر�RELEASE���ȸ����������鶼������Ч�� 
     */

    [Tooltip("�Ƿ��ǲ�������")]
    public bool isMock;


    [Tooltip("�Ƿ�����������")]
    public bool isUseProtectApplication = false;


    [Tooltip("ƽ̨����")]
    public string platformName = "PssOn00152";

    [Tooltip("��������")]
    public string gameTheme = "GOOD FORTUNE RETURNS";

    [Tooltip("��������")]
    public string agentName = "PssOn00152";

    // ƽ̨yyyddmmhhmmss + 6Ϊ����룿��
    // ƽ̨_yyyddmmhhmms
    //appkey��Ψһ�ģ�����ʹ��ͬ��clientVersion��ƻ������׿����̨��PC����appkey����Ψһ�ģ�
    [Tooltip("app��key")]
    public string appKey;


    [Tooltip("�ͻ��˰汾")]
    public string appVersion = "1.0.0";


    [Tooltip("��Դ������")]
    public string resourceServer = "http://8.138.140.180:8124";

    public string platformResourceServerUrl => $"{resourceServer}/{platformName}";


    [Space]
    [Title("��̨����")]


    [Tooltip("��̨����url")]
    public string machineDebugUrl = "192.168.3.82";//"192.168.3.82:8092";

    [Space]
    [Title("��Ϸ����")]

    [Tooltip("�����ϱ�url")]
    public string reportUrl = "http://192.168.3.152/api/game_log/send";




    [Tooltip("����ҳ����·��")]
    public string posterUrl = "";

    [Tooltip("����ҳlogo·��")]
    public string logoUrl = "Assets/Resources/Common/Sprites/g152_icon.png";



    [Tooltip("��Ϸ���ݿ���")]
    public string dbName = "Games.db";

    [Space]
    [Title("����")]
    [Tooltip("�ڱ༭�����������ȸ�����")]
    public bool isTestUseHotfixAtEditor = false;
    public bool IsUseHotfix()
    {
        if (Application.isEditor && isTestUseHotfixAtEditor)
        {
            return true;
        }
        return !Application.isEditor;
    }

    /**/
    [Tooltip("�ڱ༭�����������ȸ�����")]
    public bool isTestMachineButtonAtEditor = false;
    public bool IsMachine()
    {
        if (Application.isEditor && isTestMachineButtonAtEditor)
        {
            return true;
        }
        return isMachine;
    }

    [Title("����")]

    public LogFilter logFilter { get; set; }

    public static int GetClientVersionNumber()
    {
    	string[] versions = Instance.appVersion.Split(new char[]{ '.' });
    	return Int32.Parse(versions[0]) * 10000 + Int32.Parse(versions[1]) * 100 + Int32.Parse(versions[2]);
    }
    public static int GetClientMajorVersionNumber()
    {
    	string[] versions = Instance.appVersion.Split(new char[]{ '.' });
    	return Int32.Parse(versions[0]);
    }

    public static string GetPlatformName()
    {
#if UNITY_EDITOR
        return GetPlatformName(EditorUserBuildSettings.activeBuildTarget);
#else
        return GetPlatformName(Application.platform);
#endif
    }

#if UNITY_EDITOR
    private static string GetPlatformName(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {
        case BuildTarget.Android:
    		return "Android";
    	case BuildTarget.iOS:
    		return "iOS";
    	case BuildTarget.WebGL:
    		return "Canvas";
        case BuildTarget.WSAPlayer:
    		return "Windows";
    	case BuildTarget.StandaloneWindows:
    	case BuildTarget.StandaloneWindows64:
            return "Gameroom";
    	case BuildTarget.StandaloneOSX:
    		return "OSX_Standalone";
    		// Add more build targets for your own.
    		// If you add more targets, don't forget to add the same platforms to GetPlatform(RuntimePlatform) function.
    	default:
    		return null;
    	}
    }
#endif

    private static string GetPlatformName(RuntimePlatform runtimePlatform)
    {
        switch (runtimePlatform)
        {
        case RuntimePlatform.Android:
#if PLATFORM_AMAZON
            return "Amazon";
#else
            return "Android";
#endif
        case RuntimePlatform.IPhonePlayer:
            return "iOS";
        case RuntimePlatform.WebGLPlayer:
            return "Canvas";
        case RuntimePlatform.WSAPlayerARM:
        case RuntimePlatform.WSAPlayerX64:
        case RuntimePlatform.WSAPlayerX86:
            return "Windows";
        case RuntimePlatform.WindowsPlayer:
            return "Gameroom";
        case RuntimePlatform.OSXPlayer:
            return "OSX_Standalone";
            // Add more build targets for your own.
            // If you add more targets, don't forget to add the same platforms to GetPlatform(RuntimePlatform) function.
        default:
            return "UNKNOWN";
        }
    }

    public static string GetStreamingAssetsPath()
    {
        return Application.streamingAssetsPath;
    }



    public static string GetDeviceModel()
    {
        string deviceModel = SystemInfo.deviceModel;
        if (string.IsNullOrEmpty (deviceModel)) {
            deviceModel = "ModelUnknown";
        }
        return deviceModel;
    }

    public static string GetOperatingSystem()
    {
        string operatingSystem = SystemInfo.operatingSystem;
        if (string.IsNullOrEmpty (operatingSystem)) {
            operatingSystem = "Unknown";
        }

        return operatingSystem;
    }

    public static string GetDeviceType()
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        if(SystemInfo.deviceModel.Contains("iPad"))
        {
            return "IPAD";
        }
        else
        {
            return "IPHONE";
        }
#elif UNITY_ANDROID && PLATFORM_AMAZON && !UNITY_EDITOR
        return "KINDLE";
#elif UNITY_ANDROID && !UNITY_EDITOR
        return "GOOGLE";
#elif UNITY_WSA && !UNITY_EDITOR
        return "WSA";
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        return "GAMEROOM";
#elif UNITY_WEBGL && !UNITY_EDITOR
        return "CANVAS";
#elif UNITY_STANDALONE_OSX && !UNITY_EDITOR
        return "IPHONE"; // OSX is treated as iOS in server logic. Thus, treat this test platform like iOS.
#else
        string platform = GetPlatformName();

        if(platform == "Android")
        {
            return "GOOGLE";
        }
        else
        {
            return "IPHONE";
        }
#endif
    }

    public static string GetCachePath()
    {
#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
        return Application.dataPath;
#else
        return Application.temporaryCachePath;
#endif
    }

    public static string GetApplicationStage()
    {
#if BUILD_RROD
        return "prod";
#elif BUILD_ST
        return "st";
#elif BUILD_QA
        return "qa";
#elif BUILD_QA_DEV
        return "qa_dev";
#else
        return "dev";
#endif
    }

    public static bool LogSystem()
    {
    	return (Instance.logFilter & LogFilter.System) == LogFilter.System;
    }

    public static bool LogUnity()
    {
    	return (Instance.logFilter & LogFilter.Unity) == LogFilter.Unity;
    }

    public static bool LogNodeCanvas()
    {
    	return (Instance.logFilter & LogFilter.NodeCanvas) == LogFilter.NodeCanvas;
    }

    public static bool LogBundle()
    {
        return  (Instance.logFilter & LogFilter.Bundle) == LogFilter.Bundle;
    }

    public static bool LogScene()
    {
    	return (Instance.logFilter & LogFilter.Scene) == LogFilter.Scene;
    }

    public static bool LogNetwork()
    {
    	return (Instance.logFilter & LogFilter.Network) == LogFilter.Network;
    }

    public static bool LogAnalytics()
    {
    	return (Instance.logFilter & LogFilter.Analytics) == LogFilter.Analytics;
    }

    public static bool LogPerformance()
    {
    	return (Instance.logFilter & LogFilter.Performance) == LogFilter.Performance;
    }

    public static bool LogTestSuite()
    {
    	return (Instance.logFilter & LogFilter.TestSuite) == LogFilter.TestSuite;
    }

    public static bool LogTest()
    {
        return (Instance.logFilter & LogFilter.Test) == LogFilter.Test;
    }


}



#if UNITY_EDITOR
// �Զ���༭���ű��������޸� ExampleScript �� Inspector ������ʾ
[CustomEditor(typeof(ApplicationSettings))]
public class ApplicationSettingsEditor : Editor
{

    private bool boolParam;
    private int intParam;
    private string stringParam;
    private bool isAot;
    private bool isAutoHotfixUrl;
    private string hotfixUrl = "./";



    string GetHotfixUrl()  {
        string[] vers = ApplicationSettings.Instance.appVersion.Split('.');
        string rootFolder = "";
        if (ApplicationSettings.Instance.isRelease)
        {
            rootFolder = string.Join("_", vers);
        }
        else
        {
            rootFolder = vers[0];
        }
        string appType = ApplicationSettings.Instance.isRelease ? "release" : "debug";
        return $"./{appType}/{EditorUserBuildSettings.activeBuildTarget.ToString().ToLower()}/{rootFolder}";
    }


    Color originalColor;

    public override void OnInspectorGUI()
    {
        // ����Ĭ�ϵ� Inspector ����
        DrawDefaultInspector();

        // ת��Ŀ�����Ϊ ExampleScript ����
        //ApplicationSettings exampleScript = (ApplicationSettings)target;


        // ===============================================================

        /*
        // ��ʼһ����ֱ������
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("���Բ���", EditorStyles.boldLabel);

        // �����������͵������ֶ�
        boolParam = EditorGUILayout.Toggle("��������", boolParam);
        // �����������͵������ֶ�
        intParam = EditorGUILayout.IntField("��������", intParam);
        // �����ַ������͵������ֶ�
        stringParam = EditorGUILayout.TextField("�ַ�������", stringParam);

        // ����һ����ť
        if (GUILayout.Button("ȷ��"))
        {
            //target.CreatVersion();
        }
        // ������ֱ������
        EditorGUILayout.EndVertical();
        */


        // ===============================================================


        // ��ʼһ����ֱ������
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        originalColor = GUI.contentColor;
        GUI.contentColor = Color.green;
        GUILayout.Label("����app�汾", EditorStyles.boldLabel);
        GUI.contentColor = originalColor;


        isAot = EditorGUILayout.Toggle("�Ƿ��޸�AOT����", isAot);

        isAutoHotfixUrl = EditorGUILayout.Toggle("�Ƿ��Զ�����Զ���ȸ�Ŀ¼", isAutoHotfixUrl);
        hotfixUrl = EditorGUILayout.TextField("Ĭ��Զ���ȸ�Ŀ¼", hotfixUrl);


        // ����һ����ť
        if (GUILayout.Button("ȷ��"))
        {
            CreatVersion(isAot);
        }
        // ������ֱ������
        EditorGUILayout.EndVertical();



        // ===============================================================
        // ��ʼһ����ֱ������
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        originalColor = GUI.contentColor;
        GUI.contentColor = Color.green;
        GUILayout.Label("�ع�app�汾", EditorStyles.boldLabel);
        GUI.contentColor = originalColor;

        // ����һ����ť
        if (GUILayout.Button("ȷ��"))
        {
            GobackVersion();
        }
        // ������ֱ������
        EditorGUILayout.EndVertical();


        // ===============================================================
        // ��ʼһ����ֱ������
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        originalColor = GUI.contentColor;
        GUI.contentColor = Color.green;
        GUILayout.Label("ͬ��app�汾", EditorStyles.boldLabel);
        GUI.contentColor = originalColor;

        // ����һ����ť
        if (GUILayout.Button("ȷ��"))
        {
            GetVersion();
        }
        // ������ֱ������
        EditorGUILayout.EndVertical();

    }




    public void CreatVersion(bool isChangeAot = false)
    {
        DateTime localDateTime = DateTimeOffset.UtcNow.LocalDateTime;
        string ms = localDateTime.ToString("yyyyMMddHHmmss");

        string appType = ApplicationSettings.Instance.isRelease ? "release" : "debug";

        string buildTarget = ApplicationSettings.Instance.isMachine ? "machine" : EditorUserBuildSettings.activeBuildTarget.ToString().ToLower();

        ApplicationSettings.Instance.appKey = $"{ApplicationSettings.Instance.platformName}_{appType}_{buildTarget}_{ms}";



        #region �޸� total_version

        JObject totalVersionSAFile = JObject.Parse(File.ReadAllText(PathHelper.totalVersionSAPTH));
        JArray lst = totalVersionSAFile["data"] as JArray;



        string lastAppKey = (lst[0] as JObject)["app_key"].ToObject<string>();
        string lastAppVersion = (lst[0] as JObject)["app_version"].ToObject<string>();


        string[] lastAppKeyInfos = lastAppKey.Split('_');
        string[] lastAppVerInfos = lastAppVersion.Split('.');

        string targetAppVer = "";

        if (isChangeAot)
        {
            string v1 = ApplicationSettings.Instance.isRelease ? "1" : "0";
            targetAppVer = $"{int.Parse(lastAppVerInfos[0]) + 1}.{v1}.0";
        }
        else
        {
            string v1 = lastAppVerInfos[1];
            int v1d = int.Parse(v1) + 1;
            //�Ƿ���ż��
            bool isEvenNumber = v1d % 2 == 0;
            if (ApplicationSettings.Instance.isRelease && isEvenNumber)
                v1d++;
            else if (!ApplicationSettings.Instance.isRelease && !isEvenNumber)
                v1d++;
            targetAppVer = $"{lastAppVerInfos[0]}.{v1d}.0";
        }
        ApplicationSettings.Instance.appVersion = targetAppVer;

        JObject nodeItem = new JObject();
        nodeItem.Add("agent_name", ApplicationSettings.Instance.agentName);
        //nodeItem.Add("app", $"{ApplicationSettings.Instance.appKey}.apk");
        nodeItem.Add("app", $"--");
        nodeItem.Add("app_key", ApplicationSettings.Instance.appKey);
        nodeItem.Add("app_version", ApplicationSettings.Instance.appVersion);
        nodeItem.Add("version_suggest", null);

        if (isAutoHotfixUrl)
        {
            nodeItem.Add("hotfix_url", GetHotfixUrl());
        }
        else
        {
            nodeItem.Add("hotfix_url", hotfixUrl);
        }

        lst.Insert(0, nodeItem);

        totalVersionSAFile["updated_at"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        string content = totalVersionSAFile.ToString();
        File.WriteAllText(PathHelper.totalVersionSAPTH, content);
        #endregion

        AssetDatabase.Refresh();
    }



    public void GobackVersion()
    {

        JObject totalVersionSAFile = JObject.Parse(File.ReadAllText(PathHelper.totalVersionSAPTH));
        JArray lst = totalVersionSAFile["data"] as JArray;

        lst.RemoveAt(0);//�ع�

        JObject target = lst[0] as JObject;
        string appVersion = target["app_version"].ToObject<string>();


        string appKey = target["app_key"].ToObject<string>();
        string[] appKeyInfos = appKey.Split('_');

        ApplicationSettings.Instance.appKey = appKey;
        ApplicationSettings.Instance.isMachine = appKeyInfos[2] == "machine";
        ApplicationSettings.Instance.isRelease = appKeyInfos[1] == "release";
        ApplicationSettings.Instance.agentName = target["agent_name"].ToObject<string>();
        ApplicationSettings.Instance.appVersion = appVersion;

        string content = totalVersionSAFile.ToString();
        File.WriteAllText(PathHelper.totalVersionSAPTH, content);

        AssetDatabase.Refresh();
    }



    public void GetVersion()
    {

        JObject totalVersionSAFile = JObject.Parse(File.ReadAllText(PathHelper.totalVersionSAPTH));
        JArray lst = totalVersionSAFile["data"] as JArray;

        JObject target = lst[0] as JObject;
        string appVersion = target["app_version"].ToObject<string>();

        string appKey = target["app_key"].ToObject<string>();
        string[] appKeyInfos = appKey.Split('_');

        ApplicationSettings.Instance.appKey = appKey;
        ApplicationSettings.Instance.isMachine = appKeyInfos[2] == "machine";
        ApplicationSettings.Instance.isRelease = appKeyInfos[1] == "release";
        ApplicationSettings.Instance.agentName = target["agent_name"].ToObject<string>();
        ApplicationSettings.Instance.appVersion = appVersion;

        AssetDatabase.Refresh();
    }

}
#endif