using System.Collections.Generic;
using UnityEngine;

public class BeizerTool
{


    /// <summary>
    /// n次贝塞尔调用
    /// </summary>
    /// <param name="vertex"></param>
    /// <param name="vertexCount"></param>
    /// <returns></returns>
    public static Vector3[] GetBeizerList_n(Vector3[] vertex, int vertexCount)
    {
        List<Vector3> pointList = new List<Vector3>();
        pointList.Clear();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            pointList.Add(Bezier_n(vertex, ratio));
        }
        //  pointList.Add(vertex[vertex.Length - 1]);


        return pointList.ToArray();
    }

    private static Vector3 Bezier_n(Vector3[] vecs, float t)
    {
        Vector3[] temp = new Vector3[vecs.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = vecs[i];
        }
        //顶点集合有多长，曲线的每一个点就需要计算多少次。


        int n = temp.Length - 1;
        for (int i = 0; i < n; i++)
        {
            //依次计算各两个相邻的顶点的插值，并保存，每次计算都会进行降阶。剩余多少阶计算多少次。直到得到最后一条线性曲线。


            for (int j = 0; j < n - i; j++)
            {
                temp[j] = Vector3.Lerp(temp[j], temp[j + 1], t);
            }
        }
        //返回当前比例下曲线的点
        return temp[0];
    }

}