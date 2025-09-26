using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JsonEditPopupInfo
{
    public string title;
    public string data;
    public bool isFormat = false;
    public Action<string> onFinishCallback;
}
public class DefaultJsonEditPopup:DefaultBasePopup
{
    static DefaultJsonEditPopup _instance;

    public static DefaultJsonEditPopup Instance
    {
        get
        {
            if (_instance == null)
            {
                if (_instance == null)
                {
                    _instance = new DefaultJsonEditPopup();
                }
            }
            return _instance;
        }
    }


    JsonEditPopupInfo info;

    Button btnClose, btnCancel, btnSave, btnModify, btnEditSoftKeyboard;

    Text txtEditKeyName, txtEditKey;

    Transform tfmJsonContent;

    InputField inpEditVlue;




    public void OpenPopup(JsonEditPopupInfo info)
    {
        this.info = info;
        
        base.OnOpen("Common/Prefabs/Popup Json Edit");



        tfmJsonContent = goPopup.transform.Find("Popup/Anchor/Nodes/Viewport/Content");

        txtEditKeyName = goPopup.transform.Find("Popup/Anchor/Editor/Top (1)/KEY").GetComponent<Text>();
        txtEditKey = goPopup.transform.Find("Popup/Anchor/Editor/Top (1)/KEY (1)").GetComponent<Text>();
        inpEditVlue = goPopup.transform.Find("Popup/Anchor/Editor/Top (2)/KEY (2)/InputField").GetComponent<InputField>();


        btnEditSoftKeyboard = goPopup.transform.Find("Popup/Anchor/Editor/Top (2)/KEY (2)/Button Soft Keyboard").GetComponent<Button>();
        btnEditSoftKeyboard.onClick.RemoveAllListeners();
        
        btnModify = goPopup.transform.Find("Popup/Anchor/Editor/Top (2)/KEY (2)/Button Modify").GetComponent<Button>();
        btnModify.onClick.RemoveAllListeners();

        btnClose = goPopup.transform.Find("Popup/Button Close").GetComponent<Button>();
        btnClose.onClick.RemoveAllListeners();
        btnClose.onClick.AddListener(() =>
        {
            ClosePopup();
            this.info.onFinishCallback?.Invoke(null);
        });


        btnCancel = goPopup.transform.Find("Popup/Anchor/Buttom/Button Cancle").GetComponent<Button>();
        btnCancel.onClick.RemoveAllListeners();
        btnCancel.onClick.AddListener(() =>
        {
            ClosePopup();
            this.info.onFinishCallback?.Invoke(null);
        });


        btnSave = goPopup.transform.Find("Popup/Anchor/Buttom/Button Save").GetComponent<Button>();
        btnSave.onClick.RemoveAllListeners();
        btnSave.onClick.AddListener(() =>
        {
            string result = string.Join("", lines);
            JObject obj = JObject.Parse(result);
            //this.info.onFinishCallback?.Invoke(JsonConvert.SerializeObject(obj)); //没有格式化
            //this.info.onFinishCallback?.Invoke(obj.ToString()); //有格式化
            string res = info.isFormat ? obj.ToString() : JsonConvert.SerializeObject(obj);
            this.info.onFinishCallback?.Invoke(res);
            //Debug.Log(res);
        });


        OpenJsonPop(info.data);
    }


    string[] lines;
    int curLineIndex;
    string curValue;

    Action<string> onJaonEditorSaveCallback;



