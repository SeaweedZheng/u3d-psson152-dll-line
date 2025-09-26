using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageData
{
    private static LanguageData instance;
    public static LanguageData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LanguageData();
            }
            return instance;
        }
    }
    public enum LanType
    {
        CN,
        EN
    }

    public LanType LanT = LanType.CN;
}
