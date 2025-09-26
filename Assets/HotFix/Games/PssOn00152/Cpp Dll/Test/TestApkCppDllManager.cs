using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using UnityEngine;


/// <summary>
/// 包内dll测试
/// </summary>
public class TestApkCppDllManager
{
    static TestApkCppDllManager _instance;
    public static TestApkCppDllManager Instance
    {
        get
        {
            if (_instance == null)
            {
                if (_instance == null)
                {
                    _instance = new TestApkCppDllManager();
                }
            }
            return _instance;
        }
    }





    #region 测试接口
    // 定义与C++结构体匹配的C#结构体
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MyStruct
    {
        public int id;
        public float value;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string name;

    }

    // 导入返回结构体的函数
    [DllImport("slot-spin.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern MyStruct GetStructData();

    // 导入返回结构体指针的函数
    [DllImport("slot-spin.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetStructPointer();

    [DllImport("slot-spin.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void FreeStructPointer(IntPtr ptr);



    // 导入 DLL 函数
    [DllImport("slot-spin.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr GetVersion01();

    [DllImport("slot-spin.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FreeVersion01(IntPtr str);





    // 导入DLL函数
    [DllImport("slot-spin.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr DevGetScorePoolData();

    [DllImport("slot-spin.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void FreeScorePoolData(IntPtr pData);



    public  ScorePoolDataNative TestGetScorePoolData()
    {
        IntPtr pData = DevGetScorePoolData();
        try
        {
            ScorePoolDataNative res = (ScorePoolDataNative)Marshal.PtrToStructure(pData, typeof(ScorePoolDataNative));
            Debug.Log($"<color=green>结构体数据</color>:  DevGetScorePoolData= {JsonConvert.SerializeObject(res)}");
            return res;
        }
        finally
        {
            FreeScorePoolData(pData);
        }
    }



    // 封装为 C# 字符串
    public void ShowVersion()
    {

        try
        {

            IntPtr ptr = GetVersion01();
            string result = Marshal.PtrToStringAnsi(ptr);// 如果是 Unicode，用 PtrToStringUni
                                                            // FreeString(ptr); // 如果 DLL 提供了释放函数，且需要手动释放
            FreeVersion01(ptr); // 必须释放

            Debug.Log($"<color=green>结构体数据</color>:  GetVersion= {result}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }
    }

    public void TestGetStructData()   //public static void TestGetStructData()
    {
        try
        {
            // 方式1：直接获取结构体
            MyStruct data = GetStructData();
            Debug.Log($"<color=green>结构体数据</color>:  GetStructData = {JsonConvert.SerializeObject(data)}");

            // 方式2：通过指针获取结构体
            IntPtr ptr = GetStructPointer();
            try
            {
                MyStruct dataFromPtr = (MyStruct)Marshal.PtrToStructure(ptr, typeof(MyStruct));
                Debug.Log($"<color=green>结构体数据</color>:  PtrToStructure= {JsonConvert.SerializeObject(dataFromPtr)}");
            }
            finally
            {
                FreeStructPointer(ptr); // 释放内存
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }
    }

    #endregion




    #region SlotSpin

    // 导入C++ DLL函数
    [DllImport("slot-spin.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern SlotResultNative GetSlotSpin(uint bet);

    // 转换为托管对象
    public SlotResultNative TestGetSlotResult(uint bet) //public static SlotResultData GetSlotResult(uint bet)
    {
        // 调用原生函数获取数据
        SlotResultNative nativeResult = GetSlotSpin(bet);

        Debug.LogWarning("<color=green>结构体数据</color>: GetSlotSpin = " + JsonConvert.SerializeObject(nativeResult));

        return nativeResult;
    }



    #endregion

}










#if !false

// 原生结构体定义（保持与DLL完全一致）
[StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct WinLineNative
    {
        public int use_multiplier;
        public byte icon_id;
        public byte link_cnt;
        public ushort multiplier;
        public ushort line_id;
        public ushort max_multiplier_alone;
        public uint pay;
        public uint total_pay;
        public long winposmask;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct WinLineListNative
    {
        public uint total_pay;
        public byte lineSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public WinLineNative[] line;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct TriggerBonusNative
    {
        public byte icon_id;
        public ushort get_times;
        public ushort multiplier;
        public ushort bonus_id;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SlotResultNative
    {
        public sbyte result;
        public ushort freeTimes;
        public ushort curFreeTimes;
        public ushort module_id;
        public uint curStatus;
        public uint nextStatus;
        public uint total_pay;
        public ushort external_multiplier;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public ushort[] can_trigger_bonus_times;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public TriggerBonusNative[] trigger_bonus_vec;

        public uint jp_pay_cent;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] icon_pattern;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public ushort[] icon_multiplier;

        public ushort select_line_count;
        public double times;

        public WinLineListNative r2l_winline_vec;
        public WinLineListNative scatter_winline_vec;
        public WinLineListNative winline_vec;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public ushort[] multiplier_vec;

        public byte change_candidate;
        public ushort multiplier_for_change;
        public WinLineListNative change_winline_vec;
    }






    // =======积分库相关===================

    // 定义与C++对应的结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct GaRtpNative
    {
        public double set;
        public double app;
        public double use;
        public double jackpot;
        public double box;
        public double give;
        public double slam;
        public double natural;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RaidDataNative
    {
        public byte mSegDttCounter;
        public byte mSegDtCounter;
        public uint mSegThresHold;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] mSegThresHoldData;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public int[] mSegData;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] // 2*8
        public long[] mSectionInTotal;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] // 2*8
        public long[] mSectionOutTotal;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SectionDataNative
    {
        public byte mRoundCounter;
        public uint funded;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] // 2*8
        public uint[] mSectionCounter;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] // 2*8
        public long[] mSection;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public uint[] mPerBet;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public uint[] mPerWin;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] mPullRemain;

        public double mOneRemain;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public double[] mPerScore;

        public uint mAvgBets;
        public GaRtpNative mRtp;
        public long mTotalPullScore;
        public uint mSegRoundThresHold;
        public int mTotalPullRemain;
        public double mTotalRtpScore;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public double[] mSectionSave;

        public double mSectionUse;
        public double mRoundBets;
        public double mRoundWins;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ScorePoolDataNative
    {
        public uint flag;
        public uint funded;
        public SectionDataNative mSectionData;
        public RaidDataNative mRaidData;
    }

#endif