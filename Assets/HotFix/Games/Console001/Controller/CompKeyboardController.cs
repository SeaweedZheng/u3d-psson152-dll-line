using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompKeyboardController : MonoBehaviour
{
    CompInputController curInput;


    public List<GameObject> keyboardRoots;

    private void Awake()
    {
        if (keyboardRoots != null)
        {
            foreach (GameObject root in keyboardRoots)
            {
                foreach (Transform itn in root.transform)
                {
                    itn.GetComponent<Button>().onClick.RemoveAllListeners();
                    itn.GetComponent<Button>().onClick.AddListener(() => { OnClickKeyboard(itn.name); });
                }
            }
        }
    }

    private void OnEnable()
    {

        EventCenter.Instance.AddEventListener<EventData>(GlobalEvent.ON_UI_INPUT_EVENT, OnEventCustomInputClick);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(GlobalEvent.ON_UI_INPUT_EVENT, OnEventCustomInputClick);

        EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_UI_INPUT_EVENT, new EventData<CompInputController>(GlobalEvent.CustomInputClick,null));
    }

    void OnEventCustomInputClick(EventData data)
    {
        if (data.name != GlobalEvent.CustomInputClick)
            return;

        //DebugUtil.Log("i am here !!!!!!!!!!!!!!!!!!!!");
        if (data.value != null)
        {
            curInput = data.value as CompInputController;
            return;
        }

        curInput = null;
       /**/
    }


    void OnClickKeyboard(string name)
    {
        string key = name.Replace("No.", "");
        switch (key)
        {
            case "OK":
                EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_UI_INPUT_EVENT, new EventData<CompInputController>(GlobalEvent.CustomInputClick, null));
                break;
            case "Clear":
                if (curInput != null)
                    curInput.Clear();
                break;
            case "Delete":
                if (curInput != null)
                    curInput.Delete();
                break;
            default:
                if (curInput != null)
                    curInput.Add(key);
                break;
        }
    }


}
