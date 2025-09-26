using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;

public class GameData
{

    public GameData(int normalTime, int unnormalTime)
    {
        this.normalTime = normalTime;
        this.unnormalTime = unnormalTime;
    }

    /// <summary> 采集的一般局数据个数 </summary>
    int normalTime = 5;

    /// <summary> 采集的特殊局数据个数 </summary>
    int unnormalTime = 5;

    /// <summary> 采集到的一般局数据列表 </summary>
    List<string> normalDatas = new List<string>();

    /// <summary> 采集到的特殊局数据列表 </summary>
    List<string> unnormalDatas = new List<string>();


    /// <summary> 历史数据队列 </summary>
    List<int> datasLength = new List<int>();


    /// <summary> 特殊数据的阀值 </summary>
    double gap = 0.02;

    public List<string> Add(string id, string protocol, object obj, bool isForce = false, string mark = "")
    {

        if (isForce == false && this.normalDatas.Count >= this.normalTime && this.unnormalDatas.Count >= this.unnormalTime) return null;

        string str = obj is string ? obj as string : JSONNodeUtil.ObjectToJsonStr(obj);

        this.datasLength.Insert(0, str.Length);

        //去掉最大的两个数据
        if (this.datasLength.Count > 10)
        {
            this.datasLength.RemoveRange(10, this.datasLength.Count - 10);
        }

        List<int> temp = new List<int>(this.datasLength);
        temp.Sort();//从小到大
        if (temp.Count > 6)
        {
            // 计算要删除的最后一个元素的索引  
            int lastIndexToRemove = temp.Count - 1;
            // 删除从倒数第二个元素开始到列表末尾的所有元素  
            temp.RemoveRange(lastIndexToRemove - 1, 2);
        }

        int sum = temp.Sum();
        double average = (double)sum / temp.Count;  // 获取均值数据
        double dat = (str.Length - average) / average;

        string name = $"g{id}__{protocol}";
        if (dat > this.gap)
        {
            if (this.unnormalDatas.Count + 1 <= this.unnormalTime || isForce == true)
            {
                this.unnormalDatas.Add(str);

                name = $"{name}__ko_{mark}{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

                DebugUtils.LogWarning($"@数据差值：{dat}  gap:{this.gap}  name:{name}");

                return new List<string> { name, str };
            }
        }
        else
        {
            if (this.normalDatas.Count + 1 <= this.normalTime || isForce == true)
            {
                this.normalDatas.Add(str);

                name = $"{name}__{mark}{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

                DebugUtils.LogWarning($"@数据差值：{dat}  gap:{this.gap}  name:{name}");

                return new List<string> { name, str };
            }

        }
        DebugUtils.Log($"@数据差值：{dat}  gap:{this.gap}  不保存: {name}");
        return null;
    }

}



/// <summary>
/// 数据保存
/// </summary>

public class MockSaveManager : MonoSingleton<MockSaveManager>
{



    Dictionary<int, Dictionary<string, GameData>> allData = new Dictionary<int, Dictionary<string, GameData>>();






    String mark = "";

    /// <summary> 免费游戏标记（时间戳后） </summary>
    String fp_mark = "";
    int fp_index = 0;
    int fp_allCount = 0;



    #region 保存数据

