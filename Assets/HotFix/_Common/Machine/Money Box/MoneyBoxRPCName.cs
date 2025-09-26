using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoneyBox
{

    public static class MoneyBoxRPCName
    {
        /// <summary> ��¼ </summary>
        public const string login = "login";

        /// <summary> ���� </summary>
        public const string ping = "ping";


        /// <summary> Ǯ�������̨�Ϸ� </summary>
        public const string CassetteRechange = "CassetteRechange";

        /// <summary> ����������Ǯ�������̨�Ϸ� </summary>
        // public const string CassetteRechange = "CassetteRechange";


        /// <summary> Ǯ���ͨ��������ӦЭ�� </summary>
        public const string CassetteReturn = "CassetteReturn";

        /// <summary> ��Ǯ���ͨ������Э�� </summary>
        public const string CassetteProcess = "CassetteProcess";


        #region ## ����̨�·ֲ���ӡ��ά��
        /// <summary> �����·ֲ����ɶ�ά��</summary>
        public const string create_qrcode = "create_qrcode";

        /// <summary> �ϱ��·ֽ��(�ɹ���ӡ��ά����ϱ�) </summary>
        public const string notify_decr_success = "notify_decr_success";
        #endregion


        #region  ## ��ά��(Ǯ���ӡ)�ڱ���̨�Ϸ֡�## ��ά��(��̨��ӡ)�ڱ���̨�Ϸ֡�
        /// <summary> ɨ���ѯ���ý��</summary>
        public const string scan_qrcode = "scan_qrcode";

        /// <summary> ȷ���Ϸ�(���Ѷ�ά��) </summary>
        public const string consume_qrcode = "consume_qrcode";
        #endregion


        #region  ## Ǯ��ͻ�Ͷ����ָ����̨�Ϸ�
        /// <summary> Ǯ�������̨�Ϸ�,�����յ��Ķ���id</summary>
        public const string cashin_rechange = "cashin_rechange";

        /// <summary> Ǯ�������̨�Ϸ�,ȷ���Ϸֲ��ϱ����� </summary>
        public const string notify_cashin_result = "notify_cashin_result";
        #endregion

    }

}