using Game;
using GameMaker;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Console001
{
    public class PopupConsoleChooseLanguage : PageMachineButtonBase
    {
        public TextMeshProUGUI tmpSelectLanguage, tmpTitle;

        public GameObject goContent;

        public Button btnClose, btnSave;

        string curSelectNumber;



        /// <summary>
        /// 可选列表
        /// </summary>
        /// <remarks>
        /// * "cn" -- "中文(简体)"<br/>
        /// * "0" -- "公众号"<br/>
        /// </remarks>
        Dictionary<string, string> numberSelectName = new Dictionary<string, string>();


        string selectedDes;
        string title;
        void Awake()
        {
            btnClose.onClick.AddListener(OnClickClose);
            btnSave.onClick.AddListener(OnClickSave);
        }

        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            title = "Choose Language";
            selectedDes = "Selected language: {0}";
            if (data.value != null)
            {
                Dictionary<string, object> args = data.value as Dictionary<string, object>;

                //numberLanguageName = (Dictionary<string, string>)args["languageLst"];
                //curLanguageNumber = (string)args["languageNumber"];

                numberSelectName = (Dictionary<string, string>)args["selectLst"];
                curSelectNumber = (string)args["selectNumber"];
                if (args.ContainsKey("selectedDes"))
                {
                    selectedDes = (string)args["selectedDes"];
                }
                if (args.ContainsKey("title"))
                {
                    title = (string)args["title"];
                }
            }

            initParam();
        }

        void initParam()
        {
            //OnChooseLanguage(curLanguageNumber);

            Transform tfmContent = goContent.transform;
            Transform clone = tfmContent.GetChild(0);
            float itemHeight = clone.GetComponent<RectTransform>().sizeDelta.y;
            float height = 0;
            for (int i = tfmContent.childCount; i < numberSelectName.Count; i++)
            {
                Transform go = GameObject.Instantiate(clone);
                go.SetParent(tfmContent);
                go.localScale = Vector3.one;
            }

            foreach (Transform item in tfmContent)
            {
                item.gameObject.SetActive(false);
            }

            for (int i = 0; i < numberSelectName.Count; i++)
            {
                height += itemHeight;
                KeyValuePair<string, string> kv = numberSelectName.ElementAt(i);
                Transform item = tfmContent.GetChild(i);
                item.gameObject.SetActive(true);
                item.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{kv.Value}";
                item.GetComponent<Button>().onClick.RemoveAllListeners();
                item.GetComponent<Button>().onClick.AddListener(
                    () => OnChooseLanguage(kv.Key)
                );

                if (kv.Key == curSelectNumber)
                {
                    item.GetComponent<Image>().color = ThemeColorSettings.HIGHLIGHT_ITEM_BG;
                }
                else
                {
                    item.GetComponent<Image>().color = ThemeColorSettings.NORMAL_ITEM_BG;
                }
            }


            goContent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(
                goContent.transform.GetComponent<RectTransform>().sizeDelta.x,
                height);


            tmpSelectLanguage.text = string.Format(I18nMgr.T(selectedDes), numberSelectName[curSelectNumber]);

            tmpTitle.text = I18nMgr.T(title);
        }

        void OnChooseLanguage(string number)
        {
            curSelectNumber = number;
            //tmpSelectLanguage.text = string.Format(I18nMgr.T("Selected language: {0}"), numberLanguageName[curLanguageNumber]);
            PageManager.Instance.ClosePage(this, new EventData<string>("Result", curSelectNumber));
        }

        void OnClickClose() => PageManager.Instance.ClosePage(this, new EventData("Exit"));

        void OnClickSave() => PageManager.Instance.ClosePage(this, new EventData<string>("Result", curSelectNumber));

    }
}