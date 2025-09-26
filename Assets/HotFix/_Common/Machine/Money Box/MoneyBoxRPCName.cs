using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoneyBox
{

    public static class MoneyBoxRPCName
    {
        /// <summary> 登录 </summary>
        public const string login = "login";

        /// <summary> 心跳 </summary>
        public const string ping = "ping";


        /// <summary> 钱箱请求机台上分 </summary>
        public const string CassetteRechange = "CassetteRechange";

        /// <summary> 【？】返回钱箱请求机台上分 </summary>
        // public const string CassetteRechange = "CassetteRechange";


        /// <summary> 钱箱的通用请求响应协议 </summary>
        public const string CassetteReturn = "CassetteReturn";

        /// <summary> 向钱箱的通用请求协议 </summary>
        public const string CassetteProcess = "CassetteProcess";


        #region ## 本机台下分并打印二维码
        /// <summary> 请求下分并生成二维码</summary>
        public const string create_qrcode = "create_qrcode";

        /// <summary> 上报下分结果(成功打印二维码后上报) </summary>
        public const string notify_decr_success = "notify_decr_success";
        #endregion


        #region  ## 二维码(钱箱打印)在本机台上分。## 二维码(机台打印)在本机台上分。
        /// <summary> 扫码查询可用金额</summary>
        public const string scan_qrcode = "scan_qrcode";

        /// <summary> 确认上分(消费二维码) </summary>
        public const string consume_qrcode = "consume_qrcode";
        #endregion


        #region  ## 钱箱客户投钞，指定机台上分
        /// <summary> 钱箱请求机台上分,返回收到的订单id</summary>
        public const string cashin_rechange = "cashin_rechange";

        /// <summary> 钱箱请求机台上分,确认上分并上报数据 </summary>
        public const string notify_cashin_result = "notify_cashin_result";
        #endregion

    }

}