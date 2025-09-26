#ifndef __COM_DATA_H__
#define __COM_DATA_H__
#include "../common/platform.h"
//����ĸ��ʽ�
/*
* 3 С�ʽ�
* 2 �вʽ�
* 1 ��ʽ�
* 0 �޴�ʽ�
*/
#define JACKPOT_MAX 4
#define JM_MAX 2
#define DATA_BIT 5
#define TABLE_LEN 10
#define HALF_TABLE 5

typedef struct _JackpotBase_t {
	double mfBaseValue[JACKPOT_MAX];		//����ֵ
	double mfLowValue[JACKPOT_MAX];		//��С����ֵ
	double mfHighValue[JACKPOT_MAX];		//��󴥷�ֵ
	double mfMidValue[JACKPOT_MAX];		//ƽ������ֵ
	int32_t mBetThreshold[JACKPOT_MAX];				//ÿ���ʽ�����ѹ��ֵ;
	int32_t mBetThresholdMax[JACKPOT_MAX];				//ÿ���ʽ�����ѹ��ֵ;
	int32_t mJackpotTrigger[JACKPOT_MAX][TABLE_LEN];
	int32_t mJackpotIdx[JACKPOT_MAX];

	double mfOutPercent[JACKPOT_MAX];		//��ʾֵ�İٷֱ�   
	double mfInnerPercent[JACKPOT_MAX];		//����ֵ�İٷֱ�
	//double mfSettingPercent[JACKPOT_MAX];	//�趨�Ĳʽ�ٷֱ�

	int32_t mJpWeight[JACKPOT_MAX];		//�ʽ����
	int32_t mJpTotalWeight;				//�ʽ�����ܺ�
	double mJpTriggerDiff[JACKPOT_MAX];		//������ֵ
	//2025/01/11�����޸�
	double mfRangeValue[JACKPOT_MAX];	//�ʽ𿪽���Χ
}JackpotBase;

typedef struct _JackpotTotal_t {
	//�������
	double mfPullRemainCent[JACKPOT_MAX];	  //�ʽ��������
	double mfOutPullRemainCent[JACKPOT_MAX];  //�ʽ���ʾ�������
	double mfInnerPullRemainCent[JACKPOT_MAX];  //�ʽ������������

	double mfJpCent[JACKPOT_MAX];			//�����Ĳʽ����
	double mfInnerRemainCent[JACKPOT_MAX];	//�ڲ�ʣ�����
	double mfInnerClPullCent[JACKPOT_MAX];	//�ڲ��ʽ��Ღ��

	double mfTotalCent[JACKPOT_MAX];		//�ʽ𿪽��ۻ���
	double mfOutTotalCent[JACKPOT_MAX];		//�ʽ���ʾ�ۻ���
	double mfTotalLottery[JACKPOT_MAX];		//�����ʽ���۷���

	//�ʽ���ʾ�Ღ��
	double mfOutPullCent[JACKPOT_MAX];
	//�ʽ������Ღ��
	double mfInnerPullCent[JACKPOT_MAX];

	//�Ღ��
	double mfOutTotalPullCent[JACKPOT_MAX];						//�ʽ���ʾ�ۼ��Ღ��
	double mfInnerTotalPullCent[JACKPOT_MAX];					//�ʽ������ۼ��Ღ��
	double mfTotalPullCent[JACKPOT_MAX];						//�ʽ��ۼ���η�
	double mfJpPullCent[JACKPOT_MAX];						//4���ʽ�Ĳʽ���η�

	//��������
	double mfCompensateCent[JACKPOT_MAX];	//��������
	double mfCompensatePullCent[JACKPOT_MAX];	//�����ʽ��Ღ��
	double mfCompensatePullRemainCent[JACKPOT_MAX];	//�����ʽ��Ღ����
	double mfDiffCent[JACKPOT_MAX];	//��ֵ

	double mfPullRemainCentTotal;					//���еĲʽ��Ღ����
}JackpotTotal;

typedef struct _JackpotData_t {
	JackpotBase mJackpotBase;
	JackpotTotal mJackpotTotal;
}JackpotData;
#endif // !__COM_DATA_H__
