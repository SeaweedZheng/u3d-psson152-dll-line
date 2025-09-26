using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MoneyBox
{
    public class AesManager
    {
        private static AesManager instance;

        private byte[] localKeyBytes;
        private byte[] localIvBytes;

        private byte[] KeyBytes;
        private byte[] IvBytes;

        private AesManager()
        {
            initAesLocal();
        }

        public void initAesLocal()
        {
            string localKey = "a4d58c5f125f2c16dd65edd0d4ab45e2485a95042741a740e5012f88e6e1df64";
            string localIv = "be7369b599f7d5bc2e102a3db2a4bfdd";

            localKeyBytes = StringToByteArray(localKey);
            localIvBytes = StringToByteArray(localIv).Take(16).ToArray();

            initAesKey(localKey);
            initAesIv(localIv);
        }

        public void initAesKey(string key)
        {
            KeyBytes = StringToByteArray(key);
        }

        public void initAesIv(string iv)
        {
            IvBytes = StringToByteArray(iv).Take(16).ToArray();
        }

        public static AesManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AesManager();
                }
                return instance;
            }
        }

        public string TryLocalEncrypt(string plainText)
        {
            Console.WriteLine($"TryEncrypt             plainText ====== {plainText} KeyBytes==={localKeyBytes}          IvBytes = {localIvBytes}");
            return UnityEncrypt(plainText, localKeyBytes, localIvBytes);
        }

        public string TryLocalDecrypt(string cipherText)
        {
            Console.WriteLine($"TryDecrypt             cipherText ====== {cipherText} KeyBytes==={localKeyBytes}          IvBytes = {localIvBytes}");

            return UnityDecrypt(cipherText, localKeyBytes, localIvBytes);
        }

        public string TryEncrypt(string plainText)
        {
            Console.WriteLine($"TryEncrypt             plainText ====== {plainText} KeyBytes==={KeyBytes}          IvBytes = {IvBytes}");
            return UnityEncrypt(plainText, KeyBytes, IvBytes);
        }

        public string TryDecrypt(string cipherText)
        {
            Console.WriteLine($"TryDecrypt             cipherText ====== {cipherText} KeyBytes==={KeyBytes}          IvBytes = {IvBytes}");

            return UnityDecrypt(cipherText, KeyBytes, IvBytes);
        }

        public string UnityEncrypt(string plainText, byte[] keyBytes, byte[] ivBytes)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string UnityDecrypt(string cipherText, byte[] keyBytes, byte[] ivBytes)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public byte[] StringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}