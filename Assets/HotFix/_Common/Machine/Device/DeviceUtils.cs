using UnityEngine;
using MoneyBox;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using Newtonsoft.Json;
public static class DeviceUtils
{

    /// <summary>
    /// ������л����ܳ�������Ʊ
    /// </summary>
    /// <returns></returns>
    public static int GetCoinOutNum()
    {
        int coinOutNum = 0;
        // �����ܻ����ٸ���
        if (_consoleBB.Instance.coinOutScaleCreditPerTicket > 1)
        {
            coinOutNum = (int)_consoleBB.Instance.myCredit / _consoleBB.Instance.coinOutScaleCreditPerTicket;
        }
        else if (_consoleBB.Instance.coinOutScaleTicketPerCredit > 1)
        {
            coinOutNum = (int)_consoleBB.Instance.myCredit * _consoleBB.Instance.coinOutScaleTicketPerCredit;
        }else if (_consoleBB.Instance.coinOutScaleCreditPerTicket ==1 && _consoleBB.Instance.coinOutScaleTicketPerCredit ==1)
        {
            coinOutNum = (int)_consoleBB.Instance.myCredit;
        }
        else
        {
            DebugUtils.LogError($"�������� creditTickeOutScale = {_consoleBB.Instance.coinOutScaleTicketPerCredit}  ticketCreditOutScale = {_consoleBB.Instance.coinOutScaleCreditPerTicket}");
        }
        return coinOutNum;
    }



    /// <summary>
    /// ������л����ܳ�������Ʊ
    /// </summary>
    /// <returns></returns>
    public static int GetCoinOutNum(int credit)
    {
        int coinOutNum = 0;
        // �����ܻ����ٸ���
        if (_consoleBB.Instance.coinOutScaleCreditPerTicket > 1)
        {
            coinOutNum = credit / _consoleBB.Instance.coinOutScaleCreditPerTicket;
        }
        else if (_consoleBB.Instance.coinOutScaleTicketPerCredit > 1)
        {
            coinOutNum = credit * _consoleBB.Instance.coinOutScaleTicketPerCredit;
        }
        else if (_consoleBB.Instance.coinOutScaleCreditPerTicket == 1 && _consoleBB.Instance.coinOutScaleTicketPerCredit == 1)
        {
            coinOutNum = credit;
        }
        else
        {
            DebugUtils.LogError($"�������� creditTickeOutScale = {_consoleBB.Instance.coinOutScaleTicketPerCredit}  ticketCreditOutScale = {_consoleBB.Instance.coinOutScaleCreditPerTicket}");
        }
        return coinOutNum;
    }



    /*
    public int GetCoinOutNum(int Credit , float moneyPrtCoinOut)
    {
        // ����ҵ�����
        double coinOutCount = Credit / pricePerCoin;
    } */

    /// <summary>
    /// count��Ʊ �ܻ����ٻ���
    /// </summary>
    /// <returns></returns>
    public static int GetCoinOutCredit(int count)
    {
        int coinOutCredit = 0;
        if (_consoleBB.Instance.coinOutScaleCreditPerTicket > 1)
        {
            coinOutCredit = count * _consoleBB.Instance.coinOutScaleCreditPerTicket;
        }
        else if (_consoleBB.Instance.coinOutScaleTicketPerCredit > 1)
        {
            coinOutCredit = count / _consoleBB.Instance.coinOutScaleTicketPerCredit;
        }else if (_consoleBB.Instance.coinOutScaleCreditPerTicket ==1 && _consoleBB.Instance.coinOutScaleTicketPerCredit==1)
        {
            coinOutCredit = count * 1;
        }
        else
        {
            DebugUtils.LogError($"�������� creditTickeOutScale = {_consoleBB.Instance.coinOutScaleTicketPerCredit}  ticketCreditOutScale = {_consoleBB.Instance.coinOutScaleCreditPerTicket}");
        }
        return coinOutCredit;
    }



    /*public static MoneyBoxMoneyOutInfo GetMoneyOutInfo(int credit)
    {
        int coinOutCount = GetCoinOutNum(credit);

        float moneyPerCoinOut = (float)GlobalsManager.Instance.GetAttribute(GlobalsManager.MoneyPerCoinOut);


        int targetMoneyOut = 0;
        int targetCoinOutCount = 0;
        int targetCoinOutCredit = 0;

        for (int i = coinOutCount; i > 0; i--)
        {
            float totalAmount = i * moneyPerCoinOut;
            if (totalAmount == (int)totalAmount)
            {
                targetCoinOutCount = i;
                targetMoneyOut = (int)totalAmount;
                targetCoinOutCredit = GetCoinOutCredit(targetCoinOutCount);
                break;
            }
        }

        return new MoneyBoxMoneyOutInfo()
        {
            coinOutCount = targetCoinOutCount,
            coinOutCredit = targetCoinOutCredit,
            moneyOut = targetMoneyOut,
        };

    }*/



    public static MoneyBoxMoneyInfo GetMoneyInInfo(int money , float moneyPerCoinIn)
    {

        int asCoinInCount = (int)(money / moneyPerCoinIn);

        int asCrefit = _consoleBB.Instance.coinInScale * asCoinInCount;

        return new MoneyBoxMoneyInfo()
        {
            asCoinInCount = asCoinInCount,
            asCredit = asCrefit,
            money = money,
        };
    }


