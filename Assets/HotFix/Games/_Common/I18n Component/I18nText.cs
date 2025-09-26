using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace GameMaker
{

    public class I18nText : MonoBehaviour
    {
        //public string id;
        public string key;
        void Awake()
        {
            EventCenter.Instance.AddEventListener<I18nLang>(I18nManager.I18N, OnI18nChange);
        }
        private void OnDestroy()
        {
            EventCenter.Instance.RemoveEventListener<I18nLang>(I18nManager.I18N, OnI18nChange);
        }

        private void OnEnable()
        {
            OnI18nChange(I18nMgr.language);
        }

        void OnI18nChange(I18nLang lang)
        {
            if (string.IsNullOrEmpty(key))
                return;

       
            SetText(I18nMgr.T(key));
        }


        void SetText(string txt)
        {
            Text compText = transform.GetComponent<Text>();
            if (compText != null)
                compText.text = txt;

            TextMeshProUGUI compTmp = transform.GetComponent<TextMeshProUGUI>();
            if (compTmp != null)
                compTmp.text = txt;
        }

    }
}