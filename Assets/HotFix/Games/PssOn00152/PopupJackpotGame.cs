using Game;
using GameMaker;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Spine;
using EventData = GameMaker.EventData;

namespace PssOn00152
{
    public class PopupJackpotGame : PageCorBase
    {

        public GameObject goBaseAnchor,goButtonSkip;

        public FlyCoinController flyCoinCtr;

        public SkeletonAnimation sklanimJPTxt;


        // 这个准备弃用，改为sklanimJPTxt
        public SkeletonMecanim sklmecJPType01;

        public SkeletonMecanim sklmecJPType
        {
            get
            {
                if(sklmecJPType01!=null)
                    return sklmecJPType01;
                return i18JPIcon.goNow.transform.GetComponent<SkeletonMecanim>();
            }
        }

        public I18nGameObject i18JPIcon;


        Button btnSkip;
        Transform tfmBaseAnchor => goBaseAnchor.transform;
        Text txwWinCredit;

        const string COR_SHOW_JACKPOT_WIN_CREDIT = "COR_SHOW_JACKPOT_WIN_CREDIT";
        const string COR_AUTO_CLOSE = "COR_AUTO_CLOSE";



        private void Awake()
        {
            txwWinCredit = goBaseAnchor.transform.Find("Text").GetComponent<Text>();

            btnSkip = goButtonSkip.GetComponent<Button>();
            btnSkip.onClick.RemoveAllListeners();
            btnSkip.onClick.AddListener(OnClickSkip);

            goButtonSkip.SetActive(false);

            /*
            SkeletonAnimation test = new SkeletonAnimation();
            test.AnimationState.Event +=
            // 注册事件回调
            sklmecJPType.skeleton.
                //AnimationState.Event += OnSpineEvent;*/

            sklanimJPTxt.AnimationState.Event += OnSpineAnimationEvent;



        }



        void OnSpineAnimationEvent(TrackEntry trackEntry, Spine.Event e)
        {
            switch (e.Data.Name)
            {
                case "change":
                    {

                       int index = UnityEngine.Random.Range(0, 4);
                        string skinName = "";
                        switch (index)
                        {
                            case 0:
                                skinName = GRAND_SKIN_NAME;
                                break;
                            case 1:
                                skinName = MAJOR_SKIN_NAME;
                                break;
                            case 2:
                                skinName = MINOR_SKIN_NAME;
                                break;
                            case 3:
                                skinName = MINI_SKIN_NAME;
                                break;
                        }

                        sklanimJPTxt.Skeleton.SetSkin(skinName);

                        //Debug.LogError($"spine事件: change - {skinName}");
                    }
                    break;
                case "f_change":
                    {
                        string skinName = "";
                        switch (jackpotType)
                        {
                            case "jp1":
                            case "gard":
                                skinName = GRAND_SKIN_NAME;
                                break;
                            case "jp2":
                            case "major":
                                skinName = MAJOR_SKIN_NAME;
                                break;
                            case "jp3":
                            case "minor":
                                skinName = MINOR_SKIN_NAME;
                                break;
                            case "jp4":
                            case "mini":
                                skinName = MINI_SKIN_NAME;
                                break;
                        }

                        sklanimJPTxt.Skeleton.SetSkin(skinName);
                        //Debug.LogError($"spine事件: f_change - {skinName}");
                    }
                    break;
            }
        }

        private void OnEnable()
        {
            AddNetButtonHandle();
        }

        private void OnDisable()
        {

            ClearAllCor();
            flyCoinCtr.StopAllCoins();
            RemoveNetButtonHandle();
        }

        private void OnDestroy()
        {
            sklanimJPTxt.AnimationState.Event -= OnSpineAnimationEvent;
        }



        public override void OnClose(EventData data = null)
        {
            ClearAllCor();
            flyCoinCtr.StopAllCoins();

            base.OnClose(data);
        }

