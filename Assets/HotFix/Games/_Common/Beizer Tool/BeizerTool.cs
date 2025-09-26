using System.Collections.Generic;
using UnityEngine;

public class BeizerTool
{


    /// <summary>
    /// n�α���������
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
        //���㼯���ж೤�����ߵ�ÿһ�������Ҫ������ٴΡ�


        int n = temp.Length - 1;
        for (int i = 0; i < n; i++)
        {
            //���μ�����������ڵĶ���Ĳ�ֵ�������棬ÿ�μ��㶼����н��ס�ʣ����ٽ׼�����ٴΡ�ֱ���õ����һ���������ߡ�


            for (int j = 0; j < n - i; j++)
            {
                temp[j] = Vector3.Lerp(temp[j], temp[j + 1], t);
            }
        }
        //���ص�ǰ���������ߵĵ�
        return temp[0];
    }

}