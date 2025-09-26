using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public GameObject go5Kind;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public IEnumerator Show5KindPoup()
    {
        go5Kind.SetActive(true);
        yield return new WaitForSeconds(1f);
        go5Kind.SetActive(false);
    }


}