        Action onJPPoolSubCredit; // 彩金池减去彩金
       string jackpotType = "jp1";

        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            float fromCredit = 0;
            float totalEarnCredit = 1467;
            onJPPoolSubCredit = null;

            Dictionary<string, object> argDic = null;
            if (data != null)
            {
                argDic = (Dictionary<string, object>)data.value;
                jackpotType = (string)argDic["jackpotType"];
                totalEarnCredit = (float)argDic["totalEarnCredit"];
                onJPPoolSubCredit = (Action)argDic["onJPPoolSubCredit"];

                jackpotType = jackpotType.ToLower().Replace("jp_", "");
            }

            if (totalEarnCredit > 1000)
                fromCredit = (float)(totalEarnCredit / 1000) * 1000;


            DoCor(COR_SHOW_JACKPOT_WIN_CREDIT, ShowJackpotWinCredit(jackpotType,totalEarnCredit));
            //DoCor(COR_AUTO_CLOSE, AutoClose());


        }



        void OnClickSkip()
        {
            GameSoundHelper.Instance.PlayMusicSingle(SoundKey.JackpotEnd);
            ClosePage();
        }
        void ClosePage()
        {
            //GameSoundHelper.Instance.StopSound(SoundKey.JackpotBG);
            //GameSoundHelper.Instance.StopMusic();
            GameSoundHelper.Instance.StopSound(SoundKey.JackpotFlow);
            GameSoundHelper.Instance.StopSound(SoundKey.JackpotBG);

            PageManager.Instance.ClosePage(this, new EventData<string>("PopupClose", ""));
        }



        IEnumerator AutoClose(float timeM = 60f)
        {
            yield return new WaitForSeconds(timeM);
            ClosePage();
        }

        //皮肤命名
        const string GRAND_SKIN_NAME = "grand";
        const string MAJOR_SKIN_NAME = "major";
        const string MINOR_SKIN_NAME = "minor";
        const string MINI_SKIN_NAME = "mini";



        /// <summary>
        /// 设置显示的中奖彩金
        /// </summary>
        /// <param name="jackpotType"></param>
        void SetJackpotText(string jackpotType)
        {
            string targetType = jackpotType;
            if (jackpotType == "jp1")
                targetType = GRAND_SKIN_NAME;
            if (jackpotType == "jp2")
                targetType = MAJOR_SKIN_NAME;
            if (jackpotType == "jp3")
                targetType = MINOR_SKIN_NAME;
            if (jackpotType == "jp4")
                targetType = MINI_SKIN_NAME;

            string skinName = !string.IsNullOrEmpty(targetType) ? targetType : MINI_SKIN_NAME;

            sklmecJPType.initialSkinName = skinName;
            sklmecJPType.Initialize(true); //重新初始化
        }


