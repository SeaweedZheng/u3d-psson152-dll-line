using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DeviceType
{
    None = 0,
    /// <summary> ��ӡ�� </summary>
    Printer,
    /// <summary> ��Ʊ��Ʊ�� </summary>
    BillValidator,
    ///  <summary> Ͷ�һ� </summary>
    CoinIn,
    ///  <summary> ��Ʊ�� </summary>
    Ticket,
    ///  <summary> ��Ʊ�� </summary>
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

