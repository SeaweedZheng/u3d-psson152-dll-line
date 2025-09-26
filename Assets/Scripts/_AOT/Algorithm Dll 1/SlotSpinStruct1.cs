using System;
using System.Runtime.InteropServices;
using UnityEngine;


namespace SlotDllAlgorithmG152
{

    [StructLayout(LayoutKind.Sequential)] // 强制字段按声明顺序排列
    public struct PullData
    {
        public double scorePull;           // 已提拨分数
        public double slotRemain;          // 连线提拨余额
        public int slotPullTotal;
        public int slotPull;
        public double reelPullRemain;      // 转轮提拨余额
        public int reelPull;               // 转轮累积提拨库
        public double freeRemain;          // 免费提拨余额
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
        //public FgInfos fgInfo;                 // 9字节

        public byte linePos;                   // 1字节
        public byte linkPos;                   // 1
        public byte RewardPos;                 // 1
        public byte freePos;                   // 1
        public byte jpPos;                     // 1 → 累计5字节

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public byte[] lines;                  // 11字节

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] links;                  // 9字节

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] rewards;                // 10字节

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] frees;                  // 6字节

        public byte waveDir;                  // 1字节
        public sbyte waveLevel;               // 1字节
        public int waveScore;                 // 4字节
        public int waveBets;                  // 4字节
        public byte mWaveIdx;                 // 1字节 → 累计32字节

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public sbyte[] mWaveRaid;            // 32字节

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ColorInfos[] mWaveUp;         // 20字节 (2*10)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ColorInfos[] mWaveDown;        // 20字节

        public int totalBets;                // 4字节
        public int totalWins;                // 4字节 → 总计141字节
    }












    [StructLayout(LayoutKind.Sequential)]
    public struct LinkData
    {
        public byte icon;       // uint8_t 图标
        public byte link;       // uint8_t 连线数
        public uint reward;     // uint32_t 得分
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LinkInfo
    {
        public byte gameState;           // uint8_t 游戏状态
        public byte lineNum;             // uint8_t 线数
        public ushort curRound;          // uint16_t 当前轮次
        public ushort maxRound;          // uint16_t 最大轮次
        public ushort num;               // uint16_t 编号
        public uint lineMark;            // uint32_t 线标记

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] lottery;            // int32_t[4] 彩金状态

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] jackpotlottery;  // double[4] 开出彩金值

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] jackpotout;      // double[4] 彩金显示积累分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] jackpotold;      // double[4] 开出前积累分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public LinkData[] linkData;      // LinkData[3] 连线数据
    }











    [StructLayout(LayoutKind.Sequential)]
    public struct JackpotInitInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] jackpotout;  // 彩金显示积累分
    }






    [StructLayout(LayoutKind.Sequential)]
    public struct FreeGameSummary
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] round;      // 免费游戏轮数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] counter;    // 次数占比

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] score;      // 免费游戏得分
    }

    [StructLayout(LayoutKind.Sequential)] // 【注意】不要 , Pack = 1
    public struct IconSummary
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]  // 10*3
        public int[] counter;    // 连线次数（二维数组展平）

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public int[] score;      // 连线得分    // int32_t score[10][3];       //连线得分

        // 二维数组访问示例：
        // counter[i*3 + j] 对应 C++ 的 counter[i][j]
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JpSummary
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]  // 4*4
        public int[] counter;    // 彩金次数   //counter[4][4];      //4个档位的4个彩金次数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] score;      // 彩金得分  // score[4][4];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct StoreSummary
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] reelTotalPull;      // 转轮累积提拨库

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] linkTotalPull;     // 连线累积提拨库

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] linkPull;          // 连线提拨库

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] linkTotalScore;    // 连线总得分数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] freeTotalPull;     // 免费累积提拨库

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] freePull;          // 免费提拨库

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] freeTotalScore;    // 免费游戏得分数
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
        public double[] mfBaseValue;           // 基本值

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfLowValue;            // 最小触发值

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfHighValue;           // 最大触发值

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfMidValue;            // 平均触发值

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] mBetThreshold;            // 最低压分值

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] mBetThresholdMax;        // 最高压分值

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public int[] mJackpotTrigger;          // 二维数组展平 [4][10]

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] mJackpotIdx;             // 彩金索引

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfOutPercent;         // 显示百分比

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerPercent;       // 隐藏百分比

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] mJpWeight;               // 彩金比重

        public int mJpTotalWeight;             // 比重总和

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mJpTriggerDiff;       // 触发差值

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfRangeValue;         // 开奖范围
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JackpotTotal
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfPullRemainCent;     // 提拔余数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfOutPullRemainCent;  // 显示提拔余数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerPullRemainCent; // 隐藏提拔余数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfJpCent;             // 开出彩金分数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerRemainCent;    // 内部剩余积分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerClPullCent;    // 内部彩金提拨分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfTotalCent;          // 累积分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfOutTotalCent;       // 显示累积分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfTotalLottery;       // 积累分数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfOutPullCent;        // 显示提拨分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerPullCent;      // 隐藏提拨分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfOutTotalPullCent;   // 累计提拨分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfInnerTotalPullCent; // 隐藏累计提拨分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfTotalPullCent;      // 累计提拔分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfJpPullCent;         // 彩金提拔分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfCompensateCent;     // 补偿积分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfCompensatePullCent; // 补偿提拨分

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfCompensatePullRemainCent; // 补偿余数

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] mfDiffCent;           // 差值

        public double mfPullRemainCentTotal;  // 总提拔余数
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JackpotData
    {
        public JackpotBase mJackpotBase;     // 基础数据
        public JackpotTotal mJackpotTotal;   // 总量数据
    }

}