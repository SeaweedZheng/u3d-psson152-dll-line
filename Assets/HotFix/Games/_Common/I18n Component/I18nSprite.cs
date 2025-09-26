using UnityEngine;
using UnityEngine.UI;

namespace GameMaker
{
    [System.Serializable]
    public class CustomI18nSprite : UnitySerializedDictionary<I18nLang, Sprite> { }



    public class I18nSprite : MonoBehaviour
    {
        public bool isUseNativeSize = false;

        public CustomI18nSprite sprites;

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
            if (sprites.ContainsKey(lang))
                SetSprite(sprites[lang]);
        }


        void SetSprite(Sprite spr)
        {
            Image img = transform.GetComponent<Image>();
            if (img != null)
                img.sprite = spr;

            if (isUseNativeSize)
                img.SetNativeSize();
        }


    }
}