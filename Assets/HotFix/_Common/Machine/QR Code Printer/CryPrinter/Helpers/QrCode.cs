#if UNITY_ANDROID
//hello world
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.Common;

namespace CryPrinter
{
    public class ZXingQrCode
    {
        /// <summary>
        /// ����2ά�� ����һ
        /// �����ԣ�ֻ������256x256��
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static Texture2D GenerateQRImage1(string content, int width, int height)
        {
            // �����color32
            EncodingOptions options = null;
            BarcodeWriter writer = new BarcodeWriter();
            options = new EncodingOptions
            {
                Width = width,
                Height = height,
                Margin = 1,
            };
            options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            Color32[] colors = writer.Write(content);

            // ת��texture2d
            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels32(colors);
            texture.Apply();

            //�洢���ļ�
            //byte[] bytes = texture.EncodeToPNG();
            //string path = System.IO.Path.Combine(Application.dataPath, "qr.png");
            //System.IO.File.WriteAllBytes(path, bytes);

            return texture;
        }

        /// <summary>
        /// ����2ά�� ������
        /// �����ԣ�����������ߴ��������
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static Texture2D GenerateQRImageWithColor(string content, int width, int height, Color color)
        {
            BitMatrix bitMatrix;
            Texture2D texture = GenerateQRImageWithColor(content, width, height, color, out bitMatrix);
            return texture;
        }

        /// <summary>
        /// ����2ά�� ������
        /// �����ԣ�����������ߴ��������
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static Texture2D GenerateQRImageWithColor(string content, int width, int height, Color color, out BitMatrix bitMatrix)
        {
            // �����color32
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();
            //�����ַ���ת����ʽ��ȷ���ַ�����Ϣ������ȷ
            hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            // ���ö�ά���Ե��׿�ȣ�ֵԽ����׿�ȴ󣬶�ά��ͼ�С��
            hints.Add(EncodeHintType.MARGIN, 1);
            hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.M);
            //ʵ�����ַ������ƶ�ά�빤��
            bitMatrix = writer.encode(content, BarcodeFormat.QR_CODE, width, height, hints);

            // ת��texture2d
            int w = bitMatrix.Width;
            int h = bitMatrix.Height;
            Debug.Log(string.Format("w={0},h={1}", w, h));
            Texture2D texture = new Texture2D(w, h);
            for (int x = 0; x < h; x++)
            {
                for (int y = 0; y < w; y++)
                {
                    if (bitMatrix[x, y])
                    {
                        texture.SetPixel(y, x, color);
                    }
                    else
                    {
                        texture.SetPixel(y, x, Color.white);
                    }
                }
            }
            texture.Apply();

            //�洢���ļ�
            //byte[] bytes = texture.EncodeToPNG();
            //string path = System.IO.Path.Combine(Application.dataPath, "qr.png");
            //System.IO.File.WriteAllBytes(path, bytes); 
            return texture;
        }

        /// <summary>
        /// ����2ά�� ������
        /// �ڷ������Ļ����ϣ����Сͼ��
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Texture2D GenerateQRImageWithColorAndIcon(string content, int width, int height, Color color, Texture2D centerIcon)
        {
            BitMatrix bitMatrix;
            Texture2D texture = GenerateQRImageWithColor(content, width, height, color, out bitMatrix);
            int w = bitMatrix.Width;
            int h = bitMatrix.Height;
            // ���Сͼ
            int halfWidth = texture.width / 2;
            int halfHeight = texture.height / 2;
            int halfWidthOfIcon = centerIcon.width / 2;
            int halfHeightOfIcon = centerIcon.height / 2;
            int centerOffsetX = 0;
            int centerOffsetY = 0;
            for (int x = 0; x < h; x++)
            {
                for (int y = 0; y < w; y++)
                {
                    centerOffsetX = x - halfWidth;
                    centerOffsetY = y - halfHeight;
                    if (Mathf.Abs(centerOffsetX) <= halfWidthOfIcon && Mathf.Abs(centerOffsetY) <= halfHeightOfIcon)
                    {
                        texture.SetPixel(x, y, centerIcon.GetPixel(centerOffsetX + halfWidthOfIcon, centerOffsetY + halfHeightOfIcon));
                    }
                }
            }
            texture.Apply();
            // �洢���ļ�
            byte[] bytes = texture.EncodeToPNG();
            string path = System.IO.Path.Combine(Application.dataPath, "qr.png");
            System.IO.File.WriteAllBytes(path, bytes);
            return texture;
        }
    }
}
//hello world
#endif
