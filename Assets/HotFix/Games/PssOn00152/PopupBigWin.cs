using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PssOn00152
{

    /// <summary>
    /// 中奖弹窗
    /// </summary>
    /// <remarks>
    ///  旧版本中奖弹窗（720*1280）<br/>
    /// </remarks>
    public partial class PopupBigWin : PageCorBase
    {
        public GameObject goTotalWin, goBG;

        private Text txtTotalWin => goTotalWin.GetComponent<Text>();
        
        private Button btnBG => goBG.GetComponent<Button>();


        protected  void Awake()
        {
            goTotalWin = transform.Find("Anchor/Base/Anchor/Text").gameObject;

            goBG = transform.Find("Midground/BG").gameObject;
            btnBG.onClick.RemoveAllListeners();
            btnBG.onClick.AddListener(OnClickBG);
        }

        private void OnDisable()
        {
            ClearAllCor();
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
           // if (data != null)
           //     toCredit = (double)data.value;

            Dictionary<string, object> argDic = null;
            if (data != null)
            {
                argDic = (Dictionary<string, object>)data.value;
                toCredit = (long)argDic["totalEarnCredit"];
            }

            //if(toCredit>1000) fromCredit = (int)(toCredit / 1000) * 1000f;

            if (toCredit > 1000) fromCredit = (int)(toCredit * 0.6);
            if (toCredit - fromCredit > 2000) fromCredit = toCredit - 2000;

            DoCor(COR_MOVE_TOTAL_CREDIT, MoveCredit(fromCredit, toCredit));
            DoCor(COR_AUTO_CLOSE, AutoClose());

            GameSoundHelper.Instance.PlaySound(SoundKey.PopupWinBG);
            //GameSoundHelper.Instance.PlayMusicSingle(SoundKey.PopupWinBG);

            int indx = Random.Range(1,3);
            GameSoundHelper.Instance.PlaySound(indx ==1?SoundKey.PopupWinCongratulate01: SoundKey.PopupWinCongratulate02);
        }

        void ClosePage() {
            GameSoundHelper.Instance.StopSound(SoundKey.PopupWinBG);
            //GameSoundHelper.Instance.StopMusic();
            PageManager.Instance.ClosePage(this, new EventData<string>("PopupClose", ""));
        } 
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

            yield return new WaitForSeconds(0.5f);

            goTotalWin.SetActive(true);


            double credit = fromCredit;
            do
            {
                credit += 10;
                if (credit > toCredit)
                    credit = toCredit;

                txtTotalWin.text = ((long)credit).ToString(); // 不要千分号  credit.ToString("N0");

                yield return new WaitForSeconds(0.08f);

            } while (credit < toCredit) ;

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

            GameSoundHelper.Instance.PlaySound(SoundKey.BigWin);
            GameSoundHelper.Instance.StopSound(SoundKey.PopupWinBG);
            GameSoundHelper.Instance.PlaySound(SoundKey.PopupWinBeforeClose);
        }

        void ShowCredit(double credit)
        {
            ClearCor(COR_MOVE_TOTAL_CREDIT);
            goTotalWin.SetActive(true);
            goTotalWin.transform.localScale = new Vector3(1, 1, 1);
            txtTotalWin.text = ((long)credit).ToString(); //credit.ToString("N0");

            GameSoundHelper.Instance.PlaySound(SoundKey.BigWin);
            GameSoundHelper.Instance.StopSound(SoundKey.PopupWinBG);
            GameSoundHelper.Instance.PlaySound(SoundKey.PopupWinBeforeClose);
        }


        #region 机台按钮事件
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


        void OnClickBG()
        {
            if (IsCor(COR_MOVE_TOTAL_CREDIT))
            {
                ShowCredit(_toCredit);
            }
            else
            {
                ClosePage();
                //PageManager.Instance.ClosePage(this);
            }
        }
    }
}
