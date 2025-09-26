using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace PssOn00152
{

    public class PopupFreeSpinTrigger : PageCorBase
    {
        public GameObject goStartButton, goBaseAnchor, goAddFreeSpin7, goFreeSpin7;

        public Animator atorBG;

        const string COR_AUTO_CLOSE = "COR_AUTO_CLOSE";
        Button btnStartButton=> goStartButton.GetComponent<Button>();
        protected  void Awake()
        {
            //goStartButton = transform.Find("Anchor/Button Start").gameObject;

            btnStartButton.onClick.RemoveAllListeners();
            btnStartButton.onClick.AddListener(OnClickStartButton);

        }

        private void OnEnable()
        {
            AddNetButtonHandle();
        }
        private void OnDisable()
        {
            ClearAllCor();
            RemoveNetButtonHandle();
        }

        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            // DoCor(COR_AUTO_CLOSE, AutoClose());
            GameSoundHelper.Instance.PlayMusicSingle(SoundKey.FreeSpinTriggerBG);

            goAddFreeSpin7.SetActive(false);
            goFreeSpin7.SetActive(true);
            goStartButton.SetActive(true);
            if (data != null)
            {
                Dictionary<string,object> res = data.value as Dictionary<string,object>;

                if (res.ContainsKey("count"))
                {
                    goFreeSpin7.GetComponent<Text>().text = $"{(int)res["count"]}";
                }

                if (res.ContainsKey("autoCloseTimeS"))
                {
                   if(!PlayerPrefsUtils.isPauseAtPopupFreeSpinTrigger)
                        DoCor(COR_AUTO_CLOSE, AutoClose((float)res["autoCloseTimeS"]));
                }

                if (res.ContainsKey("isAddFreeGame") &&  (bool)res["isAddFreeGame"])
                {
                    DoCor(COR_AUTO_CLOSE, AutoClose(3f));
                    goAddFreeSpin7.SetActive(true);
                    goFreeSpin7.SetActive(false);
                    goStartButton.SetActive(false);
                }
            }


            goBaseAnchor.SetActive(true);
        }

        void ClosePage() => PageManager.Instance.ClosePage(this, new EventData<string>("PopupClose", ""));
        IEnumerator AutoClose(float timeS = 15f)
        {
            yield return new WaitForSeconds(timeS);

            goBaseAnchor.SetActive(false);

            atorBG.Play("End");
            yield return new WaitForSeconds(0.5f);

            ClosePage();
        }

        void OnClickStartButton() => ClosePage();



        #region 机台按钮事件
        public override void OnClickMachineButton(MachineButtonInfo info)
        {
            /*
            if (info != null)
            {
                switch (info.btnKey)
                {
                    case MachineButtonKey.BtnSpin:
                        if (info.isUp)
                            ClosePage();
                        break;
                }
            }*/
            if (info != null)
            {
                switch (info.btnKey)
                {
                    case MachineButtonKey.BtnSpin:
                        ShowUIAminButtonClick(btnStartButton, info);
                        break;
                }
            }

        }
        #endregion










        #region 网络远程按钮
        const string MARK_NET_BTN_POP_FREE_SPIN_TRIGGER = "MARK_NET_BTN_POP_FREE_SPIN_TRIGGER";
        void AddNetButtonHandle()
        {
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_SPIN,
                mark = MARK_NET_BTN_POP_FREE_SPIN_TRIGGER,
                onClick = OnNetBtnSpin,
            });

        }


        void RemoveNetButtonHandle() => NetButtonManager.Instance.ReomveHandles(MARK_NET_BTN_POP_FREE_SPIN_TRIGGER);


        void OnNetBtnSpin(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(this) != 0) return;

            if (!goStartButton.active)
            {
                info.onCallback?.Invoke(false);
                return;
            }

            //btnSkip
            NetButtonManager.Instance.ShowUIAminButtonClick(btnStartButton, MARK_NET_BTN_POP_FREE_SPIN_TRIGGER, NetButtonManager.BTN_SPIN);


            info.onCallback?.Invoke(true);
        }

        #endregion

    }
}