
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
        /// <summary> 链接钱箱的token </summary>
        public string accessToken;

        /// <summary> 登录服务器的机器id </summary>
        public string machineId;

        /// <summary> 登录服务器的机器名称 </summary>
        public string machineName;

        /// <summary> 1币多少钱 </summary>
        public float moneyPerCoinIn = 0.999f;

        /// <summary> 机台下分:快速选择下分分数</summary>
        /// <remarks>
        /// * 按投币个数算出对应下分分数
        /// </remarks>
        public List<int> coinInNumLst = new List<int>();
 
        /// <summary> 钱箱二维码加解密秘钥 </summary>
        public string bankQRCodeAesKey;

    }
}