    public void GetData(string protocol, object data, int gameId = -1, int normalTime = 30, int unnormalTime = 30)
    {
        string dataStr = data is string ? data as string : JSONNodeUtil.ObjectToJsonStr(data);

        if (!(dataStr.IndexOf("protocol_key") >= 0))
        {
            JSONNode _data = JSONNode.Parse(dataStr);
            JSONNode res = JSONNode.Parse("{}");
            res.Add("protocol_key", protocol);
            res.Add("data", _data);
            dataStr = res.ToString();
        }

        bool isForce = false;
        mark = "";
        JSONNode d = JSONNode.Parse(dataStr);

        // bool isCollected = false;

        if (d["data"].HasKey("contents"))
        {

            int game_id = d["data"]["contents"]["game_id"];
            int bonus_id = 0;
            int added_spin_count = 0;
            if (d["data"]["contents"]["bonus_result"].Count > 0)
            {
                bonus_id = d["data"]["contents"]["bonus_result"][0]["bonus_id"];  //判断bonus_id
                added_spin_count = d["data"]["contents"]["bonus_result"][0]["result"]?["added_spin_count"] ?? 0;  //判断是否免费游戏
            }

            // 金猪 -- 特殊的免费游戏
            if (game_id == 92 && bonus_id == 9201 && added_spin_count > 0)
            {
                //isCollected = true;
                fp_allCount += added_spin_count;
                if (fp_mark == "")
                {
                    fp_mark = CreatUniqueID();
                }
                DebugUtils.Log($" {protocol}  mark = {mark}");
            }

            // 白虎-- 特殊的免费游戏
            if (game_id == 149)
            {

                for (int i = 0; i < d["data"]["contents"]["bonus_result"].Count; i++)
                {
                    if (d["data"]["contents"]["bonus_result"][i]["bonus_id"] == 14903)
                    {
                        bonus_id = 14903;
                        added_spin_count = d["data"]["contents"]["bonus_result"][i]["result"]?["added_spin_count"] ?? 0;
                        break;
                    }
                }

                if (bonus_id == 14903 && added_spin_count > 0)
                {
                    //isCollected = true;
                    fp_allCount += added_spin_count;
                    if (fp_mark == "")
                    {
                        fp_mark = CreatUniqueID();
                    }
                    DebugUtils.Log($" {protocol}  mark = {mark}");

                }
            }


            // HAPPY_DOLLARS 开心美刀
            if (game_id == 93)
            {

                for (int i = 0; i < d["data"]["contents"]["bonus_result"].Count; i++)
                {
                    if (d["data"]["contents"]["bonus_result"][i]["bonus_id"] == 9300)
                    {
                        bonus_id = 9300;
                        break;
                    }
                }

                if (bonus_id == 9300)
                {
                    //isCollected = true;
                    fp_allCount = 12;

                    fp_mark = CreatUniqueID();

                    DebugUtils.Log($" {protocol}  mark = {mark}");

                }
            }

            if (fp_allCount > 0)
            {
                //isCollected = true;
                isForce = true;
                mark = $"spec__fp{fp_mark}_{fp_index}_";
                if (++fp_index == fp_allCount + 1)
                {
                    fp_allCount = 0;
                    fp_index = 0;
                    fp_mark = "";
                }
            }

            if (game_id == 137 && bonus_id == 13702 && added_spin_count > 0)
            {
                isForce = true;
                if (fp_mark == "")
                {
                    fp_mark = CreatUniqueID();
                }
                DebugUtils.Log($" {protocol}  mark = {mark}");
                mark = $"spec__fp{fp_mark}_all_free_game_";
            }


            // 一般的免费游戏
            if (isForce == false && d["data"]["contents"].HasKey("free_spin_info"))
            {
                if (d["data"]["contents"]["free_spin_info"]["total_count"] > 0) //免费游戏
                {
                    //isCollected = true;

                    isForce = true;

                    if (fp_mark == "")
                    {
                        fp_mark = CreatUniqueID();
                    }

                    mark = $"spec__fp{fp_mark}_{fp_index}_";
                    fp_index++;

                    DebugUtils.Log($" {protocol}  mark = {mark}");
                }
                else
                {
                    fp_mark = "";
                    fp_index = 0;
                }
            }


            if (isForce == false)
            {
                //isCollected = true;
                isForce = true;
                mark = $"force_";
            }
        }



        if (!this.allData.ContainsKey(gameId))
        {
            this.allData.Add(gameId, new Dictionary<string, GameData>());
        }
        if (!this.allData[gameId].ContainsKey(protocol))
        {
            this.allData[gameId].Add(protocol, new GameData(normalTime, unnormalTime));
        }

        string id = gameId == -1 ? "" : gameId.ToString();
        List<string> lst = this.allData[gameId][protocol].Add(id, protocol, dataStr, isForce, mark);


        //保存数据
        if (lst != null)
        {
            //this.WriteText(Application.persistentDataPath + $"/mock/g{id}/", $"g{id}__{protocol}__{lst[0]}.json", lst[1]);
            this.WriteText(Application.persistentDataPath + $"/mock/g{id}/", $"{lst[0]}.json", lst[1]);
        }

    }


    /// <summary>
    /// 时间搓后5位
    /// </summary>
    /// <returns></returns>
    private string CreatUniqueID()
    {
        string timestampStr = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        return timestampStr.Substring(timestampStr.Length - 5);
    }

    private void WriteText(string fileDif, string fileName, string message)
    {

        if (string.IsNullOrEmpty(fileDif) || string.IsNullOrEmpty(message)) return;

        if (!Directory.Exists(fileDif))
        {
            Directory.CreateDirectory(fileDif);
        }
        string textPath = Path.Combine(fileDif, fileName);
        DebugUtils.Log("保留数据： " + textPath);
        StreamWriter sw = null;
        if (!File.Exists(textPath))
        {
            sw = File.CreateText(textPath);
        }
        else
        {
            sw = File.AppendText(textPath);
        }
        sw.WriteLine(message + '\n');
        sw.Close();
        sw.Dispose();
    }


    #endregion










    [Button]
    public void testD()
    {
        var anonymousObj = new { name = "小明", age = 30 };

        // 获取匿名对象的类型  
        Type anonymousType = anonymousObj.GetType();

        // 获取 name 字段的值  
        PropertyInfo nameProperty = anonymousType.GetProperty("name");
        string nameValue = (string)nameProperty.GetValue(anonymousObj);
        DebugUtils.Log($"Name: {nameValue}");

        // 获取 age 字段的值  
        PropertyInfo ageProperty = anonymousType.GetProperty("age");
        int ageValue = (int)ageProperty.GetValue(anonymousObj);
        DebugUtils.Log($"Age: {ageValue}");
    }




}


