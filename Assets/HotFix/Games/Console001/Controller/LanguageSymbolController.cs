using UnityEngine.UI;
using UnityEngine;
using GameMaker;
using System;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

[System.Serializable]
public class CustomDicLanguageSymbols : UnitySerializedDictionary<I18nLang, Sprite> { }
public class LanguageSymbolController : MonoBehaviour
{

    public CustomDicLanguageSymbols languageSymbols;

    void Awake()
    {
        string name = _consoleBB.Instance.language;
        //BlackboardUtils.GetValue<string>("@console/language");
        OnChangeLanguage((I18nLang)Enum.Parse(typeof(I18nLang), name));
        EventCenter.Instance.AddEventListener<I18nLang>(I18nManager.I18N, OnChangeLanguage);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<I18nLang>(I18nManager.I18N, OnChangeLanguage);
    }

    // Update is called once per frame
    void OnChangeLanguage(I18nLang lang)
    {
        transform.GetComponent<Image>().sprite = languageSymbols[lang];
    }
}
