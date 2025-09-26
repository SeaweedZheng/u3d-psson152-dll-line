using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DeviceType
{
    None = 0,
    /// <summary> 打印机 </summary>
    Printer,
    /// <summary> 钞票进票机 </summary>
    BillValidator,
    ///  <summary> 投币机 </summary>
    CoinIn,
    ///  <summary> 退票机 </summary>
    Ticket,
    ///  <summary> 退票机 </summary>
    Wire
}


[System.Serializable]
public class DeviceInfo
{
    public DeviceType type;
    public string name;
    public string model;
    public string manufacturer;
    public int number;
    public bool isInOut;
    public string des;
}

