using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleButtonTest : MonoBehaviour
{
    public Text txtComp;

    int i = 1;
    void Awake()
    {

        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    private void OnEnable()
    {
        txtComp.text = "";
        i = 1;
    }

    void OnClick()
    {
        txtComp.text = $"{txtComp.text}{i}";
        if (++i >= 10)
            i = 1;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
