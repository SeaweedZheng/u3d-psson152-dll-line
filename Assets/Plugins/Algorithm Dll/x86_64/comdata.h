#ifndef __COM_DATA_H__
#define __COM_DATA_H__
#include "../common/platform.h"
//最多四个彩金
/*
* 3 小彩金
* 2 中彩金
* 1 大彩金
* 0 巨大彩金
*/
#define JACKPOT_MAX 4
#define JM_MAX 2
#define DATA_BIT 5
#define TABLE_LEN 10
#define HALF_TABLE 5

typedef struct _JackpotBase_t {
	double mfBaseValue[JACKPOT_MAX];		//基本值
	double mfLowValue[JACKPOT_MAX];		//最小触发值
	double mfHighValue[JACKPOT_MAX];		//最大触发值
	double mfMidValue[JACKPOT_MAX];		//平均触发值
	int32_t mBetThreshold[JACKPOT_MAX];				//每个彩金的最低压分值;
	int32_t mBetThresholdMax[JACKPOT_MAX];				//每个彩金的最高压分值;
	int32_t mJackpotTrigger[JACKPOT_MAX][TABLE_LEN];
	int32_t mJackpotIdx[JACKPOT_MAX];

	double mfOutPercent[JACKPOT_MAX];		//显示值的百分比   
	double mfInnerPercent[JACKPOT_MAX];		//隐藏值的百分比
	//double mfSettingPercent[JACKPOT_MAX];	//设定的彩金百分比

	int32_t mJpWeight[JACKPOT_MAX];		//彩金比重
	int32_t mJpTotalWeight;				//彩金比重总和
	double mJpTriggerDiff[JACKPOT_MAX];		//触发差值
	//2025/01/11增加修改
	double mfRangeValue[JACKPOT_MAX];	//彩金开奖范围
}JackpotBase;

typedef struct _JackpotTotal_t {
	//提拔余数
	double mfPullRemainCent[JACKPOT_MAX];	  //彩金提拔余数
	double mfOutPullRemainCent[JACKPOT_MAX];  //彩金显示提拔余数
	double mfInnerPullRemainCent[JACKPOT_MAX];  //彩金隐藏提拔余数

	double mfJpCent[JACKPOT_MAX];			//开出的彩金分数
	double mfInnerRemainCent[JACKPOT_MAX];	//内部剩余积分
	double mfInnerClPullCent[JACKPOT_MAX];	//内部彩金提拨分

	double mfTotalCent[JACKPOT_MAX];		//彩金开奖累积分
	double mfOutTotalCent[JACKPOT_MAX];		//彩金显示累积分
	double mfTotalLottery[JACKPOT_MAX];		//开出彩金积累分数

	//彩金显示提拨分
	double mfOutPullCent[JACKPOT_MAX];
	//彩金隐藏提拨分
	double mfInnerPullCent[JACKPOT_MAX];

	//提拨分
	double mfOutTotalPullCent[JACKPOT_MAX];						//彩金显示累计提拨分
	double mfInnerTotalPullCent[JACKPOT_MAX];					//彩金隐藏累计提拨分
	double mfTotalPullCent[JACKPOT_MAX];						//彩金累计提拔分
	double mfJpPullCent[JACKPOT_MAX];						//4个彩金的彩金提拔分

	//补偿积分
	double mfCompensateCent[JACKPOT_MAX];	//补偿积分
	double mfCompensatePullCent[JACKPOT_MAX];	//补偿彩金提拨分
	double mfCompensatePullRemainCent[JACKPOT_MAX];	//补偿彩金提拨余数
	double mfDiffCent[JACKPOT_MAX];	//差值

	double mfPullRemainCentTotal;					//所有的彩金提拨余数
}JackpotTotal;

typedef struct _JackpotData_t {
	JackpotBase mJackpotBase;
	JackpotTotal mJackpotTotal;
}JackpotData;
#endif // !__COM_DATA_H__
