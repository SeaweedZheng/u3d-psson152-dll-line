using Hal;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SBoxApi
{

    public class WinLine
    {
        public int lineId;          //线的序号
        public int iconId;          //中的是哪个图标
        public int multiplier;      //几条线
        public int[] winpos;        //中奖的位置
        public int totalPay;        //当前线赢分
        public int winLineType;     //赢线类型,0:SlotResult_WinLine_WinLineType_kCommon,1:SlotResult_WinLine_WinLineType_kXTotalBet,2:SlotResult_WinLine_WinLineType_kXTotalBetTrigger
    }

    public class SBoxSlotSpinData
    {
        public int result;          //返回结果，0表示成功,-1表示失败
        public int pid;             //玩家id,目前为0
        public int bet;
        public int totalPay;        //赢分
        public int credit;          //玩家当前的credit
        public int curState;        //当前状态 为0时是普通状态,为1时是免费游戏状态
        public int nextState;       //下次状态 为0时是普通状态,为1时是免费游戏状态
        public int freeTimes;       //免费游戏次数
        public int maxFreeTimes;    //总的免费次数
        public int curFreeTimes;    //当前跑了多少次免费次数
        public int multiplier_for_change;//
        /*
         *      0   1   2   3   5
         *      6   7   8   9   10
         *      11  12  13  14  15
         */
        public int[] icons;         //这一局的画面3*5


        public WinLine[] scatterLines;
        public WinLine[] winLines;
        public WinLine[] changeLines;
    }


    public class SBoxGameJackpotData
    {
        public int result;          //返回结果，0表示成功,-1表示失败
        public int[] data;          //彩金数据 [jp1当前值, jp2当前值, jp3当前值, jp4当前值, jp1-4中奖标志位, jp1中奖金额, jp2中奖金额, jp3中奖金额, jp4中奖金额, jp1中奖前的值, jp2中奖前的值, jp3中奖前的值, jp4中奖前的值]
                                    // jp1-4中奖标志: 1=jp1中奖  2=jp2中奖  4=jp3中奖  8=jp4中奖
    }

    public partial class SBoxGodOfWealth : SBoxIdea
    {
        private static bool HasAnyBitSet(BitArray bits)
        {
            foreach (bool bit in bits)
            {
                if (bit) return true;
            }
            return false;
        }

        private static string RowBitsToString(BitArray bits)
        {
            char[] arr = new char[bits.Length];
            for (int i = 0; i < bits.Length; i++)
            {
                arr[i] = bits[i] ? '1' : '0';
            }
            Array.Reverse(arr);
            return new string(arr);
        }

        // NOTE: The win_mask represent rows of reels in 4 bytes.
        //
        // +----------+------------------------+-----+
        // | bits of  |   bits map to          | row |
        // | win_mask |       reel symbols     | idx |
        // +----------+----+----+----+----+--- +-----+
        // | 24 - 31  | 24 | 25 | 26 | 27 | 28 |  3  |
        // | 16 - 23  | 16 | 17 | 18 | 19 | 20 |  2  |
        // | 8  - 15  |  8 |  9 | 10 | 11 | 12 |  1  | <- 3x5 start from here.
        // | 0  - 7   |  0 |  1 |  2 |  3 |  4 |  0  | <- 4x5 start from here.
        // +----------+----+----+----+----+----+-----+
        //
        // NOTE: Only use first 5 bits of each byte to represent 5 reels
        //
        //             |  3          2          1          0 |
        //             | 10987654 32109876 54321098 76543210 |
        //  bits used  |    ^^^^^    ^^^^^    ^^^^^    ^^^^^ |
        // ------------+---------+--------+--------+---------+
        // row_idx (y) |    3    |   2    |   1    |    0    |
        // ------------+---------+--------+--------+---------+
        //
        public static int[] ParseWinLine32(int winMask)
        {
            List<int> positions = new List<int>();
            int maskSize = sizeof(uint);

            for (int y = 0; y < maskSize; ++y)
            {
                // Read each byte of win_mask from high to low.
                int rowIdx = (maskSize - 1 - y);
                byte rowMask = (byte)((winMask >> (rowIdx << 3)) & 0xFF);
                BitArray rowBits = new BitArray(new byte[] { rowMask });

                if (HasAnyBitSet(rowBits))
                {
                    for (int x = 0; x < rowBits.Length; ++x)
                    {
                        if (!rowBits[x])
                            continue;

                        positions.Add(x * 10 + (maskSize - rowIdx));
                    }
                }

                // Make rowBits reversed for easier debugging.
                string rowBitsStr = RowBitsToString(rowBits);
                //Debug.Log($"({rowIdx}) {rowBitsStr}");
            }

            return positions.ToArray();
        }

        /*
         * 游戏彩金
         */
        public static void GameJackpot(int winCredit)
        {
            SBoxPacket sBoxPacket = new SBoxPacket(cmd: 20101, source: 1, target: 2, size: 2);

            sBoxPacket.data[0] = 0;
            sBoxPacket.data[1] = winCredit;

            SBoxIOEvent.AddListener(sBoxPacket.cmd, GameJackpotR);
            SBoxIOStream.Write(sBoxPacket);
        }


        private static void GameJackpotR(SBoxPacket sBoxPacket)
        {
            /*
            int pos = 0;
            SBoxGameJackpotData res = new SBoxGameJackpotData();
            res.result = sBoxPacket.data[pos++];
            int[] Jackpot = new int[13];
            for (int i = 0; i < 13; i++)
            {
                Jackpot[i] = sBoxPacket.data[pos++];
            }
            res.data = Jackpot;
            EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_GAME_JACKPOT, res);
            */
        }



        /*
         * spin时调用
         * pid 玩家的id
         * bet 玩家的压分
         */
        public static void SlotSpin(int pid, int bet)
        {
            SBoxPacket sBoxPacket = new SBoxPacket(cmd: 20100, source: 1, target: 2, size: 2);

            sBoxPacket.data[0] = pid;
            sBoxPacket.data[1] = bet;

            SBoxIOEvent.AddListener(sBoxPacket.cmd, SlotSpinR);
            SBoxIOStream.Write(sBoxPacket);
        }

        private static void SlotSpinR(SBoxPacket sBoxPacket)
        {
            int pos = 0;
            SBoxSlotSpinData sBoxSlotSpinData = new SBoxSlotSpinData();
            sBoxSlotSpinData.icons = new int[15];
            sBoxSlotSpinData.result = sBoxPacket.data[pos++];
            sBoxSlotSpinData.pid = sBoxPacket.data[pos++];
            sBoxSlotSpinData.bet = sBoxPacket.data[pos++];
            sBoxSlotSpinData.credit = sBoxPacket.data[pos++];
            if (sBoxSlotSpinData.result == 0)
            {
                sBoxSlotSpinData.curState = sBoxPacket.data[pos++];
                sBoxSlotSpinData.nextState = sBoxPacket.data[pos++];
                sBoxSlotSpinData.totalPay = sBoxPacket.data[pos++];
                //画面数据
                for (int i = 0; i < 15; i++)
                {
                    sBoxSlotSpinData.icons[i] = sBoxPacket.data[pos++];
                }

                //免费游戏次数
                //sBoxSlotSpinData.freeTimes = sBoxPacket.data[pos++];

                sBoxSlotSpinData.maxFreeTimes = sBoxPacket.data[pos++];
                sBoxSlotSpinData.curFreeTimes = sBoxPacket.data[pos++];
                sBoxSlotSpinData.freeTimes = sBoxSlotSpinData.maxFreeTimes;
                //赢线
                //scatter lines
                int linNum = sBoxPacket.data[pos++];
                List<WinLine> lines = new List<WinLine>();
                if (linNum > 0)
                {
                    for (int i = 0; i < linNum; i++)
                    {
                        lines.Add(new WinLine());
                        lines[i].lineId = sBoxPacket.data[pos++];
                        lines[i].iconId = sBoxPacket.data[pos++];
                        lines[i].multiplier = sBoxPacket.data[pos++];
                        int winposmask = sBoxPacket.data[pos++];
                        lines[i].winpos = ParseWinLine32(winposmask);
                        lines[i].totalPay = sBoxPacket.data[pos++];
                        lines[i].winLineType = sBoxPacket.data[pos++];
                    }

                    sBoxSlotSpinData.scatterLines = lines.ToArray();

                    //win lines
                    linNum = sBoxPacket.data[pos++];
                    if (linNum > 0)
                    {
                        lines.Clear();
                        for (int i = 0; i < linNum; i++)
                        {
                            lines.Add(new WinLine());
                            lines[i].lineId = sBoxPacket.data[pos++];
                            lines[i].iconId = sBoxPacket.data[pos++];
                            lines[i].multiplier = sBoxPacket.data[pos++];
                            int winposmask = sBoxPacket.data[pos++];
                            lines[i].winpos = ParseWinLine32(winposmask);
                            lines[i].totalPay = sBoxPacket.data[pos++];
                            lines[i].winLineType = sBoxPacket.data[pos++];
                        }
                        sBoxSlotSpinData.winLines = lines.ToArray();
                    }

                }
                else
                {
                    //win lines
                    linNum = sBoxPacket.data[pos++];
                    if (linNum > 0)
                    {
                        lines.Clear();
                        for (int i = 0; i < linNum; i++)
                        {
                            lines.Add(new WinLine());
                            lines[i].lineId = sBoxPacket.data[pos++];
                            lines[i].iconId = sBoxPacket.data[pos++];
                            lines[i].multiplier = sBoxPacket.data[pos++];
                            int winposmask = sBoxPacket.data[pos++];
                            lines[i].winpos = ParseWinLine32(winposmask);
                            lines[i].totalPay = sBoxPacket.data[pos++];
                            lines[i].winLineType = sBoxPacket.data[pos++];
                        }
                        sBoxSlotSpinData.winLines = lines.ToArray();
                    }

                    //r2l winline 
                    linNum = sBoxPacket.data[pos++];
                    if (linNum > 0)
                    {
                        lines.Clear();
                        for (int i = 0; i < linNum; i++)
                        {
                            lines.Add(new WinLine());
                            lines[i].lineId = sBoxPacket.data[pos++];
                            lines[i].iconId = sBoxPacket.data[pos++];
                            lines[i].multiplier = sBoxPacket.data[pos++];
                            int winposmask = sBoxPacket.data[pos++];
                            lines[i].winpos = ParseWinLine32(winposmask);
                            lines[i].totalPay = sBoxPacket.data[pos++];
                            lines[i].winLineType = sBoxPacket.data[pos++];
                        }

                        List<WinLine> lst = new List<WinLine>();
                        if(sBoxSlotSpinData.winLines != null)
                            lst.AddRange(sBoxSlotSpinData.winLines);
                        lst.AddRange(lines.ToArray());

                        sBoxSlotSpinData.winLines = lst.ToArray();
                    }

                    //change winlines
                    linNum = sBoxPacket.data[pos++];
                    if (linNum > 0)
                    {
                        lines.Clear();
                        for (int i = 0; i < linNum; i++)
                        {
                            lines.Add(new WinLine());
                            lines[i].lineId = sBoxPacket.data[pos++];
                            lines[i].iconId = sBoxPacket.data[pos++];
                            lines[i].multiplier = sBoxPacket.data[pos++];
                            int winposmask = sBoxPacket.data[pos++];
                            lines[i].winpos = ParseWinLine32(winposmask);
                            lines[i].totalPay = sBoxPacket.data[pos++];
                            lines[i].winLineType = sBoxPacket.data[pos++];
                        }
                        sBoxSlotSpinData.changeLines = lines.ToArray();

                        sBoxSlotSpinData.multiplier_for_change = sBoxPacket.data[pos++];
                    }
                }

            }
            EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_SLOT_SPIN, sBoxSlotSpinData);
        }
    }
}
