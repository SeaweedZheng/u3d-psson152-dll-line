using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//用于UGUI序列帧动画
public class SpriteAnimator : MonoBehaviour
{
    public bool playAwake;
    public Sprite[] frames;
    public float speed = 0.05f;
    public int actionFrame = -1;
    public UnityEvent frameEvent;

    private Image container;
    private int ticked;
    private float time;
    private bool doAnim;

    private void Awake()
    {
        container = GetComponent<Image>();
        Init();
    }

    private void Init()
    {
        ticked = 0;
        time = 0;
        doAnim = playAwake;
        container.sprite = frames[0];
    }

    public void Play()
    {
        ticked = 0;
        time = 0;
        doAnim = true;
        container.sprite = frames[0];
    }

    public void Pause()
    {
        doAnim = false;
    }

    public void Resume()
    {
        doAnim = true;
    }

    public void Stop()
    {
        ticked = 0;
        time = 0;
        doAnim = false;
        container.sprite = frames[0];
    }

    void Update()
    {
        if (doAnim)
        {
            time += Time.deltaTime;
            if (time > speed)
            {
                ticked++;
                if (ticked == frames.Length)
                    ticked = 0;
                else
                    time = 0;

                if (ticked == actionFrame)
                    frameEvent.Invoke();

                container.sprite = frames[ticked];
                container.SetNativeSize();
            }
        }
    }
}
