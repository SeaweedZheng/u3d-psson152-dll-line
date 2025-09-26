using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoLoader : ImageLoader
{
    void Start()
    {
        if (!string.IsNullOrEmpty(ApplicationSettings.Instance.logoUrl))
        {
            LoadImage(ApplicationSettings.Instance.logoUrl, (spr) =>
            {
                Image compImg = transform.GetComponent<Image>();
                compImg.enabled = true;
                compImg.sprite = spr;
                compImg.SetNativeSize();
            });
        }
    }
}

