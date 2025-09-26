using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Console001
{
    public class PopupConsoleSetMachineID : PageMachineButtonBase
    {
        public TextMeshProUGUI tmpTip1, tmpTip2;

        public Button btnClose, btnConfirm;

        public CompInputController compInput1;
        public CompInputController compInput2;

        void Awake()
        {
            btnConfirm.onClick.AddListener(OnClickConfirm);
            btnClose.onClick.AddListener(OnClickClose);
        }

        void OnClickClose()
        {
            PageManager.Instance.ClosePage(this, new EventData("Exit"));
        }


        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            InitParam();

            Dictionary<string, object> argDic = null;
            if (data != null)
            {
                argDic = (Dictionary<string, object>)data.value;
          
            }

        }


        void InitParam()
        {
            //password1 = "";
            //password2 = "";
            compInput1.value = "";
            compInput2.value = "";
            tmpTip1.text = "";
            tmpTip2.text = "";
        }


        void OnClickConfirm()
        {
            tmpTip1.text = "";
            tmpTip2.text = "";

            if (compInput1.value.Length != 4)
            {
                string errMsg = string.Format(I18nMgr.T("The {0} must be {1} digits long"),
                    I18nMgr.T("Agent ID"), 4);
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip1.text = errMsg;
            }
            else if (compInput2.value.Length != 8)
            {
                string errMsg = string.Format(I18nMgr.T("The {0} must be {1} digits long"),
                    I18nMgr.T("Machine ID"), 8);
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip2.text = errMsg;
            }
            else if (string.IsNullOrEmpty(compInput1.value))
            {
                TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Please enter Agent ID"));
            }
            else if (string.IsNullOrEmpty(compInput2.value))
            {
                TipPopupHandler.Instance.OpenPopup(I18nMgr.T("'Please enter Machine ID"));
            }
            else if ( !compInput2.value.StartsWith(compInput1.value))
            {
                TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Machine ID must start with Agent ID"));
            }
            else
            {
                PageManager.Instance.ClosePage(this, new EventData<string>("Result", compInput2.value));
            }
        }

    }
}