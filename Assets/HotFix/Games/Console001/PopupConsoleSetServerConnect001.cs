using Game;
using GameMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Console001
{
    public class PopupConsoleSetServerConnect001 : PageMachineButtonBase
    {
        public TextMeshProUGUI tmpTitle, tmpTip1, tmpTip2;

        public Button btnClose, btnConfirm;

        public CompInputController compInputIP;
        public CompInputController compInputPort;


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
                if (argDic.ContainsKey("title"))
                {
                    tmpTitle.text = (string)argDic["title"];
                }
            }

        }


        void InitParam()
        {
            //password1 = "";
            //password2 = "";
            compInputIP.value = "";
            compInputPort.value = "";
            tmpTip1.text = "";
            tmpTip2.text = "";
        }


        void OnClickConfirm()
        {
            tmpTip1.text = "";
            tmpTip2.text = "";



            int port = -1;

            bool isPortNumb = false;

            try
            {
                port = string.IsNullOrEmpty(compInputPort.value) ? -1 : int.Parse(compInputPort.value);
            }
            catch (Exception e)
            {
                isPortNumb = false;
            }

            /*
            if (ip1 < 0 && ip2 < 0 && ip3 < 0 && ip4 < 0)
            {
                string errMsg = string.Format(I18nMgr.T("Please enter {0}"), I18nMgr.T("Server IP Address"));
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip1.text = errMsg;
            }
            else if (ip1 < 0 || ip1 > 255 || ip2 < 0 || ip2 > 255
                || ip3 < 0 || ip3 > 255 || ip4 < 0 || ip4 > 255)
            {
                string errMsg = string.Format(I18nMgr.T("The {0} must be between {1} and {2}"),
                    I18nMgr.T("Server IP Address"), "0.0.0.0", "255.255.255.255");
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip1.text = errMsg;
            }
            else */
            if (string.IsNullOrEmpty(compInputIP.value))
            {
                string errMsg = string.Format(I18nMgr.T("Please enter {0}"), I18nMgr.T("Server IP Address"));
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip1.text = errMsg;
            }
            else if (port < 0)
            {
                string errMsg = string.Format(I18nMgr.T("Please enter {0}"), I18nMgr.T("Server Port Number"));
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip2.text = errMsg;
            }
            else if ((port < 0 || port > 65535) || isPortNumb)
            {
                string errMsg = string.Format(I18nMgr.T("The {0} must be between {1} and {2}"),
                    I18nMgr.T("Server Port Number"), 0, 65535);
                TipPopupHandler.Instance.OpenPopup(errMsg);
                tmpTip2.text = errMsg;
            }
            else
            {
                string addr = $"{compInputIP.value}:{port}";
                PageManager.Instance.ClosePage(this, new EventData<string>("Result", addr));

                Debug.Log($"addr = {addr}");
            }
        }

    }
}