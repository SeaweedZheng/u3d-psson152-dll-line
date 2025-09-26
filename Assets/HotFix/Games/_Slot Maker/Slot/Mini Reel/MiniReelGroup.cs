using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;


public class MiniReelGroup : MonoBehaviour
{

    public float nowData = 0f;

    public string format = "N2";

    //private List<char> dataStr = new List<char>();

    public Transform tfmReels;

    public GameObject reelToClone;


    /// <summary> 
    /// �����������ֵ
    /// </summary>
    /// <remarks>
    /// ���������ֵ������ֵ����ֱ������ΪĿ��ֵ
    /// </remarks>
    public float maxAddDifNumber = 20f;

    void Start()
    {
       // _SetData(nowData);
    }

    void SetNumb(List<char> dataStr, Action callBack = null)
    {

        for (int i = dataStr.Count; i < tfmReels.childCount; i++)
        {
            tfmReels.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = tfmReels.childCount; i < dataStr.Count; i++)
        {
            Transform tfm = Instantiate(reelToClone).transform;
            //tfm.parent = tfmReels; //���������ø���������������scale��rotation��Щ������������Ч
            tfm.SetParent(tfmReels);
            tfm.localScale = Vector3.one;
            tfm.rotation = Quaternion.identity;
        }

        int count = dataStr.Count;
        Action cb = () =>
        {
            if (--count <= 0 && callBack != null)
                callBack();
        };

        for (int i = 0; i < dataStr.Count; i++)
        {
            Transform tfm = tfmReels.GetChild(i);
            tfm.gameObject.SetActive(true);
            tfm.GetComponent<MiniReel>().TurnOrKeepSymbol(1f, dataStr[i].ToString(), cb);
        }
    }


    /// <summary>
    /// ��ȡ��С�ĵ�λֵ��0.990 -- 0.01;  0.9870 -- 0.001��
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <remarks>
    /// * ��ĩ�Ƕ���λ
    /// * �����ڶ�λ�ǡ���С���ӵ�λ��
    /// </remarks>
    float GetUnit(string value)
    {
        float res = 0;
        if (value.Contains("."))
        {
            res = 1;
            string dec = value.Split('.')[1];
            char[] charArray = dec.ToCharArray();
            for (int i = 0; i < charArray.Length - 1; i++)
            {
                res = res / 10f;
            }
        }
        else
        {
            res = 10;
        }
        return res;
    }


    /// <summary> Ҫ��������������</summary>
    float toData;

    /// <summary> ��ĩβ������ </summary>
    int lastNumber;

    /// <summary> Ҫ��������������(�ų����Ϊ����λ0)</summary>
    float toDataExcludeLastNumber;

    /// <summary> 
    /// ����ֵ
    /// </summary>
    /// <remarks>
    /// * 0.997 -- addUnit = 0.01
    /// *  0.6 -- addUnit = 1
    /// </remarks> 
    float addUnit;

    void CalculatedData(float value)
    {
        toData = value;// float.Parse(new string(charArray));

        addUnit = GetUnit(value.ToString(format));

        char[] charArray = value.ToString(format).ToCharArray();

        List<char> charLst = new List<char>(charArray);
        lastNumber = int.Parse(charLst[charLst.Count - 1].ToString()); //��ĩβ������
        charLst[charLst.Count - 1] = '0';

        toDataExcludeLastNumber = float.Parse(new string(charLst.ToArray()));
    }


    Coroutine _corAddTo;




