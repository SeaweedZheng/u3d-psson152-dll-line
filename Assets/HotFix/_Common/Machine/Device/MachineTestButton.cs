using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTestButton : MonoBehaviour
{

    bool isDownCtrl = false;
    void Update()
    {
        if (!Application.isEditor)
            return;

        if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            isDownCtrl = true;
        }
        if (Input.GetKeyUp(KeyCode.RightControl) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            isDownCtrl = false;
        }


        //¿ªÊ¼ÓÎÏ· Spin down
        if (isDownCtrl && (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyUp(KeyCode.KeypadEnter)))
        {
            EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_DOWN,
                MachineDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnSpin));
        }


        if (isDownCtrl && (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)))
        {
            EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_UP,
                MachineDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnSpin));
        }



        if (isDownCtrl)
        {
            /*if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus)) // score up
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_DOWN,
                    MachieDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnCreditUp));
            }
            if (Input.GetKeyUp(KeyCode.KeypadPlus) || Input.GetKeyUp(KeyCode.Plus)) // score up
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_UP,
                    MachieDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnCreditUp));
            }*/


            if (Input.GetKeyDown(KeyCode.KeypadPlus)) // score up
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_DOWN,
                    MachineDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnCreditUp));
            }
            if (Input.GetKeyUp(KeyCode.KeypadPlus)) // score up
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_UP,
                    MachineDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnCreditUp));
            }



            /*if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus)) // score down
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_DOWN,
                    MachieDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnCreditDown));
            }
            if (Input.GetKeyUp(KeyCode.KeypadMinus) || Input.GetKeyUp(KeyCode.Minus)) // score down
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_UP,
                    MachieDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnCreditDown));
            }*/
            if (Input.GetKeyDown(KeyCode.KeypadMinus)) // score down
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_DOWN,
                    MachineDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnCreditDown));
            }
            if (Input.GetKeyUp(KeyCode.KeypadMinus)) // score down
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_UP,
                    MachineDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnCreditDown));
            }



            if (Input.GetKeyDown(KeyCode.F1)) // ticket out
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_DOWN,
                    MachineDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnTicketOut));
            }
            if (Input.GetKeyUp(KeyCode.F1)) // ticket out
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_UP,
                    MachineDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnTicketOut));
            }


            if (Input.GetKeyDown(KeyCode.F2)) // print out
            {
                //BtnPrint
            }
            if (Input.GetKeyUp(KeyCode.F2)) // print out
            {
                MachineDeviceController.Instance.DoPrinterOut();
            }


            if (Input.GetKeyDown(KeyCode.F3)) // Console out
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_DOWN,
                    MachineDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnConsole));
            }
            if (Input.GetKeyUp(KeyCode.F3)) // Console out
            {
                EventCenter.Instance.EventTrigger<ulong>(EventHandle.HARDWARE_KEY_UP,
                    MachineDeviceController.Instance.GetButtonValue(MachineButtonKey.BtnConsole));
            }
        }


        if (isDownCtrl)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // bet up
            {

            }
            if (Input.GetKeyUp(KeyCode.UpArrow)) // bet up
            {

            }

            if (Input.GetKeyDown(KeyCode.DownArrow)) // bet down
            {

            }
            if (Input.GetKeyUp(KeyCode.DownArrow)) // bet down
            {

            }

            if (Input.GetKeyDown(KeyCode.M))
            {

            }
            if (Input.GetKeyUp(KeyCode.M))
            {

            }


            if (Input.GetKeyDown(KeyCode.RightArrow))
            {

            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {

            }


            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {

            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {

            }

            if (Input.GetKeyDown(KeyCode.T))
            {

            }
            if (Input.GetKeyUp(KeyCode.T))
            {

            }

            if (Input.GetKeyDown(KeyCode.S))
            {

            }
            if (Input.GetKeyUp(KeyCode.S))
            {

            }
        }
    }




}
