using Game;
using SBoxApi;
using System.Collections.Generic;
using GameMaker;
using UnityEngine;
using _consoleBB = PssOn00152.ConsoleBlackboard02;
using Newtonsoft.Json;
using System.Collections;


public class DeviceCoder : CorBehaviour
{
    const string COR_CHECK_CODER_REPEAT = "COR_CHECK_CODER_REPEAT";
    void OnEnable()
    {
        /*
        if (!ApplicationSettings.Instance.isMachine)
            return;
        */

        //UI--��ʾLicenen
        EventCenter.Instance.AddEventListener<EventData>(MachineUIEvent.ON_MACHINE_UI_EVENT, OnMachineUIEvent);


        DoCor(COR_CHECK_CODER_REPEAT, DoTaskRepeat(CheckSBoxNeedActivated, 10000));

    }


    private void OnDisable()
    {
        ClearCor(COR_CHECK_CODER_REPEAT);

        EventCenter.Instance.RemoveEventListener<EventData>(MachineUIEvent.ON_MACHINE_UI_EVENT, OnMachineUIEvent);

        if (seqIdActive != null && MachineDataManager.Instance != null)
            MachineDataManager.Instance.RemoveRequestAt((int)seqIdActive);
        seqIdActive = null;

        if(seqIdCode != null && MachineDataManager.Instance != null)
            MachineDataManager.Instance.RemoveRequestAt((int)seqIdCode);
        seqIdCode = null;
    }


    const string MARK_IS_POPUP_CHECK_ACTIVE = "MARK_IS_POPUP_CHECK_ACTIVE";

    /*
    1:����ʱ�䵽(ʱ��������)
    2:ӯ�൵��
    3:��ʷ���ݹ���
    4:ǫ̈̄����(����ֱ���)
    5:���ֱ���
    */

    /*
    //������:
    //0:�������� 
    //1:���ֱ���
    //2��ǫ̈̄����������ֱ�����
    //3:ӯ�൵��
    //4:������δ���� 
    //5:��ӡ��ȱֽ 
    //6:�Ƿ�����1 
    //7:�Ƿ�����2
    //8������ʱ�䵽��ʱ�������أ�
    //9:��ʷ���ݹ���
    //10:���ڴ�ӡ
    //11����ӡ���е�����
    //12����ӡ����δ�ر�
    */


    /// <summary>
    /// ����
    /// </summary>
    /// <remarks>
    /// ������ֵ���塿<br/>
    /// * 0:
    /// * 1: 
    /// * 2: ӯ���������崻�
    /// 
    /// 
    /// 
    /// </remarks>
    private void CheckSBoxNeedActivated()
    {
        //������
        seqIdActive = MachineDataManager.Instance.RequestIsCodingActive((res) =>
        {
            int code = (int)res;
            bool isActive = code == 0;

            DebugUtils.Log($"check code; isActive = {isActive}");

            _consoleBB.Instance.isMachineActive = isActive;
            //BlackboardUtils.SetValue<bool>("@console/isMachineActive", isActive);

            if (!(bool)isActive)
            {
                //��̨û�д�
                if (PageManager.Instance.IndexOf(PageName.Console001PageConsoleMain) == -1)
                {

                    if (!CommonPopupHandler.Instance.isOpen(MARK_IS_POPUP_CHECK_ACTIVE)
                       //&& ErrorPopupHandler.Instance.curPopupType != ErrorPopupType.SystemTextOnly
                       )
                        CommonPopupHandler.Instance.OpenPopupSingle(new CommonPopupInfo()
                        {
                            text = string.Format(I18nMgr.T("<size=24>Please activate : {0}</size>"), code),
                            type = CommonPopupType.SystemTextOnly,
                            buttonAutoClose1 = false,
                            buttonAutoClose2 = false,
                            isUseXButton = false,
                            mark = MARK_IS_POPUP_CHECK_ACTIVE,
                        });
                }
            }
        });
    }

    int? seqIdActive;
    int? seqIdCode;
    private void OnMachineUIEvent(EventData evt)
    {
        if (evt.name == MachineUIEvent.ShowCoding)
        {
            seqIdCode = MachineDataManager.Instance.RequestMachineCodingInfo((object res) =>
            {
                OnResponseShowCoder(res as SBoxCoderData);

            }, (BagelCodeError err) =>
            {

            });
        }else if (evt.name == MachineUIEvent.CheckCodeActive)
        {
            CheckSBoxNeedActivated();
        }
    }



