using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[RequireComponent(typeof(Animator))]
public class ToggleAnimatorTrigger : MonoBehaviour
{
    private const string kDefaultOnAnimName = "On";
    private const string kDefaultOffAnimName = "Off";

    [SerializeField]
    private string m_OnTrigger = kDefaultOnAnimName;

    [SerializeField]
    private string m_OffTrigger = kDefaultOffAnimName;


    public  string onTaigger
    {
        get => m_OnTrigger;
    }
    public string offTrigger
    {
        get => m_OffTrigger;
    }


    private Animator m_Animator;
    private Toggle m_Toggle;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void Start()
    {
        m_Animator.SetTrigger(m_Toggle.isOn ? m_OnTrigger : m_OffTrigger);
    }
    private void OnToggleValueChanged(bool on)
    {
        m_Animator.ResetTrigger(m_OnTrigger);
        m_Animator.ResetTrigger(m_OffTrigger);
        m_Animator.SetTrigger(on ? m_OnTrigger : m_OffTrigger);
    }
}