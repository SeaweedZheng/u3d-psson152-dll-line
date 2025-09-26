using Game;
using GameMaker;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace PssOn00152
{
    public class PopupQrCoinIn : PageCorBase
    {
        public GameObject goQr;
        public Button btnClose;

        string qrcodeUrl;

        public void Awake()
        {
            btnClose.onClick.AddListener(OnClose);
        }




        public void OnEnable()
        {
           EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, OnIotCoinInCompleted);
            AddNetButtonHandle();
        }
        public void OnDisable()
        {
            EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_COIN_IN_OUT_EVENT, OnIotCoinInCompleted);
            RemoveNetButtonHandle();
        }

        void OnIotCoinInCompleted(EventData res )
        {
            if (res.name == GlobalEvent.IOTCoinInCompleted)
            {
                int creditIn = (int)res.value;

                TipPopupHandler.Instance.OpenPopupOnce(string.Format(I18nMgr.T("recharge {0} game credits completed."), creditIn));
            }
        }



        public void OnClose() => PageManager.Instance.ClosePage(this);
        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            if(data != null)
            {
                Dictionary<string,object> agrs = (Dictionary<string, object>)data.value;

                qrcodeUrl = (string)agrs["qrcodeUrl"];
                CreatQrImage(qrcodeUrl, 220);
            }
            else
            {
                CreatQrImage("hello word", 220);
            }

        }

        void CreatQrImage(string qrcodeUrl, float size = 220)
        {       
            Texture2D t2d = Utils.GenerateQRImageWithColor(qrcodeUrl, UnityEngine.Color.black, (int)size, (int)size);
            //goQr.GetComponent<Image>().material.mainTexture = t2d;
            Sprite sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
            goQr.GetComponent<Image>().sprite = sprite;
            goQr.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 220);
        }



        #region 机台按钮事件
        public override void OnClickMachineButton(MachineButtonInfo info)
        {
            if (info != null)
            {
                switch (info.btnKey)
                {
                    case MachineButtonKey.BtnSpin:
                        ShowUIAminButtonClick(btnClose, info);
                        break;
                }
            }
        }
        #endregion





        #region 网络远程按钮
        const string MARK_NET_BTN_POP_IOT_QR_CODE = "MARK_NET_BTN_POP_IOT_QR_CODE";
        void AddNetButtonHandle()
        {
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_SPIN,
                mark = MARK_NET_BTN_POP_IOT_QR_CODE,
                onClick = OnNetBtnSpin,
            });

        }


        void RemoveNetButtonHandle() => NetButtonManager.Instance.ReomveHandles(MARK_NET_BTN_POP_IOT_QR_CODE);


        void OnNetBtnSpin(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(this) != 0) return;


            //btnSkip
            NetButtonManager.Instance.ShowUIAminButtonClick(btnClose, MARK_NET_BTN_POP_IOT_QR_CODE, NetButtonManager.BTN_SPIN);


            info.onCallback?.Invoke(true);
        }

        #endregion

    }


}
