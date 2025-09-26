using UnityEngine;

public interface IMaskPopupHandel
{
    void OpenPopup(string data = null);
    void ClosePopup();

    void SetContent(string data);
}


public class DefaultMaskPopup :  DefaultBasePopup, IMaskPopupHandel
{
    static DefaultMaskPopup _instance;

    public static DefaultMaskPopup Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DefaultMaskPopup();
            }
            return _instance;
        }
    }


    public new void OpenPopup(string data = null)
    {
        base.OnOpen("Common/Prefabs/Popup Default Mask");

    }

    public void SetContent(string data)
    {

    }
}
