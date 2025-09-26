using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testss : MonoBehaviour
{
    public Button testBtn;
    // Start is called before the first frame update
    void Start()
    {
        //  var csredis = new CSRedis.CSRedisClient("192.168.3.54:6379,password=123456,defaultDatabase=27,prefix=my_");
        //csredis.Set("axxff", 12333, 3600);
        //csredis.Dispose();
        //csredis = null;
        //bool ret = cryRedis.RedisMgr.Instance.InitRedis();
        //cryRedis.RedisMgr.Instance.Set("fuck", "fuck", 30);

        testBtn.onClick.AddListener(() =>
        {
            //cryRedis.RedisMgr.Instance.Set("fuck", "fuck", 30);
            // long ret = cryRedis.RedisMgr.Instance.Publish("0x12", "");
            //Debug.Log("redis Publish result:" + ret);

            //except code
            /*SASMessage msg = new SASMessage();
            msg.eventId = SasConstant.POWER_OFF_DROP_DOOR_ACCESS;
            msg.data = "";
            SasCommand.Instance.PushGeneralPoll(msg);*/

            //meter code
            string code = string.Format("0x{0:X4}", SasConstant.IN_HOUSE_TRANSFERS_TO_HOST_THAT_INCLUDED_NONRESTRICTED_AMOUNTS);
            int value = 42; // 假设这是某个动态变化的值
            string message = string.Format("{{\"value\":{0}}}", value);
            SasCommand.Instance.SetMeter(code,message);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
