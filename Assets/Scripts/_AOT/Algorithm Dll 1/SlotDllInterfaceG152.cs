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
        /// ��ȡdll�汾��
        /// </summary>
        /// <param name="ptrStr"></param>
        [DllImport("msmatch")]
        public static extern void DevVersion(ref byte ptrStr);



        /// <summary>
        /// ��̨�ϵ��ʼ������
        /// </summary>
        /// <param name="gameState">��Ϸװ��</param>
        /// <param name="dif">�Ѷ�</param>
        /// <param name="funded">��������</param>
        /// <remarks>
        /// * ��̨�ϵ����0
        /// * �־û����桰��������������̨�ϵ�����ýӿ�
        /// * �־û����桰�Ѷȡ�
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevInit(int gameState, int dif, int funded);


        /// <summary>
        /// ��ȡslotSpin����
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="bet"></param>
        /// <param name="pPullData"></param>
        /// <param name="pSlotData"></param>
        /// <param name="pLinkInfo"></param>
        /// <returns>��Ϸ״̬</returns>

        [DllImport("msmatch")]
        public static extern int DevSpin(int pid, int bet, ref PullData pPullData, ref SlotData pSlotData, ref LinkInfo pLinkInfo);



        /// <summary>
        /// ������������̨
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="pPullData"></param>
        /// <param name="pSlotData"></param>
        /// <param name="pLinkInfo"></param>
        /// <remarks>
        ///  * ��������ʱ�����pPullData��pSlotData��pLinkInfo�����ݿ�����ֵ�Ļ���
        ///  * ����Ҫ��������ӿڽ��������õ�dll��,���������DevInit����á�
        /// </remarks>

        [DllImport("msmatch")]
        public static extern void DevSetData(int sel, ref PullData pPullData, ref SlotData pSlotData, ref LinkInfo pLinkInfo);



        /// <summary>
        /// �����Ϸ��ʼ
        /// </summary>
        [DllImport("msmatch")]
        public static extern void DevFreeStart();



        /// <summary>
        /// �����Ϸ����
        /// </summary>
        [DllImport("msmatch")]
        public static extern void DevFreeEnd();




        /// <summary>
        /// �ʽ��н���Ҫ����
        /// </summary>
        [DllImport("msmatch")]
        public static extern void DevJpEnd();





        /// <summary>
        /// ��̨-���ò�������
        /// </summary>
        [DllImport("msmatch")]
        public static extern void DevSetWaveScore(int waveScore);


        /// <summary>
        /// ��̨-���ò�������
        /// </summary>
        /// <remarks>
        /// * ���������������ֵ̨�������ڴ��У���Ҫ�������־û��洢������ʱ���ظ���̨��
        /// </remarks>
        [DllImport("msmatch")]
        public static extern int DevGetWaveScore();




        /// <summary>
        /// ��ȡ��Ϸ�ʽ�ֵ
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="pJackpotInitInfo"></param>
        /// <remarks>
        /// * ��ͬѺ��ֵ����Ӧ����Ϸ�ʽ�Χ�ǲ�ͬ�ġ�
        /// * ÿ�ο����������л�Ѻע���ʱ�������»�ȡ��ǰ��Ϸ�ʽ�ֵ��
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevGetJackpot(int sel,ref JackpotInitInfo pJackpotInitInfo);



        /// <summary>
        /// ����4���ʽ����
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="pJackpotInitInfo"></param>
        /// <remarks>
        /// * ÿ�ο�������4�����س־û��Ĳʽ�ֵ���ظ���̨��
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevSetJackpotData(int sel, ref JackpotData pJackpotData);

        /// <summary>
        /// ��ȡ4���ʽ����
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="pJackpotInitInfo"></param>
        /// <remarks>
        /// * 4���ʽ�ֵ����ֻ̨�Ǳ������ڴ��У�
        /// * ÿ��spin����Ҫ��ȡ��ǰ���ĵ��ʽ������sel = 0,1,2,3���������س־û�����
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevGetJackpotData(int sel, ref JackpotData pJackpotData);


        /// <summary>
        /// ��ȡ��̨�ڴ��е��ϱ�����
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="pJackpotInitInfo"></param>
        /// <remarks>
        /// * ÿ��Spin��Ҫ��ȡ�ϱ����ݡ�
        /// * ��ȡ���ϱ����ݣ�Ҫ���س־û����棬��������̨������
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevGetSummary(ref Summary pSummary);


        /// <summary>
        /// �����ϱ����ݸ���̨
        /// </summary>
        /// <param name="pSummary"></param>
        /// <remarks>
        /// * ��̨���ϱ�����ֻ�Ǳ������ڴ��У�Ҫ���س־û����档
        /// * ��̨�ϵ�󣬽����ϱ����ݷ�������̨
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevSetSummary(ref Summary pSummary);


        /// <summary>
        /// ��ʾRTP
        /// </summary>
        /// <param name="pSummary"></param>
        /// <remarks>
        /// * ��̨���ϱ�����ֻ�Ǳ������ڴ��У�Ҫ���س־û����档
        /// </remarks>
        [DllImport("msmatch")]
        public static extern void DevSummary();





        /// <summary>
        /// �����Ѷ�
        /// </summary>
        /// <param name="level"></param>
        [DllImport("msmatch")]
        public static extern void DevSetLevel(int level);



        /// <summary>
        /// ��ȡ�Ѷ�
        /// </summary>
        /// <returns></returns>
        [DllImport("msmatch")]
        public static extern int DevGetLevel();





        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DllImport("msmatch")]
        public static extern int DevClear();



    }
}