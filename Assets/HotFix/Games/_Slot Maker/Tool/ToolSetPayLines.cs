using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(ToolSetPayLines))]
public class ToolSetPayLinesEditor : Editor
{
    int intParam;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ToolSetPayLines script = target as ToolSetPayLines; // 绘制滚动条

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        //intParam = EditorGUILayout.IntField("输入游戏id", intParam);
        if (GUILayout.Button("修改线数据"))
        {
            //script.SetPayLines(intParam);
            script.SetPayLines();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        if (GUILayout.Button("显示颜色"))
        {
            script.GetColor();
        }
        EditorGUILayout.EndVertical();

    }
}


#endif
public class ToolSetPayLines : MonoBehaviour
{


    public int row = 0;

    public int column = 0;

    public int symbolHeight = 0;

    public int symbolWidth = 0;

    public int spacingX = 0;

    public int spacingY = 0;

    public int gameId = -1;

    /// <summary> 总共点的个数 </summary>
    public int dotNum => column + 2;

    //0,1,2,3,4,5



    static readonly string[] colors = new string[] { "#64D1FF", "#ED7D31", "#00FF00", "#00FFC0", "#8448FF", "#9000FF", "#FF50E8", "#FF7FC1", "#00FCFF", "#0048FF", "#00B4FF", "#FAE801", "#9CFF00", "#00FFDC", "#CC8AFF", "#D800FF", "#FF8EF0", "#E3002B", "#64D1FF", "#ED7D31", "#0048FF", "#00BE00", "#FFD200", "#5400FF", "#AD43FF", "#FF00DE", "#FF0084", "#FFAC53", "#00B4FF", "#FAE801" };
    public void SetPayLines()
    {
#if UNITY_EDITOR

        if (gameId == -1 || row == 0 || column == 0 || symbolHeight == 0 || symbolWidth == 0)
        {
            Debug.LogError("请先配置参数");
            return;
        }


        string path = ConfigUtils.GetGameInfoURL(gameId);
            //$"Assets/GameRes/_Common/Game Maker/ABs/Datas/{gameId}/game_info_g{gameId}.json";
        TextAsset jsn = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);

        JObject gameInfoNode = JObject.Parse(jsn.text);
        JArray payLinesInfo = gameInfoNode["pay_lines"] as JArray;


        Debug.Log($"pay_lines = {payLinesInfo.ToString()}");
        // payLinesInfo.Count

        Transform tfmChd = transform.GetChild(0);

        for (int i = transform.childCount; i < payLinesInfo.Count; i++)
        {
            Transform tfm = Instantiate(tfmChd);
            tfm.SetParent(transform);
            tfm.localPosition = Vector3.zero;
            tfm.localScale = Vector3.one;
        }





        List<LineRenderer> lr = new List<LineRenderer>();

        int number = 0;
        foreach (Transform tfmCld in transform)
        {
            lr.Add(tfmCld.GetComponent<LineRenderer>());
            tfmCld.name = $"Line ({++number})";
        }


        // int  row *2 /2 = 

        int totalHigh = row * symbolHeight + (row - 1) * spacingY;

        int totalWidth = column * symbolWidth + (column - 1) * spacingX;

        int centerX = totalWidth / 2;

        int centerY = totalHigh / 2;


        int smh0_5 = symbolHeight / 2;
        int smw0_5 = symbolWidth / 2;


        int colorIdx = -1;
        for (int i = 0; i < lr.Count; i++)
        {
            // 设置颜色
            if (++colorIdx >= colors.Length)
                colorIdx = 0;
            Color col = FromHex(colors[colorIdx]);
            lr[i].startColor = col;
            lr[i].endColor = col;


            // 设置位置
            lr[i].positionCount = dotNum;
            JArray lineInfo = payLinesInfo[i] as JArray;

            int z = -(i + 1);
            /*
             * 5列就有5+2个点
             * 在滚轮的左上角为原点，Y轴向上，X轴向右。
             * [0,1,2,1,0]  索引为列，值为行。
             * 计算每个中奖图标的x,y。再用中点减去该x,y的到最终的值
             */
            for (int dotIdx = 1; dotIdx < dotNum - 1; dotIdx++)
            {
                int colIdx = dotIdx - 1;
                int rowIdx = lineInfo[colIdx].ToObject<int>();
                int x = (1 + colIdx * 2) * smw0_5 + spacingX * colIdx;
                int y = (1 + rowIdx * 2) * smh0_5 + spacingY * rowIdx;

                int x1 = x - centerX;
                int y1 = centerY - y;

                Debug.Log($"j={dotIdx - 1} data={lineInfo.ToString()} sw0.5= {smw0_5}  sh0.5= {smh0_5} centerX= {centerX}  centerY= {centerY} x={x} ,y={y} totalWidth={totalWidth} totalHigh={totalHigh} x1={x1} y1={y1}");


                lr[i].SetPosition(dotIdx, new Vector3(x1, y1, z));
            }

            {

                int rowIdx0 = lineInfo[0].ToObject<int>();
                int y0 = (1 + rowIdx0 * 2) * smh0_5 + spacingY * rowIdx0;
                y0 = centerY - y0;
                lr[i].SetPosition(0, new Vector3(0 - centerX, y0, z));
            }

            {
                int rowIdx0 = lineInfo[lineInfo.Count - 1].ToObject<int>();
                int y0 = (1 + rowIdx0 * 2) * smh0_5 + spacingY * rowIdx0;
                y0 = centerY - y0;
                lr[i].SetPosition(dotNum - 1, new Vector3(totalWidth - centerX, y0, z));

            }



            // X -- 宽度  -- 水平
            // Y -- 高度  -- 垂直
        }

#endif


    }


    public void GetColor()
    {
        string res = "[";
        for (int i = 0; i < 30; i++)
        {
            LineRenderer lr = transform.GetChild(i).GetComponent<LineRenderer>();
            res += $"\"{ToHex(lr.startColor)}\",";
        }
        res += "];";
        Debug.Log(res);
    }




    public static Color FromHex(string hex = "#666666")
    {
        if (hex.StartsWith("#"))
            hex = hex.Substring(1);

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        // 若包含透明度信息
        byte a = 255;
        if (hex.Length >= 8)
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color32(r, g, b, a);
    }


    // 将 Color 转换为 #RRGGBB 格式
    public static string ToHex(Color color)
    {
        return string.Format("#{0:X2}{1:X2}{2:X2}",
            Mathf.RoundToInt(color.r * 255),
            Mathf.RoundToInt(color.g * 255),
            Mathf.RoundToInt(color.b * 255));
    }

    // 将 Color 转换为 #RRGGBBAA 格式（包含透明度）
    public static string ToHexWithAlpha(Color color)
    {
        return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}",
            Mathf.RoundToInt(color.r * 255),
            Mathf.RoundToInt(color.g * 255),
            Mathf.RoundToInt(color.b * 255),
            Mathf.RoundToInt(color.a * 255));
    }


}



