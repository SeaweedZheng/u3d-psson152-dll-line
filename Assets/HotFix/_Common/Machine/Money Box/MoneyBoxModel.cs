
using System.Collections.Generic;

namespace MoneyBox
{
    public class MoneyBoxModel
    {
        private static MoneyBoxModel instance;

        public static MoneyBoxModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MoneyBoxModel();
                }
                return instance;
            }
        }
        /// <summary> ����Ǯ���token </summary>
        public string accessToken;

        /// <summary> ��¼�������Ļ���id </summary>
        public string machineId;

        /// <summary> ��¼�������Ļ������� </summary>
        public string machineName;

        /// <summary> 1�Ҷ���Ǯ </summary>
        public float moneyPerCoinIn = 0.999f;

        /// <summary> ��̨�·�:����ѡ���·ַ���</summary>
        /// <remarks>
        /// * ��Ͷ�Ҹ��������Ӧ�·ַ���
        /// </remarks>
        public List<int> coinInNumLst = new List<int>();
 
        /// <summary> Ǯ���ά��ӽ�����Կ </summary>
        public string bankQRCodeAesKey;

    }
}