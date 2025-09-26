using Game;
using GameMaker;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Console001
{
    public class PopupConsoleKeyboard002 : PageMachineButtonBase
    {
        public GameObject goBtnsRoot, goTitle, goContent;
        public GameObject goCancel, goOK;

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

            goCancel.transform.GetComponent<Button>().onClick.AddListener(() => OnClickKeyboard(goCancel.name));
            goOK.transform.GetComponent<Button>().onClick.AddListener(() => OnClickKeyboard(goOK.name));
        }


        /// <summary> 抬头 </summary>
        string title;
        /// <summary> 是否明文 </summary>
        bool isPlaintext;

        string input;

        public void InitParam()
        {
            title = "无标题";
            isPlaintext = true;
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
                if (argDic.ContainsKey("content"))
                {
                    input = (string)argDic["content"];
                }
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

       /* void OnClickKeyboard(string name)
        {
            if (index == 9 || index == 12)
            {
                PageManager.Instance.ClosePage(this, new EventData("Exit"));
            }
            else if (index == 11 || index == 13)
            {
                PageManager.Instance.ClosePage(this, new EventData<string>("Result", input));
            }
            else if (index == 13)
            {
                if (input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                }
            }
            else if (index == 10)
            {
                input = $"{input}0";
            }
            else
            {
                input = $"{input}{index + 1}";
            }

            tmpContetn.text = GetContent();
        }*/

        void OnClickKeyboard(string name)
        {
            string key = name.Replace("No.", "");
            switch (key)
            {
                case "OK":
                    PageManager.Instance.ClosePage(this, new EventData<string>("Result", input));
                    break;
                case "Cancel":
                    PageManager.Instance.ClosePage(this, new EventData("Exit"));
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

        }

    }
}
