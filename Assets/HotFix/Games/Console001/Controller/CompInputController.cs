using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameMaker;
using TMPro;
public class CompInputController : CorBehaviour
{
    public TextMeshProUGUI tmpInput;
    public Button btn;


    public bool isPlaintext = false;
    void Awake()
    {
           btn.onClick.AddListener(OnClick);     
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<GameMaker.EventData>(GlobalEvent.ON_UI_INPUT_EVENT, OnEventCustomInputClick);
        InitParam();
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<GameMaker.EventData>(GlobalEvent.ON_UI_INPUT_EVENT, OnEventCustomInputClick);
        ClearAllCor();
    }
    void OnClick()
    {
       EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_UI_INPUT_EVENT,
             new EventData<object>(GlobalEvent.CustomInputClick,
             transform.GetComponent<CompInputController>())
             ); //new EventData<CompInputController>

        /*EventCenter.Instance.EventTrigger(GlobalEvent.ON_UI_EVENT,
            new GameMaker.EventData<string>(GlobalEvent.CustomInputClick, "i am test")); //这个会报错*/


         /* EventCenter.Instance.EventTrigger<GameMaker.EventData>(GlobalEvent.ON_UI_EVENT,
            new GameMaker.EventData<string>(GlobalEvent.CustomInputClick, "i am test"));*/
    }

    const string COR_START_INPUT = "COR_START_INPUT";

    public void OnEventCustomInputClick(EventData data)
    {
        //DebugUtil.Log(data.value);
        //return;
        if (data.name != GlobalEvent.CustomInputClick)
            return;
        if (data.value != null)
        {
            if (data.value as CompInputController == this){
                DoCor(COR_START_INPUT, ShowTip());
                return;            
            }
        }

        ClearCor(COR_START_INPUT);
        isShowTip = false;
        tmpInput.text = GetContent();
    }


    string GetContent()
    {
        if (isPlaintext)
        {
            return isShowTip ? $"{inputStr}|" : $"{inputStr} ";
        }
        else
        {
            string res = "";
            for (int i = 0; i < inputStr.Length; i++)
            {
                res += "*";
            }
            return isShowTip ? $"{res}|" : $"{res} ";
        }
    }

    public void SetPlaintext(bool isPlt)
    {
        isPlaintext = isPlt;
        tmpInput.text = GetContent();
    }
    public void InitParam()
    {
        inputStr = "";
        isShowTip = false;
        tmpInput.text = GetContent();
    }

    string inputStr = "";

    public string value
    {
        get => inputStr;
        set => inputStr = value;
    }

    bool isShowTip = false;
    IEnumerator ShowTip()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            isShowTip = !isShowTip;

            tmpInput.text = GetContent();
        }
    }


    public void Delete()
    {
        if (inputStr.Length > 0)
        {
            inputStr = inputStr.Substring(0, inputStr.Length - 1);
        }
        tmpInput.text = GetContent();
    }
    public void Clear()
    {
        inputStr = "";
        tmpInput.text = GetContent();
    }

    public void Add(string data)
    {
        inputStr += data;
        tmpInput.text = GetContent();
    }


}
