using System.Runtime.InteropServices;



namespace SlotDllAlgorithmG152
{
    public enum SboxIdeaGameState
    {
        NormalSpin = 1,
        FreeSpinStart = 2,
        FreeSpin = 3,
        FreeSpinEnd = 4,
        JackpotGame = 5,
    }
    public static class SlotDllInterfaceG152
    {

        /// <summary>
        /// 获取dll版本号
        /// </summary>
        /// <param name="ptrStr"></param>
        [DllImport("msmatch")]
        public static extern void DevVersion(ref byte ptrStr);



        /// <summary>
        /// 机台上电初始化调用
        /// </summary>
        /// <param name="gameState">游戏装载</param>
        /// <param name="dif">难度</param>
        /// <param name="funded">波动分数</param>
        /// <remarks>
        /// * 机台上电后传入0
        /// * 持久化保存“波动分数”，机台上电后存入该接口
        /// * 持久化保存“难度”
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevInit(int gameState, int dif, int funded);


        /// <summary>
        /// 获取slotSpin数据
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="bet"></param>
        /// <param name="pPullData"></param>
        /// <param name="pSlotData"></param>
        /// <param name="pLinkInfo"></param>
        /// <returns>游戏状态</returns>

        [DllImport("msmatch")]
        public static extern int DevSpin(int pid, int bet, ref PullData pPullData, ref SlotData pSlotData, ref LinkInfo pLinkInfo);



        /// <summary>
        /// 返还参数给机台
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="pPullData"></param>
        /// <param name="pSlotData"></param>
        /// <param name="pLinkInfo"></param>
        /// <remarks>
        ///  * 机器启动时，如果pPullData，pSlotData，pLinkInfo在数据库中有值的话，
        ///  * 就需要调用这个接口将数据设置到dll中,这个函数在DevInit后调用。
        /// </remarks>

        [DllImport("msmatch")]
        public static extern void DevSetData(int sel, ref PullData pPullData, ref SlotData pSlotData, ref LinkInfo pLinkInfo);



        /// <summary>
        /// 免费游戏开始
        /// </summary>
        [DllImport("msmatch")]
        public static extern void DevFreeStart();



        /// <summary>
        /// 免费游戏结束
        /// </summary>
        [DllImport("msmatch")]
        public static extern void DevFreeEnd();




        /// <summary>
        /// 彩金中奖后，要调用
        /// </summary>
        [DllImport("msmatch")]
        public static extern void DevJpEnd();





        /// <summary>
        /// 后台-设置波动分数
        /// </summary>
        [DllImport("msmatch")]
        public static extern void DevSetWaveScore(int waveScore);


        /// <summary>
        /// 后台-设置波动分数
        /// </summary>
        /// <remarks>
        /// * 这个波动分数，机台值保存在内存中，需要软件对齐持久化存储。开机时传回给机台。
        /// </remarks>
        [DllImport("msmatch")]
        public static extern int DevGetWaveScore();




        /// <summary>
        /// 获取游戏彩金值
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="pJackpotInitInfo"></param>
        /// <remarks>
        /// * 不同押分值，对应的游戏彩金范围是不同的。
        /// * 每次开机，或是切换押注金额时，需重新获取当前游戏彩金值。
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevGetJackpot(int sel,ref JackpotInitInfo pJackpotInitInfo);



        /// <summary>
        /// 返还4档彩金参数
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="pJackpotInitInfo"></param>
        /// <remarks>
        /// * 每次开机，将4档本地持久化的彩金值返回给机台。
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevSetJackpotData(int sel, ref JackpotData pJackpotData);

        /// <summary>
        /// 获取4档彩金参数
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="pJackpotInitInfo"></param>
        /// <remarks>
        /// * 4档彩金值，机台只是保存在内存中！
        /// * 每次spin后，需要获取当前的四档彩金参数（sel = 0,1,2,3），并本地持久化保存
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevGetJackpotData(int sel, ref JackpotData pJackpotData);


        /// <summary>
        /// 获取机台内存中的上报数据
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="pJackpotInitInfo"></param>
        /// <remarks>
        /// * 每次Spin后，要获取上报数据。
        /// * 获取的上报数据，要本地持久化保存，并发给后台服务器
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevGetSummary(ref Summary pSummary);


        /// <summary>
        /// 返还上报数据给机台
        /// </summary>
        /// <param name="pSummary"></param>
        /// <remarks>
        /// * 机台的上报数据只是保存在内存中，要本地持久化保存。
        /// * 机台上电后，将并上报数据返还给机台
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevSetSummary(ref Summary pSummary);


        /// <summary>
        /// 显示RTP
        /// </summary>
        /// <param name="pSummary"></param>
        /// <remarks>
        /// * 机台的上报数据只是保存在内存中，要本地持久化保存。
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevSummary();





        /// <summary>
        /// 设置难度
        /// </summary>
        /// <param name="level"></param>
        [DllImport("msmatch")]
        public static extern void DevSetLevel(int level);



        /// <summary>
        /// 获取难度
        /// </summary>
        /// <returns></returns>
        [DllImport("msmatch")]
        public static extern int DevGetLevel();





        /// <summary>
        /// 打码清零
        /// </summary>
        /// <returns></returns>
        [DllImport("msmatch")]
        public static extern int DevClear();



    }
}