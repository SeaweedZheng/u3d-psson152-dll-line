using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Console001
{
    public class PopupConsoleSetPassword001 : PageMachineButtonBase
    {
        public TextMeshProUGUI tmpTip1, tmpTip2, tmpTitle;

        public Button btnClose, btnConfirm;

        public PIDButtonX btnEye1, btnEye2;


        public CompInputController compInput1;
        public CompInputController compInput2;
        void Awake()
        {
            btnConfirm.onClick.AddListener(OnClickConfirm);
            btnClose.onClick.AddListener(OnClickClose);

            btnEye1.onClickDown.AddListener(OnClickDownEye1);
            btnEye1.onClickUp.AddListener(OnClickUpEye1);
            btnEye2.onClickDown.AddListener(OnClickDownEye2);
            btnEye2.onClickUp.AddListener(OnClickUpEye2);
        }


        int pwdMustCount = -1;

        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            InitParam();

            Dictionary<string, object> argDic = null;
            if (data != null)
            {
                argDic = (Dictionary<string, object>)data.value;
                tmpTitle.text = (string)argDic["title"];
                if (argDic.ContainsKey("pwdMustCount"))
                {
                    pwdMustCount = (int)argDic["pwdMustCount"];
                }
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
            pwdMustCount = -1;
        }

        //string password1 = "";
        //string password2 = "";


        void OnClickDownEye1()
        {
            compInput1.SetPlaintext(true);
        }
        void OnClickUpEye1()
        {
            compInput1.SetPlaintext(false);
        }
        void OnClickDownEye2()
        {
            compInput2.SetPlaintext(true);
        }
        void OnClickUpEye2()
        {
            compInput2.SetPlaintext(false);
        }


        void OnClickClose()
        {
            PageManager.Instance.ClosePage(this, new EventData("Exit"));
        }

        void OnClickConfirm()
        {
            tmpTip1.text = "";
            tmpTip2.text = "";

            if (pwdMustCount > 0 && compInput1.value.Length != pwdMustCount)
            {
                string errMsg = string.Format(I18nMgr.T("The password must be {0} digits long"), pwdMustCount);
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip1.text = errMsg;
            }
            else if (pwdMustCount > 0 && compInput2.value.Length  != pwdMustCount)
            {
                string errMsg = string.Format(I18nMgr.T("The password must be {0} digits long"), pwdMustCount);
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip2.text = errMsg;
            }
            else if (pwdMustCount <0 && (compInput1.value.Length < 6 || compInput1.value.Length > 12))
            {
                string errMsg = string.Format(I18nMgr.T( "The password length must be at least {0} characters and no more than {1} characters"),6,12);
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip1.text = errMsg;
            }
            else if (pwdMustCount < 0 && (compInput2.value.Length < 6 || compInput2.value.Length > 12))
            {
                string errMsg = string.Format(I18nMgr.T("The password length must be at least {0} characters and no more than {1} characters"), 6, 12);
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip2.text = errMsg;
            }
            else if (string.IsNullOrEmpty(compInput1.value))
            {
                TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Please enter your new password first"));
            }
            else if (string.IsNullOrEmpty(compInput2.value))
            {
                TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Please enter your password confirmation"));
            }
            else if (compInput1.value != compInput2.value)
            {
                TipPopupHandler.Instance.OpenPopup(I18nMgr.T("The passwords entered twice are not the same"));
            }
            else
            {
                PageManager.Instance.ClosePage(this, new EventData<string>("Result", compInput1.value));
            }
        }

    }
}
