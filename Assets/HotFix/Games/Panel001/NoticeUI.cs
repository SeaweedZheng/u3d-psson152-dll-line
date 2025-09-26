using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NoticeUI : MonoBehaviour
{


    public enum NoticeState
    {
        Credit,
        Describe,
    }

    [SerializeField]
    private NoticeState m_State = NoticeState.Credit;
    public NoticeState state
    {
        get => m_State;
        set
        {
            m_State = value;
            if (m_State == NoticeState.Describe)
            {
                if(goDescribe != null)
                    goDescribe.SetActive(true);
                if (goMoney != null)
                    goMoney.SetActive(false);
                if (goCredit != null)
                    goCredit.SetActive(false);
            }
            else
            {
                if (goDescribe != null)
                    goDescribe.SetActive(false);
                if (goMoney != null)
                    goMoney.SetActive(true);
                if (goCredit != null)
                    goCredit.SetActive(true);
            }
        }
    }

    [SerializeField]
    private string m_Title;
    public string title
    {
        get => m_Title;
        set
        {
            if (tmpTitle != null)
            {
                tmpTitle.text = value;
            }
            else if (txtTitle != null)
            {
                txtTitle.text = value;
            }
        }
    }

    [SerializeField]
    private long m_Credit;
    public long credit
    {
        get => m_Credit;
        set
        {
            if (tmpCredit != null)
            {
                tmpCredit.text = SetFormat(value);
            }
            else if (txtCredit != null)
            {
                txtCredit.text = SetFormat(value);
            }

            if (tmpMoney != null)
            {
                tmpMoney.text = $"${value.ToString("N2")}";
            }
            else if (txtMoney != null)
            {
                txtMoney.text = $"${value.ToString("N2")}";
            }
        }
    }


    [SerializeField]
    private string m_Describe;

    public string describe
    {
        get => m_Describe;
        set
        {
            m_Describe = value;

            if (tmpDescribe != null)
            {
                tmpDescribe.text = value;
            }
            else if (txtDescribe != null)
            {
                txtDescribe.text = value;
            }
            else if (describeObjs != null)
            {
                foreach (var item in describeObjs)
                {
                    item.obj.SetActive(item.describe == value);
                }
            }
        }
    }




    public GameObject goTitle;
    private Text txtTitle => goTitle.GetComponent<Text>();
    private TextMeshProUGUI tmpTitle => goTitle.GetComponent<TextMeshProUGUI>();


    public GameObject goMoney;
    private Text txtMoney => goMoney == null? null: goMoney.GetComponent<Text>();
    private TextMeshProUGUI tmpMoney => goMoney == null ? null : goMoney.GetComponent<TextMeshProUGUI>();


    public GameObject goCredit;
    private Text txtCredit => goCredit.GetComponent<Text>();
    private TextMeshProUGUI tmpCredit => goCredit.GetComponent<TextMeshProUGUI>();


    public GameObject goDescribe;
    private Text txtDescribe => goDescribe.GetComponent<Text>();
    private TextMeshProUGUI tmpDescribe => goDescribe.GetComponent<TextMeshProUGUI>();
    //private Image imgDescribe => goDescribe.GetComponent<Image>();


    public List<DescribeGameObject> describeObjs;



    [System.Serializable]
    public class DescribeGameObject
    {
        public string describe;

        public GameObject obj;
    }



    public long nowCredit;
    public long toCredit;

    Coroutine _cor;

    private  void ClearCor()
    {
        if (_cor != null)
            StopCoroutine(_cor);
        _cor = null;

    }

    public void SetToCredit(long toCredit)
    {
        ClearCor();
        nowCredit = toCredit;
        credit = nowCredit;
    }


    public void MoveToCredit(long toCredit)
    {
        ClearCor();
        _cor = StartCoroutine(_MoveToCredit(nowCredit, toCredit));
    }


    public void MoveToCredit(long fromCredit, long toCredit)
    {
        ClearCor();
        _cor = StartCoroutine(_MoveToCredit(fromCredit,toCredit));
    }

    IEnumerator _MoveToCredit(long fromCredit, long toCredit)
    {
        nowCredit = fromCredit;
        credit = nowCredit;
        float startTimeS = Time.unscaledTime;
        while (nowCredit != toCredit && Time.unscaledTime - startTimeS <3f)
        {     
            yield return new WaitForSeconds(0.01f);
            nowCredit += nowCredit > toCredit? -1:1;
            credit = nowCredit;    
        }
        credit = toCredit;
    }



    string SetFormat(long credit)
    {
        string lang = "sch";
        switch (lang)
        {
            case "en":
                return $"${credit.ToString("N0")}";
                //return $"${credit.ToString("N2")}";
                //return credit.ToString("C");
                //return credit.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            case "sch":
            default:
                return credit.ToString();
        }
    }
}
