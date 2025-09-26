using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

namespace GameMaker
{
    public interface IMyInterface
    {
        // 接口方法，没有方法体
        void MyMethod();

        // 可以有属性
        int MyProperty { get; set; }

        // 可以有事件
        event Action MyEvent;
    }


    public interface ICommonPopup
    {
        void SetContent(EventData data);
    }


    public class SystemCommonPopup : BasePageSingleton<SystemCommonPopup> , ICommonPopup
    {

        public GameObject goOK, goYesNo, goOkWithTitle, goTextOnly, goSystemReset;

        private Dictionary<CommonPopupType, GameObject> poupContent; 
        protected  void Awake()
        {
            poupContent = new Dictionary<CommonPopupType, GameObject>()
            {
                [CommonPopupType.OK] = goOK,
                [CommonPopupType.OkWithTitle] = goOkWithTitle,
                [CommonPopupType.YesNo] = goYesNo,
                [CommonPopupType.TextOnly] = goTextOnly,
                [CommonPopupType.SystemReset] = goSystemReset,
            };
        }



        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);
            SetContent(data);
        }


        public void SetContent(EventData data)
        {
            if (data == null )
                return;

            CommonPopupInfo info = data.value as CommonPopupInfo;


            foreach (GameObject go in poupContent.Values)
            {
                go.SetActive(false);
            }
            poupContent[info.type].SetActive(true);

            Transform tfmTarget = poupContent[info.type].transform;
            TextMeshProUGUI tmpContent = null, tmpTitle = null, tmpBtn1 = null, tmpBtn2 = null;
            Button btn1 = null, btn2 = null, btnX = null;


            tmpContent = tfmTarget.Find("Base/Layout/Content Area/Contents Layout/Text").GetComponent<TextMeshProUGUI>();
            btnX = tfmTarget.Find("Base/Button Close Area/Button Close").GetComponent<Button>();

            switch (info.type)
            {
                case CommonPopupType.SystemReset:
                case CommonPopupType.OK:
                    tmpBtn1 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (1)/Anchor/Text").GetComponent<TextMeshProUGUI>();
                    btn1 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (1)").GetComponent<Button>();
                    break;
                case CommonPopupType.YesNo:
                    tmpBtn1 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (1)/Anchor/Text").GetComponent<TextMeshProUGUI>();
                    btn1 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (1)").GetComponent<Button>();
                    tmpBtn2 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (2)/Anchor/Text").GetComponent<TextMeshProUGUI>();
                    btn2 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (2)").GetComponent<Button>();
                    break;
                case CommonPopupType.OkWithTitle:
                    tmpTitle = tfmTarget.Find("Base/Layout/Title Area/Text").GetComponent<TextMeshProUGUI>();
                    tmpBtn1 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (1)/Anchor/Text").GetComponent<TextMeshProUGUI>();
                    btn1 = tfmTarget.Find("Base/Layout/Button Area/Button Layout/Button (1)").GetComponent<Button>();
                    break;
                case CommonPopupType.SystemTextOnly:
                case CommonPopupType.TextOnly:
                    break;
            }


            tmpContent.text = info.text;

            if (tmpTitle != null)
            {
                tmpTitle.text = info.title;
            }
            if (tmpBtn1 != null)
            {
                tmpBtn1.text = info.buttonText1;
            }
            if (tmpBtn2 != null)
            {
                tmpBtn2.text = info.buttonText2;
            }

            if (!info.isUseXButton)
            {
                btnX.onClick.RemoveAllListeners();
                btnX.gameObject.SetActive(false);
            }
            else
            {
                btnX.onClick.RemoveAllListeners();
                btnX.onClick.AddListener(() => {
                    info.callbackX?.Invoke();
                    CommonPopupHandler.Instance.ClosePopup();
                });
                btnX.gameObject.SetActive(true);
            }

            if (btn1 != null)
            {
                btn1.onClick.RemoveAllListeners();
                btn1.onClick.AddListener(() => {
                    info.callback1?.Invoke();
                    if (info.buttonAutoClose1)
                        CommonPopupHandler.Instance.ClosePopup();
                });
            }
            if (btn2 != null)
            {
                btn2.onClick.RemoveAllListeners();
                btn2.onClick.AddListener(() => {
                    info.callback2?.Invoke();
                    if (info.buttonAutoClose2)
                        CommonPopupHandler.Instance.ClosePopup();
                });
            }

        }

    }
}
