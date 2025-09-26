using GameMaker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackboardInfo : MonoBehaviour
{
    public string path = "";
    public string argBefore = "";
    public string argAfter = "";
    public string format = ""; // ²»ÒªÇ§·ÖºÅ  "N0";
    public GameObject goTarget;
    Text txtComp => goTarget.GetComponent<Text>();
    TextMeshProUGUI tmpComp => goTarget.GetComponent<TextMeshProUGUI>();

    void Awake()
    {
        if (goTarget == null)
        {
            goTarget = gameObject;
        }
        _property = null;
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, OnPropertyChanged);
        //SetData(path);
        OnPropertyChanged();
    }

    public void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, OnPropertyChanged);
    }

    //"./grandJP"

    string _property = null;
    string property
    {
        get
        {
            if (_property == null)
            {
                string[] str = path.Split('/');
                _property = $"{str[0]}/{str[1]}";
            }
            return _property;
        }
    }

    void OnPropertyChanged(EventData res = null)
    {
        if (res == null || res.name == property)
        {
            SetData(path);
        }
    }

    public void SetData(string pth)
    {
        object value = BlackboardUtils.GetValue<object>(null, pth);
        string str = "";
        format = format.Replace(" ", "");
        try
        {
            double tmp = double.Parse(value.ToString());
            str = string.IsNullOrEmpty(format) ? tmp.ToString() : tmp.ToString(format);
        }
        catch
        {
            str = value.ToString();
        }

        //Debug.LogError($"str :{str}  pth: {pth} value: {value}");
        if (txtComp != null)
        {
            txtComp.text = $"{argBefore}{str}{argAfter}";
        }

        if (tmpComp != null)
        {
            tmpComp.text = $"{argBefore}{str}{argAfter}";
        }
    }


}
