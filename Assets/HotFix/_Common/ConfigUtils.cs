using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ConfigUtils
{
    /// <summary> ��Ϸid </summary>
    public static int curGameId => MainBlackboard.Instance.gameID;


    /// <summary> �������ļ�����������׺��</summary>
    public static readonly string[] i18nLoadFile = new string[] { "i18n_po152", "i18n_console001" };

    /// <summary> ��Ϸ�����ļ�·�� </summary>
    public static string GetGameInfoURL(int gameId) => $"Assets/GameRes/_Common/Game Maker/ABs/{gameId}/Datas/game_info_g{gameId}.json";
    //$"Assets/GameRes/_Common/Game Maker/ABs/Datas/{gameId}/game_info_g{gameId}.json";

    /// <summary> ��ϷͼƬ·�� </summary>
    public static string GetGameAvatarURL(int gameId) => $"Assets/GameRes/_Common/Game Maker/ABs/{gameId}/Game Icon/Game Avatar {gameId}.png";
    //$"Assets/GameRes/_Common/Game Maker/ABs/Datas/{gameId}/game_info_g{gameId}.json";


    /// <summary> ��ϷGM�ļ�·�� </summary>
    public static string GetGameGMURL(int gameId) =>
        $"Assets/GameRes/_Common/Game Maker/ABs/{gameId}/GM/tmg_mock_gm_g{gameId}.json";
    //$"Assets/GameRes/_Common/Game Maker/ABs/{gameId}/Game Icon/Game Avatar {gameId}.png";

    /// <summary> ��Ϸ��Ϣ </summary>      
    public static string curGameGMURL => GetGameGMURL(curGameId);

    /// <summary> ��Ϸ��Ϣ </summary>
    public static string curGameInfoURL => GetGameInfoURL(curGameId);

    /// <summary> ��Ϸͷ�� </summary>
    public static string curGameAvatarURL => GetGameAvatarURL(curGameId);


    /// <summary> ��ȡͼ��·�� </summary>
    public static string GetSymbolUrl(int symbolNumber) {

        string numberStr;
        if (symbolNumber < 10)
            numberStr = $"0{symbolNumber}";
        else
            numberStr = $"{symbolNumber}";

        return $"Assets/GameRes/Games/PssOn00152 (1080x1920)/ABs/Sprites/Symbol Icon/symbol_{numberStr}.png";
    }
    public static string GetErrorCode() => "Assets/GameRes/_Common/Game Maker/ABs/Datas/error_code.json";



    /// <summary>
    /// ������Ѿ�������������Ϸ��Ӧ�������ļ���
    /// </summary>
    /// <param name="gameId"></param>
    /// <returns></returns>
    public static string GetGMMockData(int gameId) => $"Assets/GameRes/_Common/Game Maker/ABs/Datas/{gameId}/GM Mock/mock_gm_g{gameId}.json";
}
