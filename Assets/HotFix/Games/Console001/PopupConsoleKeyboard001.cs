using Game;
using GameMaker;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Console001
{
    public class PopupConsoleKeyboard001 : PageMachineButtonBase
    {
        public GameObject goBtnsRoot, goTitle, goContent,goClose;


        TextMeshProUGUI tmpTitle, tmpContetn;
        void Awake()
        {
            tmpTitle = goTitle.transform.GetComponent<TextMeshProUGUI>();
            tmpContetn = goContent.transform.GetComponent<TextMeshProUGUI>();

            int i = 0;
            foreach (Transform tfm in goBtnsRoot.transform)
            {
                int idx = i;
                tfm.GetComponent<Button>().onClick.AddListener(() => OnClickKeyboard(tfm.name));
                i++;
            }
            goClose.GetComponent<Button>().onClick.AddListener(() => OnClickKeyboard("No.Cancel"));
        }


        /// <summary> 抬头 </summary>
        string title;
        /// <summary> 是否明文 </summary>
        bool isPlaintext;
        /// <summary> 内容长度 </summary>
        //int contentLength;

        string input;

        public void InitParam()
        {
            title = "无标题";
            isPlaintext = true;
            //contentLength = 0;
            input = "";
        }

        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            InitParam();

            Dictionary<string, object> argDic = null;
            if (data != null)
            {
                argDic = (Dictionary<string, object>)data.value;
                title = (string)argDic["title"];
                isPlaintext = (bool)argDic["isPlaintext"];

                //contentLength = (int)argDic["contentLength"];

                if (argDic.ContainsKey("content"))
                {
                    input = (string)argDic["content"];
                }

                /*if (argDic.ContainsKey("isBtnClose") && false == (bool)argDic["isBtnClose"] )
                {
                    goClose.GetComponent<Button>().interactable = false;
                }
                else
                {
                    goClose.GetComponent<Button>().interactable = true;
                }*/
            }

            tmpTitle.text = title;
            tmpContetn.text = GetContent();
        }

        string GetContent()
        {
            if (isPlaintext)
            {
                return input;
            }
            else
            {
                string res = "";
                for (int i = 0; i < input.Length; i++)
                {
                    res += "*";
                }
                return res;
            }
        }

        void OnClickKeyboard(string name)
        {
            string key = name.Replace("No.","");
            switch (key)
            {
                case "Cancel":
                    PageManager.Instance.ClosePage(this, new EventData("Exit"));
                    break;
                case "OK":
                    PageManager.Instance.ClosePage(this, new EventData<string>("Result", input));
                    break;
                case "Delete":
                    if (input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                    }
                    break;
                default:
                    input = $"{input}{key}";
                    break;
            }

            tmpContetn.text = GetContent();

            /*
            if (input.Length >= contentLength)
            {
                PageManager.Instance.ClosePage(this, new EventData<string>("Result",input));
            }*/
        }

    }
}
