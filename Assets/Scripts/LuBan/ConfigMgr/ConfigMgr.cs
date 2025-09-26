using cfg;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ConfigMgr
{
    public static readonly Tables Config;
    private static ConfigMgr instance;
    public static ConfigMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ConfigMgr();
            }
            return instance;
        }
    }

    public void Init()
    {
        //空调用 构造函数会自动初始化
    }

    static ConfigMgr()
    {
        Config = new Tables(ConfigLoder);
    }

    public string GetLan(int id)
    {
        string str;
        if(LanguageData.Instance.LanT == LanguageData.LanType.CN)
        {
            str = ConfigMgr.Config.Language.Get(id).Cn;
        }
        else
        {
            str = ConfigMgr.Config.Language.Get(id).En;
        }
        return str;
    }

    private static JSONNode ConfigLoder(string file)
    {
#if UNITY_EDITOR
        return JSON.Parse(File.ReadAllText(Application.dataPath + "/GameRes/LuBan/GenerateDatas/bytes/" + file + ".json", System.Text.Encoding.UTF8));
#elif UNITY_ANDROID
        AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/GameRes/bytes/" + file + ".unity3d");
        TextAsset textAsset = assetBundle.LoadAsset<TextAsset>(file + ".json");
        return JSON.Parse(textAsset.text);
#else
        AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/GameRes/bytes/" + file + ".unity3d");
        TextAsset textAsset = assetBundle.LoadAsset<TextAsset>(file + ".json");
        return JSON.Parse(textAsset.text);
#endif
    }
}
