using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSRedis;
using System;
using System.IO;
using GameMaker;
using Newtonsoft.Json;
using System.Timers;
using PssOn00152;
namespace cryRedis
{
    public class SubTicketData
    {
        //public int id;
        public int eventId;
        public string data;
    }
    public class RedisMgr  // :  MonoSingleton<RedisMgr>
    {

        static RedisMgr instance;
        public static RedisMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RedisMgr();
                }
                return instance;
            }
        }


        public string Host = "127.0.0.1";  //redis��ip��ַ
        public int Port = 6379;            //redis�Ķ˿�
        public string PassWord = "123456"; //redis������
        public int Db = 0;  //ʹ���ĸ�db
        public int PoolSize = 50; //�ش�С
        public string Prefix = string.Empty; //key��ǰ׺

        Action<int, string> rpcDownHandler;
        // Start is called before the first frame update

        Action<CSRedisClient.PSubscribePMessageEventArgs> _rpcDownHandler;

        public void Init(string host, int port, string passWord , int db, int poolSize , string prefix , Action<int, string> rpcDownHandler)
        {

            this.Host = host;
            this.Port = port;
            this.PassWord = passWord;
            this.Db = db;
            this.PoolSize = poolSize;
            this.Prefix = prefix;

            this._rpcDownHandler = (res) =>
            {
                Debug.LogWarning($"��Sas Redis��PSubscribe down  res��{JsonConvert.SerializeObject(res)}");
                SubTicketData rspData = JsonConvert.DeserializeObject<SubTicketData>(res.Body);
                if(rspData != null)
                {
                    // ��ʱ���� SasCommand.Instance.rpcDownHandler(rspData.eventId, rspData.data);
                    rpcDownHandler?.Invoke(rspData.eventId, rspData.data);
                }
            };

            isEnableReconnect = true;
            Connect();
        }

        bool _isConnectRedis = false;

        public bool isConnectRedis => _isConnectRedis;
        void Connect()
        {
            try
            {
                _Close();

                Debug.LogWarning($"��Sas Redis�� Connet host{Host} port: {Port} passWord: {PassWord}");

                string connectionString = string.Format("{0}:{1},password={2},defaultDatabase={3},poolsize={4},prefix={5}", Host, Port, PassWord, Db, PoolSize, Prefix);
                csredis = new CSRedisClient(connectionString);
                RedisHelper.Initialization(csredis);
                _isConnectRedis = RedisHelper.Ping();

                if (!_isConnectRedis)
                {
                    Debug.LogWarning("��Sas Redis��cannot connect redis");
                }
                else
                {
                    Debug.LogWarning("��Sas Redis��redis connect sucess ");
                    CheckConnect();

                    //if(objPSubscribe != null) objPSubscribe.PUnsubscribe();
                    objPSubscribe = PSubscribe(new string[] { SasConstant.SAS_SUB_TICKET }, _rpcDownHandler);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                _isConnectRedis = false;
            }
            finally
            {
                if(_isConnectRedis == false && isEnableReconnect)
                {
                    Reconnect();
                }
            };
        }

        /// <summary>
        /// ���Ķ���
        /// </summary>
        CSRedisClient.PSubscribeObject objPSubscribe;

        CSRedisClient csredis;

        //��ʼ��redis
        /*public bool InitRedis()
        {
            string connectionString = string.Format("{0}:{1},password={2},defaultDatabase={3},poolsize={4},prefix={5}", Host, Port, PassWord,Db,PoolSize,Prefix);
            csredis = new CSRedisClient(connectionString);
            RedisHelper.Initialization(csredis);
            return RedisHelper.Ping();
        }*/


        void _Close()
        {
            _isConnectRedis = false;

            if (csredis != null)
                csredis.Dispose();
            csredis = null;

            if (reconnectTimer != null)
                reconnectTimer.Stop();
            reconnectTimer = null;

            if (checkConnectTimer != null)
                checkConnectTimer.Stop();
            checkConnectTimer = null;

            if (objPSubscribe != null)
                objPSubscribe.PUnsubscribe();
            objPSubscribe = null;

        }
        public void Close()
        {
            isEnableReconnect = false;

            _Close();
        }


       
        /// <summary>
        /// ����
        /// </summary>
        public void Reconnect()
        {

            Debug.LogWarning($"��Sas Redis��Reconnect...");

            if (reconnectTimer != null)
                reconnectTimer.Stop();

            double ms = 5000;
            reconnectTimer = new System.Timers.Timer(ms);
            reconnectTimer.AutoReset = false; // �Ƿ��ظ�ִ��
            reconnectTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                Connect();
            };
            //reconnectTimer.Enabled = true; //��ʼִ��
            reconnectTimer.Start();
        }
        bool isEnableReconnect = true;
        System.Timers.Timer reconnectTimer;

        /// <summary>
        /// ������
        /// </summary>
        public void CheckConnect()
        {

            if (checkConnectTimer != null)
                checkConnectTimer.Stop();


            double ms = 10000;
            checkConnectTimer = new System.Timers.Timer(ms);
            checkConnectTimer.AutoReset = true; // �Ƿ��ظ�ִ��
            checkConnectTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {

                try
                {
                    _isConnectRedis = RedisHelper.Ping();  // ֱ�ӿ�������
                }
                catch (Exception ex)
                {
                    _isConnectRedis = false;
                }
                finally
                {
                    Debug.LogWarning($"��Sas Redis��Check Connect... {_isConnectRedis}");
                    if (_isConnectRedis == false && isEnableReconnect)
                    {
                        Reconnect();
                    }
                }
            };
            //checkConnectTimer.Enabled = true; //��ʼִ��
            checkConnectTimer.Start();
        }
        System.Timers.Timer checkConnectTimer;




        //���÷���
        #region Pub/Sub
        /// <summary>
        /// ���ڽ���Ϣ���͵�ָ�������ڵ��Ƶ����������Ϣ������ʽ��1|message
        /// </summary>
        /// <param name="channel">Ƶ����</param>
        /// <param name="message">��Ϣ�ı�</param>
        /// <returns></returns>
        public long Publish(string channel, string message) => RedisHelper.Publish(channel, message);
        /// <summary>
        /// ���ڽ���Ϣ���͵�ָ�������ڵ��Ƶ������ Publish ������ͬ����������Ϣidͷ���� 1|
        /// </summary>
        /// <param name="channel">Ƶ����</param>
        /// <param name="message">��Ϣ�ı�</param>
        /// <returns></returns>
        public long PublishNoneMessageId(string channel, string message) => RedisHelper.PublishNoneMessageId(channel, message);
        /// <summary>
        /// �鿴���ж���Ƶ��
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string[] PubSubChannels(string pattern) => RedisHelper.PubSubChannels(pattern);
        /// <summary>
        /// �鿴����ģ�����Ķ˵�����<para></para>
        /// ע�⣺����ģʽ�£������ͻ��˵Ķ��Ŀ��ܲ��᷵��
        /// </summary>
        /// <returns></returns>
        public long PubSubNumPat() => RedisHelper.PubSubNumPat();
        /// <summary>
        /// �鿴���ж��Ķ˵�����<para></para>
        /// ע�⣺����ģʽ�£������ͻ��˵Ķ��Ŀ��ܲ��᷵��
        /// </summary>
        /// <param name="channels">Ƶ��</param>
        /// <returns></returns>
        public Dictionary<string, long> PubSubNumSub(params string[] channels) => RedisHelper.PubSubNumSub(channels);
        /// <summary>
        /// ���ģ����ݷ������򷵻�SubscribeObject��Subscribe(("chan1", msg => Console.WriteLine(msg.Body)), ("chan2", msg => Console.WriteLine(msg.Body)))
        /// </summary>
        /// <param name="channels">Ƶ���ͽ�����</param>
        /// <returns>���ؿ�ֹͣ���ĵĶ���</returns>
        public CSRedisClient.SubscribeObject Subscribe(params (string, Action<CSRedisClient.SubscribeMessageEventArgs>)[] channels) => RedisHelper.Subscribe(channels);
        /// <summary>
        /// ģ�����ģ��������з����ڵ�(ͬ����Ϣֻ����һ�Σ�������SubscribeObject��PSubscribe(new [] { "chan1*", "chan2*" }, msg => Console.WriteLine(msg.Body))
        /// </summary>
        /// <param name="channelPatterns">ģ��Ƶ��</param>
        /// <param name="pmessage">������</param>
        /// <returns>���ؿ�ֹͣģ�����ĵĶ���</returns>
        public CSRedisClient.PSubscribeObject PSubscribe(string[] channelPatterns, Action<CSRedisClient.PSubscribePMessageEventArgs> pmessage) => RedisHelper.PSubscribe(channelPatterns, pmessage);
        #endregion


        #region Sorted Set
        /// <summary>
        /// �����򼯺����һ��������Ա�����߸����Ѵ��ڳ�Ա�ķ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="scoreMembers">һ��������Ա����</param>
        /// <returns></returns>
        public long ZAdd(string key, params (decimal, object)[] scoreMembers) => RedisHelper.ZAdd(key, scoreMembers);
        /// <summary>
        /// ��ȡ���򼯺ϵĳ�Ա����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public long ZCard(string key) => RedisHelper.ZCard(key);
        /// <summary>
        /// ���������򼯺���ָ����������ĳ�Ա����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ decimal.MinValue 1</param>
        /// <param name="max">�������ֵ decimal.MaxValue 10</param>
        /// <returns></returns>
        public long ZCount(string key, decimal min, decimal max) => RedisHelper.ZCount(key, min, max);
        /// <summary>
        /// ���������򼯺���ָ����������ĳ�Ա����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ -inf (1 1</param>
        /// <param name="max">�������ֵ +inf (10 10</param>
        /// <returns></returns>
        public long ZCount(string key, string min, string max) => RedisHelper.ZCount(key, min, max);
        /// <summary>
        /// ���򼯺��ж�ָ����Ա�ķ����������� increment
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <param name="increment">����ֵ(Ĭ��=1)</param>
        /// <returns></returns>
        public decimal ZIncrBy(string key, string member, decimal increment = 1) => RedisHelper.ZIncrBy(key, member, increment);

        /// <summary>
        /// ���������һ���������򼯵Ľ�������������洢���µ����򼯺� destination ��
        /// </summary>
        /// <param name="destination">�µ����򼯺ϣ�����prefixǰ�</param>
        /// <param name="weights">ʹ�� WEIGHTS ѡ������Ϊ ÿ�� �������� �ֱ� ָ��һ���˷����ӡ����û��ָ�� WEIGHTS ѡ��˷�����Ĭ������Ϊ 1 ��</param>
        /// <param name="aggregate">Sum | Min | Max</param>
        /// <param name="keys">һ���������򼯺ϣ�����prefixǰ�</param>
        /// <returns></returns>
        public long ZInterStore(string destination, decimal[] weights, RedisAggregate aggregate, params string[] keys) => RedisHelper.ZInterStore(destination, weights, aggregate, keys);

        /// <summary>
        /// ͨ���������䷵�����򼯺ϳ�ָ�������ڵĳ�Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public string[] ZRange(string key, long start, long stop) => RedisHelper.ZRange(key, start, stop);
        /// <summary>
        /// ͨ���������䷵�����򼯺ϳ�ָ�������ڵĳ�Ա
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public T[] ZRange<T>(string key, long start, long stop) => RedisHelper.ZRange<T>(key, start, stop);
        /// <summary>
        /// ͨ���������䷵�����򼯺ϳ�ָ�������ڵĳ�Ա�ͷ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRangeWithScores(string key, long start, long stop) => RedisHelper.ZRangeWithScores(key, start, stop);
        /// <summary>
        /// ͨ���������䷵�����򼯺ϳ�ָ�������ڵĳ�Ա�ͷ���
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRangeWithScores<T>(string key, long start, long stop) => RedisHelper.ZRangeWithScores<T>(key, start, stop);

        /// <summary>
        /// ͨ�������������򼯺�ָ�������ڵĳ�Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ decimal.MinValue 1</param>
        /// <param name="max">�������ֵ decimal.MaxValue 10</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public string[] ZRangeByScore(string key, decimal min, decimal max, long? limit = null, long offset = 0) =>
            RedisHelper.ZRangeByScore(key, min, max, limit, offset);
        /// <summary>
        /// ͨ�������������򼯺�ָ�������ڵĳ�Ա
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ decimal.MinValue 1</param>
        /// <param name="max">�������ֵ decimal.MaxValue 10</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public T[] ZRangeByScore<T>(string key, decimal min, decimal max, long? limit = null, long offset = 0) =>
            RedisHelper.ZRangeByScore<T>(key, min, max, limit, offset);
        /// <summary>
        /// ͨ�������������򼯺�ָ�������ڵĳ�Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ -inf (1 1</param>
        /// <param name="max">�������ֵ +inf (10 10</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public string[] ZRangeByScore(string key, string min, string max, long? limit = null, long offset = 0) =>
            RedisHelper.ZRangeByScore(key, min, max, limit, offset);
        /// <summary>
        /// ͨ�������������򼯺�ָ�������ڵĳ�Ա
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ -inf (1 1</param>
        /// <param name="max">�������ֵ +inf (10 10</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public T[] ZRangeByScore<T>(string key, string min, string max, long? limit = null, long offset = 0) =>
            RedisHelper.ZRangeByScore<T>(key, min, max, limit, offset);

        /// <summary>
        /// ͨ�������������򼯺�ָ�������ڵĳ�Ա�ͷ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ decimal.MinValue 1</param>
        /// <param name="max">�������ֵ decimal.MaxValue 10</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRangeByScoreWithScores(string key, decimal min, decimal max, long? limit = null, long offset = 0) =>
            RedisHelper.ZRangeByScoreWithScores(key, min, max, limit, offset);
        /// <summary>
        /// ͨ�������������򼯺�ָ�������ڵĳ�Ա�ͷ���
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ decimal.MinValue 1</param>
        /// <param name="max">�������ֵ decimal.MaxValue 10</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRangeByScoreWithScores<T>(string key, decimal min, decimal max, long? limit = null, long offset = 0) =>
            RedisHelper.ZRangeByScoreWithScores<T>(key, min, max, limit, offset);
        /// <summary>
        /// ͨ�������������򼯺�ָ�������ڵĳ�Ա�ͷ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ -inf (1 1</param>
        /// <param name="max">�������ֵ +inf (10 10</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRangeByScoreWithScores(string key, string min, string max, long? limit = null, long offset = 0) =>
            RedisHelper.ZRangeByScoreWithScores(key, min, max, limit, offset);
        /// <summary>
        /// ͨ�������������򼯺�ָ�������ڵĳ�Ա�ͷ���
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ -inf (1 1</param>
        /// <param name="max">�������ֵ +inf (10 10</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRangeByScoreWithScores<T>(string key, string min, string max, long? limit = null, long offset = 0) =>
               RedisHelper.ZRangeByScoreWithScores<T>(key, min, max, limit, offset);

        /// <summary>
        /// �������򼯺���ָ����Ա������
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <returns></returns>
        public long? ZRank(string key, object member) => RedisHelper.ZRank(key, member);
        /// <summary>
        /// �Ƴ����򼯺��е�һ��������Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">һ��������Ա</param>
        /// <returns></returns>
        public long ZRem<T>(string key, params T[] member) => RedisHelper.ZRem(key, member);
        /// <summary>
        /// �Ƴ����򼯺��и�����������������г�Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public long ZRemRangeByRank(string key, long start, long stop) => RedisHelper.ZRemRangeByRank(key, start, stop);
        /// <summary>
        /// �Ƴ����򼯺��и����ķ�����������г�Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ decimal.MinValue 1</param>
        /// <param name="max">�������ֵ decimal.MaxValue 10</param>
        /// <returns></returns>
        public long ZRemRangeByScore(string key, decimal min, decimal max) => RedisHelper.ZRemRangeByScore(key, min, max);
        /// <summary>
        /// �Ƴ����򼯺��и����ķ�����������г�Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">������Сֵ -inf (1 1</param>
        /// <param name="max">�������ֵ +inf (10 10</param>
        /// <returns></returns>
        public long ZRemRangeByScore(string key, string min, string max) => RedisHelper.ZRemRangeByScore(key, min, max);

        /// <summary>
        /// ����������ָ�������ڵĳ�Ա��ͨ�������������Ӹߵ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public string[] ZRevRange(string key, long start, long stop) => RedisHelper.ZRevRange(key, start, stop);
        /// <summary>
        /// ����������ָ�������ڵĳ�Ա��ͨ�������������Ӹߵ���
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public T[] ZRevRange<T>(string key, long start, long stop) => RedisHelper.ZRevRange<T>(key, start, stop);
        /// <summary>
        /// ����������ָ�������ڵĳ�Ա�ͷ�����ͨ�������������Ӹߵ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRevRangeWithScores(string key, long start, long stop) => RedisHelper.ZRevRangeWithScores(key, start, stop);
        /// <summary>
        /// ����������ָ�������ڵĳ�Ա�ͷ�����ͨ�������������Ӹߵ���
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRevRangeWithScores<T>(string key, long start, long stop) => RedisHelper.ZRevRangeWithScores<T>(key, start, stop);

        /// <summary>
        /// ����������ָ�����������ڵĳ�Ա�������Ӹߵ�������
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="max">�������ֵ decimal.MaxValue 10</param>
        /// <param name="min">������Сֵ decimal.MinValue 1</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public string[] ZRevRangeByScore(string key, decimal max, decimal min, long? limit = null, long? offset = 0) => RedisHelper.ZRevRangeByScore(key, max, min, limit, offset);
        /// <summary>
        /// ����������ָ�����������ڵĳ�Ա�������Ӹߵ�������
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="max">�������ֵ decimal.MaxValue 10</param>
        /// <param name="min">������Сֵ decimal.MinValue 1</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public T[] ZRevRangeByScore<T>(string key, decimal max, decimal min, long? limit = null, long offset = 0) =>
            RedisHelper.ZRevRangeByScore<T>(key, max, min, limit, offset);
        /// <summary>
        /// ����������ָ�����������ڵĳ�Ա�������Ӹߵ�������
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="max">�������ֵ +inf (10 10</param>
        /// <param name="min">������Сֵ -inf (1 1</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public string[] ZRevRangeByScore(string key, string max, string min, long? limit = null, long? offset = 0) => RedisHelper.ZRevRangeByScore(key, max, min, limit, offset);
        /// <summary>
        /// ����������ָ�����������ڵĳ�Ա�������Ӹߵ�������
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="max">�������ֵ +inf (10 10</param>
        /// <param name="min">������Сֵ -inf (1 1</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public T[] ZRevRangeByScore<T>(string key, string max, string min, long? limit = null, long offset = 0) =>
               RedisHelper.ZRevRangeByScore<T>(key, max, min, limit, offset);

        /// <summary>
        /// ����������ָ�����������ڵĳ�Ա�ͷ����������Ӹߵ�������
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="max">�������ֵ decimal.MaxValue 10</param>
        /// <param name="min">������Сֵ decimal.MinValue 1</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRevRangeByScoreWithScores(string key, decimal max, decimal min, long? limit = null, long offset = 0) =>
             RedisHelper.ZRevRangeByScoreWithScores(key, max, min, limit, offset);
        /// <summary>
        /// ����������ָ�����������ڵĳ�Ա�ͷ����������Ӹߵ�������
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="max">�������ֵ decimal.MaxValue 10</param>
        /// <param name="min">������Сֵ decimal.MinValue 1</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRevRangeByScoreWithScores<T>(string key, decimal max, decimal min, long? limit = null, long offset = 0) =>
               RedisHelper.ZRevRangeByScoreWithScores<T>(key, max, min, limit, offset);
        /// <summary>
        /// ����������ָ�����������ڵĳ�Ա�ͷ����������Ӹߵ�������
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="max">�������ֵ +inf (10 10</param>
        /// <param name="min">������Сֵ -inf (1 1</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRevRangeByScoreWithScores(string key, string max, string min, long? limit = null, long offset = 0) =>
               RedisHelper.ZRevRangeByScoreWithScores(key, max, min, limit, offset);
        /// <summary>
        /// ����������ָ�����������ڵĳ�Ա�ͷ����������Ӹߵ�������
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="max">�������ֵ +inf (10 10</param>
        /// <param name="min">������Сֵ -inf (1 1</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRevRangeByScoreWithScores<T>(string key, string max, string min, long? limit = null, long offset = 0) =>
            RedisHelper.ZRevRangeByScoreWithScores<T>(key, max, min, limit, offset);

        /// <summary>
        /// �������򼯺���ָ����Ա�����������򼯳�Ա������ֵ�ݼ�(�Ӵ�С)����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <returns></returns>
        public long? ZRevRank(string key, object member) => RedisHelper.ZRevRank(key, member);
        /// <summary>
        /// ���������У���Ա�ķ���ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <returns></returns>
        public decimal? ZScore(string key, object member) => RedisHelper.ZScore(key, member);

        /// <summary>
        /// ���������һ���������򼯵Ĳ�������������洢���µ����򼯺� destination ��
        /// </summary>
        /// <param name="destination">�µ����򼯺ϣ�����prefixǰ�</param>
        /// <param name="weights">ʹ�� WEIGHTS ѡ������Ϊ ÿ�� �������� �ֱ� ָ��һ���˷����ӡ����û��ָ�� WEIGHTS ѡ��˷�����Ĭ������Ϊ 1 ��</param>
        /// <param name="aggregate">Sum | Min | Max</param>
        /// <param name="keys">һ���������򼯺ϣ�����prefixǰ�</param>
        /// <returns></returns>
        public long ZUnionStore(string destination, decimal[] weights, RedisAggregate aggregate, params string[] keys) => RedisHelper.ZUnionStore(destination, weights, aggregate, keys);

        /// <summary>
        /// �������򼯺��е�Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="cursor">λ��</param>
        /// <param name="pattern">ģʽ</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public RedisScan<(string member, decimal score)> ZScan(string key, long cursor, string pattern = null, long? count = null) =>
               RedisHelper.ZScan(key, cursor, pattern, count);
        /// <summary>
        /// �������򼯺��е�Ԫ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="cursor">λ��</param>
        /// <param name="pattern">ģʽ</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public RedisScan<(T member, decimal score)> ZScan<T>(string key, long cursor, string pattern = null, long? count = null) =>
               RedisHelper.ZScan<T>(key, cursor, pattern, count);

        /// <summary>
        /// �����򼯺ϵ����г�Ա��������ͬ�ķ�ֵʱ�����򼯺ϵ�Ԫ�ػ���ݳ�Ա���ֵ����������������������Է��ظ��������򼯺ϼ� key �У�ֵ���� min �� max ֮��ĳ�Ա��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">'(' ��ʾ�����ڷ�Χ��'[' ��ʾ�������ڷ�Χ��'+' �������'-' �����ޡ� ZRANGEBYLEX zset - + ������������򼯺��е�����Ԫ��</param>
        /// <param name="max">'(' ��ʾ�����ڷ�Χ��'[' ��ʾ�������ڷ�Χ��'+' �������'-' �����ޡ� ZRANGEBYLEX zset - + ������������򼯺��е�����Ԫ��</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public string[] ZRangeByLex(string key, string min, string max, long? limit = null, long offset = 0) =>
               RedisHelper.ZRangeByLex(key, min, max, limit, offset);
        /// <summary>
        /// �����򼯺ϵ����г�Ա��������ͬ�ķ�ֵʱ�����򼯺ϵ�Ԫ�ػ���ݳ�Ա���ֵ����������������������Է��ظ��������򼯺ϼ� key �У�ֵ���� min �� max ֮��ĳ�Ա��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">'(' ��ʾ�����ڷ�Χ��'[' ��ʾ�������ڷ�Χ��'+' �������'-' �����ޡ� ZRANGEBYLEX zset - + ������������򼯺��е�����Ԫ��</param>
        /// <param name="max">'(' ��ʾ�����ڷ�Χ��'[' ��ʾ�������ڷ�Χ��'+' �������'-' �����ޡ� ZRANGEBYLEX zset - + ������������򼯺��е�����Ԫ��</param>
        /// <param name="limit">���ض��ٳ�Ա</param>
        /// <param name="offset">��������ƫ��λ��</param>
        /// <returns></returns>
        public T[] ZRangeByLex<T>(string key, string min, string max, long? limit = null, long offset = 0) =>
            RedisHelper.ZRangeByLex<T>(key, min, max, limit, offset);

        /// <summary>
        /// �����򼯺ϵ����г�Ա��������ͬ�ķ�ֵʱ�����򼯺ϵ�Ԫ�ػ���ݳ�Ա���ֵ����������������������Է��ظ��������򼯺ϼ� key �У�ֵ���� min �� max ֮��ĳ�Ա��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">'(' ��ʾ�����ڷ�Χ��'[' ��ʾ�������ڷ�Χ��'+' �������'-' �����ޡ� ZRANGEBYLEX zset - + ������������򼯺��е�����Ԫ��</param>
        /// <param name="max">'(' ��ʾ�����ڷ�Χ��'[' ��ʾ�������ڷ�Χ��'+' �������'-' �����ޡ� ZRANGEBYLEX zset - + ������������򼯺��е�����Ԫ��</param>
        /// <returns></returns>
        public long ZRemRangeByLex(string key, string min, string max) =>
               RedisHelper.ZRemRangeByLex(key, min, max);
        /// <summary>
        /// �����򼯺ϵ����г�Ա��������ͬ�ķ�ֵʱ�����򼯺ϵ�Ԫ�ػ���ݳ�Ա���ֵ����������������������Է��ظ��������򼯺ϼ� key �У�ֵ���� min �� max ֮��ĳ�Ա��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="min">'(' ��ʾ�����ڷ�Χ��'[' ��ʾ�������ڷ�Χ��'+' �������'-' �����ޡ� ZRANGEBYLEX zset - + ������������򼯺��е�����Ԫ��</param>
        /// <param name="max">'(' ��ʾ�����ڷ�Χ��'[' ��ʾ�������ڷ�Χ��'+' �������'-' �����ޡ� ZRANGEBYLEX zset - + ������������򼯺��е�����Ԫ��</param>
        /// <returns></returns>
        public long ZLexCount(string key, string min, string max) =>
               RedisHelper.ZLexCount(key, min, max);

        /// <summary>
        /// [redis-server 5.0.0] ɾ�����������򼯺�key�е����count��������ߵ÷ֵĳ�Ա����δָ����count��Ĭ��ֵΪ1��ָ��һ���������򼯺ϵĻ�����count����������� �����ض��Ԫ��ʱ�򣬵÷���ߵ�Ԫ�ؽ��ǵ�һ��Ԫ�أ�Ȼ���Ƿ����ϵ͵�Ԫ�ء�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZPopMax(string key, long count) =>
            RedisHelper.ZPopMax(key, count);

        /// <summary>
        /// [redis-server 5.0.0] ɾ�����������򼯺�key�е����count��������ߵ÷ֵĳ�Ա����δָ����count��Ĭ��ֵΪ1��ָ��һ���������򼯺ϵĻ�����count����������� �����ض��Ԫ��ʱ�򣬵÷���ߵ�Ԫ�ؽ��ǵ�һ��Ԫ�أ�Ȼ���Ƿ����ϵ͵�Ԫ�ء�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZPopMax<T>(string key, long count) =>
            RedisHelper.ZPopMax<T>(key, count);

        /// <summary>
        /// [redis-server 5.0.0] ɾ�����������򼯺�key�е����count��������͵÷ֵĳ�Ա����δָ����count��Ĭ��ֵΪ1��ָ��һ���������򼯺ϵĻ�����count����������� �����ض��Ԫ��ʱ�򣬵÷���͵�Ԫ�ؽ��ǵ�һ��Ԫ�أ�Ȼ���Ƿ����ϸߵ�Ԫ�ء�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZPopMin(string key, long count) =>
            RedisHelper.ZPopMin(key, count);

        /// <summary>
        /// [redis-server 5.0.0] ɾ�����������򼯺�key�е����count��������͵÷ֵĳ�Ա����δָ����count��Ĭ��ֵΪ1��ָ��һ���������򼯺ϵĻ�����count����������� �����ض��Ԫ��ʱ�򣬵÷���͵�Ԫ�ؽ��ǵ�һ��Ԫ�أ�Ȼ���Ƿ����ϸߵ�Ԫ�ء�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZPopMin<T>(string key, long count) =>
            RedisHelper.ZPopMin<T>(key, count);
        #endregion

        #region Set
        /// <summary>
        /// �򼯺����һ��������Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="members">һ��������Ա</param>
        /// <returns></returns>
        public long SAdd<T>(string key, params T[] members) => RedisHelper.SAdd(key, members);
        /// <summary>
        /// ��ȡ���ϵĳ�Ա��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public long SCard(string key) => RedisHelper.SCard(key);
        /// <summary>
        /// ���ظ������м��ϵĲ
        /// </summary>
        /// <param name="keys">����prefixǰ�</param>
        /// <returns></returns>
        public string[] SDiff(params string[] keys) => RedisHelper.SDiff(keys);
        /// <summary>
        /// ���ظ������м��ϵĲ
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="keys">����prefixǰ�</param>
        /// <returns></returns>
        public T[] SDiff<T>(params string[] keys) => RedisHelper.SDiff<T>(keys);
        /// <summary>
        /// ���ظ������м��ϵĲ���洢�� destination ��
        /// </summary>
        /// <param name="destination">�µ����򼯺ϣ�����prefixǰ�</param>
        /// <param name="keys">һ���������򼯺ϣ�����prefixǰ�</param>
        /// <returns></returns>
        public long SDiffStore(string destination, params string[] keys) => RedisHelper.SDiffStore(destination, keys);
        /// <summary>
        /// ���ظ������м��ϵĽ���
        /// </summary>
        /// <param name="keys">����prefixǰ�</param>
        /// <returns></returns>
        public string[] SInter(params string[] keys) => RedisHelper.SInter(keys);
        /// <summary>
        /// ���ظ������м��ϵĽ���
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="keys">����prefixǰ�</param>
        /// <returns></returns>
        public T[] SInter<T>(params string[] keys) => RedisHelper.SInter<T>(keys);
        /// <summary>
        /// ���ظ������м��ϵĽ������洢�� destination ��
        /// </summary>
        /// <param name="destination">�µ����򼯺ϣ�����prefixǰ�</param>
        /// <param name="keys">һ���������򼯺ϣ�����prefixǰ�</param>
        /// <returns></returns>
        public long SInterStore(string destination, params string[] keys) => RedisHelper.SInterStore(destination, keys);
        /// <summary>
        /// �ж� member Ԫ���Ƿ��Ǽ��� key �ĳ�Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <returns></returns>
        public bool SIsMember(string key, object member) => RedisHelper.SIsMember(key, member);
        /// <summary>
        /// ���ؼ����е����г�Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public string[] SMembers(string key) => RedisHelper.SMembers(key);
        /// <summary>
        /// ���ؼ����е����г�Ա
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public T[] SMembers<T>(string key) => RedisHelper.SMembers<T>(key);
        /// <summary>
        /// �� member Ԫ�ش� source �����ƶ��� destination ����
        /// </summary>
        /// <param name="source">���򼯺�key������prefixǰ�</param>
        /// <param name="destination">Ŀ�����򼯺�key������prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <returns></returns>
        public bool SMove(string source, string destination, object member) => RedisHelper.SMove(source, destination, member);
        /// <summary>
        /// �Ƴ������ؼ����е�һ�����Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public string SPop(string key) => RedisHelper.SPop(key);
        /// <summary>
        /// �Ƴ������ؼ����е�һ�����Ԫ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public T SPop<T>(string key) => RedisHelper.SPop<T>(key);
        /// <summary>
        /// [redis-server 3.2] �Ƴ������ؼ����е�һ���������Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="count">�Ƴ������صĸ���</param>
        /// <returns></returns>
        public string[] SPop(string key, long count) => RedisHelper.SPop(key, count);
        /// <summary>
        /// [redis-server 3.2] �Ƴ������ؼ����е�һ���������Ԫ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="count">�Ƴ������صĸ���</param>
        /// <returns></returns>
        public T[] SPop<T>(string key, long count) => RedisHelper.SPop<T>(key, count);
        /// <summary>
        /// ���ؼ����е�һ�����Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public string SRandMember(string key) => RedisHelper.SRandMember(key);
        /// <summary>
        /// ���ؼ����е�һ�����Ԫ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public T SRandMember<T>(string key) => RedisHelper.SRandMember<T>(key);
        /// <summary>
        /// ���ؼ�����һ�������������Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="count">���ظ���</param>
        /// <returns></returns>
        public string[] SRandMembers(string key, int count = 1) => RedisHelper.SRandMembers(key, count);
        /// <summary>
        /// ���ؼ�����һ�������������Ԫ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="count">���ظ���</param>
        /// <returns></returns>
        public T[] SRandMembers<T>(string key, int count = 1) => RedisHelper.SRandMembers<T>(key, count);
        /// <summary>
        /// �Ƴ�������һ��������Ա
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="members">һ��������Ա</param>
        /// <returns></returns>
        public long SRem<T>(string key, params T[] members) => RedisHelper.SRem(key, members);
        /// <summary>
        /// �������и������ϵĲ���
        /// </summary>
        /// <param name="keys">����prefixǰ�</param>
        /// <returns></returns>
        public string[] SUnion(params string[] keys) => RedisHelper.SUnion(keys);
        /// <summary>
        /// �������и������ϵĲ���
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="keys">����prefixǰ�</param>
        /// <returns></returns>
        public T[] SUnion<T>(params string[] keys) => RedisHelper.SUnion<T>(keys);
        /// <summary>
        /// ���и������ϵĲ����洢�� destination ������
        /// </summary>
        /// <param name="destination">�µ����򼯺ϣ�����prefixǰ�</param>
        /// <param name="keys">һ���������򼯺ϣ�����prefixǰ�</param>
        /// <returns></returns>
        public long SUnionStore(string destination, params string[] keys) => RedisHelper.SUnionStore(destination, keys);
        /// <summary>
        /// ���������е�Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="cursor">λ��</param>
        /// <param name="pattern">ģʽ</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public RedisScan<string> SScan(string key, long cursor, string pattern = null, long? count = null) =>
            RedisHelper.SScan(key, cursor, pattern, count);
        /// <summary>
        /// ���������е�Ԫ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="cursor">λ��</param>
        /// <param name="pattern">ģʽ</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public RedisScan<T> SScan<T>(string key, long cursor, string pattern = null, long? count = null) =>
               RedisHelper.SScan<T>(key, cursor, pattern, count);
        #endregion

        #region List
        /// <summary>
        /// ���� LPOP ����������汾���������б���û���κ�Ԫ�ؿɹ�������ʱ�����ӽ��� BLPOP ����������ֱ���ȴ���ʱ���ֿɵ���Ԫ��Ϊֹ����ʱ����null
        /// </summary>
        /// <param name="timeout">��ʱ(��)</param>
        /// <param name="keys">һ�������б�����prefixǰ�</param>
        /// <returns></returns>
        public (string key, string value)? BLPopWithKey(int timeout, params string[] keys) => RedisHelper.BLPopWithKey(timeout, keys);
        /// <summary>
        /// ���� LPOP ����������汾���������б���û���κ�Ԫ�ؿɹ�������ʱ�����ӽ��� BLPOP ����������ֱ���ȴ���ʱ���ֿɵ���Ԫ��Ϊֹ����ʱ����null
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="timeout">��ʱ(��)</param>
        /// <param name="keys">һ�������б�����prefixǰ�</param>
        /// <returns></returns>
        public (string key, T value)? BLPopWithKey<T>(int timeout, params string[] keys) => RedisHelper.BLPopWithKey<T>(timeout, keys);
        /// <summary>
        /// ���� LPOP ����������汾���������б���û���κ�Ԫ�ؿɹ�������ʱ�����ӽ��� BLPOP ����������ֱ���ȴ���ʱ���ֿɵ���Ԫ��Ϊֹ����ʱ����null
        /// </summary>
        /// <param name="timeout">��ʱ(��)</param>
        /// <param name="keys">һ�������б�����prefixǰ�</param>
        /// <returns></returns>
        public string BLPop(int timeout, params string[] keys) => RedisHelper.BLPop(timeout, keys);
        /// <summary>
        /// ���� LPOP ����������汾���������б���û���κ�Ԫ�ؿɹ�������ʱ�����ӽ��� BLPOP ����������ֱ���ȴ���ʱ���ֿɵ���Ԫ��Ϊֹ����ʱ����null
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="timeout">��ʱ(��)</param>
        /// <param name="keys">һ�������б�����prefixǰ�</param>
        /// <returns></returns>
        public T BLPop<T>(int timeout, params string[] keys) => RedisHelper.BLPop<T>(timeout, keys);
        /// <summary>
        /// ���� RPOP ����������汾���������б���û���κ�Ԫ�ؿɹ�������ʱ�����ӽ��� BRPOP ����������ֱ���ȴ���ʱ���ֿɵ���Ԫ��Ϊֹ����ʱ����null
        /// </summary>
        /// <param name="timeout">��ʱ(��)</param>
        /// <param name="keys">һ�������б�����prefixǰ�</param>
        /// <returns></returns>
        public (string key, string value)? BRPopWithKey(int timeout, params string[] keys) => RedisHelper.BRPopWithKey(timeout, keys);
        /// <summary>
        /// ���� RPOP ����������汾���������б���û���κ�Ԫ�ؿɹ�������ʱ�����ӽ��� BRPOP ����������ֱ���ȴ���ʱ���ֿɵ���Ԫ��Ϊֹ����ʱ����null
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="timeout">��ʱ(��)</param>
        /// <param name="keys">һ�������б�����prefixǰ�</param>
        /// <returns></returns>
        public (string key, T value)? BRPopWithKey<T>(int timeout, params string[] keys) => RedisHelper.BRPopWithKey<T>(timeout, keys);
        /// <summary>
        /// ���� RPOP ����������汾���������б���û���κ�Ԫ�ؿɹ�������ʱ�����ӽ��� BRPOP ����������ֱ���ȴ���ʱ���ֿɵ���Ԫ��Ϊֹ����ʱ����null
        /// </summary>
        /// <param name="timeout">��ʱ(��)</param>
        /// <param name="keys">һ�������б�����prefixǰ�</param>
        /// <returns></returns>
        public string BRPop(int timeout, params string[] keys) => RedisHelper.BRPop(timeout, keys);
        /// <summary>
        /// ���� RPOP ����������汾���������б���û���κ�Ԫ�ؿɹ�������ʱ�����ӽ��� BRPOP ����������ֱ���ȴ���ʱ���ֿɵ���Ԫ��Ϊֹ����ʱ����null
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="timeout">��ʱ(��)</param>
        /// <param name="keys">һ�������б�����prefixǰ�</param>
        /// <returns></returns>
        public T BRPop<T>(int timeout, params string[] keys) => RedisHelper.BRPop<T>(timeout, keys);
        /// <summary>
        /// BRPOPLPUSH �� RPOPLPUSH �������汾���������б� source ��Ϊ��ʱ�� BRPOPLPUSH �ı��ֺ� RPOPLPUSH һ����
        /// ���б� source Ϊ��ʱ�� BRPOPLPUSH ����������ӣ�ֱ���ȴ���ʱ��������һ���ͻ��˶� source ִ�� LPUSH �� RPUSH ����Ϊֹ��
        /// </summary>
        /// <param name="source">Դkey������prefixǰ�</param>
        /// <param name="destination">Ŀ��key������prefixǰ�</param>
        /// <param name="timeout">��ʱ(��)</param>
        /// <returns></returns>
        public string BRPopLPush(string source, string destination, int timeout) => RedisHelper.BRPopLPush(source, destination, timeout);
        /// <summary>
        /// BRPOPLPUSH �� RPOPLPUSH �������汾���������б� source ��Ϊ��ʱ�� BRPOPLPUSH �ı��ֺ� RPOPLPUSH һ����
        /// ���б� source Ϊ��ʱ�� BRPOPLPUSH ����������ӣ�ֱ���ȴ���ʱ��������һ���ͻ��˶� source ִ�� LPUSH �� RPUSH ����Ϊֹ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="source">Դkey������prefixǰ�</param>
        /// <param name="destination">Ŀ��key������prefixǰ�</param>
        /// <param name="timeout">��ʱ(��)</param>
        /// <returns></returns>
        public T BRPopLPush<T>(string source, string destination, int timeout) => RedisHelper.BRPopLPush<T>(source, destination, timeout);
        /// <summary>
        /// ͨ��������ȡ�б��е�Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="index">����</param>
        /// <returns></returns>
        public string LIndex(string key, long index) => RedisHelper.LIndex(key, index);
        /// <summary>
        /// ͨ��������ȡ�б��е�Ԫ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="index">����</param>
        /// <returns></returns>
        public T LIndex<T>(string key, long index) => RedisHelper.LIndex<T>(key, index);
        /// <summary>
        /// ���б��е�Ԫ��ǰ�����Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="pivot">�б��Ԫ��</param>
        /// <param name="value">��Ԫ��</param>
        /// <returns></returns>
        public long LInsertBefore(string key, object pivot, object value) => RedisHelper.LInsertBefore(key, pivot, value);
        /// <summary>
        /// ���б��е�Ԫ�غ������Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="pivot">�б��Ԫ��</param>
        /// <param name="value">��Ԫ��</param>
        /// <returns></returns>
        public long LInsertAfter(string key, object pivot, object value) => RedisHelper.LInsertAfter(key, pivot, value);
        /// <summary>
        /// ��ȡ�б���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public long LLen(string key) => RedisHelper.LLen(key);
        /// <summary>
        /// �Ƴ�����ȡ�б�ĵ�һ��Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public string LPop(string key) => RedisHelper.LPop(key);
        /// <summary>
        /// �Ƴ�����ȡ�б�ĵ�һ��Ԫ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public T LPop<T>(string key) => RedisHelper.LPop<T>(key);
        /// <summary>
        /// ��һ������ֵ���뵽�б�ͷ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">һ������ֵ</param>
        /// <returns>ִ�� LPUSH ������б�ĳ���</returns>
        public long LPush<T>(string key, params T[] value) => RedisHelper.LPush(key, value);
        /// <summary>
        /// ��һ��ֵ���뵽�Ѵ��ڵ��б�ͷ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">ֵ</param>
        /// <returns>ִ�� LPUSHX ������б�ĳ��ȡ�</returns>
        public long LPushX(string key, object value) => RedisHelper.LPushX(key, value);
        /// <summary>
        /// ��ȡ�б�ָ����Χ�ڵ�Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public string[] LRange(string key, long start, long stop) => RedisHelper.LRange(key, start, stop);
        /// <summary>
        /// ��ȡ�б�ָ����Χ�ڵ�Ԫ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public T[] LRange<T>(string key, long start, long stop) => RedisHelper.LRange<T>(key, start, stop);
        /// <summary>
        /// ���ݲ��� count ��ֵ���Ƴ��б�������� value ��ȵ�Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="count">�Ƴ�������������0ʱ�ӱ�ͷɾ������count��С��0ʱ�ӱ�βɾ������-count������0�Ƴ�����</param>
        /// <param name="value">Ԫ��</param>
        /// <returns></returns>
        public long LRem(string key, long count, object value) => RedisHelper.LRem(key, count, value);
        /// <summary>
        /// ͨ�����������б�Ԫ�ص�ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="index">����</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public bool LSet(string key, long index, object value) => RedisHelper.LSet(key, index, value);
        /// <summary>
        /// ��һ���б�����޼������б�ֻ����ָ�������ڵ�Ԫ�أ�����ָ������֮�ڵ�Ԫ�ض�����ɾ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="stop">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public bool LTrim(string key, long start, long stop) => RedisHelper.LTrim(key, start, stop);
        /// <summary>
        /// �Ƴ�����ȡ�б����һ��Ԫ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public string RPop(string key) => RedisHelper.RPop(key);
        /// <summary>
        /// �Ƴ�����ȡ�б����һ��Ԫ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public T RPop<T>(string key) => RedisHelper.RPop<T>(key);
        /// <summary>
        /// ���б� source �е����һ��Ԫ��(βԪ��)�����������ظ��ͻ��ˡ�
        /// �� source ������Ԫ�ز��뵽�б� destination ����Ϊ destination �б�ĵ�ͷԪ�ء�
        /// </summary>
        /// <param name="source">Դkey������prefixǰ�</param>
        /// <param name="destination">Ŀ��key������prefixǰ�</param>
        /// <returns></returns>
        public string RPopLPush(string source, string destination) => RedisHelper.RPopLPush(source, destination);
        /// <summary>
        /// ���б� source �е����һ��Ԫ��(βԪ��)�����������ظ��ͻ��ˡ�
        /// �� source ������Ԫ�ز��뵽�б� destination ����Ϊ destination �б�ĵ�ͷԪ�ء�
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="source">Դkey������prefixǰ�</param>
        /// <param name="destination">Ŀ��key������prefixǰ�</param>
        /// <returns></returns>
        public T RPopLPush<T>(string source, string destination) => RedisHelper.RPopLPush<T>(source, destination);
        /// <summary>
        /// ���б������һ������ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">һ������ֵ</param>
        /// <returns>ִ�� RPUSH ������б�ĳ���</returns>
        public long RPush<T>(string key, params T[] value) => RedisHelper.RPush(key, value);
        /// <summary>
        /// Ϊ�Ѵ��ڵ��б����ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">һ������ֵ</param>
        /// <returns>ִ�� RPUSHX ������б�ĳ���</returns>
        public long RPushX(string key, object value) => RedisHelper.RPushX(key, value);
        #endregion

        #region Hash
        /// <summary>
        /// ɾ��һ��������ϣ���ֶ�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="fields">�ֶ�</param>
        /// <returns></returns>
        public long HDel(string key, params string[] fields) => RedisHelper.HDel(key, fields);
        /// <summary>
        /// �鿴��ϣ�� key �У�ָ�����ֶ��Ƿ����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        public bool HExists(string key, string field) => RedisHelper.HExists(key, field);
        /// <summary>
        /// ��ȡ�洢�ڹ�ϣ����ָ���ֶε�ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        public string HGet(string key, string field) => RedisHelper.HGet(key, field);
        /// <summary>
        /// ��ȡ�洢�ڹ�ϣ����ָ���ֶε�ֵ
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="field">�ֶ�</param>
        /// <returns></returns>
        public T HGet<T>(string key, string field) => RedisHelper.HGet<T>(key, field);
        /// <summary>
        /// ��ȡ�ڹ�ϣ����ָ�� key �������ֶκ�ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public Dictionary<string, string> HGetAll(string key) => RedisHelper.HGetAll(key);
        /// <summary>
        /// ��ȡ�ڹ�ϣ����ָ�� key �������ֶκ�ֵ
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public Dictionary<string, T> HGetAll<T>(string key) => RedisHelper.HGetAll<T>(key);
        /// <summary>
        /// Ϊ��ϣ�� key �е�ָ���ֶε�����ֵ�������� increment
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">����ֵ(Ĭ��=1)</param>
        /// <returns></returns>
        public long HIncrBy(string key, string field, long value = 1) => RedisHelper.HIncrBy(key, field, value);
        /// <summary>
        /// Ϊ��ϣ�� key �е�ָ���ֶε�����ֵ�������� increment
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">����ֵ(Ĭ��=1)</param>
        /// <returns></returns>
        public decimal HIncrByFloat(string key, string field, decimal value = 1) => RedisHelper.HIncrByFloat(key, field, value);
        /// <summary>
        /// ��ȡ���й�ϣ���е��ֶ�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public string[] HKeys(string key) => RedisHelper.HKeys(key);
        /// <summary>
        /// ��ȡ��ϣ�����ֶε�����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public long HLen(string key) => RedisHelper.HLen(key);
        /// <summary>
        /// ��ȡ�洢�ڹ�ϣ���ж���ֶε�ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="fields">�ֶ�</param>
        /// <returns></returns>
        public string[] HMGet(string key, params string[] fields) => RedisHelper.HMGet(key, fields);
        /// <summary>
        /// ��ȡ�洢�ڹ�ϣ���ж���ֶε�ֵ
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="fields">һ�������ֶ�</param>
        /// <returns></returns>
        public T[] HMGet<T>(string key, params string[] fields) => RedisHelper.HMGet<T>(key, fields);
        /// <summary>
        /// ͬʱ����� field-value (��-ֵ)�����õ���ϣ�� key ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public bool HMSet(string key, params object[] keyValues) => RedisHelper.HMSet(key, keyValues);
        /// <summary>
        /// ����ϣ�� key �е��ֶ� field ��ֵ��Ϊ value
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <returns>����ֶ��ǹ�ϣ���е�һ���½��ֶΣ�����ֵ���óɹ�������true�������ϣ�������ֶ��Ѿ������Ҿ�ֵ�ѱ���ֵ���ǣ�����false��</returns>
        public bool HSet(string key, string field, object value) => RedisHelper.HSet(key, field, value);
        /// <summary>
        /// ֻ�����ֶ� field ������ʱ�����ù�ϣ���ֶε�ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ(string �� byte[])</param>
        /// <returns></returns>
        public bool HSetNx(string key, string field, object value) => RedisHelper.HSetNx(key, field, value);
        /// <summary>
        /// ��ȡ��ϣ��������ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public string[] HVals(string key) => RedisHelper.HVals(key);
        /// <summary>
        /// ��ȡ��ϣ��������ֵ
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public T[] HVals<T>(string key) => RedisHelper.HVals<T>(key);
        /// <summary>
        /// ������ϣ���еļ�ֵ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="cursor">λ��</param>
        /// <param name="pattern">ģʽ</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public RedisScan<(string field, string value)> HScan(string key, long cursor, string pattern = null, long? count = null) =>
            RedisHelper.HScan(key, cursor, pattern, count);
        /// <summary>
        /// ������ϣ���еļ�ֵ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="cursor">λ��</param>
        /// <param name="pattern">ģʽ</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public RedisScan<(string field, T value)> HScan<T>(string key, long cursor, string pattern = null, long? count = null) =>
            RedisHelper.HScan<T>(key, cursor, pattern, count);
        #endregion

        #region String
        /// <summary>
        /// ��� key �Ѿ����ڲ�����һ���ַ����� APPEND ���ָ���� value ׷�ӵ��� key ԭ��ֵ��value����ĩβ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">�ַ���</param>
        /// <returns>׷��ָ��ֵ֮�� key ���ַ����ĳ���</returns>
        public long Append(string key, object value) => RedisHelper.Append(key, value);
        /// <summary>
        /// �������λ�ñ�����Ϊ 1 �ı���λ������
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ��</param>
        /// <param name="end">����λ��</param>
        /// <returns></returns>
        public long BitCount(string key, long start, long end) => RedisHelper.BitCount(key, start, end);
        /// <summary>
        /// ��һ���������������λ���ַ��� key ����λԪ����������������浽 destkey ��
        /// </summary>
        /// <param name="op">And | Or | XOr | Not</param>
        /// <param name="destKey">����prefixǰ�</param>
        /// <param name="keys">����prefixǰ�</param>
        /// <returns>���浽 destkey �ĳ��ȣ������� key ����ĳ������</returns>
        public long BitOp(RedisBitOp op, string destKey, params string[] keys) => RedisHelper.BitOp(op, destKey, keys);
        /// <summary>
        /// �� key �������ֵ�����ҷ�Χ�ڵ�һ��������Ϊ1����0��bitλ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="bit">����ֵ</param>
        /// <param name="start">��ʼλ�ã�-1�����һ����-2�ǵ����ڶ���</param>
        /// <param name="end">���λ�ã�-1�����һ����-2�ǵ����ڶ���</param>
        /// <returns>���ط�Χ�ڵ�һ��������Ϊ1����0��bitλ</returns>
        public long BitPos(string key, bool bit, long? start = null, long? end = null) => RedisHelper.BitPos(key, bit, start, end);
        /// <summary>
        /// ��ȡָ�� key ��ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public string Get(string key) => RedisHelper.Get(key);
        /// <summary>
        /// ��ȡָ�� key ��ֵ
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public T Get<T>(string key) => RedisHelper.Get<T>(key);
        /// <summary>
        /// ��ȡָ�� key ��ֵ�����ô���󷵻أ�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="destination">��ȡ��д��Ŀ������</param>
        /// <param name="bufferSize">��ȡ������</param>
        public void Get(string key, Stream destination, int bufferSize = 1024) => RedisHelper.Get(key, destination, bufferSize);
        /// <summary>
        /// �� key �������ֵ����ȡָ��ƫ�����ϵ�λ(bit)
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="offset">ƫ����</param>
        /// <returns></returns>
        public bool GetBit(string key, uint offset) => RedisHelper.GetBit(key, offset);
        /// <summary>
        /// ���� key ���ַ���ֵ�����ַ�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="end">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public string GetRange(string key, long start, long end) => RedisHelper.GetRange(key, start, end);
        /// <summary>
        /// ���� key ���ַ���ֵ�����ַ�
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="start">��ʼλ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <param name="end">����λ�ã�0��ʾ��һ��Ԫ�أ�-1��ʾ���һ��Ԫ��</param>
        /// <returns></returns>
        public T GetRange<T>(string key, long start, long end) => RedisHelper.GetRange<T>(key, start, end);
        /// <summary>
        /// ������ key ��ֵ��Ϊ value �������� key �ľ�ֵ(old value)
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public string GetSet(string key, object value) => RedisHelper.GetSet(key, value);
        /// <summary>
        /// ������ key ��ֵ��Ϊ value �������� key �ľ�ֵ(old value)
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public T GetSet<T>(string key, object value) => RedisHelper.GetSet<T>(key, value);
        /// <summary>
        /// �� key �������ֵ���ϸ���������ֵ��increment��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">����ֵ(Ĭ��=1)</param>
        /// <returns></returns>
        public long IncrBy(string key, long value = 1) => RedisHelper.IncrBy(key, value);
        /// <summary>
        /// �� key �������ֵ���ϸ����ĸ�������ֵ��increment��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">����ֵ(Ĭ��=1)</param>
        /// <returns></returns>
        public decimal IncrByFloat(string key, decimal value = 1) => RedisHelper.IncrByFloat(key, value);
        /// <summary>
        /// ��ȡ���ָ�� key ��ֵ(����)
        /// </summary>
        /// <param name="keys">����prefixǰ�</param>
        /// <returns></returns>
        public string[] MGet(params string[] keys) => RedisHelper.MGet(keys);
        /// <summary>
        /// ��ȡ���ָ�� key ��ֵ(����)
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="keys">����prefixǰ�</param>
        /// <returns></returns>
        public T[] MGet<T>(params string[] keys) => RedisHelper.MGet<T>(keys);
        /// <summary>
        /// ͬʱ����һ������ key-value ��
        /// </summary>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public bool MSet(params object[] keyValues) => RedisHelper.MSet(keyValues);
        /// <summary>
        /// ͬʱ����һ������ key-value �ԣ����ҽ������и��� key ��������
        /// </summary>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public bool MSetNx(params object[] keyValues) => RedisHelper.MSetNx(keyValues);
        /// <summary>
        /// ����ָ�� key ��ֵ������д�����object��֧��string | byte[] | ��ֵ | ����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="expireSeconds">����(�뵥λ)</param>
        /// <param name="exists">Nx, Xx</param>
        /// <returns></returns>
        public bool Set(string key, object value, int expireSeconds = -1, RedisExistence? exists = null) => RedisHelper.Set(key, value, expireSeconds, exists);
        public bool Set(string key, object value, TimeSpan expire, RedisExistence? exists = null) => RedisHelper.Set(key, value, expire, exists);
        /// <summary>
        /// �� key ��������ַ���ֵ�����û����ָ��ƫ�����ϵ�λ(bit)
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="offset">ƫ����</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public bool SetBit(string key, uint offset, bool value) => RedisHelper.SetBit(key, offset, value);
        /// <summary>
        /// ֻ���� key ������ʱ���� key ��ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public bool SetNx(string key, object value) => RedisHelper.SetNx(key, value);
        /// <summary>
        /// �� value ������д���� key ��������ַ���ֵ����ƫ���� offset ��ʼ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="offset">ƫ����</param>
        /// <param name="value">ֵ</param>
        /// <returns>���޸ĺ���ַ�������</returns>
        public long SetRange(string key, uint offset, object value) => RedisHelper.SetRange(key, offset, value);
        /// <summary>
        /// ���� key ��������ַ���ֵ�ĳ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public long StrLen(string key) => RedisHelper.StrLen(key);
        #endregion

        #region Key
        /// <summary>
        /// ������ key ����ʱɾ�� key
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public long Del(params string[] key) => RedisHelper.Del(key);
        /// <summary>
        /// ���л����� key �������ر����л���ֵ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public byte[] Dump(string key) => RedisHelper.Dump(key);
        /// <summary>
        /// ������ key �Ƿ����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public bool Exists(string key) => RedisHelper.Exists(key);
        /// <summary>
        /// [redis-server 3.0] ��������� key �Ƿ����
        /// </summary>
        /// <param name="keys">����prefixǰ�</param>
        /// <returns></returns>
        public long Exists(string[] keys) => RedisHelper.Exists(keys);
        /// <summary>
        /// Ϊ���� key ���ù���ʱ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="seconds">��������</param>
        /// <returns></returns>
        public bool Expire(string key, int seconds) => RedisHelper.Expire(key, seconds);
        /// <summary>
        /// Ϊ���� key ���ù���ʱ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="expire">����ʱ��</param>
        /// <returns></returns>
        public bool Expire(string key, TimeSpan expire) => RedisHelper.Expire(key, expire);
        /// <summary>
        /// Ϊ���� key ���ù���ʱ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="expire">����ʱ��</param>
        /// <returns></returns>
        public bool ExpireAt(string key, DateTime expire) => RedisHelper.ExpireAt(key, expire);
        /// <summary>
        /// �������з����ڵ��з��ϸ���ģʽ(pattern)�� key
        /// <para>Keys�������ص�keys[]����prefix��ʹ��ǰ�����д���</para>
        /// </summary>
        /// <param name="pattern">�磺runoob*</param>
        /// <returns></returns>
        public string[] Keys(string pattern) => RedisHelper.Keys(pattern);
        /// <summary>
        /// ����ǰ���ݿ�� key �ƶ������������ݿ� db ����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="database">���ݿ�</param>
        /// <returns></returns>
        public bool Move(string key, int database) => RedisHelper.Move(key, database);
        /// <summary>
        /// �÷��ظ��� key �������ֵ��ʹ�õ��ڲ���ʾ(representation)
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public string ObjectEncoding(string key) => RedisHelper.ObjectEncoding(key);
        /// <summary>
        /// �÷��ظ��� key �����������ֵ�Ĵ�������������Ҫ���ڳ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public long? ObjectRefCount(string key) => RedisHelper.ObjectRefCount(key);
        /// <summary>
        /// ���ظ��� key �Դ��������Ŀ�תʱ��(idle�� û�б���ȡҲû�б�д��)������Ϊ��λ
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public long? ObjectIdleTime(string key) => RedisHelper.ObjectIdleTime(key);
        /// <summary>
        /// �Ƴ� key �Ĺ���ʱ�䣬key ���־ñ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public bool Persist(string key) => RedisHelper.Persist(key);
        /// <summary>
        /// Ϊ���� key ���ù���ʱ�䣨���룩
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="milliseconds">���ں�����</param>
        /// <returns></returns>
        public bool PExpire(string key, int milliseconds) => RedisHelper.PExpire(key, milliseconds);
        /// <summary>
        /// Ϊ���� key ���ù���ʱ�䣨���룩
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="expire">����ʱ��</param>
        /// <returns></returns>
        public bool PExpire(string key, TimeSpan expire) => RedisHelper.PExpire(key, expire);
        /// <summary>
        /// Ϊ���� key ���ù���ʱ�䣨���룩
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="expire">����ʱ��</param>
        /// <returns></returns>
        public bool PExpireAt(string key, DateTime expire) => RedisHelper.PExpireAt(key, expire);
        /// <summary>
        /// �Ժ���Ϊ��λ���� key ��ʣ��Ĺ���ʱ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public long PTtl(string key) => RedisHelper.PTtl(key);
        /// <summary>
        /// �����нڵ����������һ�� key
        /// </summary>
        /// <returns>���ص� key ������� prefixǰꡣ����ȥ���󷵻�</returns>
        public string RandomKey() => RedisHelper.RandomKey();
        /// <summary>
        /// �޸� key ������
        /// </summary>
        /// <param name="key">�����ƣ�����prefixǰ�</param>
        /// <param name="newKey">�����ƣ�����prefixǰ�</param>
        /// <returns></returns>
        public bool Rename(string key, string newKey) => RedisHelper.Rename(key, newKey);
        /// <summary>
        /// �޸� key ������
        /// </summary>
        /// <param name="key">�����ƣ�����prefixǰ�</param>
        /// <param name="newKey">�����ƣ�����prefixǰ�</param>
        /// <returns></returns>
        public bool RenameNx(string key, string newKey) => RedisHelper.RenameNx(key, newKey);
        /// <summary>
        /// �����л����������л�ֵ���������͸����� key ����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="serializedValue">���л�ֵ</param>
        /// <returns></returns>
        public bool Restore(string key, byte[] serializedValue) => RedisHelper.Restore(key, serializedValue);
        /// <summary>
        /// �����л����������л�ֵ���������͸����� key ����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="ttlMilliseconds">����Ϊ��λΪ key ��������ʱ��</param>
        /// <param name="serializedValue">���л�ֵ</param>
        /// <returns></returns>
        public bool Restore(string key, long ttlMilliseconds, byte[] serializedValue) => RedisHelper.Restore(key, ttlMilliseconds, serializedValue);
        /// <summary>
        /// ���ظ����б����ϡ����򼯺� key �о��������Ԫ�أ��������ϣ�http://doc.redisfans.com/key/sort.html
        /// </summary>
        /// <param name="key">�б����ϡ����򼯺ϣ�����prefixǰ�</param>
        /// <param name="count">����</param>
        /// <param name="offset">ƫ����</param>
        /// <param name="by">�����ֶ�</param>
        /// <param name="dir">����ʽ</param>
        /// <param name="isAlpha">���ַ��������ֽ�������</param>
        /// <param name="get">��������Ľ����ȡ����Ӧ�ļ�ֵ</param>
        /// <returns></returns>
        public string[] Sort(string key, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get) =>
            RedisHelper.Sort(key, count, offset, by, dir, isAlpha, get);
        /// <summary>
        /// ��������б����ϡ����򼯺� key �о��������Ԫ�أ��������ϣ�http://doc.redisfans.com/key/sort.html
        /// </summary>
        /// <param name="key">�б����ϡ����򼯺ϣ�����prefixǰ�</param>
        /// <param name="destination">Ŀ��key������prefixǰ�</param>
        /// <param name="count">����</param>
        /// <param name="offset">ƫ����</param>
        /// <param name="by">�����ֶ�</param>
        /// <param name="dir">����ʽ</param>
        /// <param name="isAlpha">���ַ��������ֽ�������</param>
        /// <param name="get">��������Ľ����ȡ����Ӧ�ļ�ֵ</param>
        /// <returns></returns>
        public long SortAndStore(string key, string destination, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get) =>
            RedisHelper.SortAndStore(key, destination, count, offset, by, dir, isAlpha, get);
        /// <summary>
        /// ����Ϊ��λ�����ظ��� key ��ʣ������ʱ��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public long Ttl(string key) => RedisHelper.Ttl(key);
        /// <summary>
        /// ���� key �������ֵ������
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <returns></returns>
        public KeyType Type(string key) => RedisHelper.Type(key);
        /// <summary>
        /// ������ǰ���ݿ��е����ݿ��
        /// </summary>
        /// <param name="cursor">λ��</param>
        /// <param name="pattern">ģʽ</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public RedisScan<string> Scan(long cursor, string pattern = null, long? count = null) => RedisHelper.Scan(cursor, pattern, count);
        /// <summary>
        /// ������ǰ���ݿ��е����ݿ��
        /// </summary>
        /// <typeparam name="T">byte[] ����������</typeparam>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="cursor">λ��</param>
        /// <param name="pattern">ģʽ</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public RedisScan<T> Scan<T>(string key, long cursor, string pattern = null, long? count = null) => RedisHelper.Scan<T>(key,cursor, pattern, count);
        #endregion

        #region Geo redis-server 3.2
        /// <summary>
        /// ��ָ���ĵ���ռ�λ�ã�γ�ȡ����ȡ���Ա����ӵ�ָ����key�С���Щ���ݽ���洢��sorted set������Ŀ����Ϊ�˷���ʹ��GEORADIUS����GEORADIUSBYMEMBER��������ݽ��а뾶��ѯ�Ȳ�����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="longitude">����</param>
        /// <param name="latitude">γ��</param>
        /// <param name="member">��Ա</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public bool GeoAdd(string key, decimal longitude, decimal latitude, object member) => RedisHelper.GeoAdd(key, longitude, latitude, member);
        /// <summary>
        /// ��ָ���ĵ���ռ�λ�ã�γ�ȡ����ȡ���Ա����ӵ�ָ����key�С���Щ���ݽ���洢��sorted set������Ŀ����Ϊ�˷���ʹ��GEORADIUS����GEORADIUSBYMEMBER��������ݽ��а뾶��ѯ�Ȳ�����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="values">������ӵ�ֵ</param>
        /// <returns>��ӵ�sorted setԪ�ص���Ŀ�����������Ѹ���score��Ԫ�ء�</returns>
        public long GeoAdd(string key, params (decimal longitude, decimal latitude, object member)[] values) => RedisHelper.GeoAdd(key, values);
        /// <summary>
        /// ������������λ��֮��ľ��롣�������λ��֮�������һ�������ڣ� ��ô����ؿ�ֵ��GEODIST �����ڼ������ʱ��������Ϊ���������Σ� �ڼ�������£� ��һ����������� 0.5% ����
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member1">��Ա1</param>
        /// <param name="member2">��Ա2</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <returns>������ľ������˫���ȸ���������ʽ�����ء� ���������λ��Ԫ�ز����ڣ� ��ô����ؿ�ֵ��</returns>
        public decimal? GeoDist(string key, object member1, object member2, GeoUnit unit = GeoUnit.m) => RedisHelper.GeoDist(key, member1, member2, unit);
        /// <summary>
        /// ����һ������λ��Ԫ�ص� Geohash ��ʾ��ͨ��ʹ�ñ�ʾλ�õ�Ԫ��ʹ�ò�ͬ�ļ�����ʹ��Geohashλ��52���������롣���ڱ���ͽ����������ʹ�õĳ�ʼ��С��������겻ͬ������ı���Ҳ��ͬ�ڱ�׼��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="members">�����ѯ�ĳ�Ա</param>
        /// <returns>һ�����飬 �����ÿ�����һ�� geohash �� ����ص� geohash ��λ�����û�������λ��Ԫ�ص�λ��һһ��Ӧ��</returns>
        public string[] GeoHash(string key, object[] members) => RedisHelper.GeoHash(key, members);
        /// <summary>
        /// ��key�ﷵ�����и���λ��Ԫ�ص�λ�ã����Ⱥ�γ�ȣ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="members">�����ѯ�ĳ�Ա</param>
        /// <returns>GEOPOS �����һ�����飬 �����е�ÿ���������Ԫ����ɣ� ��һ��Ԫ��Ϊ����λ��Ԫ�صľ��ȣ� ���ڶ���Ԫ����Ϊ����λ��Ԫ�ص�γ�ȡ���������λ��Ԫ�ز�����ʱ�� ��Ӧ��������Ϊ��ֵ��</returns>
        public (decimal longitude, decimal latitude)?[] GeoPos(string key, object[] members) => RedisHelper.GeoPos(key, members);

        /// <summary>
        /// �Ը����ľ�γ��Ϊ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�ء�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="longitude">����</param>
        /// <param name="latitude">γ��</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public string[] GeoRadius(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadius(key, longitude, latitude, radius, unit, count, sorting);
        /// <summary>
        /// �Ը����ľ�γ��Ϊ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�ء�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="longitude">����</param>
        /// <param name="latitude">γ��</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public T[] GeoRadius<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadius<T>(key, longitude, latitude, radius, unit, count, sorting);

        /// <summary>
        /// �Ը����ľ�γ��Ϊ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������룩��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="longitude">����</param>
        /// <param name="latitude">γ��</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public (string member, decimal dist)[] GeoRadiusWithDist(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadiusWithDist(key, longitude, latitude, radius, unit, count, sorting);
        /// <summary>
        /// �Ը����ľ�γ��Ϊ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������룩��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="longitude">����</param>
        /// <param name="latitude">γ��</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public (T member, decimal dist)[] GeoRadiusWithDist<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadiusWithDist<T>(key, longitude, latitude, radius, unit, count, sorting);

        ///// <summary>
        ///// �Ը����ľ�γ��Ϊ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������ȡ�γ�ȣ���
        ///// </summary>
        ///// <param name="key">����prefixǰ�</param>
        ///// <param name="longitude">����</param>
        ///// <param name="latitude">γ��</param>
        ///// <param name="radius">����</param>
        ///// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        ///// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        ///// <param name="sorting">����</param>
        ///// <returns></returns>
        //private (string member, decimal longitude, decimal latitude)[] GeoRadiusWithCoord(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
        //	RedisHelper.GeoRadiusWithCoord(key, longitude, latitude, radius, unit, count, sorting);
        ///// <summary>
        ///// �Ը����ľ�γ��Ϊ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������ȡ�γ�ȣ���
        ///// </summary>
        ///// <param name="key">����prefixǰ�</param>
        ///// <param name="longitude">����</param>
        ///// <param name="latitude">γ��</param>
        ///// <param name="radius">����</param>
        ///// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        ///// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        ///// <param name="sorting">����</param>
        ///// <returns></returns>
        //private (T member, decimal longitude, decimal latitude)[] GeoRadiusWithCoord<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
        //	RedisHelper.GeoRadiusWithCoord<T>(key, longitude, latitude, radius, unit, count, sorting);

        /// <summary>
        /// �Ը����ľ�γ��Ϊ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������롢���ȡ�γ�ȣ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="longitude">����</param>
        /// <param name="latitude">γ��</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public (string member, decimal dist, decimal longitude, decimal latitude)[] GeoRadiusWithDistAndCoord(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadiusWithDistAndCoord(key, longitude, latitude, radius, unit, count, sorting);
        /// <summary>
        /// �Ը����ľ�γ��Ϊ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������롢���ȡ�γ�ȣ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="longitude">����</param>
        /// <param name="latitude">γ��</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public (T member, decimal dist, decimal longitude, decimal latitude)[] GeoRadiusWithDistAndCoord<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadiusWithDistAndCoord<T>(key, longitude, latitude, radius, unit, count, sorting);

        /// <summary>
        /// �Ը����ĳ�ԱΪ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�ء�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public string[] GeoRadiusByMember(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadiusByMember(key, member, radius, unit, count, sorting);
        /// <summary>
        /// �Ը����ĳ�ԱΪ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�ء�
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public T[] GeoRadiusByMember<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadiusByMember<T>(key, member, radius, unit, count, sorting);

        /// <summary>
        /// �Ը����ĳ�ԱΪ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������룩��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public (string member, decimal dist)[] GeoRadiusByMemberWithDist(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadiusByMemberWithDist(key, member, radius, unit, count, sorting);
        /// <summary>
        /// �Ը����ĳ�ԱΪ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������룩��
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public (T member, decimal dist)[] GeoRadiusByMemberWithDist<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadiusByMemberWithDist<T>(key, member, radius, unit, count, sorting);

        ///// <summary>
        ///// �Ը����ĳ�ԱΪ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������ȡ�γ�ȣ���
        ///// </summary>
        ///// <param name="key">����prefixǰ�</param>
        ///// <param name="member">��Ա</param>
        ///// <param name="radius">����</param>
        ///// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        ///// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        ///// <param name="sorting">����</param>
        ///// <returns></returns>
        //private (string member, decimal longitude, decimal latitude)[] GeoRadiusByMemberWithCoord(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
        //	RedisHelper.GeoRadiusByMemberWithCoord(key, member, radius, unit, count, sorting);
        ///// <summary>
        ///// �Ը����ĳ�ԱΪ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������ȡ�γ�ȣ���
        ///// </summary>
        ///// <param name="key">����prefixǰ�</param>
        ///// <param name="member">��Ա</param>
        ///// <param name="radius">����</param>
        ///// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        ///// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        ///// <param name="sorting">����</param>
        ///// <returns></returns>
        //private (T member, decimal longitude, decimal latitude)[] GeoRadiusByMemberWithCoord<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
        //	RedisHelper.GeoRadiusByMemberWithCoord<T>(key, member, radius, unit, count, sorting);

        /// <summary>
        /// �Ը����ĳ�ԱΪ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������롢���ȡ�γ�ȣ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public (string member, decimal dist, decimal longitude, decimal latitude)[] GeoRadiusByMemberWithDistAndCoord(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadiusByMemberWithDistAndCoord(key, member, radius, unit, count, sorting);
        /// <summary>
        /// �Ը����ĳ�ԱΪ���ģ� ���ؼ�������λ��Ԫ�ص��У� �����ĵľ��벻�������������������λ��Ԫ�أ��������롢���ȡ�γ�ȣ���
        /// </summary>
        /// <param name="key">����prefixǰ�</param>
        /// <param name="member">��Ա</param>
        /// <param name="radius">����</param>
        /// <param name="unit">m ��ʾ��λΪ�ף�km ��ʾ��λΪǧ�ף�mi ��ʾ��λΪӢ�ft ��ʾ��λΪӢ�ߣ�</param>
        /// <param name="count">��Ȼ�û�����ʹ�� COUNT ѡ��ȥ��ȡǰ N ��ƥ��Ԫ�أ� ������Ϊ�������ڲ����ܻ���Ҫ�����б�ƥ���Ԫ�ؽ��д��� �����ڶ�һ���ǳ���������������ʱ�� ��ʹֻʹ�� COUNT ѡ��ȥ��ȡ����Ԫ�أ� �����ִ���ٶ�Ҳ���ܻ�ǳ����� ���Ǵ���һ������˵�� ʹ�� COUNT ѡ��ȥ������Ҫ���ص�Ԫ�������� ���ڼ��ٴ�����˵��Ȼ�Ƿǳ����õġ�</param>
        /// <param name="sorting">����</param>
        /// <returns></returns>
        public (T member, decimal dist, decimal longitude, decimal latitude)[] GeoRadiusByMemberWithDistAndCoord<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            RedisHelper.GeoRadiusByMemberWithDistAndCoord<T>(key, member, radius, unit, count, sorting);
        #endregion

        /// <summary> 
        /// �����ֲ�ʽ��������ʱ����null
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="timeoutSeconds">��ʱ���룩</param>
        /// <param name="autoDelay">�Զ��ӳ�����ʱʱ�䣬���Ź��̵߳ĳ�ʱʱ��ΪtimeoutSeconds/2 �� �ڿ��Ź��̳߳�ʱʱ��ʱ�Զ��ӳ�����ʱ��ΪtimeoutSeconds�����ǳ��������˳�������������ʱ��</param>
        /// <returns></returns>
        public CSRedisClientLock Lock(string name, int timeoutSeconds, bool autoDelay = true) => RedisHelper.Lock(name, timeoutSeconds);
    }

}
