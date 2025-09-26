using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameMaker
{
    public class PanelEvent
    {

        //public const string ON_CREDIT_CHANGE_EVENT = "ON_CREDIT_CHANGE_EVENT";


        public const string ON_PANEL_INPUT_EVENT = "ON_PANEL_INPUT_EVENT";
        //public const string OnClickSpinButton = "OnClickSpinButton";
        //public const string OnLongClickSpinButton = "OnLongClickSpinButton";
        /// <summary> spin 按钮按下 - value : bool (is long click)</summary>
        public const string SpinButtonClick = "SpinButtonClick"; //OnSpinButtonEvent

        /// <summary> 钱箱按钮点击 </summary>
        public const string RedeemButtonClick = "RedeemButtonClick"; //OnSpinButtonEvent
        ///////////////////
        ///

        // const string //OnSpinButtonEvent
    }


    public class CreditEventData
    {
        public long nowCredit = -1;
        public long toCredit = -1;
        public bool isAnim = false;
    }
}

