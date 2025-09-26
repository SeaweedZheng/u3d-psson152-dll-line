using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using _DeviceInfo = GameMaker.DeviceInfo;

namespace Console001
{

    public class PopupConsoleChooseDevice : PageMachineButtonBase
    {
        public TextMeshProUGUI tmpTitle,tmpSelectModel;

        public GameObject goContent;

        public Button btnClose;// btnSave;

        int curDeviceNumber;

        List<_DeviceInfo> numberDeviceName = new List<_DeviceInfo>();


       // public static readonly Color HIGHLIGHT_BG_COLOR = new Color(0f / 255f, 253f / 255f, 141f / 255f, 255f / 255f);
       // public static readonly Color NORMAL_BG_COLOR = new Color(9f / 255f, 34f / 255f, 31f / 255f, 255f / 255f);
        void Awake()
        {
            btnClose.onClick.AddListener(OnClickClose);
        }


        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            if (data.value != null)
            {
                Dictionary<string, object> args = data.value as Dictionary<string, object>;

                tmpTitle.text = (string)args["title"];
                numberDeviceName = (List<_DeviceInfo>)args["deviceLst"];
                curDeviceNumber = (int)args["deviceNumber"];
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
            for (int i = tfmContent.childCount; i < numberDeviceName.Count; i++)
            {
                Transform go = GameObject.Instantiate(clone);
                go.SetParent(tfmContent);
                go.localScale = Vector3.one;
            }

            foreach (Transform item in tfmContent)
            {
                item.gameObject.SetActive(false);
            }

            for (int i = 0; i < numberDeviceName.Count; i++)
            {
                height += itemHeight;
                _DeviceInfo data = numberDeviceName[i];
                Transform item = tfmContent.GetChild(i);
                item.gameObject.SetActive(true);
                item.Find("Text (1)").GetComponent<TextMeshProUGUI>().text = $"{data.manufacturer}";
                item.Find("Text (2)").GetComponent<TextMeshProUGUI>().text = $"{data.model}";
                item.GetComponent<Button>().onClick.RemoveAllListeners();
                item.GetComponent<Button>().onClick.AddListener(
                    () => OnChooseDevice(data.number)
                );

                if (data.number == curDeviceNumber)
                {
                    //item.GetComponent<Image>().color = HIGHLIGHT_BG_COLOR;
                    item.GetComponent<Image>().color = ThemeColorSettings.HIGHLIGHT_ITEM_BG;
                    tmpSelectModel.text = string.Format(I18nMgr.T("Selected Model: {0}"), $"{data.manufacturer} : {data.model}");
                }
                else
                {
                    //item.GetComponent<Image>().color = NORMAL_BG_COLOR;
                    item.GetComponent<Image>().color = ThemeColorSettings.NORMAL_ITEM_BG;
                }
            }


            goContent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(
                goContent.transform.GetComponent<RectTransform>().sizeDelta.x,
                height);
        }

        void OnClickClose() => PageManager.Instance.ClosePage(this, new EventData("Exit"));

        void OnClickSave() => PageManager.Instance.ClosePage(this, new EventData<int>("Result", curDeviceNumber));

        void OnChooseDevice(int number)
        {
            curDeviceNumber = number;
            //tmpSelectLanguage.text = string.Format(I18nMgr.T("Selected language: {0}"), numberLanguageName[curLanguageNumber]);
            PageManager.Instance.ClosePage(this, new EventData<int>("Result", curDeviceNumber));
        }
    }
}