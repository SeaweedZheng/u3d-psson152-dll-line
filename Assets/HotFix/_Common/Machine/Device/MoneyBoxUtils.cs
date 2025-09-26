using GameMaker;
using System.Linq;

namespace MoneyBox
{
    // 【网络接口文档】：http://192.168.3.174:11111/md/show_subcate.php?foldername=%E9%92%B1%E7%AE%B1%E7%AC%AC%E4%BA%8C%E7%89%88%28%E6%98%8E%E6%96%87%29

    public static class MoneyBoxUtils
    {

        public const string ivValue = "a3b4c5d6e7f8901234567890abcdef12";

        /// <summary> 每个钱箱唯一的二维码加密秘钥 </summary>
        public const string bank_aes_key = "5112940e3fd36924681690ddd4abf323ac2b1ffae646c14fc5e871d17702a473";

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splitValue"></param>
        /// <returns></returns>
        public static string EncryptQRCode(string value, string splitValue)
        {
            try
            {
                string temp = value.Replace(splitValue, "");
                byte[] iv = AesManager.Instance.StringToByteArray(ivValue).Take(16).ToArray();
                string aes_key = !string.IsNullOrEmpty(MoneyBoxModel.Instance.bankQRCodeAesKey) ?
                    MoneyBoxModel.Instance.bankQRCodeAesKey : bank_aes_key;
                byte[] key = AesManager.Instance.StringToByteArray(aes_key);
                string result = AesManager.Instance.UnityEncrypt(temp, key, iv);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecryptQRCode(string value)
        {
            try
            {
                byte[] iv = AesManager.Instance.StringToByteArray(ivValue).Take(16).ToArray();
                string content = value;
                string aes_key = !string.IsNullOrEmpty(MoneyBoxModel.Instance.bankQRCodeAesKey) ?
                    MoneyBoxModel.Instance.bankQRCodeAesKey : bank_aes_key;
                byte[] key = AesManager.Instance.StringToByteArray(aes_key);
                string code = AesManager.Instance.UnityDecrypt(content, key, iv);
                //AESScript.Decrypt(content, key, iv);
                return code;
            }
            catch
            {
                return null;
            }
        }
    }

}