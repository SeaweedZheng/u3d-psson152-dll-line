#ifndef __USB_DEV_H__
#define __USB_DEV_H__

#if _WIN32
#define USBEXPORT _declspec(dllexport)
#else
#define USBEXPORT
#endif//_WIN32
#include "./GameAlgo/godwdaddy/CommonStruct.h"

extern "C" {
	/*
	*  调用DevInit后再调用DevSetScorePoolData
	*  ScorePoolData 的数据是需要存储在数据库里面的，每次启动是需要读出数据然后调用DevSetScorePoolData传到算法内
	*  app需要定时调用DevGetScorePoolData获取ScorePoolData来进行保存
	*  funded现在为波动分数，默认值设为10000
	*/
	USBEXPORT void DevInit(int gameState, int dif, int funded);

	/*
	* slot 每次spin时调用这个来获取结果
	* bet当前的押注
	* srtp根据总压，总赢算出的当前RTP
	*/
	USBEXPORT int DevSpin(int pid, int bet, pull_data* pPullData, slot_data* pSlotData, LinkInfo* pLinkInfo);
	

	/*
	* 机器启动时，如果pPullData，pSlotData，pLinkInfo在数据库中有值的话，
	* 就需要调用这个接口将数据设置到dll中,这个函数在DevInit后调用。
	*/
	USBEXPORT void DevSetData(int sel, pull_data* pPullData, slot_data* pSlotData, LinkInfo* pLinkInfo);

	/*
	* 每次开出彩金，表演彩金动画结束后调用
	*/
	USBEXPORT void DevJpEnd(void);
	/*
	* 获取dll的版本号
	*/
	USBEXPORT void DevVersion(char* pVer);

	/*
	* 设置波动分数
	*/
	USBEXPORT void DevSetWaveScore(int waveScore);

	/*
	* 获取波动分数
	*/
	USBEXPORT int DevGetWaveScore(void);

	/*
	* 获取当前档位的彩金显示值
	*/
	USBEXPORT void DevGetJackpot(int sel, JackpotInitInfo* pJackpotInitInfo);

	/*
	* 设置彩金的保存数据
	*/
	USBEXPORT void DevSetJackpotData(int sel, JackpotData* pJackpotData);

	/*
	*  获取彩金需要保存的数据
	*/
	USBEXPORT void DevGetJackpotData(int sel, JackpotData* pJackpotData);

	/*
	* 获取上报数据
	*/
	USBEXPORT void DevGetSummary(Summary* pSummary);

	/*
	* 设置上报数据
	*/
	USBEXPORT void DevSetSummary(Summary* pSummary);

	/*
	* 测试显示RTP
	*/
	USBEXPORT void DevSummary(void);
	////////////////////////////////////////////////////////////////////////////////
	/*
	* 一下四个接口弃用
	*/
	USBEXPORT void DevSetScorePoolData(ScorePoolData* spd);
	USBEXPORT void DevGetScorePoolData(ScorePoolData* spd);
	USBEXPORT void DevSetLevel(int level);
	USBEXPORT int DevGetLevel(void);
	////////////////////////////////////////////////////////////////////////////////
}
	

#endif//__USB_DEV_H__