    public static MoneyBoxMoneyInfo GetMoneyOutInfo(int credit)
    {
        //int coinOutCount = GetCoinOutNum(credit);

        int coinInCount = credit / _consoleBB.Instance.coinInScale;

        //float moneyPerCoinOut = (float)GlobalsManager.Instance.GetAttribute(GlobalsManager.MoneyPerCoinOut);
        float momeyPerCoinIn = MoneyBoxModel.Instance.moneyPerCoinIn;

        int targetMoneyOut = 0;
        int targetAsCoinInCount = 0;
        int targetAsCoinInCredit = 0;

        for (int i = coinInCount; i > 0; i--)
        {
            float totalAmount = i * momeyPerCoinIn;
            if (totalAmount == (int)totalAmount)
            {
                targetAsCoinInCount = i;
                targetMoneyOut = (int)totalAmount;
                targetAsCoinInCredit = targetAsCoinInCount * _consoleBB.Instance.coinInScale;
                break;
            }
        }

        return new MoneyBoxMoneyInfo()
        {
            asCoinInCount = targetAsCoinInCount,
            asCredit = targetAsCoinInCredit,
            money = targetMoneyOut,
        };

    }
    //        



    /// <summary>
    /// ��ǰ��ӡ��
    /// </summary>
    /// <returns></returns>
    public static PrinterSelect GetCurSelectPrinter()
    {
        string curSelectPrinter = _consoleBB.Instance.sboxPrinterList[_consoleBB.Instance.selectPrinterNumber];

        curSelectPrinter = curSelectPrinter.ToLower();

        if (curSelectPrinter.Contains("itc"))
        {
            return PrinterSelect.ITC__GP_58CR;
        }
        if (curSelectPrinter.Contains("pti"))
        {
            return PrinterSelect.PTI__Phoenix;
        }
        if (curSelectPrinter.Contains("950") && curSelectPrinter.Contains("epic"))
        {
            return PrinterSelect.ITHACA__Epic950;
        }
        if (curSelectPrinter.Contains("950") && curSelectPrinter.Contains("jcm"))
        {
            return PrinterSelect.JCM950;
        }

        return PrinterSelect.None;
    }

    /// <summary>
    /// ��ǰ��ѡ��sas��ӡ����
    /// </summary>
    /// <returns></returns>
    public static bool IsCurSasPrinter()
    {
        PrinterSelect pSele = GetCurSelectPrinter();
        return pSele == PrinterSelect.ITHACA__Epic950 || pSele == PrinterSelect.JCM950;
    }

    public static bool IsCurQRCodePrinter()
    {
        PrinterSelect pSele = GetCurSelectPrinter();
        return pSele == PrinterSelect.PTI__Phoenix;
    }



    /// <summary>
    /// ��ǰѡ���ֽ����
    /// </summary>
    /// <returns></returns>
    public static BillerSelect GetCurSelectBiller()
    {
        string curSelectBiller01 = _consoleBB.Instance.sboxBillerList[_consoleBB.Instance.selectBillerNumber];

        string curSelectBiller = curSelectBiller01.ToLower();
        //Debug.LogError($"@ GetCurSelectBiller = {curSelectBiller01}  == {curSelectBiller} == {_consoleBB.Instance.selectBillerNumber} == {JsonConvert.SerializeObject(_consoleBB.Instance.sboxBillerList)}");

        if (curSelectBiller.Contains("mei"))
        {
            return BillerSelect.MEI__AE2831_D5;
        }
        if (curSelectBiller.Contains("pyramid") && curSelectBiller.Contains("5000"))
        {
            return BillerSelect.PYRAMID__APEX_5000_SERIES;
        }
        if (curSelectBiller.Contains("pyramid") && curSelectBiller.Contains("7000"))
        {
            return BillerSelect.PYRAMID__APEX_7000_SERIES;
        }
        if (curSelectBiller.Contains("ict") )
        {
            return BillerSelect.ICT__PA7_TAO;
        }
        return BillerSelect.None;
    }


    /// <summary>
    /// ��ǰ��ѡ��sasֽ������
    /// </summary>
    /// <returns></returns>
    public static bool IsCurSasBiller()
    {
        BillerSelect bSele = GetCurSelectBiller();
        return bSele == BillerSelect.MEI__AE2831_D5;
    }


}


public enum PrinterSelect
{
    None,
    /// <summary> ��ͨ��ӡ�� </summary>
    ITC__GP_58CR,
    /// <summary> ��ά���ӡ�� </summary>
    PTI__Phoenix,
    /// <summary> Sas 950��ӡ�� </summary>
    ITHACA__Epic950,
    /// <summary> Sas 950��ӡ��</summary>
    JCM950,
}



public enum BillerSelect
{
    None,
    /// <summary> Sas ֽ���� </summary>
    MEI__AE2831_D5,
    /// <summary> PYRAMIDֽ���� </summary>
    PYRAMID__APEX_5000_SERIES,
    /// <summary> PYRAMIDֽ���� </summary>
    PYRAMID__APEX_7000_SERIES,
    /// <summary> ��ֽͨ����</summary>
    ICT__PA7_TAO,
}



/// <summary>
/// Ǯ��������Ǯ�Ľ��
/// </summary>
public class MoneyBoxMoneyInfo
{
    /// <summary> ��ǰ�ĳ�Ʊ����Ͷ����ٸ��� </summary>
    public int asCoinInCount;
    /// <summary> ��ǰ�ĳ�Ʊ���ƶ�����һ��� </summary>
    public int asCredit;
    /// <summary> �����˵���Ǯ��ֵ </summary>
    public int money;
}