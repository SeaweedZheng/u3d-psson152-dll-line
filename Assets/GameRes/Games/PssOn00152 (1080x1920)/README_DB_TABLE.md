# 充值记录 (ok)

* [coin_in_out_record]-投退避记录
* user_id:
* in_out：进钱/出钱
* credit_before: 玩家开玩前的金额
* credit_after: 玩家结束玩的金额
* credit : 冲入的游戏币
* device(model/number) 
* date:
 
# 支持的设备型号 (ok)

* [support_devices]
* type : 类型 （INTEGER）
* name : 名称 
* model: 设备型号
* manufacturer: 厂家
* in_out: 1进，0出
* des: 描述
 
# 已接入设备设备型号 (ok)

* [devices]
* type : 类型 （INTEGER）
* number / device_id: 编号
* port: 端口
* enable: 开启
* state: 状态（正常、故障、掉线）
* in_out: 1进，0出
* in_out_scale: 投退币比率
 

# 语言

* [support_language]
* number: en
* name: English
* money_symbol:$
 

# 机器信息（ok）

*
* [machine]
* machine_type: 机器类型
* machine_name: 机器名称
* machine_id: 机器id
*// number: 编号 
* manufacture： 厂家
* address: 地址
* phone: 电话
* fax：
* email:
* manufacture_date:生产日期 
* channel_id: 渠道id
* agent_id: 代理id
* platform: 平台号
* theme_game_name: 主题游戏名
* theme_game_id: 主题游戏id
* disk_ver: 硬盘版本
* app_ver: app版本
* creat_time:  app安裝時間
* state: 状态
* modify_date: 修改日期
 

# 系统-机器设置


* [users]
* user_name: admin / shift / manger
* user_id
* password:
* type:
* last_login_time:
* token:
* permission_id： 1
* modify_date




# Buttons

* [buttons]
* name : Spin
* port: 端口
* enable: 是否使用
* panel_number: 所在面板
* pos_column:
* pos_row:
 
# setting

*
* [system_setting]
* //wire_model:[{model:5 number:1,scale:-1},{model:5 number:1,scale:-1}]
* //printer_model:[{model:5 number:1,scale:-1},{model:5 number:1,scale:-1}]  //纸钞机
* //bill_validator_model:[{model:5 number:1,scale:-1},{model:5 number:1,scale:-1}]//出票机
* //ticket_model:[{model:5 number:1,scale:-1},{model:5 number:1,scale:-1}]

* language: 语言 

* sound:
* muise:
* demo_voice:


 
* bills_list: [{value:5 allowed:1},{value:5 allowed:1}]  // 投钱列表
 
* display_credit_list: [{"name":"Money","number":1},{"name":"Points","number":2}]
* display_credit_as: 1    //显示钱还是游戏分
 
* network:使用网络
 
* debug: 测试
 
* coin_in_limit: 投币最大额度
* credit_limit: 
* payout_limit: 退钱最大额度
 
* max_game_record: 100
* max_jackpot_record: 1000
* max_warn_record: 1000
* max_event_record: 1000
 
* coin_in_scale: 投币比例： 1:100 (1币：100游戏分)
* bills_in_scale: 投钞比例： 1:100 (1钞票：100游戏分)
* ticket_out_scale: 退票比例： 1:100 (1票：100游戏分)
* printer_out_scale: 打印比例： 1:100 (1钞票：100游戏分)
 
* money_meter_scale:
 
* modify_date: 修改日期
 

# 每个压注设置

* [bet]
* game_id:
* bet_min:
* bet_max:
* bet_list: json:[{value:5 allowed:1},{value:5 allowed:1}]
* bet_list_allowed:
* default_bet_index: 0;
* default_apostar:
* default_lines:
* modify_date: 修改日期
  

 
# game


* [game]
* game_id: 游戲id
* game_name: 游戲名称
* type: 游戏类型（slot）
* ver:
* enable: 开启游戏
* sort：排序
* custom: 游戏自定义数据
* likes: 喜欢
* collect: 收藏
* hall_icon:  大厅图标
* last_update_time: 游戏更新时间
* game_rate: 难度
* game_level: 
* modify_date: 修改日期
 
# 游戏分析


* [game_analysis]
* game_id: 游戲id
* ver:
* launch_time: 上次开始时间
* end_time: 上次结束时间
* run_time: 运行时间
* total_win_credit: 玩家总赢
* total_bet_credit: 玩家总压注
* custom: 游戏自定义数据
* modify_date: 修改日期
  
 

* [game-custom]
* column:
* row:
* lines:
 

# jackpot 设置


* [jackpot]
* game_id: 所属游戏
* name: jackpot名称
* level :1-JP1 2-JP2 3-JP3
* range_min: jackpot 开奖范围最小值
* range_max: jackpot 开奖范围最大值
* enable: 开启该jackpot
* hit_per : 中奖概率
* total_bet_min : 最小压注条件
* modify_date: 修改日期 
 
# jackpot 获奖记录
 

* [jackpot_record]
* user_id:
* game_id: 所属游戏
* game_uid: 该局游戏编号
* name: JP1
* level :1-JP1 2-JP2 3-JP3
* win_credit: 赢钱金额
* date: 日期
 


# 单局游戏记录


* [slot_game_record]
* user_id: 玩家id 
* game_id: 游戲id
* game_uid: 该局游戏编号
* scene: 场景数据jsondata
* total_bet: 玩家总下注
* lines_played: 下注的线数 
* credit_before: 玩家开玩前的金额
* credit_after: 玩家结束玩的金额
* total_win_credit: 玩家总赢
* game_result : 游戏结果
* jackpot_win_credit:
* base_game_win_credit: 基本游戏赢钱
* free_spin_win_credit: 免费游戏赢钱
* bonus_game_win_credit:  小游戏赢钱
* link_game_win_credit:
* skill_game_win_credit:
* reward_credit: 奖励金额
* date: 日期
  
 
 
 
 
 
 
 