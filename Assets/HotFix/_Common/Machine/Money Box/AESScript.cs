using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Text;
using System.IO;

namespace MoneyBox
{
    public class AESScript
    {
        //public static string AppPostBase64Key = "wu6XK3LmFLCXIH9HJFYDJQdxRvv6ldnf3hUEFU5EzGk=";
        public static string AppPostBase64Key = "h+PG0fLGvLvIRYVxW/ZaUnrsrBe6Z9KMohZSLqRqYMs=";
        // 将Base64编码的字符串解码为字节数组
        private static byte[] DecodeBase64Key(string base64Key)
        {
            return Convert.FromBase64String(base64Key);
        }
        // AES加密
        public static string Encrypt(string plainText, string base64Key, byte[] iv)
        {
            byte[] key = DecodeBase64Key(base64Key);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Padding = PaddingMode.PKCS7; // 在.NET中，这实际上是PKCS#5的变种，但对于AES来说是等效的
                aesAlg.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(iv, 0, iv.Length); // 可选：将IV存储在密文前面（对于某些应用场景很有用）
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        // AES解密
        public static string Decrypt(string cipherTextBase64, string base64Key)
        {
            byte[] key = DecodeBase64Key(base64Key);
            byte[] cipherText = Convert.FromBase64String(cipherTextBase64);

            byte[] iv = new byte[16]; // AES的块大小是128位（16字节）
            Array.Copy(cipherText, 0, iv, 0, iv.Length); // 如果IV存储在密文前面，则提取它
            byte[] encryptedText = new byte[cipherText.Length - iv.Length];
            Array.Copy(cipherText, iv.Length, encryptedText, 0, encryptedText.Length);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Padding = PaddingMode.PKCS7; // 在.NET中，这实际上是PKCS#5的变种，但对于AES来说是等效的
                aesAlg.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        // 生成一个随机的IV（初始化向量）
        public static byte[] GenerateIv()
        {
            byte[] iv = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }

        // 使用示例
        public static void TestMain()
        {
            //string originalText = "{ \"accessToken\": \"j9nV6haLcWpB9CnpQcxEaQtjxLAu8ve3755\", \"dynamicKey\": \"0JfIUvCrN4OSZhFpxGslKvfe7pgNiUSGyV9c88sDl7A=\", \"mqttBroker\": \"192.168.3.49:36267/mqtt\" }";
            string originalText = "{\"amount\":0,\"playerId\":null,\"gameMachineId\":null,\"prcode\":null,\"lang\":\"en\",\"codeContent\":null,\"orderno\":null,\"cashBoxes\":null,\"cassettes\":null,\"cdmDevStatus\":{\"bSLS\":false,\"bFCS1\":false,\"bFCS2\":false,\"bFCS3\":false,\"bFCS4\":false,\"bFCS5\":false,\"bFCS6\":false,\"bSCS\":false,\"bPCS\":false,\"bPRFS\":false,\"bPES\":false,\"bTES\":false,\"bRFS\":false,\"bPRVES\":false,\"bLOCK1\":false,\"bLOCK2\":false,\"bLOCK3\":false,\"bLOCK4\":false,\"bLOCK5\":false,\"bLOCK6\":false,\"bRVES\":false,\"bSCPS\":false,\"bSLPS\":false,\"bCLLS1\":false,\"bCLLS2\":false,\"bCLLS3\":false,\"bCLLS4\":false,\"bCLLS5\":false,\"bCLLS6\":false,\"byCIDS\":null,\"byReserve\":null},\"cdmDispense\":{\"Code\":0,\"iDispenseNumber\":null,\"iTotalCashOut\":0,\"iTotalRetract\":0,\"iRealCashOut\":null,\"iRetractCash\":null,\"iMaySameCash\":null},\"devReturn\":{\"Code\":0,\"iLogicCode\":0,\"iPhyCode\":0,\"iHandle\":0,\"iType\":0,\"acDevReturn\":null,\"acReserve\":null},\"password\":null,\"role\":null,\"storeAddr\":null,\"storeEmail\":null,\"storeName\":null,\"storeTel\":null,\"newPassword\":null,\"oldPassword\":null,\"remainingCount\":null,\"warningCount\":null,\"isLocal\":null,\"startDate\":null,\"endDate\":null,\"rechargeType\":null,\"pageNum\":0,\"BillListIndex\":0,\"BillListStr\":null,\"ctxt\":null,\"accountCount\":0,\"accountPrefix\":null,\"methodId\":null,\"topic\":null,\"MacID\":0}";

            //string Static_base64Key = "nSFQxn9+lZBLu1by7E9ibvZEPljhvMC3GQo9plFR9RI=";
            string base64Key = "0S+h91rjhCXQjFd67/NmGVaaHWlsnHMZqWi+zGiZ5zs=";
            byte[] iv = GenerateIv();

            string encryptedText = Encrypt(originalText, base64Key, iv);
            Debug.Log($"Encrypted: {encryptedText}");

            string decryptedText = Decrypt(encryptedText, base64Key);
            Debug.Log($"Decrypted: {decryptedText}");
        }


        //二维码加密使用

        public static string QRcodeIV = "a3b4c5d6e7f8901234567890abcdef12";
        public static string UnityEncrypt(string plainText, byte[] keyBytes, byte[] ivBytes)
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
        /// <summary>
        /// 生成随机盐值
        /// </summary>
        /// <returns></returns>
        public static string UnityDecrypt(string cipherText, byte[] keyBytes, byte[] ivBytes)
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

        public static byte[] StringToByteArray(string hex)
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