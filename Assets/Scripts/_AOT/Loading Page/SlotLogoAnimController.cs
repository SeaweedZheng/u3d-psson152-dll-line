using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotLogoAnimController : MonoBehaviour
{


    public Transform tfmReel1, tfmReel2, tfmReel3;


    public GameObject goMachineClickUp, goMachineClickDown;


    #region Reel Anim
    float gapTime = 0.08f;
    private void OnEnable()
    {
        StopAllCoroutines();
        goMachineClickDown.SetActive(false);
        goMachineClickUp.SetActive(true);
        ResetReel(tfmReel1);
        ResetReel(tfmReel2);
        ResetReel(tfmReel3);
        StartCoroutine(LogoAmin());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    void ResetReel(Transform tfmReel)
    {
        foreach (Transform chd in tfmReel)
            chd.gameObject.SetActive(false);

        tfmReel.GetChild(0).gameObject.SetActive(true);
    }
    IEnumerator LogoAmin()
    {
        yield return new WaitForSeconds(10f);

        bool isToStop;
        bool isAllStop;
        while (true)
        {
            float startRunTimeS = Time.time;
            isToStop = false;
            isAllStop = false;

            goMachineClickDown.SetActive(true);
            goMachineClickUp.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            goMachineClickDown.SetActive(false);
            goMachineClickUp.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            yield return new WaitForSeconds(gapTime);

            ReelRunNext(tfmReel1, isToStop);

            yield return new WaitForSeconds(gapTime);
            ReelRunNext(tfmReel1, isToStop);
            ReelRunNext(tfmReel2, isToStop);

            yield return new WaitForSeconds(gapTime);

            ReelRunNext(tfmReel1, isToStop);
            ReelRunNext(tfmReel2, isToStop);
            ReelRunNext(tfmReel3, isToStop);

            while (isToStop == false || isAllStop == false)
            {
                float nowRunTimeS = Time.time;
                if (nowRunTimeS - startRunTimeS > 2f)
                {
                    isToStop = true;
                }

                yield return new WaitForSeconds(gapTime);


                bool A = ReelRunNext(tfmReel1, isToStop);
                bool B = ReelRunNext(tfmReel2, isToStop);
                bool C = ReelRunNext(tfmReel3, isToStop);
                isAllStop = A && B && C;
            }

            yield return new WaitForSeconds(5f);
        }
    }

    bool ReelRunNext(Transform tfmReel, bool toStop)
    {
        int idx = 0;
        for (int i = 0; i < tfmReel.childCount; i++)
        {
            if (tfmReel.GetChild(i).gameObject.active)
            {
                idx = i;
                break;
            }
        }

        if (toStop && idx == 0)
            return true;

        if (++idx >= tfmReel.childCount)
            idx = 0;

        foreach (Transform chd in tfmReel)
            chd.gameObject.SetActive(false);

        tfmReel.GetChild(idx).gameObject.SetActive(true);

        return false;
    }
    #endregion


}
