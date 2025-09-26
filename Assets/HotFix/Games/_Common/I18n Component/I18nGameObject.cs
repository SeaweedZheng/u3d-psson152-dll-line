using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameMaker
{
    [System.Serializable]
    public class CustomI18nGameObject : UnitySerializedDictionary<I18nLang, GameObject> { }

    public class I18nGameObject : MonoBehaviour
    {

        public CustomI18nGameObject gos;

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


        private GameObject _goNow;

        public GameObject goNow
        {
            get => _goNow;
        }
        void OnI18nChange(I18nLang lang)
        {
            if (gos.ContainsKey(lang))
            {
                foreach (KeyValuePair<I18nLang, GameObject> kv in gos)
                {
                    if (lang == kv.Key)
                        continue;
                    kv.Value.SetActive(false);
                }
                _goNow = gos[lang];
                _goNow.SetActive(true);
            }
        }

    }
}