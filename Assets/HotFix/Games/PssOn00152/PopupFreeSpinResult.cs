using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using GameMaker;
using UnityEngine.EventSystems;
//ChinaTown100
namespace PssOn00152
{

    public class PopupFreeSpinResult : PageCorBase
    {

        public GameObject goTotalWin, goBG;

        private Text txtTotalWin => goTotalWin.GetComponent<Text>();


        private Button btnBG => goBG.GetComponent<Button>();

        protected  void Awake()
        {

            goTotalWin = transform.Find("Anchor/Total Win").gameObject;


            goBG = transform.Find("Midground/BG").gameObject;
            btnBG.onClick.RemoveAllListeners();
            btnBG.onClick.AddListener(OnClickBG);
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

        const string COR_MOVE_TOTAL_CREDIT = "COR_MOVE_TOTAL_CREDIT";
        const string COR_SHOW_TOTAL_CREDIT = "COR_SHOW_TOTAL_CREDIT";
        const string COR_AUTO_CLOSE = "COR_AUTO_CLOSE";
        double _toCredit;
        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            double fromCredit = 0;
            double toCredit = 1467.1f;
            if (data != null)
                toCredit = (double)data.value;
            if (toCredit > 1000)
                fromCredit = (int)(toCredit / 1000) * 1000f;

            DoCor(COR_MOVE_TOTAL_CREDIT, MoveCredit(fromCredit, toCredit));

            //DoCor(COR_AUTO_CLOSE, AutoClose());

            GameSoundHelper.Instance.PlayMusicSingle(SoundKey.FreeSpinResultBG);
        }


        void ClosePage ()=> PageManager.Instance.ClosePage(this, new EventData<string>("PopupClose", "Free Spin Result Popup Closed"));
        IEnumerator AutoClose(float timeM = 15f)
        {
            yield return new WaitForSeconds(timeM);
            ClosePage();
        }

        IEnumerator MoveCredit(double fromCredit, double toCredit)
        {
            _toCredit = toCredit;

            float scale = 0.7f;
            goTotalWin.transform.localScale = new Vector3(scale, scale, 1);

            goTotalWin.SetActive(false);

            yield return new WaitForSeconds(0.8f);

            goTotalWin.SetActive(true);


            double credit = fromCredit;

            float startTimeS = Time.unscaledTime;
            do
            {
                credit += 10;
                if (credit > toCredit)
                    credit = toCredit;

                txtTotalWin.text = ((long)credit).ToString(); // 不要千分号  credit.ToString("N0");

                yield return new WaitForSeconds(0.05f);

            } while (credit < toCredit && Time.unscaledTime - startTimeS <4f);

            txtTotalWin.text = ((long)toCredit).ToString(); // 不要千分号  credit.ToString("N0");

            while (scale < 1.2)
            {
                scale += 0.05f;
                goTotalWin.transform.localScale = new Vector3(scale, scale, 1);
                yield return new WaitForSeconds(0.02f);
            }
            scale = 1.2f;
            while (scale > 1)
            {
                scale -= 0.05f;
                goTotalWin.transform.localScale = new Vector3(scale, scale, 1);
                yield return new WaitForSeconds(0.02f);
            }
            scale = 1f;
            goTotalWin.transform.localScale = new Vector3(scale, scale, 1);

            DoCor(COR_AUTO_CLOSE, AutoClose(4f));
        }

        void ShowCredit(double credit)
        {
            ClearCor(COR_MOVE_TOTAL_CREDIT);
            goTotalWin.SetActive(true);
            goTotalWin.transform.localScale = new Vector3(1, 1, 1);
            txtTotalWin.text = ((long)credit).ToString(); // 不要千分号  credit.ToString("N0");

            DoCor(COR_AUTO_CLOSE, AutoClose(4f));
        }

        void OnClickBG()
        {
            if (IsCor(COR_MOVE_TOTAL_CREDIT))
            {
                ShowCredit(_toCredit);
            }
            else
            {
                ClearCor(COR_AUTO_CLOSE);
                ClosePage();
            }
        }


        #region 机台按钮事件
        MachineButtonEventDispatcher machineButtonEventDispatcher => transform.GetComponent<MachineButtonEventDispatcher>();
        public override void OnTop()
        {
            base.OnTop();
            if (machineButtonEventDispatcher != null)
                machineButtonEventDispatcher.OnTop();
        }
        public override void OnClickMachineButton(MachineButtonInfo info)
        {
            if (info != null)
            {
                switch (info.btnKey)
                {
                    case MachineButtonKey.BtnSpin:
                        if (info.isUp)
                            OnClickBG();
                        break;
                }
            }
        }
        #endregion





        #region 网络远程按钮
        const string MARK_NET_BTN_POP_FREE_SPIN_RESULT = "MARK_NET_BTN_POP_FREE_SPIN_RESULT";
        void AddNetButtonHandle()
        {
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_SPIN,
                mark = MARK_NET_BTN_POP_FREE_SPIN_RESULT,
                onClick = OnNetBtnSpin,
            });

        }


        void RemoveNetButtonHandle() => NetButtonManager.Instance.ReomveHandles(MARK_NET_BTN_POP_FREE_SPIN_RESULT);


        void OnNetBtnSpin(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(this) != 0) return;


            OnClickBG();

            info.onCallback?.Invoke(true);
        }

        #endregion
    }
}
