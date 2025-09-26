using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageController : PageScrollView
{

    public GameObject goToggleGroup;


    Transform tfmTgGroup;

    void Start()
    {
        if (goToggleGroup == null)
            return;

        tfmTgGroup = goToggleGroup.transform;

        for (int i = tfmTgGroup.childCount; i< pageNormalizedPos.Length; i++)
        {
            Transform tfm = Instantiate(tfmTgGroup.GetChild(0)).transform;
            tfm.SetParent(tfmTgGroup);
            tfm.localScale = Vector3.one;
        }

        foreach (Transform tfmTg in goToggleGroup.transform)
        {
            tfmTg.gameObject.SetActive(false);
        }

        for (int i = 0; i < pageNormalizedPos.Length; i++)
        {
            Transform tfmTg = tfmTgGroup.GetChild(i);
            tfmTg.gameObject.SetActive(true);
            Toggle compTg = tfmTg.GetComponent<Toggle>();
            compTg.onValueChanged.AddListener((isOn) =>
            {
                OnToggleValueChanged(tfmTg.GetSiblingIndex(), isOn);
            });

            Transform tfmLabel = tfmTg.Find("Label");
            if (tfmLabel != null)
            {
                Text txtLabel = tfmLabel.GetComponent<Text>();
                if(txtLabel!=null)
                    txtLabel.text = $"{i + 1}";
                TextMeshProUGUI tmpLabel = tfmLabel.GetComponent<TextMeshProUGUI>();
                if (tmpLabel != null)
                    tmpLabel.text = $"{i + 1}";
            }
        }

        pageHandle.AddListener(OnPageChange);
    }
    void OnPageChange(int index)
    {
        tfmTgGroup.GetChild(index).GetComponent<Toggle>().isOn = true;
    }
    void OnToggleValueChanged(int index,bool isOn)
    {
        if (isOn && currentPageIndex != index)
        {
            PageSet(index);
        }
    }
    

    /// <summary>
    /// 延时跳转页面
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="ms"></param>
    public void PageSet(int pageIndex,int ms)
    {
        StartCoroutine(DoPageSet(pageIndex, ms));
    }
    IEnumerator DoPageSet(int pageIndex, int ms)
    {
        yield return new WaitForSeconds(((float)ms) /1000f);
        //延时设置才，ToggleGroup才起作用！
        PageSet(pageIndex);
    }

}
