using GameMaker;
using SlotMaker;
using System.Collections.Generic;


[System.Serializable]
public class GameSenceData
{
    /// <summary> ��ֿ��������� </summary>
    public string respone;

    /// <summary> ���������ϱ���id�ţ��ۼӣ� </summary>
    public int reportId = 74282;

    /// <summary> �������ݵ�ʱ�䣨�뼶�� </summary>
    public long timeS = 0;

    /// <summary> ��Ϸ��ţ��ڼ�����Ϸ�� </summary>
    public long gameNumber = 0;  //guid

    /// <summary> ���������Ϸ�� ������Ϸid </summary>
    public long gameNumberFreeSpinTrigger = 0; //guid


    /// <summary> ��ʷ��Ӯ/��ʷ��ѹ </summary>
    /// float rtp = historyTotalBet == 0 ?  (float) historyTotalWin : (float) historyTotalWin / (float) historyTotalBet;
    public float rtp;

    /// <summary> ��ǰ�Ƿ��������Ϸ </summary>
    public bool isFreeSpin;

    /// <summary> ��ǰ�����Ϸ���ӵĴ��� </summary>
    public int freeSpinAddNum;

    /// <summary> ��ǰ���� </summary>
    public string curStripsIndex; // "BS" "FS"

    /// <summary> ��һ�ֹ��� </summary>
    public string nextStripsIndex;

    /// <summary> ���ֽ������� </summary>
    public string strDeckRowCol;

    /// <summary> ���ֽ������� - ���� </summary>
    public List<int> deckRowCol;

    /// <summary> ��ѵ��־��� </summary>
    public int freeSpinPlayTimes;

    /// <summary> �����Ϸ�ܾ��� </summary>
    public int freeSpinTotalTimes;

    /// <summary> �����Ϸ�ۼ���Ӯ </summary>
    public long freeSpinTotalWinCredit;

    /// <summary> ����Ӯ������ </summary>
    public List<SymbolWin> winList;

    /// <summary> 5�������� </summary>
    //public SymbolWin win5Kind;

    /// <summary> ���������Ϸ���� </summary>
    public SymbolWin winFreeSpinTrigger;

    /// <summary> ������Ӯ </summary>
    //public long totalWinCredit;

    /// <summary> ������ϷӮ </summary>
    public long baseGameWinCredit;

    /// <summary> �ʽ�Ӯ </summary>
    public long jackpotWinCredit;

    /// <summary> ��Ϸǰ���� </summary>
    public long creditBefore;

    /// <summary> ��Ϸ����� </summary>
    public long creditAfter;

    /// <summary> ������ѹ </summary>
    public long totalBet;

    /// <summary> �޽�ֵ </summary>
    public float jpGrand;
    /// <summary> ͷ��ֵ </summary>
    public float jpMajor;
    /// <summary> ��ֵ </summary>
    public float jpMinor;
    /// <summary> С��ֵ </summary>
    public float jpMini;

    /// <summary> �ʽ��н�Ӯ�� </summary>
    // public long jpWinCredit;

    /// <summary> �ʽ������� </summary>
    //public string jpWinName;

    /// <summary> �ʽ��еȼ� </summary>
    //public string jpWinLevel;

    /// <summary> �ʽ��н���Ϣ </summary>
    public JackpotWinInfo jpWinInfo;
}