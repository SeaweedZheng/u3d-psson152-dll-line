using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ӡFPS
/// </summary>
public class FPS : MonoBehaviour
{
    float _updateInterval = 1f;//�趨����֡�ʵ�ʱ����Ϊ1��  
    float _accum = .0f;//�ۻ�ʱ��  
    int _frames = 0;//��_updateIntervalʱ���������˶���֡  
    float _timeLeft;
    string fpsFormat;
    public Text fpsText;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _timeLeft = _updateInterval;
    }

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(0, 0, 200, 200), fpsFormat);
    //}

    void Update()
    {
        _timeLeft -= Time.deltaTime;
        //Time.timeScale���Կ���Update ��LateUpdate ��ִ���ٶ�,  
        //Time.deltaTime��������㣬������һ֡��ʱ��  
        //������ɵõ���Ӧ��һ֡���õ�ʱ��  
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;//֡��  

        if (_timeLeft <= 0)
        {
            float fps = _accum / _frames;
            fpsFormat = System.String.Format("{0:F2}FPS", fps);//������λС��  

            _timeLeft = _updateInterval;
            _accum = .0f;
            _frames = 0;
        }

        fpsText.text = fpsFormat;
    }
}
