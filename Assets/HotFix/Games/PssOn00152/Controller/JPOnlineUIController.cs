#define TEST_SELF
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using GameMaker;


using _consoleBB = PssOn00152.ConsoleBlackboard02;

public class JPOnlineUIController : MonoBehaviour
{
    public Image image;
   
    MessageDelegates onPropertyChangedEventDelegates;
    void Awake()
    {
        /*if (Application.isEditor)
            image.material = new Material(Shader.Find("UI/Unlit/ImageGray"));*/

        onPropertyChangedEventDelegates = new MessageDelegates
         (
             new Dictionary<string, EventDelegate>
             {
                    { "@console/isJackpotOnLine", OnPropertyChangeIsJackpotOnline},
             }
         );

        OnPropertyChangeIsJackpotOnline(null);
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<bool>(EventHandle.NETWORK_STATUS, OnEventNetworkStatus);
        EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);

        OnEventNetworkStatus(ClientWS.Instance.IsConnected);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<bool>(EventHandle.NETWORK_STATUS, OnEventNetworkStatus);
        EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);
    }


    void OnPropertyChangeIsJackpotOnline(EventData res)
    {
        bool isJackpotHall = false;
        if (res == null)
            isJackpotHall = _consoleBB.Instance.isJackpotOnLine;
        else
            isJackpotHall = ((int)res.value) == 1;

        image.gameObject?.SetActive(isJackpotHall);
    }


    void OnEventNetworkStatus(bool isConnect)
    {
        if (!isConnect)
            image.material.SetFloat("_SetGray", 1);
        else
            image.material.SetFloat("_SetGray", 0);
    }
    void Update()
    {
#if TEST_SELF
        if(Input.GetKeyUp(KeyCode.F1))
            image.material.SetFloat("_SetGray" ,1);

        if (Input.GetKeyUp(KeyCode.F2))
            image.material.SetFloat("_SetGray", 0);
#endif
    }
}
