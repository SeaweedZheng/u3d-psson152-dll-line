using UnityEngine;
using UnityEngine.Events;


public class MonoHelper : MonoBehaviour
{
    public UnityEvent updateHandle = new UnityEvent();

    public UnityEvent fixeUpdateHandle = new UnityEvent();


    // Update is called once per frame
    void Update()
    {
        updateHandle?.Invoke();
    }

    private void FixedUpdate()
    {
        fixeUpdateHandle?.Invoke();
    }
}
