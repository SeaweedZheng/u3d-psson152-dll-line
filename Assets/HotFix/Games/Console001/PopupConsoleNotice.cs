using Game;
using GameMaker;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Console001
{
    public class PopupConsoleNotice : PageCorBase
    {


        public TextMeshProUGUI tmpTitle;


        public GameObject goContentAnchor, goNavigateAnchor;


        public Button btnCancel, btnNext;


        List<List<string>> pages = new List<List<string>>();


        int pageContentRow = 15;

        int curIndexPage = 0;


        void Awake()
        {
            btnCancel.onClick.AddListener(OnClickClose);
            btnNext.onClick.AddListener(OnClickNext);
        }

        void OnClickClose() {
            ClearCor(AUTO_CLOSE_MS);
            PageManager.Instance.ClosePage(this, new EventData("Exit"));
        } 

        void OnClickNext()
        {
            ClearCor(AUTO_CLOSE_MS);

            if (++curIndexPage >= pages.Count)
                curIndexPage = 0;

            SetCurPage(curIndexPage);
            ActiveNavigate(curIndexPage);
        }

        int autoCloseMs;

        const string AUTO_CLOSE_MS = "AUTO_CLOSE_MS";
        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            string title = I18nMgr.T("Game Update Announcement");
            List<string> content = new List<string>();
            autoCloseMs = -1;
            if (data != null && data.value != null)
            {
                Dictionary<string, object> args = data.value as Dictionary<string, object>;
                content = (List<string>) args["content"];
                title = (string) args["title"];

                if (args.ContainsKey("AutoCloseMS"))
                {
                    autoCloseMs = (int)args["AutoCloseMS"];
                }

            }

            tmpTitle.text = title;

            pages = SplitIntoPages(content, pageContentRow);

            initParam();

            AutoClose();
        }


        void AutoClose()
        {
            if (autoCloseMs > 0)
                DoCor(AUTO_CLOSE_MS, DoTask(() => {
                    OnClickClose();
                }, autoCloseMs));
        }

        void initParam()
        {
            curIndexPage = 0;
            SetCurPage(curIndexPage);
            initNavigate();
        }


        void SetCurPage(int index)
        {
            Transform tfmContentAnchor = goContentAnchor.transform;

            foreach (Transform chd in tfmContentAnchor)
            {
                chd.GetComponent<Text>().text = "";
            }

            if (index < pages.Count)
            {
                for(int i=0; i < pages[index].Count; i++)
                {
                    Transform tfmChd = tfmContentAnchor.GetChild(i);
                    //tfmChd.gameObject.SetActive(true);
                    tfmChd.GetComponent<Text>().text = pages[index][i];
                }
            }

        }

        void initNavigate()
        {
            Transform tfmNavigateAnchor = goNavigateAnchor.transform;
            GameObject goClone = tfmNavigateAnchor.GetChild(0).gameObject;

            for (int i = tfmNavigateAnchor.childCount; i < pages.Count; i++)
            {
                GameObject go = Instantiate(goClone);
                go.transform.parent = tfmNavigateAnchor;
            }

            foreach (Transform chd in tfmNavigateAnchor)
            {
                chd.gameObject.SetActive(false);
            }
            ActiveNavigate(curIndexPage);
        }
        void ActiveNavigate(int index)
        {
            Transform tfmNavigateAnchor = goNavigateAnchor.transform;
            for (int i = 0; i < pages.Count; i++)
            {
                Transform tfm = tfmNavigateAnchor.GetChild(i);
                tfm.gameObject.SetActive(true);
                tfm.Find("Hight").gameObject.SetActive(false);
            }
            tfmNavigateAnchor.GetChild(index).Find("Hight").gameObject.SetActive(true);
        }

        List<List<string>> SplitIntoPages(List<string> rows, int pageSize)
        {
            List<List<string>> pages = new List<List<string>>();

            for (int i = 0; i < rows.Count; i += pageSize)
            {
                int remainingCount = Math.Min(pageSize, rows.Count - i);
                List<string> page = rows.GetRange(i, remainingCount);
                pages.Add(page);
            }

            return pages;
        }



        public override void OnClickMachineButton(MachineButtonInfo info)
        {
            if (info != null)
             {
                switch (info.btnKey)
                {
                    case MachineButtonKey.BtnTicketOut:
                        ShowUIAminButtonClick(btnCancel, info);
                        break;
                    case MachineButtonKey.BtnSpin:
                        ShowUIAminButtonClick(btnNext, info);
                        break;
                }
             }
        }

    }
}
