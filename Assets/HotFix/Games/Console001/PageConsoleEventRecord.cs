using Game;
using TMPro;
using UnityEngine.UI;

namespace Console001
{
    public class PageConsoleEventRecord : PageMachineButtonBase
    {
        public Button btnClose;

        public TextMeshProUGUI tmpButtom;

        private void Awake()
        {
            btnClose.onClick.AddListener(OnClickClose);

            tmpButtom.text = string.Format(I18nMgr.T("Event Record, Page {0} of {1}"),1,1);
        }


        void OnClickClose()
        {
            PageManager.Instance.ClosePage(this);
        }
    }
}
