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
	*  ����DevInit���ٵ���DevSetScorePoolData
	*  ScorePoolData ����������Ҫ�洢�����ݿ�����ģ�ÿ����������Ҫ��������Ȼ�����DevSetScorePoolData�����㷨��
	*  app��Ҫ��ʱ����DevGetScorePoolData��ȡScorePoolData�����б���
	*  funded����Ϊ����������Ĭ��ֵ��Ϊ10000
	*/
	USBEXPORT void DevInit(int gameState, int dif, int funded);

	/*
	* slot ÿ��spinʱ�����������ȡ���
	* bet��ǰ��Ѻע
	* srtp������ѹ����Ӯ����ĵ�ǰRTP
	*/
	USBEXPORT int DevSpin(int pid, int bet, pull_data* pPullData, slot_data* pSlotData, LinkInfo* pLinkInfo);
	

	/*
	* ��������ʱ�����pPullData��pSlotData��pLinkInfo�����ݿ�����ֵ�Ļ���
	* ����Ҫ��������ӿڽ��������õ�dll��,���������DevInit����á�
	*/
	USBEXPORT void DevSetData(int sel, pull_data* pPullData, slot_data* pSlotData, LinkInfo* pLinkInfo);

	/*
	* ÿ�ο����ʽ𣬱��ݲʽ𶯻����������
	*/
	USBEXPORT void DevJpEnd(void);
	/*
	* ��ȡdll�İ汾��
	*/
	USBEXPORT void DevVersion(char* pVer);

	/*
	* ���ò�������
	*/
	USBEXPORT void DevSetWaveScore(int waveScore);

	/*
	* ��ȡ��������
	*/
	USBEXPORT int DevGetWaveScore(void);

	/*
	* ��ȡ��ǰ��λ�Ĳʽ���ʾֵ
	*/
	USBEXPORT void DevGetJackpot(int sel, JackpotInitInfo* pJackpotInitInfo);

	/*
	* ���òʽ�ı�������
	*/
	USBEXPORT void DevSetJackpotData(int sel, JackpotData* pJackpotData);

	/*
	*  ��ȡ�ʽ���Ҫ���������
	*/
	USBEXPORT void DevGetJackpotData(int sel, JackpotData* pJackpotData);

	/*
	* ��ȡ�ϱ�����
	*/
	USBEXPORT void DevGetSummary(Summary* pSummary);

	/*
	* �����ϱ�����
	*/
	USBEXPORT void DevSetSummary(Summary* pSummary);

	/*
	* ������ʾRTP
	*/
	USBEXPORT void DevSummary(void);
	////////////////////////////////////////////////////////////////////////////////
	/*
	* һ���ĸ��ӿ�����
	*/
	USBEXPORT void DevSetScorePoolData(ScorePoolData* spd);
	USBEXPORT void DevGetScorePoolData(ScorePoolData* spd);
	USBEXPORT void DevSetLevel(int level);
	USBEXPORT int DevGetLevel(void);
	////////////////////////////////////////////////////////////////////////////////
}
	

#endif//__USB_DEV_H__