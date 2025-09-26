using Game;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameMaker;
using System.Collections.Generic;
using System;
//using _mainBB = GameMaker.MainBlackboard;

using _consoleBB = PssOn00152.ConsoleBlackboard02;
using Newtonsoft.Json.Linq;


namespace PssOn00152
{
    public class PopupGameLoading :PageMachineButtonBase
    {

        public GameObject goProgressBar,goText, goVer;
        public Button btnEnterGame;

        private Text txtContent => goText.GetComponent<Text>();

        private Text txtVer => goVer.GetComponent<Text>();

        private Slider slider;

        protected  void Awake()
        {

            slider = goProgressBar.transform.Find("Progress Bar UV").GetComponent<Slider>();

            btnEnterGame.onClick.RemoveAllListeners();
            btnEnterGame.onClick.AddListener(OnClickButton);

            btnEnterGame.gameObject.SetActive(false);
            goProgressBar.SetActive(true);
        }

        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            txtVer.text = GlobalData.hotfixVersion;

            CheckUpdateInfo();
        }



        async void CheckUpdateInfo()
        {
            SetProgress(0, $"({(0).ToString("N0")}%)");

            if (_consoleBB.Instance.isUpdateInfo)  {


                TextAsset textAsset = ResourceManager.Instance.LoadAssetAtPathOnce<TextAsset>("Assets/GameRes/LuBan/GenerateDatas/bytes/i18n_old_hotfix_info_notice.json");
                //JArray jArray = JArray.Parse(textAsset.text);
                Debug.Log("新版打包获取文件结果 ：" + textAsset.text);  


                string data = I18nManager.Instance.LoadData("i18n_old_hotfix_info_notice").ToString();
                Debug.Log(data);
                JArray jArray = JArray.Parse(data);

                string content = null;
                for (int i = 0; i<jArray.Count; i++)
                {
                    JToken item = jArray[i];

                    // if (item["hotfix_key"].ToObject<string>() == GlobalData.hotfixKey  && item["hotfix_version"].ToObject<string>() == GlobalData.hotfixVersion)
                    if (item["hotfix_key"].ToObject<string>() == GlobalData.hotfixKey)
                    {
                        content = item[I18nMgr.language.ToString()].ToObject<string>();
                        break;
                    }
                }


                // JSON.Parse(textAsset.text);


                if (content != null)
                {
                    UpdateInfoHelper info = new UpdateInfoHelper(); 

                    EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleNotice,
                        new EventData<Dictionary<string, object>>("",
                            new Dictionary<string, object>()
                            {
                                ["title"] = info.GetTitle(),
                                ["content"] = info.GetUpdateContent(content),
                                ["AutoCloseMS"] = 10000,
                            }));
                }
            }

            StartCoroutine(Loading());
        }


        void SetProgress(float val,string msg)
        {
            slider.value = val;
            txtContent.text = msg;
        }
        IEnumerator Loading()
        {

            float val = 0f;
            string content = "加载中...";

            SetProgress(val, $"{content}({(val * 100).ToString("N0")}%)");

            while (val<1)
            {

                SetProgress(val, $"{content}({(val * 100).ToString("N0")}%)");
                yield return new WaitForSeconds(0.10f);

                val += 0.05f;
            }

            SetProgress(1, $"{content}({(100).ToString("N0")}%)");

            yield return new WaitForSeconds(0.8f);

            //btnEnterGame.gameObject.SetActive(true);
            //goProgressBar.SetActive(false);

            OnClickButton();
        }
        void OnClickButton()
        {
            if (GlobalData.isProtectApplication)
                return;
            Debug.Log("保护解除");
            PageManager.Instance.OpenPage(PageName.PO152PageGameMain1080);
            PageManager.Instance.ClosePage(this);
        }





        #region 机台按钮事件
        public override void OnClickMachineButton(MachineButtonInfo info)
        {
            if (info != null)
            {
                switch (info.btnKey)
                {
                    case MachineButtonKey.BtnSpin:

                        if (btnEnterGame.gameObject.active)
                        {
                            ShowUIAminButtonClick(btnEnterGame, info);
                        }
                            /*if (!info.isUp && btnEnterGame.interactable && btnEnterGame.gameObject.active )
                            {
                                只有按下动画，不触发事件
                                btnEnterGame.OnPointerDown(new PointerEventData(null)
                                {
                                    button = PointerEventData.InputButton.Left,
                                });
                                //ShowUIAminButtonClick(btnEnterGame, info);
                            }
                            else if (info.isUp && btnEnterGame.interactable && btnEnterGame.gameObject.active)
                            {
                                只有弹起动画，不触发事件
                                btnEnterGame.OnPointerUp(new PointerEventData(null)
                                {
                                    button = PointerEventData.InputButton.Left,
                                });

                                DoCor("CLICK_BTN", DoTask(() =>
                                {
                                    //btnEnterGame.OnSubmit(null);  // 或 OnClickButton();
                                    btnEnterGame.onClick.Invoke();
                                },150));
                                //ShowUIAminButtonClick(btnEnterGame, info);
                            }*/
                        break;
                }
            }
        }
        #endregion




    }


}
