using Newtonsoft.Json;
using SBoxApi;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameMaker
{
    public class ResponseInfo
    {
        public int seqID;
        public Action<object> successCallback;

        /// <summary>
        /// ʧ�ܻص�
        /// </summary>
        /// <remark>
        /// * ʧ�ܺ��������ǽӿ��Զ����ʧ�����ݡ�
        /// * ��ʱ���ظ�����Ҳ�����ʧ�ܺ�����
        /// </remark>
        public Action<BagelCodeError> errorCallback;
        public long time;

        /// <summary> ʹ�ñ�ǩ��ɾ������ </summary>
        public string mark;
    }


    public class SeverHelper
    {
        /// <summary> ���󷽷�  </summary>
        public Action<string, object> requestFunc;

        /// <summary>���û�յ����ݶϿ�ms</summary>
        public  int receiveOvertimeMS = -1; //   120 * 1000;


        public bool isDebug = false;
        /*
         //������GameObject ʹ����Э�̣�����ʱ���٣�
         requestFunc(string rpcName, object req){
            Delay(()=>{

            },10)
        }
         */

        public string prefix = "";

        Dictionary<string, ResponseInfo> dicResponse = new Dictionary<string, ResponseInfo>();
        private int seqID = 0;
        private int CreatSeqID()
        {
            List<int> temp = new List<int>();
            foreach (KeyValuePair<string, ResponseInfo> kv in this.dicResponse)
                temp.Add(kv.Value.seqID);
            do
            {
                if (++this.seqID > 10000)
                    this.seqID = 1;
            } while (temp.Contains(seqID));
            return seqID;
        }

        void OnDebugRpcDown(string eventName, object res)
        {
            //if (eventName != SBoxEventHandle.SBOX_SLOT_SPIN && eventName != SBoxEventHandle.SBOX_SET_PLAYER_BETS)  return;

            //if (eventName != MachineDataManager.RpcNameGameJackpot && eventName != SBoxEventHandle.SBOX_SLOT_SPIN) return;


            if (isDebug)
                DebugUtils.LogWarning($"==@ {prefix}<color=yellow>rpc down</color>: {eventName}  data: {JsonConvert.SerializeObject(res)}");
                //DebugUtil.LogWarning($"==@ {mark}<color=yellow>rpc down</color>: {eventName}  data: {JSONNodeUtil.ObjectToJsonStr(res)}");

            //JSONNodeUtil.ObjectToJsonStr ��int[] = [1,2,3] ֱ�ӱ�� { }  �����⣡��
        }
        void OnDebugRpcUp(string eventName, object req)
        {
            //if (eventName != SBoxEventHandle.SBOX_SLOT_SPIN/ && eventName != SBoxEventHandle.SBOX_SET_PLAYER_BETS) return;


            //if (eventName != MachineDataManager.RpcNameGameJackpot && eventName != SBoxEventHandle.SBOX_SLOT_SPIN) return;


            if (isDebug)
                DebugUtils.LogWarning($"==@ {prefix}<color=green>rpc up</color>: {eventName}  data: {JsonConvert.SerializeObject(req)}");
                //DebugUtil.LogWarning($"==@ {mark}<color=green>rpc up</color>: {eventName}  data: {JSONNodeUtil.ObjectToJsonStr(req)}");
        }

        public void OnSuccessResponseData(string eventName, object res)
        {

            OnDebugRpcDown(eventName, res);

            if (dicResponse.ContainsKey(eventName))
            {
                ResponseInfo info = dicResponse[eventName];
                dicResponse.Remove(eventName);
                info.successCallback?.Invoke(res);
            }
        }

        public void OnErrorResponseData(string eventName, object res)
        {
            OnDebugRpcDown(eventName, res);

            if (dicResponse.ContainsKey(eventName))
            {
                ResponseInfo info = dicResponse[eventName];
                dicResponse.Remove(eventName);
                
                info.errorCallback?.Invoke(new BagelCodeError{ response = res});
            }
        }

        public void OnResponsData(string eventName, object res, bool isErr)
        {
            if (isErr)
                OnErrorResponseData(eventName, res);
            else
                OnSuccessResponseData(eventName, res);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="req"></param>
        /// <param name="successCallback"></param>
        /// <param name="errorCallback"></param>
        /// <returns></returns>
        public int RequestData(string eventName, object req, Action<object> successCallback, Action<BagelCodeError> errorCallback,string mark = null)
        {
            OnDebugRpcUp(eventName, req);

            int id = CreatSeqID();

            if (!dicResponse.ContainsKey(eventName))
                dicResponse.Add(eventName, new ResponseInfo() { });
            else
            { //���е�ɾ��
              //int seqID = dicResponse[eventName].seqID;
                BagelCodeError res = new BagelCodeError(){msg = "Request is repeated",};
                DebugUtils.LogWarning($"==@ {prefix}: {eventName} ; Request is repeated");
                dicResponse[eventName].errorCallback?.Invoke(res);
            }
            dicResponse[eventName].successCallback = successCallback;
            dicResponse[eventName].errorCallback = errorCallback;
            dicResponse[eventName].seqID = id;
            dicResponse[eventName].time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            dicResponse[eventName].mark = mark;

            //��ʱ������ʱ�ò���id
            taskQueue.Enqueue(() => requestFunc.Invoke(eventName, req));
            return id;
        }

        public void RemoveRequestAt(int seqID)
        {
            int idx = dicResponse.Count;
            while (--idx >= 0)
            {
                KeyValuePair<string, ResponseInfo> item = dicResponse.ElementAt(idx);
                if (item.Value.seqID == seqID)
                {
                    dicResponse.Remove(item.Key);
                }
            }
        }

        public void RemoveRequestAt(string mark)
        {
            int idx = dicResponse.Count;
            while (--idx >= 0)
            {
                KeyValuePair<string, ResponseInfo> item = dicResponse.ElementAt(idx);
                if (item.Value.mark == mark)
                {
                    dicResponse.Remove(item.Key);
                }
            }
        }


        private bool isDirty = true;
        private Queue<Action> taskQueue = new Queue<Action>();

        /// <summary> ��Ϸ����ʱ�� </summary>
        float lastRunTimeS = 0;
        public void Update()
        {
            float nowRunTimeS = Time.unscaledTime;
            if (nowRunTimeS - lastRunTimeS > 0.05f)
            {
                lastRunTimeS = nowRunTimeS;
                if (isDirty)
                {
                    isDirty = false;
                    while (taskQueue.Count > 0)
                    {
                        var task = taskQueue.Dequeue();
                        task.Invoke();
                    }
                    isDirty = true;
                }
            }

            CheckRequestOvertime();
        }

        public void CheckRequestOvertime()
        {
            if (receiveOvertimeMS == -1)
                return;

            long nowTimeMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            int idx = dicResponse.Count;
            while (--idx >= 0)
            {
                KeyValuePair<string, ResponseInfo> item = dicResponse.ElementAt(idx);
                if (nowTimeMs - item.Value.time > receiveOvertimeMS)
                {
                    dicResponse.Remove(item.Key);
                    BagelCodeError res = new BagelCodeError(){msg = "Request is overtime",};
                    DebugUtils.LogWarning($"==@ {prefix}: {item.Key} ; Request is overtime ; {receiveOvertimeMS}ms");
                    item.Value.errorCallback?.Invoke(res);
                }
            }
        }
    }



}





