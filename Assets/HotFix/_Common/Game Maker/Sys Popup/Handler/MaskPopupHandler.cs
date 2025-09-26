using Game;
using System.Collections;
using System;

namespace GameMaker
{
    public class MaskPopupHandler : MonoSingleton<MaskPopupHandler>
    {

        CorController _corCtrl;

        PageName popupMask = PageName.PageSystemMask;

        const string COR_AUTO_CLOSE = "COR_AUTO_CLOSE";

        private void Awake()
        {
            _corCtrl = new CorController(this);
        }
        void DoCor(string name, IEnumerator routine) => _corCtrl.DoCor(name, routine);
        void ClearCor(string name) => _corCtrl.ClearCor(name);
        IEnumerator DoTask(Action cb, int ms = 0) => _corCtrl.DoTask(cb, ms);




        public void OpenPopup(int ms = 0)
        {
            if (ms > 0)
                DoCor(COR_AUTO_CLOSE, DoTask(() =>
                {
                    PageManager.Instance.ClosePage(popupMask);
                }, ms));

            if (PageManager.Instance.IndexOf(popupMask) == -1)
                PageManager.Instance.OpenPage(popupMask);
        }

        public void ClosePopup()
        {
            ClearCor(COR_AUTO_CLOSE);
            if (PageManager.Instance.IndexOf(popupMask) != -1)
            {
                PageManager.Instance.ClosePage(popupMask);
            }
        }
    }
}
