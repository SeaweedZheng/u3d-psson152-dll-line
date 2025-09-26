using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWidth : MonoBehaviour
{

    bool isDirty = true;
    float lastTime = 0f;

    private void OnEnable()
    {
        isDirty = true;
    }

    void Update()
    {
        float diffS = Time.time - lastTime;
        if (isDirty && diffS > 0.8f)
        {
            isDirty = false;

            lastTime = Time.time;   

            RectTransform rect = transform.GetComponent<RectTransform>();
            Vector2 v2 = rect.rect.size;

            Text txt = transform.GetComponent<Text>();
            if (txt != null)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, txt.preferredWidth);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, v2.y);
            }

            TextMeshProUGUI tmp = transform.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {

                //DebugUtil.Log($" autoSizeTextContainer = {tmp.autoSizeTextContainer} enableAutoSizing = {tmp.enableAutoSizing}");
                if (! tmp.enableAutoSizing)
                {
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tmp.preferredWidth);
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, v2.y);
                }

            }

            isDirty = true;
        }

    }

}

public class PIDTextMeshProUGUI01 :  TextMeshProUGUI
{
    [Tooltip("按字体的长度，修改当前节点的宽度")]
    public bool isChangeWidth = false;

    protected override void Start()
    {
        base.Start();
        ChangeWidth();
    }

    void ChangeWidth()
    {
        if (!isChangeWidth)
            return;

        RectTransform rect = transform.GetComponent<RectTransform>();
        Vector2 v2 = rect.rect.size;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, base.preferredWidth);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, v2.y);
    }

    public override string text
    {
        get => base.text;
        set
        {
            base.text = value;
            ChangeWidth();
        }
    }
}

public class PIDText : UnityEngine.UI.Text
{
    [Tooltip("按字体的长度，修改当前节点的宽度")]
    public bool isChangeWidth = false;

    protected override void Start()
    {
        base.Start();
        ChangeWidth();
    }

    void ChangeWidth()
    {
        if (!isChangeWidth)
            return;

        RectTransform rect =  transform.GetComponent<RectTransform>();
        Vector2 v2 = rect.rect.size;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, base.preferredWidth);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, v2.y);
    }

    public override string text
    {
        get => base.text;
        set
        {
            base.text = value;
            ChangeWidth();
        }
    }
}