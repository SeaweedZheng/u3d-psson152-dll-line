### 出奖推送接口

游戏机出奖推送出奖内容

**请求地址:**

http://{ip}:{port}/dsc/AemsOrder/Act/PushGamePrize

**提交方式：**

Post

**使用场景**：

1.游戏机中奖，推送游戏机信息及中奖内容

2.游戏机开机时，推送一条包含游戏机信息的初始化信息


**业务参数**

| 字段名称    | 字段标题 | 数据类型      | 备注                                                         |
| ----------- | -------- | ------------- | ------------------------------------------------------------ |
| MachineID   | 机台ID   | varchar(30)   |                                                              |
| MachineName | 机台名称 | varchar(50)   |                                                              |
| MachCode    | 机位编号 | varchar(30)   |                                                              |
| PrizeCode   | 奖项编号 | varchar(10)   | 常规中奖时：推送奖项编号<br>开机时： 推送固定编号 "Init" 作为初始化通知 |
| PrizeName   | 奖项名称 | varchar(50)   |                                                              |
| PrizeType   | 奖项类型 | int           | 1.币  2.票                                                   |
| Num         | 奖品数量 | decimal(18,2) |                                                              |

**响应参数 **（异步推送 不用等待推送结果）

| 字段名称 | 数据类型             | 描述              |
| -------- | -------------------- | ----------------- |
| exstatus | int output           | 执行结果。 0 成功 |
| exmsg    | nvarchar(200) output | 执行结果描述      |

**错误码列表**

| 错误码 | 描述 |
| ------ | ---- |
| 0      | 成功 |
| 1      | 失败 |

**请求实例**

```json

{
    "Data": {
        "MachineID": "AAAAA111",
        "MachineName": "极速雪摩",
        "MachCode": "AAAAA111001",
        "PrizeCode": "1",
        "PrizeName": "奖励1",
        "PrizeType": 1,
        "Num": 200
    }
}

```

**响应实例**

```json

{
	"exstatus": 0,
	"exmsg": "成功",
	"data": null,
	"help": {}
}
```


# 外网调试接口：

调试阶段可以直接外网调用我们公司的地址：
http://shiruan.zs-sr.cn:9091/dsc/AemsOrder/Act/PushGamePrize
 
{
  "Data": {
    "MachineID": "AAAAA111",
    "MachineName": "塔塔岛",
    "MachCode": "AAAAA111001",
    "PrizeCode": "3",
    "PrizeName": "币塔",
    "PrizeType": 3,
    "Num": 200
  }
}

游戏机对接的话  加个配置，填写服务器IP或者对接的整个接口地址。


# 报错放回
response: {
  "exstatus": 500,
  "exmsg": "未将对象引用设置到对象的实例。",
  "data": null,
  "help": {}
} 
