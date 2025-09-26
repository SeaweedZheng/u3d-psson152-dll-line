using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Code
{
    #region  ������
    /// <summary> ��Ӧ���� </summary>
    public static int OK = 0;
    #endregion


    #region  Net 0 - 999
    #endregion

    #region  Hall 1000 - 1999
    #endregion

    #region Game 2000 - 2999
    #endregion

    #region  Machine 3000 - 3999
    /// <summary> �㷨�� ��Ǯ��Ӧ��ʱ </summary>
    public static int DEVICE_SBOX_COIN_IN_OVERTIME = 3001;
    #endregion

    #region Device 4000 - 4999

    /* ==== Ӳ������Ĭ�� 0-9 */
    /// <summary> ����Ͷ�˱Ҷ����� </summary>
    public static int DEVICE_CREAT_ORDER_NUMBER = 4000;

    /* ====���·� 20-29 */

    /*  ==== ��ӡ��ȱֽ 30-39 */
    /// <summary> ��ӡ��ȱֽ </summary>
    public static int DEVICE_PRINTER_OUT_OF_PAGE = 4031;
    /* ==== Ͷ�һ� 40-49 */
    /* ==== ��Ʊ�� 50-59 */
    /* ==== ֽ���� 60-69 */


    /* ==== �ÿ� ��Ʊ 70-79 */

    /// <summary> �ÿ���Ʊ�ɹ� </summary>
    public static int DEVICE_IOT_COIN_OUT_SUCCESS = 4070;


    /// <summary> ���ݶԲ��� </summary>
    public static int DEVICE_IOT_COIN_OUT_DATA_MISMATCH = 4071;

    /// <summary> �Ҳ����������� </summary>
    public static int DEVICE_IOT_COIN_OUT_CACHE_NOT_FIND = 4072;
    /// <summary> �ÿ���Ʊapi���� </summary>
    public static int DEVICE_IOT_COIN_OUT_API_ERR = 4073;

    /// <summary> �ÿ���Ʊ���ػ���û�ҵ�</summary>
    public static int DEVICE_IOT_COIN_OUT_CACHE_ORDER_IS_NOT_FIND = 4074;

    /// <summary> �ÿ�Mqtt�Ͽ�����</summary>
    public static int DEVICE_IOT_MQTT_NOT_CONNECT = 4075;

    /// <summary> �ÿ�δ��¼��δ��ȡ��ά�����ݣ� </summary>
    public static int DEVICE_IOT_NOT_SIGN_IN = 4076;


    /* ==== �ÿ� Ͷ�� 80-89 */
    public static int DEVICE_IOT_COIN_IN_SUCCESS = 4080;

    /// <summary> û�С�Ͷ�Ұ�΢�źš� </summary>
    public static int DEVICE_IOT_COIN_IN_NOT_BIND_WECHAT_ACCOUNT= 4081;


    /* ==== Ǯ�䣺�����Ϸ� 90-100 */
    public static int DEVICE_MB_QRCODE_IN_SUCCESS = 4090;

    /* ==== Ǯ�䣺�����·� 100-109 */
    public static int DEVICE_MB_QRCODE_OUT_SUCCESS = 4100;

    /// <summary> �·�����㵥idʧ��</summary>
    public static int DEVICE_MB_GET_ORDER_ID_FAIL = 4101;


    /* ==== Ǯ�䣺Ǯ��Ҫ���̨�Ϸֱ����Ϸ� 110-119 */
    public static int DEVICE_MB_REQ_MACH_QRCODE_IN_SUCCESS = 4110;

    #endregion
}
