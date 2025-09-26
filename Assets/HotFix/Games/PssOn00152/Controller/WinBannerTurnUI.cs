using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBannerTurnUI : MonoBehaviour
{
    private void OnEnable()
    {
        TurnBarrner();
    }

    private void OnDisable()
    {
        StopBarrner();
    }

    Coroutine _cor;
    public void TurnBarrner()
    {
        ClearCor();
        _cor = StartCoroutine(_TurnBarrner());
    }

    public void StopBarrner()
    {
        ClearCor();
    }


    private void ClearCor()
    {
        if (_cor != null)
        {
            StopCoroutine(_cor);
        }
        _cor = null;
    }

    CanvasGroup cg => transform.GetComponent<CanvasGroup>();

    IEnumerator _TurnBarrner()
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }

        while (true)
        {
            foreach (Transform item in transform)
            {
                item.gameObject.SetActive(true);


                //½¥±ä
                cg.alpha = 0f;
                while (cg.alpha < 1)
                {
                    cg.alpha += 0.1f;
                    yield return new WaitForSeconds(0.06f);
                }

                yield return new WaitForSeconds(2f);

                //½¥±ä
                cg.alpha = 1f;
                while (cg.alpha >= 0.1f)
                {
                    cg.alpha -= 0.1f;
                    yield return new WaitForSeconds(0.06f);
                }

                item.gameObject.SetActive(false);
            }
        }
    }
    
}
