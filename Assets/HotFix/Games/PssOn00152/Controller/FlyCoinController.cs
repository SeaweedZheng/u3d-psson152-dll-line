using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;

public class FlyCoinController : CorBehaviour
{
    public GameObject goAnchor;

    public List<Sprite> coinImg = new List<Sprite>();

    Transform tfmAnchor => goAnchor.GetComponent<Transform>();

    void Start()
    {

    }

    const string COR_FlY_COINS = "COR_FlY_COINS";

    [Button]
    public void FlayCoins()
    {
        DoCor(COR_FlY_COINS, _FlayCoins());
    }

    public void StopCoins()
    {
        ClearCor(COR_FlY_COINS);
    }

    private void OnDisable()
    {
        ClearAllCor();
        StopAllCoins();
    }


    public void StopAllCoins()
    {
        for (int i = 0; i < tfmAnchor.childCount; i++)
        {
            GameObject go = tfmAnchor.GetChild(i).gameObject;
            if (go.active)
            {
                FlyCoinUI uiCoin = go.GetComponent<FlyCoinUI>();
                uiCoin.Kill();
                go.SetActive(false);
            }
        }
    }

    IEnumerator  _FlayCoins()
    {

        while (true)
        {
            int j = 50;
            while (--j > 0)
            {

                GameObject goCoin = null;

                for (int i = tfmAnchor.childCount - 1; i > 0; i--)
                {
                    GameObject go = tfmAnchor.GetChild(i).gameObject;
                    if (go.active == false)
                    {
                        goCoin = go;
                        go.transform.SetSiblingIndex(1);
                        break;
                    }
                }

                if (goCoin == null)
                {
                    GameObject go = Instantiate(tfmAnchor.GetChild(0).gameObject);
                    go.transform.SetParent(tfmAnchor);
                    go.transform.SetSiblingIndex(1);

                    //float CoinX = Random.Range(-(720 / 2), (720 / 2));
                    float CoinX = Random.Range(-(720 / 3), (720 / 3));
                    float CoinY = Random.Range(-100, 100);

                    go.transform.localPosition = new Vector3(CoinX, CoinY, 0);
                    go.transform.localScale = new Vector3(1.4f, 1.4f, 1);


                    goCoin = go;
                }

                goCoin.SetActive(true);

                FlyCoinUI uiCoin = goCoin.GetComponent<FlyCoinUI>();

                // »»Í¼Æ¬
                if (coinImg.Count > 0)
                {
                    uiCoin.SetSprite(coinImg[Random.Range(0, coinImg.Count)]);
                }

                uiCoin.isClockWise = Random.Range(0, 2) == 1;
                bool isLeft = Random.Range(0, 2) == 1;

                uiCoin.DoAnim(isLeft, () =>
                {
                    uiCoin.gameObject.SetActive(false);
                });
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(3f);
        }
    }
}
