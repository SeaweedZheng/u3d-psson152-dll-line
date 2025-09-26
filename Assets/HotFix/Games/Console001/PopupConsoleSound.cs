using Game;
using GameMaker;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Console001
{
    public class PopupConsoleSound : PageMachineButtonBase
    {
        public Slider sldMusic, sldSound;
        public Toggle tgDemoVoice;
        public Button btnClose;

        void Awake()
        {
            //btnClose = transform.Find("Anchor/Button Close Area/Button Close").GetComponent<Button>();
            btnClose.onClick.AddListener(OnClickClose);

            sldMusic.onValueChanged.AddListener(OnMuiscChange);
            sldSound.onValueChanged.AddListener(OnSoundEffChang);
        }




        bool oldIsDemoVoice;
        float oldMusic;
        float oldSound;
        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            Dictionary<string, object> argDic = null;
            if (data != null)
            {
                argDic = (Dictionary<string, object>)data.value;
                sldMusic.value = (float)argDic["music"];
                sldSound.value = (float)argDic["sound"];
                tgDemoVoice.isOn = (bool)argDic["isDemoVoice"];

                oldMusic = sldMusic.value;
                oldSound = sldSound.value;
                oldIsDemoVoice = tgDemoVoice.isOn;
            }

        }
        void OnClickClose()
        {
            if (oldMusic == sldMusic.value 
                && oldSound == sldSound.value
                && oldIsDemoVoice == tgDemoVoice.isOn)
            {
                PageManager.Instance.ClosePage(this);
            }
            else
            {
                PageManager.Instance.ClosePage(this,
                    new EventData<Dictionary<string,object>>("Resault",
                    new Dictionary<string, object>()
                    {
                        ["music"] = sldMusic.value,
                        ["sound"] = sldSound.value,
                        ["isDemoVoice"] = tgDemoVoice.isOn,
                    }));
            }

        }

        /// <summary>
        /// “Ù–ß
        /// </summary>
        /// <param name="sound"></param>
        void OnSoundEffChang(float sound)
        {
            SoundManager.Instance.SetEFFVolumScale(sound);
        }


        /// <summary>
        /// ±≥æ∞“Ù¿÷
        /// </summary>
        /// <param name="music"></param>
        void OnMuiscChange(float music)
        {
            SoundManager.Instance.SetBGMVolumScale(music);
        }
    }
}
