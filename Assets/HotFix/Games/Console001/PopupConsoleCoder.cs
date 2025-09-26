using Game;
using GameMaker;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Console001
{
    public class PopupConsoleCoder : PageMachineButtonBase
    {
        public GameObject goClose, goBtnsRoot, goTitle, goContent;

        TextMeshProUGUI tmpTitle, tmpInput;
        void Awake()
        {
            tmpTitle = goTitle.transform.GetComponent<TextMeshProUGUI>();
            tmpInput = goContent.transform.Find("Input").transform.GetComponent<TextMeshProUGUI>();

            int i = 0;
            foreach (Transform tfm in goBtnsRoot.transform)
            {
                int idx = i;
                tfm.GetComponent<Button>().onClick.AddListener(() => OnClickKeyboard(tfm.name));
                i++;
            }
            goClose.GetComponent<Button>().onClick.AddListener(() => OnClickKeyboard("No.Cancel"));
        }


        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            InitParam();

            Dictionary<string, object> argDic = null;
            if (data != null)
            {
                argDic = (Dictionary<string, object>)data.value;
                //title = (string)argDic["title"];
                //isPlaintext = (bool)argDic["isPlaintext"];
                //contentLength = (int)argDic["contentLength"];
                if (argDic.ContainsKey("content"))
                {
                    input = (string)argDic["content"];
                }

                Transform tfmContent = goContent.transform;
                tfmContent.Find("A").GetComponent<TextMeshProUGUI>().text = "A: " + (string)argDic["A"];
                tfmContent.Find("B").GetComponent<TextMeshProUGUI>().text = "B: " + (string)argDic["B"];
                tfmContent.Find("C").GetComponent<TextMeshProUGUI>().text = "C: " + (string)argDic["C"];
                tfmContent.Find("D").GetComponent<TextMeshProUGUI>().text = "D: " + (string)argDic["D"];
                tfmContent.Find("E").GetComponent<TextMeshProUGUI>().text = "E: " + (string)argDic["E"];
                //tfmContent.Find("Time").GetComponent<TextMeshProUGUI>().text =$"remain: {argDic["Day"]} days; {argDic["Hour"]} hours; {argDic["Minute"]} minute;";
                tfmContent.Find("Time").GetComponent<TextMeshProUGUI>().text = 
                    string.Format(I18nMgr.T("remaining time: {0} days; {1} hours; {2} minute;"), argDic["Day"], argDic["Hour"], argDic["Minute"]);
                
                                   
            }

            //tmpInput.text = GetContent();
            tmpInput.text = input;
        }
        public void InitParam()
        {
            //title = "无标题";
            isPlaintext = false;
            //contentLength = 0;
            input = "";
        }


        string input;

        /// <summary> 是否明文 </summary>
        bool isPlaintext;

        void OnClickKeyboard(string name)
        {
            string key = name.Replace("No.", "");
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
                    {
                        if (input.Length == 12)
                        {
                            TipPopupHandler.Instance.OpenPopup(I18nMgr.T("Code cannot exceed 12 characters"));
                        }
                        else
                        {
                            input = $"{input}{key}";
                        }

                    }

                    break;
            }

            //tmpInput.text = GetContent();
            tmpInput.text = input;
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


    }
}
