using UnityEngine;


public class AndroidSystemHelper : MonoSingleton<AndroidSystemHelper>
{
    private AndroidJavaObject nativeObject;


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        nativeObject = new AndroidJavaObject("com.cryfx.game.libserialport.SystemHelper");
        if (nativeObject == null) return;
        nativeObject.Call("Init");

    }

    public void SetSystemTime(int year, int month, int day, int hour, int minute, int second)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (nativeObject == null)
            return;

        nativeObject.Call("SetSystemTime", year, month, day, hour, minute, second);
    }

    public void ScreenFlip()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (nativeObject == null)
            return;

        nativeObject.Call("ScreenFlip");
    }

}
