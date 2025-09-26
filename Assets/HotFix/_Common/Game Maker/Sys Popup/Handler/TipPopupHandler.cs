using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITipPopup
{
    void ShowTip(string msg);

    bool Contains(string msg);
}
public class TipPopupHandler : MonoSingleton<TipPopupHandler>
{

    //public List<TipPopupInfo> errorStack = new List<TipPopupInfo>();


    PageBase poup = null;

    ITipPopup iPopup => poup as ITipPopup;

    public PageName popupName => PageName.PopupSystemTip;
    public void OpenPopup(string msg)
    {

        if (PageManager.Instance.IndexOf(popupName) == -1)
        {
            poup = PageManager.Instance.OpenPage(popupName, new EventData<string>("Tip", msg));
        }
        else
        {
            iPopup.ShowTip(msg);
        }
    }

    public void OpenPopupOnce(string msg)
    {
        if(poup != null && iPopup.Contains(msg))
            return;
        OpenPopup(msg);
    }

    public void ClosePopup()
    {
        if (PageManager.Instance.IndexOf(popupName) == -1)
        {
            PageManager.Instance.ClosePage(popupName);
            poup = null;
        }
    }

}
