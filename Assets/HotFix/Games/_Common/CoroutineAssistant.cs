using System.Collections;
using System;

/// <summary>
/// 普通脚本都可用上协程
/// </summary>
public class CoroutineAssistant : MonoSingleton<CoroutineAssistant>
{
    private CorController _corCtrl;
    private CorController corCtrl
    {
        get
        {
            if (_corCtrl == null)
            {
                _corCtrl = new CorController(this);
            }
            return _corCtrl;
        }
    }

    public void ClearCor(string name) => corCtrl.ClearCor(name);

    public void ClearAllCor() => corCtrl.ClearAllCor();

    public void DoCor(string name, IEnumerator routine) => corCtrl.DoCor(name, routine);

    public bool IsCor(string name) => corCtrl.IsCor(name);

    public IEnumerator DoTaskRepeat(Action cb, int ms) => corCtrl.DoTaskRepeat(cb, ms);

    public IEnumerator DoTask(Action cb, int ms) => corCtrl.DoTask(cb, ms);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
