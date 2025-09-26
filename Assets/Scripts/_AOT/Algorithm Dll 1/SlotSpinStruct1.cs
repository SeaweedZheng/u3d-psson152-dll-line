using System;
using System.Runtime.InteropServices;
using UnityEngine;


namespace SlotDllAlgorithmG152
{

    [StructLayout(LayoutKind.Sequential)] // ǿ���ֶΰ�����˳������
    public struct PullData
    {
        public double scorePull;           // ���Ღ����
        public double slotRemain;          // �����Ღ���
        public int slotPullTotal;
        public int slotPull;
        public double reelPullRemain;      // ת���Ღ���
        public int reelPull;               // ת���ۻ��Ღ��
        public double freeRemain;          // ����Ღ���
        public int freePullTotal;
        public int freePull;
    }




    /*

    [StructLayout(LayoutKind.Sequential)]
    public struct FgInfos
    {
        public byte freeGame;       // uint8_t
        public ushort freeCurRound; // uint16_t
        public ushort freeMaxRound; // uint16_t
        public uint reward;        // uint32_t
    }*/

    [StructLayout(LayoutKind.Sequential)]
    public struct ColorInfos
    {
        public byte npos;          // uint8_t
        public byte pos;           // uint8_t
        public long color;         // int64_t
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SlotData
    {
        //public FgInfos fgInfo;                 // 9�ֽ�

        public byte linePos;                   // 1�ֽ�
        public byte linkPos;                   // 1
        public byte RewardPos;                 // 1
        public byte freePos;                   // 1
        public byte jpPos;                     // 1 �� �ۼ�5�ֽ�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public byte[] lines;                  // 11�ֽ�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] links;                  // 9�ֽ�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] rewards;                // 10�ֽ�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] frees;                  // 6�ֽ�

        public byte waveDir;                  // 1�ֽ�
        public sbyte waveLevel;               // 1�ֽ�
        public int waveScore;                 // 4�ֽ�
        public int waveBets;                  // 4�ֽ�
        public byte mWaveIdx;                 // 1�ֽ� �� �ۼ�32�ֽ�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public sbyte[] mWaveRaid;            // 32�ֽ�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ColorInfos[] mWaveUp;         // 20�ֽ� (2*10)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ColorInfos[] mWaveDown;        // 20�ֽ�

        public int totalBets;                // 4�ֽ�
        public int totalWins;                // 4�ֽ� �� �ܼ�141�ֽ�
    }












    [StructLayout(LayoutKind.Sequential)]
    public struct LinkData
    {
        public byte icon;       // uint8_t ͼ��
        public byte link;       // uint8_t ������
        public uint reward;     // uint32_t �÷�
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LinkInfo
    {
        public byte gameState;           // uint8_t ��Ϸ״̬
        public byte lineNum;             // uint8_t ����
        public ushort curRound;          // uint16_t ��ǰ�ִ�
        public ushort maxRound;          // uint16_t ����ִ�
        public ushort num;               // uint16_t ���
        public uint lineMark;            // uint32_t �߱��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] lottery;            // int32_t[4] �ʽ�״̬

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] jackpotlottery;  // double[4] �����ʽ�ֵ

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] jackpotout;      // double[4] �ʽ���ʾ���۷�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] jackpotold;      // double[4] ����ǰ���۷�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public LinkData[] linkData;      // LinkData[3] ��������
    }











    [StructLayout(LayoutKind.Sequential)]
    public struct JackpotInitInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] jackpotout;  // �ʽ���ʾ���۷�
    }






    [StructLayout(LayoutKind.Sequential)]
    public struct FreeGameSummary
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] round;      // �����Ϸ����

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] counter;    // ����ռ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] score;      // �����Ϸ�÷�
    }

    [StructLayout(LayoutKind.Sequential)] // ��ע�⡿��Ҫ , Pack = 1
    public struct IconSummary
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]  // 10*3
        public int[] counter;    // ���ߴ�������ά����չƽ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public int[] score;      // ���ߵ÷�    // int32_t score[10][3];       //���ߵ÷�

        // ��ά�������ʾ����
        // counter[i*3 + j] ��Ӧ C++ �� counter[i][j]
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JpSummary
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]  // 4*4
        public int[] counter;    // �ʽ����   //counter[4][4];      //4����λ��4���ʽ����

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] score;      // �ʽ�÷�  // score[4][4];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct StoreSummary
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] reelTotalPull;      // ת���ۻ��Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] linkTotalPull;     // �����ۻ��Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] linkPull;          // �����Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] linkTotalScore;    // �����ܵ÷���

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] freeTotalPull;     // ����ۻ��Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] freePull;          // ����Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] freeTotalScore;    // �����Ϸ�÷���
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Summary
    {
        public FreeGameSummary freeGameSummary;
        public IconSummary iconSummary;
        public JpSummary jpSummary;
        public StoreSummary storeSummary;
    }









    [StructLayout(LayoutKind.Sequential)]
    public struct JackpotBase
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfBaseValue;           // ����ֵ

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfLowValue;            // ��С����ֵ

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfHighValue;           // ��󴥷�ֵ

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfMidValue;            // ƽ������ֵ

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] mBetThreshold;            // ���ѹ��ֵ

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] mBetThresholdMax;        // ���ѹ��ֵ

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public int[] mJackpotTrigger;          // ��ά����չƽ [4][10]

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] mJackpotIdx;             // �ʽ�����

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfOutPercent;         // ��ʾ�ٷֱ�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerPercent;       // ���ذٷֱ�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] mJpWeight;               // �ʽ����

        public int mJpTotalWeight;             // �����ܺ�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mJpTriggerDiff;       // ������ֵ

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfRangeValue;         // ������Χ
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JackpotTotal
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfPullRemainCent;     // �������

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfOutPullRemainCent;  // ��ʾ�������

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerPullRemainCent; // �����������

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfJpCent;             // �����ʽ����

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerRemainCent;    // �ڲ�ʣ�����

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerClPullCent;    // �ڲ��ʽ��Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfTotalCent;          // �ۻ���

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfOutTotalCent;       // ��ʾ�ۻ���

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfTotalLottery;       // ���۷���

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfOutPullCent;        // ��ʾ�Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerPullCent;      // �����Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfOutTotalPullCent;   // �ۼ��Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerTotalPullCent; // �����ۼ��Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfTotalPullCent;      // �ۼ���η�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfJpPullCent;         // �ʽ���η�

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfCompensateCent;     // ��������

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfCompensatePullCent; // �����Ღ��

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfCompensatePullRemainCent; // ��������

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfDiffCent;           // ��ֵ

        public double mfPullRemainCentTotal;  // ���������
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JackpotData
    {
        public JackpotBase mJackpotBase;     // ��������
        public JackpotTotal mJackpotTotal;   // ��������
    }

}