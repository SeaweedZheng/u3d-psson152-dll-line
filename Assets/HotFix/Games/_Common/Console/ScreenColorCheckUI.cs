using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 机台屏幕颜色测试脚本
/// </summary>
public class ScreenColorCheckUI : MonoBehaviour
{
    public Button BtnBack;
    public Image BackImage;

    private float time;
    private Color[] colors = new Color[5] { Color.red, Color.green, Color.blue, Color.white, Color.black };
    private int colorIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        BtnBack.onClick.AddListener(() =>
        {
            SetActive(false);
        });
    }
    private void OnEnable()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.unscaledDeltaTime;
        if (time > 1)
        {
            BackImage.color = colors[colorIndex];
            colorIndex++;
            colorIndex %= colors.Length;
            time = 0;
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    private void OnDisable()
    {
        time = 0;
        colorIndex = 0;
    }
}
