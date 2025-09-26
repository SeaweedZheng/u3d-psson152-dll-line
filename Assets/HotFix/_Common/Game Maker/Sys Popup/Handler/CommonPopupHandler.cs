using Console001;
using Game;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System;

#endif
using System.Collections.Generic;
using UnityEngine.UI;


namespace GameMaker
{
    public partial class CommonPopupHandler : MonoSingleton<CommonPopupHandler>
    {
        public List<CommonPopupInfo> errorStack = new List<CommonPopupInfo>();

        PageBase pageSystemPoup = null;

        public bool isOpen() => pageSystemPoup != null;

        public bool isOpen(string mark)
        {
            if (errorStack.Count > 0)
            {
                for (int i = 0; i < errorStack.Count; i++)
                {
                    if (errorStack[i].mark == mark)
                        return true;
                }
            }
            return false;
        }

        public CommonPopupType curPopupType => errorStack.Count > 0 ? errorStack[0].type : CommonPopupType.None;


        ICommonPopup iPopup => pageSystemPoup as ICommonPopup;

        //ConsoleCommonPopup iPopup => pageSystemPoup as ConsoleCommonPopup;

        PageName popupCommon = PageName.Console001PopupConsoleCommon;   //PageName.PopupSystemCommon;

        public void OpenPopup(CommonPopupInfo info)
        {
            if (!string.IsNullOrEmpty(info.mark))  //过滤重复消息，并置顶显示
            {
                int i = errorStack.Count;
                while (--i >= 0)
                {
                    if (errorStack[i].mark == info.mark)
                        errorStack.RemoveAt(i);
                }
            }

            errorStack.Insert(0, info);
            if (pageSystemPoup == null)
            {
                pageSystemPoup = PageManager.Instance.OpenPage(popupCommon, new EventData<CommonPopupInfo>("Null", info));
            }
            else
            {
                //pageSystemPoup.SetContent(new EventData<ErrorPopupInfo>("Null", info));
                iPopup.SetContent(new EventData<CommonPopupInfo>("Null", info));
            }
        }

        public void OpenPopupSingle(CommonPopupInfo info)
        {
            if (string.IsNullOrEmpty(info.mark))
                info.mark = info.text;
            OpenPopup(info);
        }

        public void ClosePopup()
        {
            if (pageSystemPoup == null)
                return;

            errorStack.RemoveAt(0);

            if (errorStack.Count == 0)
            {
                curButtons.Clear();
                PageManager.Instance.ClosePage(pageSystemPoup);
                pageSystemPoup = null;
            }
            else
            {
                CommonPopupInfo info = errorStack[0];
                //pageSystemPoup.SetContent(new EventData<ErrorPopupInfo>("Null", info));
                iPopup.SetContent(new EventData<CommonPopupInfo>("Null", info));
            }
        }


        public void ClosePopup(string mark)
        {
            if (pageSystemPoup == null)
                return;

            int idx = -1;
            for (int i = 0; i < errorStack.Count; i++)
            {
                if (errorStack[i].mark == mark)
                {
                    idx = i;
                    break;
                }
            }

            if (idx == 0)
                ClosePopup();
            else if (idx > 0)
                errorStack.RemoveAt(idx);
        }



        public void CloseAllPopup()
        {
            errorStack.Clear();
            //ClosePopup();
            if (pageSystemPoup != null)
            {
                PageManager.Instance.ClosePage(pageSystemPoup);
                pageSystemPoup = null;
            }
        }



#if UNITY_EDITOR


        [Button]
        public void TestErrorPoup1()
        {
            CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
            {
                text = I18nMgr.T("<size=24>打印机未连接</size>"),
                type = CommonPopupType.OK,
                buttonText1 = I18nMgr.T("OK"),
                buttonAutoClose1 = true,
                callback1 = delegate
                {
                },
                isUseXButton = false,
            });
        }