        /*
         * 
         public SkeletonAnimation skeletonanimation;
         
         //换皮肤
         skeletonanimation.initialSkinName = "1";
         skeletonanimation.Initialize(true); //重新初始化
         skeletonanimation.AnimationState.Complete += completeEvent;//重新初始化后也需要重新注册动画回调事件函数
         skeletonanimation.AnimationState.SetAnimation(0, "a_01_idle1", true);

        //播放动画
        skeletonanimation.AnimationState.SetAnimation(0, "XXX_angry", false);//播放点击动画


    //定义动画回调事件函数
        public void completeEvent(Spine.TrackEntry trackEntry)
        {
            if (Touch)
            {
                Touch = false;
                //点击动画播放完成后切换回待机动画
                skeletonanimation.AnimationState.SetAnimation(0, "a_01_idle1", true);
            }
        }
        */
        IEnumerator ShowJackpotWinCredit(string jackpotType,float toCredit)
        {
            GameSoundHelper.Instance.StopMusic();
            GameSoundHelper.Instance.PlaySound(SoundKey.JackpotFlow);
            //GameSoundHelper.Instance.PlayMusicSingle(SoundKey.JackpotFlow);


            //【方案2】 重新开始播放动画
            sklanimJPTxt.AnimationState.SetAnimation(0, "begin_m", false);

            
            SetJackpotText(jackpotType); //【方案1】 这方法即将废弃掉


            txwWinCredit.text = toCredit.ToString("N0");

            CloseButttonSpin();
            goBaseAnchor.SetActive(false);
            goButtonSkip.SetActive(false);


            yield return new WaitForSeconds(12f);  //先掉金币??


            GameSoundHelper.Instance.PlaySound(SoundKey.JackpotBG);
            //GameSoundHelper.Instance.PlayMusicSingle(SoundKey.JackpotBG);

            // yield return new WaitForSeconds(2f);

            OpenButtonSpin();
            goBaseAnchor.SetActive(true);
            goButtonSkip.SetActive(true);


            // 彩金池减去彩金
            onJPPoolSubCredit?.Invoke();
            onJPPoolSubCredit = null;

            //渐变
            float scale = 0.8f;
            while (scale < 1.5f)
            {
                tfmBaseAnchor.localScale = new Vector3(scale, scale, 1f);
                scale += 0.1f;
                yield return new WaitForSeconds(0.04f);
            }

            //渐变
            scale = 1.5f;
            while (scale >= 1f)
            {
                tfmBaseAnchor.localScale = new Vector3(scale, scale, 1f);
                scale -= 0.1f;
                yield return new WaitForSeconds(0.05f);
            }

            tfmBaseAnchor.localScale = new Vector3(1f, 1f, 1f);

            yield return new WaitForSeconds(1.5f);

            // 抛硬币
            flyCoinCtr.FlayCoins();

            if (!PlayerPrefsUtils.isPauseAtPopupJackpotGame)
                DoCor(COR_AUTO_CLOSE, AutoClose(6));
        }

        void CloseButttonSpin() => machineButtonEventDispatcher.ChangeButtonShow(new List<MachineButtonKey>() {
                    }, MachineButtonType.Regular, false);

        void OpenButtonSpin() => machineButtonEventDispatcher.ChangeButtonShow(new List<MachineButtonKey>() {
                        MachineButtonKey.BtnSpin,
                    }, MachineButtonType.Regular, false);

        #region 机台按钮事件
        public override void OnClickMachineButton(MachineButtonInfo info)
        {
            if (info != null)
            {
                switch (info.btnKey)
                {
                    case MachineButtonKey.BtnSpin:

                        //if (info.isUp && btnSkip.gameObject.active) ClosePage();
                        if (btnSkip.gameObject.active)
                            ShowUIAminButtonClick(btnSkip, info);
                        break;
                }
            }
        }
        #endregion





        #region 网络远程按钮
        const string MARK_NET_BTN_POP_GAME_JACKPOT = "MARK_NET_BTN_POP_GAME_JACKPOT";
        void AddNetButtonHandle()
        {
            NetButtonManager.Instance.AddHandles(new NetButtonHandle()
            {
                buttonName = NetButtonManager.BTN_SPIN,
                mark = MARK_NET_BTN_POP_GAME_JACKPOT,
                onClick = OnNetBtnSpin,
            });

        }

        void RemoveNetButtonHandle() => NetButtonManager.Instance.ReomveHandles(MARK_NET_BTN_POP_GAME_JACKPOT);


        void OnNetBtnSpin(NetButtonInfo info)
        {
            if (info.dataType != NetButtonManager.DATA_MACHINE_BUTTON_CONTROL) return;
            if (PageManager.Instance.IndexOf(this) != 0) return;

            if (!btnSkip.gameObject.active)
            {
                info.onCallback?.Invoke(false);
                return;
            }


            if (btnSkip.gameObject.active)
                NetButtonManager.Instance.ShowUIAminButtonClick(btnSkip, MARK_NET_BTN_POP_GAME_JACKPOT, NetButtonManager.BTN_SPIN);


            info.onCallback?.Invoke(true);
        }


        #endregion
    }

}