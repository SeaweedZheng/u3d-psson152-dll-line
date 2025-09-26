using GameMaker;
using SlotMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotDeckGenerator : MonoSingleton<SlotDeckGenerator>
{
    public string GenerateGameArray(List<List<int>> allLines, List<int> symbolNumber,
        List<WinningLineInfo> winningLines, int[] exclude, List<SymbolInclude> include)
    {
        if (winningLines == null)
            winningLines = new List<WinningLineInfo>();
        // ��ʼ����Ϸ�������
        List<List<int>>  gameResultList = new List<List<int>>();
        for (int raw = 0; raw < 3; raw++)
        {
            // Ϊÿ�д���һ������5��0�� List<int>�����������
            List<int> row = new List<int>();
            for (int col = 0; col < 5; col++)
            {
                row.Add(-1);
            }

            gameResultList.Add(row); // ������ӵ�������
        }

        List<int> excludeLst = new List<int>();
        excludeLst.AddRange(exclude);

        foreach (WinningLineInfo item in winningLines)
        {
            excludeLst.Add(item.SymbolNumber);

            int lineIndex = item.LineNumber - 1;

            List<int> line = allLines[lineIndex];

            for (int cIndex = 0; cIndex < item.WinCount; cIndex++)
            {
                int rIndex = line[cIndex];
                gameResultList[rIndex][cIndex] = item.SymbolNumber;
            }
        }

        foreach (SymbolInclude symbolInclude in include)
        {
            int colIdx = symbolInclude.colIdx;
            int rowIdx = symbolInclude.colIdx;

            if (colIdx == -1 && rowIdx == -1)
            {
                do
                {
                    colIdx = UnityEngine.Random.Range(0, 5);
                    rowIdx = UnityEngine.Random.Range(0, 3);
                } while (gameResultList[rowIdx][colIdx] != -1);
            }
            else if (colIdx == -1)
            {
                do
                {
                    colIdx = UnityEngine.Random.Range(0, 5);
                } while (gameResultList[rowIdx][colIdx] != -1);
            }
            else if (rowIdx == -1)
            {
                do
                {
                    rowIdx = UnityEngine.Random.Range(0, 3);
                } while (gameResultList[rowIdx][colIdx] != -1);
            }

            gameResultList[rowIdx][colIdx] = symbolInclude.symbolNumber;
        }

        for (int i = 0; i < 3; i++)
        {
            if (gameResultList[i][2] == -1)
            {
                int middleSymbolNumber = -1;
                do
                {
                    int symbolIdx = UnityEngine.Random.Range(0, symbolNumber.Count);
                    middleSymbolNumber = symbolNumber[symbolIdx];
                } while (excludeLst.Contains(middleSymbolNumber));

                excludeLst.Add(middleSymbolNumber);

                gameResultList[i][2] = middleSymbolNumber;
            }
        }


        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (gameResultList[i][j] == -1)
                {
                    int tempSymbolNumber = -1;
                    do
                    {
                        int symbolIdx = UnityEngine.Random.Range(0, symbolNumber.Count);
                        tempSymbolNumber = symbolNumber[symbolIdx];
                    } while (excludeLst.Contains(tempSymbolNumber));

                    gameResultList[i][j] = tempSymbolNumber;
                }
            }
        }

        string strDeckRowCol = SlotTool.GetDeckColRow(gameResultList);
        return strDeckRowCol;

    }



    public string GenerateGameArray(List<List<int>> allLines, List<int> symbolNumber,
        List<WinningLineInfo> winningLines, int[] exclude)
    {

        if (winningLines == null)
            winningLines = new List<WinningLineInfo>();

        // ��ʼ����Ϸ�������
        List<List<int>> gameResultList = new List<List<int>>();
        for (int raw = 0; raw < 3; raw++)
        {
            // Ϊÿ�д���һ������5��0�� List<int>�����������
            List<int> row = new List<int>();
            for (int col = 0; col < 5; col++)
            {
                row.Add(-1);
            }

            gameResultList.Add(row); // ������ӵ�������
        }


        List<int> excludeLst = new List<int>();
        excludeLst.AddRange(exclude);

        foreach (WinningLineInfo item in winningLines)
        {
            // public int LineNumber;   // ��·�ţ�1����·������
            //    public int SymbolNumber; // �н����ţ����ų��ڣ�
            //    public int WinCount;     // �н�������3-5���������ֻ��Ϊ1��
            excludeLst.Add(item.SymbolNumber);

            int lineIndex = item.LineNumber - 1;

            List<int> line = allLines[lineIndex];

            for (int cIndex = 0; cIndex < item.WinCount; cIndex++)
            {
                int rIndex = line[cIndex];
                gameResultList[rIndex][cIndex] = item.SymbolNumber;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (gameResultList[i][2] == -1)
            {
                int middleSymbolNumber = -1;
                do
                {
                    int symbolIdx = UnityEngine.Random.Range(0, symbolNumber.Count);
                    middleSymbolNumber = symbolNumber[symbolIdx];
                } while (excludeLst.Contains(middleSymbolNumber));

                excludeLst.Add(middleSymbolNumber);

                gameResultList[i][2] = middleSymbolNumber;
            }
        }


        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (gameResultList[i][j] == -1)
                {
                    int tempSymbolNumber = -1;
                    do
                    {
                        int symbolIdx = UnityEngine.Random.Range(0, symbolNumber.Count);
                        tempSymbolNumber = symbolNumber[symbolIdx];
                    } while (excludeLst.Contains(tempSymbolNumber));

                    gameResultList[i][j] = tempSymbolNumber;
                }
            }
        }

        string strDeckRowCol = SlotTool.GetDeckColRow(gameResultList);
        return strDeckRowCol;
    }


}


public class WinningLineInfo
{
    public int LineNumber; // ��·�ţ�1����·������
    public int SymbolNumber; // �н����ţ����ų��ڣ�
    public int WinCount; // �н�������3-5���������ֻ��Ϊ1��
}


public class SymbolInclude
{
    public int colIdx = -1;
    public int rowIdx = -1;
    public int symbolNumber = -1;
}