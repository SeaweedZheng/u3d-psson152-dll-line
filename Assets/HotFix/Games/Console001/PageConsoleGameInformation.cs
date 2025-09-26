using Game;
using GameMaker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _consoleBB = PssOn00152.ConsoleBlackboard02;

namespace Console001
{
    public class PageConsoleGameInformation : PageMachineButtonBase
    {

        public Button btnClose;

        public TextMeshProUGUI tmpPlatform, tmpMachineName, tmpGameTheme, tmpDiskVer,
            tmpSofwareVer,tmpHardwareVer,tmpAlgorithmVer;

       // public UIRoundConor_RawImage imgProducedBy;

        public Image imgProducedBy;

        //ProducedBy
        private void Awake()
        {
            btnClose.onClick.AddListener(OnClickClose);
        }


        public override void OnOpen(PageName name, EventData data)
        {
            base.OnOpen(name, data);

            string themeGameName = ApplicationSettings.Instance.gameTheme;
            tmpMachineName.text = I18nMgr.T(themeGameName);
            tmpGameTheme.text = I18nMgr.T(themeGameName);
            tmpSofwareVer.text = $"{ApplicationSettings.Instance.appVersion}/{GlobalData.hotfixVersion}"; // _consoleBB.Instance.platform;
            tmpHardwareVer.text = _consoleBB.Instance.hardwareVer;
            tmpAlgorithmVer.text = _consoleBB.Instance.algorithmVer;



            imgProducedBy.sprite = spriteProduced;

        }


        Sprite _spriteProduced;
        Sprite spriteProduced
        {
            get
            {
                if(_spriteProduced == null)
                {
                    //Texture2D texture = ResourceManager.Instance.LoadAssetAtPathOnce<Texture2D>("Assets/GameRes/Games/PssOn00152 (1080x1920)/ABs/Sprites/Game Icon/PssOn00152.png");
                    Texture2D texture = ResourceManager.Instance.LoadAssetAtPathOnce<Texture2D>(ConfigUtils.curGameAvatarURL);
                    _spriteProduced = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
                return _spriteProduced;
            }
        }


        void OnClickClose()
        {
            PageManager.Instance.ClosePage(this);
        }
    }
}