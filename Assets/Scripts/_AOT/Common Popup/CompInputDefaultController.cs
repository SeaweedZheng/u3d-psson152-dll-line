using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompInputDefaultController : MonoBehaviour
{
    public Text txtInput;
    public Button btn;


    public bool isPlaintext = true;
    void Awake()
    {
        btn.onClick.AddListener(OnClick);
    }

    private void OnEnable()
    {
        InitParam();
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnClick()
    {
        GetFocus();
    }

    Coroutine corInput;


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
        txtInput.text = GetContent();
    }
    public void InitParam()
    {
        inputStr = "";
        isShowTip = false;
        txtInput.text = GetContent();
    }

    string inputStr = "";

    public string value
    {
        get => inputStr;
        set
        {
            inputStr = value;
            txtInput.text = GetContent();
        }
    }

    bool isShowTip = false;
    IEnumerator ShowTip()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            isShowTip = !isShowTip;

            txtInput.text = GetContent();
        }
    }


    public void Delete()
    {
        if (inputStr.Length > 0)
        {
            inputStr = inputStr.Substring(0, inputStr.Length - 1);
        }
        txtInput.text = GetContent();
    }
    public void Clear()
    {
        inputStr = "";
        txtInput.text = GetContent();
    }

    public void Add(string data)
    {
        inputStr += data;
        txtInput.text = GetContent();
    }

    public void LostFocus()
    {
        if (corInput != null)
            StopCoroutine(corInput);
        isShowTip = false;
        txtInput.text = GetContent();
    }
    public void GetFocus()
    {
        if (corInput != null)
            StopCoroutine(corInput);
        corInput = StartCoroutine(ShowTip());

        CompKeyboardDefaultController[] compKB = GameObject.FindObjectsOfType<CompKeyboardDefaultController>();
        foreach (CompKeyboardDefaultController comp in compKB)
        {
            if (comp.gameObject.active == true)
            {
                comp.SetFocus(this);
                return;
            }
        }
    }
}