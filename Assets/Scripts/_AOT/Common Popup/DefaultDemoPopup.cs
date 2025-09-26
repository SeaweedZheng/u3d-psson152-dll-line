using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultPopup
{
    public class DefaultDemoPopup : DefaultBasePopup
    {
        static DefaultDemoPopup _instance;

        public static DefaultDemoPopup Instance
        {
            get
            {
                if (_instance == null)
                {
                     _instance = new DefaultDemoPopup();
                }
                return _instance;
            }
        }

        JsonEditPopupInfo info;

        Button btnClose, btnConfirm;

        public void OpenPopup(JsonEditPopupInfo info)
        {
            this.info = info;

            base.OnOpen("Common/Prefabs/Popup Json Editor");
            /*
            if (goPopup == null)
            {
                GameObject root = GameObject.Find("Canvas Overlay");
                GameObject goClone = Resources.Load<GameObject>("Common/Prefabs/Popup Json Editor");
                goPopup = GameObject.Instantiate(goClone);
                goPopup.transform.SetParent(root.transform, false);

                goPopup.transform.localPosition = Vector3.zero;
                goPopup.transform.localScale = Vector3.one;

                RectTransform rectTransform = goPopup.GetComponent<RectTransform>();
                rectTransform.anchorMin = Vector2.zero;       // 左下锚点 (0, 0)
                rectTransform.anchorMax = Vector2.one;        // 右上锚点 (1, 1)
                                                              // 设置 Offsets 为 0（与边缘对齐）
                rectTransform.offsetMin = Vector2.zero;       // left = 0, bottom = 0
                rectTransform.offsetMax = Vector2.zero;       // right = 0, top = 0

                InitParam();
            }
            goPopup.transform.SetSiblingIndex(goPopup.transform.parent.childCount - 1);
            goPopup.SetActive(true);
            */
        }

        protected override void InitParam()
        {
            btnClose = goPopup.transform.Find("Popup/Button Close").GetComponent<Button>();
            btnClose.onClick.AddListener(() =>
            {
                ClosePopup();

            });

            btnConfirm = goPopup.transform.Find("Popup/Button Confirm").GetComponent<Button>();
            btnConfirm.onClick.AddListener(() =>
            {
                ClosePopup();

            });
        }

    }
}