    IEnumerator _AddToData(float value)
    {

        if (this.nowData >= value)
        {
            //Debug.Log($"--1-- ");
            _SetData(value);
            _corAddTo = null;

            yield break;
        }

        //Debug.Log($"--2-- ");

        CalculatedData(value);

        //��ȡ���ֻ��������
        nowData = float.Parse(GetDataStr());


        int endlessLoop = 500;
        // ����һ����С���ݲ�
        float tolerance = 0.000001f;
        int xTimes = 0,yTimes = 0;


        // Debug.Log($"AA = toData = {toData}  nowData {this.nowData}  dif = {toData - nowData}  {toData - nowData < maxAddDifNumber}  ");


        //while (Math.Abs(toData - nowData) >tolerance && --endlessLoop >= 0) // �����⣺ nowData ���ܴ���  toData
        while (
            toData > nowData
            && toData - nowData < maxAddDifNumber
            && toData - nowData > tolerance  //С���ݲ���Ϊ���
            && --endlessLoop >= 0)  //
        {

            //Debug.Log($"START nowData =  {this.nowData}  toData = {toData} ; toDataExcludeLastNumber = {toDataExcludeLastNumber} ;  lastNumber = {lastNumber} ");
            bool isFinish = false;

            // START nowData =  102544  toData = 102547 ; toDataExcludeLastNumber = 102540 ;  lastNumber = 7 
            if (toDataExcludeLastNumber > this.nowData)
            {

                // ĩβ���ȹ���
                char[] charArray2 = nowData.ToString(format).ToCharArray();
                List<char> charLst2 = new List<char>(charArray2);
                int last2 = int.Parse(charLst2[charLst2.Count - 1].ToString());//��ĩβ������
                charLst2[charLst2.Count - 1] = '0';
                float _value2 = float.Parse(new string(charLst2.ToArray()));

                if (last2 != 0)
                {
                    // last numb turn from last2 to 0
                    tfmReels.GetChild(0).GetComponent<MiniReel>().TurnSymbol(0.1f, last2, 0, () => { isFinish = true; });
                    yield return new WaitUntil(() => isFinish == true);
                    isFinish = false;

                    //this.nowData = _value2 + addUnit;

                    float temp = _value2 + addUnit;
                    _SetData(temp, () => {
                        isFinish = true;
                    });
                    yield return new WaitUntil(() => isFinish == true);
                    isFinish = false;
                }

                while (
                    toDataExcludeLastNumber > nowData
                     && toDataExcludeLastNumber - nowData < maxAddDifNumber )
                {
                    // last numb turn from 1 to 0  ����1������9���ٹ�����0��
                    tfmReels.GetChild(0).GetComponent<MiniReel>().TurnSymbol(0.1f, 1, 0, () => { isFinish = true; });
                    yield return new WaitUntil(() => isFinish == true);
                    isFinish = false;

                    /*
                    bool isA = temp >= _value;
                    bool isB = temp.ToString() == _value.ToString();
                    DebugUtil.LogWarning($" ==@ TEST = {temp}   _value = {_value} {isA} {isB}");
                    */

                    /*
                    float temp = this.nowData + addUnit;
                    SetData(temp);
                    if (temp.ToString() == toDataExcludeLastNumber.ToString())
                        break;
                    */

                    float temp = this.nowData + addUnit;

                    _SetData(temp, () => {
                        isFinish = true;
                    });
                    yield return new WaitUntil(() => isFinish == true);
                    isFinish = false;

                    if (this.nowData.ToString() == toDataExcludeLastNumber.ToString())
                        break;

                    xTimes++;
                }
            }

            if(toDataExcludeLastNumber - nowData > maxAddDifNumber ){
                break;
            }

            //toData = 1001119  �´� toData = 1001120   ����  nowData = 1001129
            int lastNumber001 = lastNumber;
            char[] charArray3 = this.nowData.ToString(format).ToCharArray();
            int last3 = int.Parse(charArray3[charArray3.Length - 1].ToString());

            if (lastNumber001 != last3)
            {
                yTimes++;

                int dif = lastNumber001 - last3;

                //Debug.Log($"@@@ dif: {dif} lastNumber001: {lastNumber001}  last2: {last3} toData:{toData} nowData: {nowData} abs:{Math.Abs(toData - nowData)}");

                tfmReels.GetChild(0).GetComponent<MiniReel>().TurnSymbol(0.1f, last3, lastNumber001, () => {
                    isFinish = true;
                });

                yield return new WaitUntil(() => isFinish == true);
                isFinish = false;

                float addNumb = 0;
                if (dif < 0) // 7��0�� 
                    addNumb = addUnit;
                else //0��9
                    addNumb = dif * (addUnit / 10f);

                this.nowData += addNumb;
            }
        }

        if (endlessLoop < 0)
        {
            Debug.LogError($"is in endless loop : nowData= {nowData} ; toData= {toData} ; toDataExcludeLastNumber = {toDataExcludeLastNumber}  ; lastNumber= {lastNumber}  xTimes= {xTimes} yTimes= {yTimes} ");
        }

        yield return null;
        //Debug.Log($" nowData = {nowData} ; toData = {toData}; Reels = {GetDataStr()}");

        /**/
        if (nowData != toData)
            _SetData(toData);
        
        _corAddTo = null;
    }


    IEnumerator _AddToData(float formData, float toData)
    {
        _SetData(formData);
        yield return new WaitForSeconds(3f);
        yield return _AddToData(toData);
    }

    [Button]
    public void AddToData(float data = 12.0f)
    {
        if (_corAddTo != null)
        {
            StopCoroutine(_corAddTo);
            _corAddTo = null;
        }
        _corAddTo = StartCoroutine(_AddToData(data));
    }

    public void AddToData(float formData, float toData)
    {
        if (_corAddTo != null)
        {
            StopCoroutine(_corAddTo);
            _corAddTo = null;
        }
        _corAddTo = StartCoroutine(_AddToData(formData, toData));
    }


    public void ChangeAddToData(float data)
    {
        if (_corAddTo == null || data < nowData)
        {
            // ֹͣ�ϴι����������µĹ���
            AddToData(data);
        }
        else
        {
            //�����ϴι�����ֻ�Ǹı����յĹ���ֵ
            CalculatedData(data);
        }
    }


    public string GetDataStr()
    {
        string res = "";
        for (int i = tfmReels.childCount - 1; i >= 0; i--)
        {
            Transform item = tfmReels.GetChild(i);
            if (item.gameObject.active)
            {
                res += item.GetComponent<MiniReel>().data;
            }
        }
        return res;
    }




    [Button]
    private void _SetData(float value, Action callBack = null)
    {
        nowData = value;
        char[] charArray = nowData.ToString(format).ToCharArray();
        List<char> dataStr = new List<char>(charArray);
        dataStr.Reverse();
        SetNumb(dataStr, callBack);
    }


    public void SetData(float value, Action callBack = null)
    {
        if (_corAddTo != null)
        {
            StopCoroutine(_corAddTo);
            _corAddTo = null;
        }
        _SetData(value, callBack);
    }



    [Button]
    public void SetFontmat(string fmt)
    {
        format = fmt;
        char[] charArray = nowData.ToString(format).ToCharArray();
        List<char> dataStr = new List<char>(charArray);
        dataStr.Reverse();
        SetNumb(dataStr);
    }
}
