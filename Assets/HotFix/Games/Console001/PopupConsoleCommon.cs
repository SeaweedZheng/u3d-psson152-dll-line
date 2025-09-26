using Game;
using GameMaker;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Console001
{
    public class PopupConsoleCommon : PageMachineButtonBase ,ICommonPopup
    {
        public GameObject goOK, goYesNo, goTextOnly;

        private Dictionary<CommonPopupType, GameObject> poupContent;

        Button btn1 = null, btn2 = null, btnX = null;
        void Awake()
        {
            poupContent = new Dictionary<CommonPopupType, GameObject>()
            {
                [CommonPopupType.OK] = goOK,
                [CommonPopupType.YesNo] = goYesNo,
                [CommonPopupType.TextOnly] = goTextOnly,
                [CommonPopupType.SystemReset] = goOK,
                [CommonPopupType.SystemTextOnly] = goTextOnly,
            };
        }
        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);
            SetContent(data);
        }

        CommonPopupInfo curInfo;
        public void SetContent(EventData data)
        {
            if (data == null)
                return;

            curInfo = data.value as CommonPopupInfo;

            foreach (GameObject go in poupContent.Values)
            {
                go.SetActive(false);
            }
            poupContent[curInfo.type].SetActive(true);
            //poupContent[info.type].transform.SetSiblingIndex(poupContent[info.type].transform.parent.childCount-1);

            Transform tfmTarget = poupContent[curInfo.type].transform;
            TextMeshProUGUI tmpContent = null, tmpBtn1 = null, tmpBtn2 = null;

            CommonPopupHandler.Instance.curButtons.Clear();

            tmpContent = tfmTarget.Find("Base/Layout/Content Area/Contents Layout/Text").GetComponent<TextMeshProUGUI>();
            btnX = tfmTarget.Find("Base/Button Close Area/Button Close").GetComponent<Button>();

            switch (curInfo.type)
            {
                case CommonPopupType.SystemReset:
                case CommonPopupType.OK:
                    tmpBtn1 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (1)/Anchor/Text").GetComponent<TextMeshProUGUI>();
                    btn1 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (1)").GetComponent<Button>();
                    CommonPopupHandler.Instance.curButtons.Add(CommonPopupHandler.BTN_OK, btn1);
                    break;
                case CommonPopupType.YesNo:
                    tmpBtn1 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (1)/Anchor/Text").GetComponent<TextMeshProUGUI>();
                    btn1 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (1)").GetComponent<Button>();
                    tmpBtn2 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (2)/Anchor/Text").GetComponent<TextMeshProUGUI>();
                    btn2 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (2)").GetComponent<Button>();
                    CommonPopupHandler.Instance.curButtons.Add(CommonPopupHandler.BTN_CANCLE, btn1);
                    CommonPopupHandler.Instance.curButtons.Add(CommonPopupHandler.BTN_OK, btn2);
                    break;
                case CommonPopupType.SystemTextOnly:
                case CommonPopupType.TextOnly:
                    break;
            }


            tmpContent.text = curInfo.text;

            if (tmpBtn1 != null)
            {
                tmpBtn1.text = curInfo.buttonText1;
            }
            if (tmpBtn2 != null)
            {
                tmpBtn2.text = curInfo.buttonText2;
            }

            if (!curInfo.isUseXButton)
            {
                btnX.onClick.RemoveAllListeners();
                btnX.gameObject.SetActive(false);
            }
            else
            {
                btnX.onClick.RemoveAllListeners();
                btnX.onClick.AddListener(() => {
                    curInfo.callbackX?.Invoke();
                    //PageManager.Instance.ClosePage(this);
                    CommonPopupHandler.Instance.ClosePopup();
                });
                btnX.gameObject.SetActive(true);
                CommonPopupHandler.Instance.curButtons.Add(CommonPopupHandler.BTN_CLOSE, btnX);
            }

            if (btn1 != null)
            {
                btn1.onClick.RemoveAllListeners();
                btn1.onClick.AddListener(() => {
                    curInfo.callback1?.Invoke();
                    if (curInfo.buttonAutoClose1)
                        //PageManager.Instance.ClosePage(this);
                        CommonPopupHandler.Instance.ClosePopup();
                });
            }
            if (btn2 != null)
            {
                btn2.onClick.RemoveAllListeners();
                btn2.onClick.AddListener(() => {
                    curInfo.callback2?.Invoke();
                    if (curInfo.buttonAutoClose2)
                        //PageManager.Instance.ClosePage(this);
                        CommonPopupHandler.Instance.ClosePopup();
                });
            }

            if (PageManager.Instance.IsTop(this))
                AutoSetMachineButton();
        }
        public override void OnTop()
        {
            AutoSetMachineButton();
        }

        void AutoSetMachineButton()
        {

            switch (curInfo.type)
            {
                case CommonPopupType.SystemReset:
                case CommonPopupType.OK:
                    machineButtonEventDispatcher.ChangeButtonShow(new List<MachineButtonKey>() {
                        MachineButtonKey.BtnSpin,
                    }, MachineButtonType.Regular,false);
                    break;
                case CommonPopupType.YesNo:
                    machineButtonEventDispatcher.ChangeButtonShow(new List<MachineButtonKey>() {
                        MachineButtonKey.BtnSpin,
                        MachineButtonKey.BtnTicketOut,
                    }, MachineButtonType.Regular, true);
                    break;
                case CommonPopupType.SystemTextOnly:
                case CommonPopupType.TextOnly:
                default:
                    machineButtonEventDispatcher.ChangeButtonShow(new List<MachineButtonKey>(), MachineButtonType.Regular, false);
                    break;
            }
        }


        public override void OnClickMachineButton(MachineButtonInfo info)
        {
            if (info != null)
            {
                /*if (info.isUp) { 
                    switch (info.btnKey)
                    {
                        case MachineButtonKey.BtnSpin:

                            if (goOK.gameObject.active)
                            {
                                btn1.onClick.Invoke();
                            }
                            break;
                    }
                }*/

                /*
                if (goOK.gameObject.active 
                    && (ErrorPopupHandler.Instance.curPopupType == ErrorPopupType.OK 
                        || ErrorPopupHandler.Instance.curPopupType == ErrorPopupType.SystemReset )
                    )
                {
                    switch (info.btnKey)
                    {
                        case MachineButtonKey.BtnSpin:
                            ShowUIAminButtonClick(btn1,info);
                            break;
                    }
                }*/
                
                if (curInfo.type == CommonPopupType.OK|| curInfo.type == CommonPopupType.SystemReset)
                {
                    switch (info.btnKey)
                    {
                        case MachineButtonKey.BtnSpin:
                            ShowUIAminButtonClick(btn1, info);
                            break;
                    }
                }
                else if (curInfo.type == CommonPopupType.YesNo)
                {
                    switch (info.btnKey)
                    {
                        case MachineButtonKey.BtnSpin:
                            ShowUIAminButtonClick(btn2, info);
                            break;
                        case MachineButtonKey.BtnTicketOut:
                            ShowUIAminButtonClick(btn1, info);
                            break;
                    }
                }
            }
        }
    }
}