    /// <summary>
    /// ���ش�������
    /// </summary>
    /// <param name="data"></param>
    private async void OnResponseShowCoder(SBoxCoderData data)
    {

        //long day = data.RemainMinute / (60 * 24);
        //long hour = data.RemainMinute % (60 * 24) /60

        long totalBets = data.Bets != 0 ? data.Bets : TableBusniessTotalRecordAsyncManager.Instance.historyTotalBet;
        long totalWins = data.Wins != 0 ? data.Wins : TableBusniessTotalRecordAsyncManager.Instance.historyTotalWin;

        DebugUtils.LogWarning($"��ȡ�������� �� {JSONNodeUtil.ObjectToJsonStr(data)}");
        Dictionary<string, object> req = new Dictionary<string, object>()
        {
            ["A"] = $"{totalBets}", //data.Bets.ToString(),
            ["B"] = $"{totalWins}", //data.Wins.ToString(),
            ["C"] = data.MachineId.ToString(),
            ["D"] = data.CoderCount.ToString(),
            ["E"] = data.CheckValue.ToString(),
           // ["Day"] = (data.RemainMinute  / 60 /24).ToString(),//������
            //["Hour"] = ((data.RemainMinute / 60) % 24).ToString(),//����Сʱ
            //["Minute"] = (data.RemainMinute % 60).ToString(),//���ٷ���
            ["Day"] = (data.RemainMinute / (60 * 24)).ToString(),//������
            ["Hour"] = ((data.RemainMinute % (60 * 24) / 60)).ToString(),//����Сʱ
            ["Minute"] = (data.RemainMinute % 60).ToString(),//���ٷ���


            //((data.RemainMinute / 60) % 24).ToString();
        };

        EventData res = await PageManager.Instance.OpenPageAsync(PageName.Console001PopupConsoleCoder, 
            new EventData<Dictionary<string, object>>("", req));

        if (res.value != null)
        {

            seqIdCode = MachineDataManager.Instance.RequestSetCoding(ulong.Parse((string)res.value), (res) =>
            {
                OnCoder(res as SBoxPermissionsData);
            }, (err) =>
            {
                OnCoder(err.response as SBoxPermissionsData);
            });
        }

    }

    //const string COR_CLEAR_SQL_TABLE = "COR_CLEAR_SQL_TABLE";


    const string COR_CODE = "COR_CODE"; 
    /// <summary>
    /// ���뼤�����
    /// </summary>
    /// <param name="sBoxPermissionsData"></param>
    private void OnCoder(SBoxPermissionsData sBoxPermissionsData)
    {
        DoCor(COR_CODE, _OnCode(sBoxPermissionsData));
    }

    IEnumerator _OnCode(SBoxPermissionsData sBoxPermissionsData)
    {
        //DebugUtil.Log("������");
        //DebugUtil.Log(sBoxPermissionsData.result);
        //DebugUtil.Log(sBoxPermissionsData.permissions);

        bool isNext = false;

        bool isSuccess = sBoxPermissionsData.result == 0;
        if (isSuccess) // �ɹ�
        {
            if (sBoxPermissionsData.permissions / 1000 > 0)//2001
            {
                if (sBoxPermissionsData.permissions % 10 > 0) //����
                {

                    //�������Ϸ��¼������Ͷ�˱Ҽ�¼�����ݡ�

                    MaskPopupHandler.Instance.OpenPopup();

                    MachineRestoreManager.Instance.ClearRecordWhenCoding();

                    yield return new WaitForSeconds(5f);
                }
                else
                {
                    //������
                    Debug.Log("������");
                }
            }

            // ͬ����ҽ��
            SyncPlayerCredit();

            // �Ѽ���
            _consoleBB.Instance.isMachineActive = true;

            //�ص���������
            CommonPopupHandler.Instance.ClosePopup(MARK_IS_POPUP_CHECK_ACTIVE);

            // ���¼ӡ����»�ȡ����
            MachineDataManager.Instance.RequestReadConf((data) =>
            {
                SBoxConfData res = (SBoxConfData)data;
                _consoleBB.Instance.sboxConfData = res;
                isNext = true;
            }, (BagelCodeError err) =>
            {
                DebugUtils.LogError(err.msg);
                isNext = true;
            });


            yield return new WaitUntil(() => isNext == true);
            isNext = false;


            if (_consoleBB.Instance.betList.Count >0)
            {
                //20012 ʧ��Ҫ�ط�
                BlackboardUtils.SetValue<int>("./betIndex", 0);
                long totalBet = _consoleBB.Instance.betList[0];
                BlackboardUtils.SetValue<long>("./totalBet", totalBet);

                bool isBreak = false;
                do 
                {
                    Debug.LogWarning($"��Test�� ����ѹע�� {totalBet}");
                    MachineDataManager.Instance.RequestSetPlayerBets(0, totalBet, (res) =>
                    {
                        int result = (int)res;

                        if (result ==0)
                        {
                            isBreak = true;
                        }
                        else
                        {
                            DebugUtils.LogError($"set total bet for machine is err :{result}");
                        }
   
                        isNext = true;
                    });

                    yield return new WaitUntil(() => isNext == true);
                    isNext = false;

                } while (!isBreak);
            }

            MaskPopupHandler.Instance.ClosePopup();

            // ֪ͨ���»�ȡ�ʽ�ֵ
            EventCenter.Instance.EventTrigger<EventData>(GlobalEvent.ON_DEVICE_EVENT, new EventData(GlobalEvent.CodeCompleted));
        }

        // ��ʱ�򿪣���
        TipPopupHandler.Instance.OpenPopup(I18nMgr.T(isSuccess ? "Coding activation successful" : "Coding activation failed"));
    }





    void SyncPlayerCredit()
    {
        // ͬ����ҽ��
        MachineDataManager.Instance.RequestGetPlayerInfo((res) =>
        {
            SBoxAccount data = (SBoxAccount)res;
            int pid = _consoleBB.Instance.pid;
            List<SBoxPlayerAccount> playerAccountList = data.PlayerAccountList;
            for (int i = 0; i < playerAccountList.Count; i++)
            {
                if (playerAccountList[i].PlayerId == pid)
                {
                    _consoleBB.Instance.sboxPlayerInfo = playerAccountList[i];

                    MainBlackboardController.Instance.SyncMyTempCreditToReal(true); //ͬ����ҽ��
                    return;
                }
            }
        }, (BagelCodeError err) =>
        {
            DebugUtils.Log(err.msg);
        });
    }


}
