using TMPro;
using UnityEngine;

namespace GameMaker
{
    public class PIDTextMeshProUGUI : TextMeshProUGUI
    {
        [Tooltip("������ĳ��ȣ��޸ĵ�ǰ�ڵ�Ŀ��")]
        public bool isChangeWidth = false;

        protected override void Start()
        {
            base.Start();
            ChangeWidth();
        }

        void ChangeWidth()
        {
            if (!isChangeWidth || enableAutoSizing)
                return;

            RectTransform rect = transform.GetComponent<RectTransform>();
            Vector2 v2 = rect.rect.size;

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, base.preferredWidth);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, v2.y);
        }

        public override string text
        {
            get => base.text;
            set
            {
                base.text = value;
                ChangeWidth();
            }
        }
    }
}
