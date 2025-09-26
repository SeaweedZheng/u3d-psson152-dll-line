using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if false
namespace GameMaker
{
    //namespace BagelCode{

    public enum CommonPopupType
    {
        /// <summary>
        /// ����ʾ
        /// </summary>
        None = 0,


        /// <summary>
        /// ֻ��ʾtext�ı�
        /// </summary>
        TextOnly,

        /// <summary>
        /// ��ʾtext�ı���btn1��ť
        /// </summary>
        OK,


        /// <summary>
        /// ��ʾtitle���⣬text�ı���btn1��ť
        /// </summary>
        OkWithTitle,

        /// <summary>
        /// ��ʾtext�ı���btn1��ť��btn2��ť
        /// </summary>
        YesNo,


        /// <summary>
        /// ��ʾtext�ı���btn1��ť
        /// </summary>
        /// <remarks>
        /// �����ť��ص���¼����
        /// </remarks>
        SystemReset,


        SystemTextOnly,
    }

    public class CommonPopupInfo
    {
        public CommonPopupType type;

        /// <summary> popup title </summary>
        public string title;

        /// <summary> popup content </summary>
        public string text;

        /// <summary> button1 show text </summary>
        public string buttonText1;

        /// <summary> button2 show text </summary>
        public string buttonText2;

        /// <summary> is show button_close </summary>
        public bool isUseXButton = false;

        /// <summary> is close popup when click button1 </summary>
        public bool buttonAutoClose1 = true;

        /// <summary> is close popup when click button2 </summary>
        public bool buttonAutoClose2 = true;

        /// <summary> click button1 callback </summary>
        public UnityAction callback1;

        /// <summary> click button2 callback </summary>
        public UnityAction callback2;

        /// <summary> click button_close callback </summary>
        public UnityAction callbackX;

        public string mark = null;

       // public int autoCloseTimesS = -1;
    }

 
}
#endif