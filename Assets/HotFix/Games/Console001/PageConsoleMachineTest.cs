using UnityEngine;
using UnityEngine.UI;
using Game;
using System.Collections.Generic;
namespace Console001
{

    public class PageConsoleMachineTest : PageMachineButtonBase
    {

        public PageController ctrlPage;

        public Button btnClose,btnScreenColorCheck;

        public GameObject goFgTicketOut, goFgSpin, goFgSet, goFgScoreUp, goFgScoreDown, goFgDoor;


        public GameObject goScreenColotCheck;

        private void Awake()
        {
            btnClose.onClick.AddListener(OnClickClose);
            btnScreenColorCheck.onClick.AddListener(OnClickScreenColotCheck);

            ctrlPage.pageHandle.AddListener(OnPageChagne);

            //pageCtrl.currentPageIndex;

        }
        private void OnEnable()
        {
            InitParam();
        }

        private void InitParam()
        {
            ctrlPage.PageSet(0, 10);
        }

        int lastPageIndex = -1;
        void OnPageChagne(int index)
        {
            if (!PageManager.Instance.IsTop(this))
                return;
            ChangeMachineButton(index);
        }
        
        void ChangeMachineButton(int index)
        {
            if (lastPageIndex != index)
            {
                lastPageIndex = index;

                switch (lastPageIndex)
                {
                    case 0: //Ideck
                        machineButtonEventDispatcher.ChangeButtonShow(new List<MachineButtonKey>() {
                            MachineButtonKey.BtnSpin,
                            MachineButtonKey.BtnTicketOut,
                        }, MachineButtonType.Regular, true);
                        /*machineButtonEventDispatcher.machineCustomButton = new MachineCustomButton($"{transform.name}#{Guid.NewGuid().ToString()}")  //拦截所有按钮
                        {
                            isPriority = true,
                            btnType = MachineButtonType.Regular,
                        };
                        machineButtonEventDispatcher.OnTop();*/
                        break;
                    case 1: //Device
                    case 2: //Screen
                        machineButtonEventDispatcher.ChangeButtonShow(new List<MachineButtonKey>(), MachineButtonType.Regular, true);
                        /*machineButtonEventDispatcher.machineCustomButton = new MachineCustomButton($"{transform.name}#{Guid.NewGuid().ToString()}")  //拦截所有按钮
                        {
                            isPriority = false,
                            btnType = MachineButtonType.Regular,
                        };
                        machineButtonEventDispatcher.OnTop();*/
                        break;
                }
            }
        }

        public override void OnClickMachineButton(MachineButtonInfo info)
        {


            if (lastPageIndex == 0)
            {
                if (!info.isUp)
                {
                    switch (info.btnKey)
                    {
                        case MachineButtonKey.BtnSpin:
                            goFgSpin.SetActive(true); 
                            break;
                        case MachineButtonKey.BtnTicketOut:
                            goFgTicketOut.SetActive(true);
                            break;
                        case MachineButtonKey.BtnCreditUp:
                            goFgScoreUp.SetActive(true); 
                            break;
                        case MachineButtonKey.BtnCreditDown:
                            goFgScoreDown.SetActive(true);
                            break;
                        case MachineButtonKey.BtnConsole:
                            goFgSet.SetActive(true);
                            break;
                        case MachineButtonKey.BtnDoor:
                            goFgDoor.SetActive(true);
                            break;
                    }
                }
                else
                {
                    switch (info.btnKey)
                    {
                        case MachineButtonKey.BtnSpin:
                            goFgSpin.SetActive(false);
                            break;
                        case MachineButtonKey.BtnTicketOut:
                            goFgTicketOut.SetActive(false);
                            break;
                        case MachineButtonKey.BtnCreditUp:
                            goFgScoreUp.SetActive(false);
                            break;
                        case MachineButtonKey.BtnCreditDown:
                            goFgScoreDown.SetActive(false);
                            break;
                        case MachineButtonKey.BtnConsole:
                            goFgSet.SetActive(false);
                            break;
                        case MachineButtonKey.BtnDoor:
                            goFgDoor.SetActive(false);
                            break;
                    }
                }
            }
        }


        public override void OnTop()
        {
            lastPageIndex = -1;
            ChangeMachineButton(ctrlPage.currentPageIndex);
        }


        void OnClickClose()
        {
            PageManager.Instance.ClosePage(this);
        }



        void OnClickScreenColotCheck()
        {
            goScreenColotCheck.SetActive(true);
        }


    }
}