        [Button]
        public void TestErrorPoup(CommonPopupType type, string msg = "请输入内容", bool isUseXButton = true)
        {

            CommonPopupInfo info = new CommonPopupInfo();
            info.text = $"<size=24>{msg}</size>";
            info.type = type;

            info.buttonText1 = "No";
            info.buttonAutoClose1 = true;
            info.callback1 = delegate
            {
                DebugUtils.Log($"i am btn1 {msg}");
            };

            info.buttonText2 = "Yes";
            info.buttonAutoClose2 = true;
            info.callback2 = delegate
            {
                DebugUtils.Log($"i am btn2 {msg}");
            };

            info.isUseXButton = isUseXButton;
            info.callbackX = delegate
            {
                DebugUtils.Log($"i am btnX {msg}");
            };
            CommonPopupHandler.Instance.OpenPopupSingle(info);
        }
#endif


    }



    /// <summary>
    /// 网络按钮
    /// </summary>
    public partial class CommonPopupHandler : MonoSingleton<CommonPopupHandler> {


        public CommonPopupHandler()
        {
            AddNetButtonHandle();
        }

        // 析构函数：释放非托管资源
        ~CommonPopupHandler()
        {
            RemoveNetButtonHandle();
        }


        const string MARK_NET_BTN_COMMON_POP = "MARK_NET_BTN_COMMON_POP";
        void AddNetButtonHandle()
        {
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_SPIN,
                mark = MARK_NET_BTN_COMMON_POP,
                onClick = OnNetBtnOK,
            });
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_TICKET,
                mark = MARK_NET_BTN_COMMON_POP,
                onClick = OnNetBtnCancel,
            });
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_EXIT,
                mark = MARK_NET_BTN_COMMON_POP,
                onClick = OnNetBtnClose,
            });
        }

        void RemoveNetButtonHandle() => NetButtonManager.Instance.ReomveHandles(MARK_NET_BTN_COMMON_POP);


        void OnNetBtnOK(NetButtonInfo info)
        {
            if(info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;

            if (!isOpen()) return;

            if (PageManager.Instance.IndexOf(pageSystemPoup) != 0)return;


            bool isOk = false;
            if (curButtons.ContainsKey(BTN_OK))
            {
                NetButtonManager.Instance.ShowUIAminButtonClick(curButtons[BTN_OK], MARK_NET_BTN_COMMON_POP, NetButtonManager.BTN_SPIN);
                isOk = true;
            }
            if (info != null)
                info.onCallback?.Invoke(isOk);
        }

        void OnNetBtnCancel(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;

            if (!isOpen()) return;

            if (PageManager.Instance.IndexOf(pageSystemPoup) != 0) return;

            bool isOk = false;
            if (curButtons.ContainsKey(BTN_CANCLE))
            {
                NetButtonManager.Instance.ShowUIAminButtonClick(curButtons[BTN_CANCLE], MARK_NET_BTN_COMMON_POP, NetButtonManager.BTN_TICKET);
                isOk = true;
            }
            if (info != null)
                info.onCallback?.Invoke(isOk);
        }
        void OnNetBtnClose(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;

            if (!isOpen()) return;

            if (PageManager.Instance.IndexOf(pageSystemPoup) != 0) return;

            bool isOk = false;
            if (curButtons.ContainsKey(BTN_CLOSE))
            {
                NetButtonManager.Instance.ShowUIAminButtonClick(curButtons[BTN_CLOSE], MARK_NET_BTN_COMMON_POP, NetButtonManager.BTN_EXIT);
                isOk = true;
            }
            if (info != null)
                info.onCallback?.Invoke(isOk);
        }

        public const string BTN_CLOSE = "BTN_CLOSE";
        public const string BTN_OK = "BTN_OK";
        public const string BTN_CANCLE = "BTN_CANCLE";
        public Dictionary<string, Button> curButtons = new Dictionary<string, Button>();
    }
}
