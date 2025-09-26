using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosterLoader : ImageLoader
{
    void Start()
    {
        if (!string.IsNullOrEmpty(ApplicationSettings.Instance.posterUrl))
        {
            LoadImage(ApplicationSettings.Instance.posterUrl, (spr) =>
            {
                Image compImg = transform.GetComponent<Image>();
                compImg.enabled = true;
                compImg.sprite = spr;
                //compImg.SetNativeSize();
            });
        }
    }
}
