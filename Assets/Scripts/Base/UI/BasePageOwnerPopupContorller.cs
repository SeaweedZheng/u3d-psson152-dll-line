using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePageOwnerPopupContorller
{

    [System.Serializable]
    public class PopupInfo{
        public string name;
        public GameObject goPopup;
    }

    public List<PopupInfo> popupInfosLst= new List<PopupInfo>();

    List<GameObject> stackPopupLst = new List<GameObject>();


    public void OpenPopup(string popupName)
    {
        GameObject goPopup = null;
        for (int i = 0; i < popupInfosLst.Count; i++)
        {
            if (popupInfosLst[i].name == popupName)
            {
                goPopup = popupInfosLst[i].goPopup;
                break;
            }
        }
        if (goPopup==null)
            return;
        foreach (GameObject go in stackPopupLst)
        {
            goPopup.SetActive(false);
        }
        stackPopupLst.Insert(0, goPopup);
        goPopup.SetActive(true);
    }



    public void OpenPopup(GameObject goPopup)
    {
        foreach (GameObject go in stackPopupLst)
        {
            goPopup.SetActive(false);
        }
        stackPopupLst.Insert(0, goPopup);
        goPopup.SetActive(true);
    }

    public void ClosePopup(GameObject goPopup)
    {
        goPopup.SetActive(false);
        stackPopupLst.Remove(goPopup);

        if (stackPopupLst.Count > 0)
            stackPopupLst[0].SetActive(true);
    }
}