    public void OpenJsonPop(string jsonStr = null)
    {
        curLineIndex = 0;
        curValue = "";


        string json1 = @"
                {
                    ""msgtype"": ""resp"",
                    ""method"": ""Report"",
                    ""seq_id"": 10,
                    ""body"": [
                        {
                            ""code"": 0,
                            ""msg"": """",
                            ""game_state"": ""Idle"",
                            ""type"": ""SqlSelect"",
                            ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                            ""is_close"": false,
                            ""lst"": [0, 1, 2, 3]
                        },
                        {
                            ""code"": 1,
                            ""msg"": """",
                            ""game_state"": ""Idle"",
                            ""type"": ""SqlSelect"",
                            ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                            ""is_close"": false,
                            ""lst"": [0, 1, 2, 3]
                        },
                        {
                            ""code"": 2,
                            ""msg"": """",
                            ""game_state"": ""Idle"",
                            ""type"": ""SqlSelect"",
                            ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                            ""is_close"": false,
                            ""lst"": [0, 1, 2, 3]
                        },
                        {
                            ""code"": 3,
                            ""msg"": """",
                            ""game_state"": ""Idle"",
                            ""type"": ""SqlSelect"",
                            ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                            ""is_close"": false,
                            ""lst"": [0, 1, 2, 3]
                        },
                        {
                            ""code"": 4,
                            ""msg"": """",
                            ""game_state"": ""Idle"",
                            ""type"": ""SqlSelect"",
                            ""data"": ""输入命令非SELECT,  ==== receive sql: 214313\r\n"",
                            ""is_close"": false,
                            ""lst"": [0, 1, 2, 3]
                        }
                    ]
                }
            ";



        // 反序列化为动态对象
        JObject obj = JObject.Parse(!string.IsNullOrEmpty(jsonStr) ? jsonStr : json1);

        string result = obj.ToString();
        // Debug.Log(result);


        // 按换行符拆分（处理 \n 和 \r\n）
        lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);


        GameObject goClone = tfmJsonContent.GetChild(0).gameObject;


        float itemheight = tfmJsonContent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        //float height = gmNode.Count * itemheight;
        RectTransform rtfmContent = tfmJsonContent.GetComponent<RectTransform>();
        rtfmContent.sizeDelta = new Vector2(rtfmContent.sizeDelta.x, lines.Length * itemheight);


        for (int i = tfmJsonContent.childCount; i < lines.Length; i++)
        {
            Transform tfmNew = GameObject.Instantiate(goClone).transform;
            tfmNew.SetParent(tfmJsonContent);

            tfmNew.localPosition = Vector3.zero;
            tfmNew.localScale = Vector3.one;
            tfmNew.localRotation = Quaternion.identity;
        }

        foreach (Transform tfm in tfmJsonContent)
        {
            tfm.gameObject.SetActive(false);
        }

        for (int i = 0; i < lines.Length; i++)
        {
            Transform tfm = tfmJsonContent.GetChild(i);
            tfm.gameObject.SetActive(true);
            tfm.Find("Text").GetComponent<Text>().text = lines[i];
            Button btnComp = tfm.GetComponent<Button>();
            btnComp.onClick.RemoveAllListeners();

            int index = i;
            btnComp.onClick.AddListener(() =>
            {
                OnLineClick(index);
            });
        }

        foreach (Transform tfmChd in tfmJsonContent)
        {
            tfmChd.Find("BG Highlight").gameObject.SetActive(false);
            tfmChd.Find("Text").GetComponent<Text>().color = Color.white;
        }

        /* 遍历每一行
        foreach (string line in lines)
        {
            Debug.Log("## " + line);
        }*/
    }




    void OnLineClick(int index)
    {
        foreach (Transform tfmChd in tfmJsonContent)
        {
            tfmChd.Find("BG Highlight").gameObject.SetActive(false);
            tfmChd.Find("Text").GetComponent<Text>().color = Color.white;
        }
        tfmJsonContent.GetChild(index).Find("BG Highlight").gameObject.SetActive(true);
        tfmJsonContent.GetChild(index).Find("Text").GetComponent<Text>().color = Color.black;

        btnEditSoftKeyboard.onClick.RemoveAllListeners();
        btnModify.onClick.RemoveAllListeners();

        string targetStr = lines[index];
        if (targetStr.EndsWith("[") || targetStr.EndsWith("]") || targetStr.EndsWith("{") || targetStr.EndsWith("}"))
            return;


        curLineIndex = index;

        if (targetStr.EndsWith(","))
            targetStr = targetStr.TrimEnd(','); // 去除末尾的所有逗号

        string[] strs = targetStr.Split(':');

        if (strs.Length > 1) // 键值对
        {
            txtEditKeyName.text = "KEY:";
            txtEditKey.text = strs[0].Trim();
            curValue = strs[1].Trim();
            inpEditVlue.text = curValue;
            // strs
        }
        else  // 数组
        {
            int idx = 0;
            for (int j = index - 1; j >= 0; j--)
            {
                if (lines[j].EndsWith("["))
                {
                    break;
                }
                else
                {
                    idx++;
                }
            }

            txtEditKeyName.text = "INDEX:";
            txtEditKey.text = $"{idx}";
            curValue = strs[0].Trim();
            inpEditVlue.text = curValue;
        }




        btnModify.onClick.AddListener(() =>
        {
            OnEditorValue(inpEditVlue.text);
        });



        btnEditSoftKeyboard.onClick.AddListener(() =>
        {
            SetParameterPopupInfo inf = new SetParameterPopupInfo()
            {
                title = "设置节点数值",
                paramLst = new List<ParamInfo>()
                {
                    new ParamInfo()
                    {
                        name = $"{txtEditKeyName.text}{txtEditKey.text}:",
                        value = curValue,
                    },

                    /*new ParamInfo()
                    {
                        name = $"{txtEditKeyName.text}{txtEditKey.text}:",
                        value = curValue,
                        tipFunc = (res) =>
                        {
                            if(res.Length < 8)
                                return "输入值长度必须大于8";
                            return null;
                        }
                    },
                     new ParamInfo()
                    {
                        name = $"test2:",
                        value = "10",
                        tipFunc = (res) =>
                        {
                            try
                            {
                                int temp = int.Parse(res);
                            }catch (Exception ex) {
                                return "输入值必须是数字";
                            }
                            return null;
                        }
                    }*/
                },
                onFinishCallback = (string res) =>
                {
                    if (res != null)
                    {
                        inpEditVlue.text = res;
                    }
                }
            };
            DefaultSetParameterPopup.Instance.OpenPopup(inf);
            // SetParameterPopupHandler.Instance.OpenPopup(inf);
        });
        Debug.Log("## " + lines[index]);

    }


    void OnEditorValue(string inpValue)
    {
        //string oldStr = lines[curLineIndex];
        lines[curLineIndex] = lines[curLineIndex].Replace(curValue, inpValue);
        curValue = inpValue;
        //if (oldStr != lines[curLineIndex]) curValue = inpValue;

        Transform tfm = tfmJsonContent.GetChild(curLineIndex);
        tfm.Find("Text").GetComponent<Text>().text = lines[curLineIndex];
    }
}