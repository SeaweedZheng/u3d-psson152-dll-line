using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompKeyboardDefaultController : MonoBehaviour
{

    static public CompInputDefaultController curInput;

    public GameObject goKB123, goKBabc, goKBABC, goKBOperator;

    public List<Button> keyboardButtons;
    private void Awake()
    {

        keyboardButtons = new List<Button>();

        //此脚本通过GetComponentsInChildren<Button>(true)方法查找所有后代中的 Button 组件，包括未激活的 GameObject。如果只需要查找激活状态的 Button，可以将参数改为false。
        Button[] buttons = transform.GetComponentsInChildren<Button>(true);
        keyboardButtons.AddRange(buttons);

        foreach (Button btn in keyboardButtons)
        {
            if (btn.name.StartsWith("No."))
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => { OnClickKeyboard(btn.name); });
            }
        }
    }



    public void SetFocus(CompInputDefaultController input)
    {
        if (curInput != null && curInput != input)
        {
            curInput.LostFocus();
        }
        curInput = input;
    }



    void OnClickKeyboard(string name)
    {
        string key = name.Replace("No.", "");
        switch (key)
        {
            case "OK":
                {
                    curInput.LostFocus();
                    curInput = null;
                }
                break;
            case "Clear":
                if (curInput != null)
                    curInput.Clear();
                break;
            case "Exit":
                break;
            case "ABC":
                {
                    goKB123.SetActive(false);
                    goKBabc.SetActive(false);
                    goKBABC.SetActive(true);
                    goKBOperator.SetActive(false);
                }
                break;
            case "abc":
                {
                    goKB123.SetActive(false);
                    goKBabc.SetActive(true);
                    goKBABC.SetActive(false);
                    goKBOperator.SetActive(false);
                }
                break;
            case "123":
                {
                    goKB123.SetActive(true);
                    goKBabc.SetActive(false);
                    goKBABC.SetActive(false);
                    goKBOperator.SetActive(false);
                }
                break;
            case "#+=":
                {
                    goKB123.SetActive(false);
                    goKBabc.SetActive(false);
                    goKBABC.SetActive(false);
                    goKBOperator.SetActive(true);
                }
                break;
            case "Up":
                {
                    if (goKBabc.active)
                    {
                        goKBabc.SetActive(false);
                        goKBABC.SetActive(true);
                    }
                    else
                    {
                        goKBabc.SetActive(true);
                        goKBABC.SetActive(false);
                    }
                }
                break;
            case "Space":
                {
                    if (curInput != null)
                        curInput.Add(" ");
                }
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