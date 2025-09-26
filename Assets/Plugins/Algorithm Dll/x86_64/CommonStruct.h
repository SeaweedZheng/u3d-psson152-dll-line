#ifndef __COMMON_STRUCT_H__
#define __COMMON_STRUCT_H__
#include "comdata.h"
#include "../common/platform.h"
#include "../common/map.h"

#define BONUS_MODULE_SIZE 10
#define ID_MAX 32
#define MAX_RTP 5
#define MAX_MDL 3
#define ARRAY_SIZE 20
#ifndef UINT8_MAX
#define UINT8_MAX        0xff
#endif//UINT8_MAX
#ifndef UINT16_MAX
#define UINT16_MAX       0xffff
#endif//UINT16_MAX

#define MAX_REEL_SIZE 5

//积分库相关
#define SEG_MAX				8		
#define SEG_LEN				2 
#define SEG_ROUND			20
#define SEG_RAID			40

#define F_MAX		6
#define TB_MAX		10
#define JP_RATE             0.06
#define LINK_PER            (7.0/9.0)
#define PULL_RATE           1.05
#define SLOT_MAX	        180			//连线抽奖的阈值
#define FREE_MAX            880

#define LINK_POS_MAX        9

#define GS_NORMAL           1
#define GS_FREE             2
#define GS_JP               3
//线数
static const uint8_t gLines[] = { 1,2,3 };
static const uint8_t gNums[] = { 6,3,2 };

/*
#define S1 1
#define P1 2
#define P2 3
#define P3 4
#define P4 5
#define P5 6
#define A  7
#define K  8
#define Q  9
#define J  10
*/
static const uint8_t gAllIcons[] = {
    1,2,3,4,5,6,7,8,9,10,
};

//中几连线
static const uint8_t gLinks[] = { 3,4,5 };
static const uint8_t gLinkNums[] = { 4,3,2 };

//免费游戏
static const uint8_t gFreeRounds[] = { 6,9,12 };
static const uint8_t gFreeNums[] = { 3,2,1 };

//赔率表
static const uint16_t gPayTable[][5] = {
    {0,0,50,200,500},		//S1 玉如意
    {0,0,45,180,450},		//P1 福袋
    {0,0,40,160,400},		//P2 红包
    {0,0,35,140,350},		//P3 炮竹
    {0,0,30,120,300},		//P4 橘子
    {0,0,25,100,250},		//A
    {0,0,20,80,200},		//K
    {0,0,15,60,150},		//Q
    {0,0,10,40,100},		//J
    {0,0,5,20,50},			//10
};

//线组合
//3条线  1线 * 10000 + 2线*100 + 3线
static const uint32_t gTribles[] = {
    10203,20406,20810,21214,21618,22022,22426,23032,30709,31113,31517,
    31921,32325,32931,51428,61327,92650,102549,173241,183137,344145,363747
};

//2条线 1线 * 100 + 2线
static const uint16_t gDoubles[] = {
    203,305,405,406,510,516,522,524,532,547,548,609,615,621,623,631,645,646,709,
    710,737,741,809,810,837,841,914,918,922,932,1013,1017,1021,1031,1113,1114,
    1213,1214,1318,1322,1324,1327,1330,1333,1334,1417,1421,1423,1428,1429,1435,
    1436,1517,1518,1617,1618,1744,1840,1921,1922,1933,1935,1946,1948,1949,1950,
    2021,2022,2033,2035,2046,2048,2049,2050,2126,2139,2140,2142,2143,2225,2238,
    2239,2243,2244,2325,2326,2425,2426,2527,2528,2533,2548,2549,2627,2628,2635,
    2646,2650,2931,2932,3031,3032,3137,3138,3241,3242,3339,3341,3342,3343,3344,
    3444,3537,3538,3539,3540,3543,3640,3747,3748,3847,3848,3850,3946,3948,3949,
    3950,4048,4049,4145,4146,4245,4246,4249,4346,4348,4349,4350,4446,4450
};

//1条线
static const uint8_t gOnes[] = {
    1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,
    18,19,20,21,22,23,24,25,26,27,28,29,30,31,
    32,33,34,35,36,37,38,39,40,41,42,43,44,45,
    46,47,48,49,50
};

#define JP_COUNT	4
//波动相关
#define WV_ONE				1 //0001
#define WV_TWO				2 //0010
#define WV_THREE			3 //0011
#define WV_FOUR				4 //0100

typedef struct ColorInfos_t {
    uint8_t npos;		//当前位置
    uint8_t pos;		//目标位置
    int64_t color;		//颜色的组合
}ColorInfos;
static const uint8_t gWaveTableCounts[] = { 4,2,1,1 };
static const uint64_t gWaveTable[] = { WV_ONE,WV_TWO,WV_THREE,WV_FOUR };

typedef struct _ga_rtp {
    double set;                                // 设定几率
    double app;                                // 应用几率
    double use;                                // 使用几率
    double jackpot;                            // 彩金几率
    double box;                                // 宝箱几率
    double give;							   // 送灯几率
    double slam;							   // 满贯几率
    double natural;                            // 自然几率
} ga_rtp;

typedef struct _JackpotInitInfo {
    double jackpotout[JACKPOT_MAX];		//彩金显示积累分 
}JackpotInitInfo;

typedef struct _fgInfos {
    uint8_t freeGame;			//标明是否是免费游戏
    uint16_t freeCurRound;
    uint16_t freeMaxRound;
    uint32_t reward;
}fgInfos;

typedef struct _slot_data {
    fgInfos  fgInfo;
    uint8_t linePos;
    uint8_t linkPos;
    uint8_t RewardPos;
    uint8_t freePos;
    uint8_t jpPos;
    uint8_t lines[11];
    uint8_t links[9];
    uint8_t rewards[TB_MAX];  //10
    uint8_t frees[F_MAX];    //6
    uint8_t waveDir;        //波动方向
    int8_t waveLevel;
    int32_t waveScore;
    int32_t waveBets;
    uint8_t mWaveIdx;
    int8_t  mWaveRaid[32];
    ColorInfos mWaveUp[2];
    ColorInfos mWaveDown[2];

    int32_t totalBets;
    int32_t totalWins;
}slot_data;

typedef struct _LinkData {
    uint8_t icon;			//图标
    uint8_t link;			//几连
    uint32_t reward;			//得分
}LinkData;

typedef struct _LinkInfo {
    uint8_t gameState;
    uint8_t lineNum;
    uint16_t curRound;
    uint16_t maxRound;
    uint16_t num;
    uint32_t lineMark;
    int32_t lottery[4];	//0:表示没有开出彩金，1:表示已开出彩金
    double jackpotlottery[4];	//开出的彩金,注意:此处的单位是钱的单位，而且是乘以了100的，分机收到这个值要根据分机的分值比来转成成对应的分数，而且还要将此值除以100
    double jackpotout[4];		//彩金显示积累分
    double jackpotold[4];		//开出彩金前的显示积累分
    LinkData  linkData[3];		//最多三条线
}LinkInfo;

typedef struct _pull_data {
    //uint32_t flag;
    //提拨分数
    double scorePull;			//已提拨分数

    //连线提拨库
    double slotRemain;			//连线提拨余额
    int32_t slotPullTotal;
    int32_t slotPull;

    double reelPullRemain;      //转轮提拨余额
    int32_t reelPull;           //转轮累积提拨库

    //免费提拨库
    double freeRemain;
    int32_t freePullTotal;
    int32_t freePull;
}pull_data;

typedef struct _SectionData_t {
    uint8_t mRoundCounter;				//局数计数器 用于20局的计算
    uint32_t funded;					//自备款
    uint32_t mSectionCounter[SEG_LEN][SEG_MAX];
    int64_t mSection[SEG_LEN][SEG_MAX];		//统计用

    uint32_t mPerBet[SEG_ROUND];		//押分
    uint32_t mPerWin[SEG_ROUND];		//实中分数
    int32_t mPullRemain[SEG_ROUND];		//提拨分数
    double mOneRemain;					//提拨余数
    double mPerScore[SEG_ROUND];		//使用机率分数

    uint32_t mAvgBets;
    ga_rtp mRtp;
    int64_t mTotalPullScore;			//累计提拨分数
    uint32_t mSegRoundThresHold;
    int32_t mTotalPullRemain;			//每20局总的提拨分数
    double mTotalRtpScore;				//总的使用机率分数
    double mSectionSave[SEG_LEN];			//两段库
    double mSectionUse;				//用于使用的那一段库
    //double mRemain;						//提拨余数
    double mRoundBets;					//20局的总押分数
    double mRoundWins;					//20局的总得分数
}SectionData;

typedef struct _RaidData_t {
    uint8_t mSegDttCounter;
    uint8_t mSegDtCounter;
    uint32_t mSegThresHold;
    int32_t mSegThresHoldData[SEG_ROUND];
    int32_t mSegData[SEG_RAID];
    int64_t mSectionInTotal[SEG_LEN][SEG_MAX];		//统计用
    int64_t mSectionOutTotal[SEG_LEN][SEG_MAX];	//统计用
}RaidData;

typedef struct _ScorePoolData_t {
    uint32_t flag;
    uint32_t funded;
    SectionData mSectionData;
    RaidData mRaidData;
}ScorePoolData;

typedef struct _FreeGameSummary {
    int32_t round[3];       //免费游戏轮数
    int32_t counter[3];     //次数占比
    int32_t score[3];       //免费游戏得分
}FreeGameSummary;

typedef struct _IconSummary {
    int32_t counter[10][3];     //连线次数
    int32_t score[10][3];       //连线得分
}IconSummary;

typedef struct _JpSummary {
    int32_t counter[4][4];      //4个档位的4个彩金次数
    int32_t score[4][4];
}JpSummary;

typedef struct _StoreSummary {
    double reelTotalPull[4];        //转轮累积提拨库
    double linkTotalPull[4];        //连线累积提拨库
    double linkPull[4];             //连线提拨库
    double linkTotalScore[4];       //连线总得分数
    double freeTotalPull[4];        //免费累积提拨库
    double freePull[4];             //免费提拨库
    double freeTotalScore[4];       //免费游戏得分数
}StoreSummary;

typedef struct _Summary {
    FreeGameSummary freeGameSummary;
    IconSummary iconSummary;
    JpSummary jpSummary;
    StoreSummary storeSummary;
}Summary;

static const uint16_t kScatterLineId = 0;
static const uint8_t kInvalidSymbol = UINT8_MAX;
static const uint8_t kInvalidOption = UINT8_MAX;
static const uint16_t kInvalidCondition = UINT16_MAX;
static const uint8_t kMaxReelSize = 5;
static const uint8_t kMaxSymbolsPerReel = 4;

static const uint16_t kInvalidLineId = UINT16_MAX;

enum PayType {
    kUSL2R = 0,
    kUSR2L,
    kUS2Way,
    kScatterL2R,
    kScatterR2L,
    kScatter2Way,
    kScatterCount,
    kScatterOnLineL2R,
};
enum MultiplierType {
    kDependent = 0,
    kIndependent,
};
enum MultiplierAccType {
    kAccumulate = 0,
    kNonAccumulate,
};
enum WildType {
    kIsNotWild = 0,
    kNormalWild,
    kExpendingWild
};

enum ComputeLinkWay {
    kL2R,
    kR2L
};
typedef struct _BonusInfo {
    uint8_t  multiplier;
    uint16_t get_times;
    uint16_t max_times;
    uint16_t play_times;
    uint16_t module_id;
    uint8_t bonus_times[5];
}BonusInfo;

typedef struct _FirstRegFirstPlay {
    BonusInfo b_i_group[2];
}FirstRegFirstPlay;

typedef struct _BonusCanTriggerTimes
{
    uint16_t moule_id_;
    uint16_t times_;
}BonusCanTriggerTimes;

//typedef struct _BonuseInfoItem {
//    char ModuleID[32];
//    uint8_t MaxTimes;
//}BonuseInfoItem;

typedef struct _BonusGame {
    uint16_t moule_id_;
    uint16_t max_times;
}BonusGame;

typedef struct _Strip {
    uint8_t reelSize;
    //uint8_t weightSize;
    uint16_t reel[86];
}Strip;

typedef struct _IndMultiplier
{
    uint8_t icon_id;
    uint8_t pos[1];
    uint8_t multiplier[1];
    uint8_t multiplier_weight[1];

}IndMultiplier;

typedef struct _Symbol {
    BOOL wild_substitute;
    uint8_t id;
    uint8_t multiplier_type;
    uint8_t multiplier_acc;
    uint8_t pay_type;
    uint8_t wild_type;
    uint8_t equivalence[1];
    uint16_t pay[5];
    BonusInfo bonus_info[2];
    uint8_t not_use_multiplier_count[1];
    uint16_t multiplier[1];
    uint16_t multiplier_weight[1];
}Symbol;

typedef struct _SymbolCfg {
    BOOL IsWildSubstitute;
    uint8_t IconID;
    uint8_t BonusTimes[5];
    uint8_t MultiplierAcc;
    uint8_t PayType;
    uint8_t MultiplierType;
    uint8_t MultiplierWin[1];       //multiplier
    uint8_t MultiplierWeight[1];
    uint8_t WildType;
    uint16_t BonusID;
    uint16_t IconPays[5];
    uint8_t not_use_multiplier_count[1];
}SymbolCfg;

//typedef struct _SlotModuleConfig {
//    uint8_t strip_type;
//    uint8_t game_type;
//    uint8_t game_type_detail;
//    char module_id[ID_MAX];
//    Vector bonus_list;
//    Vector max_eliminate_times;
//
//    Vector strip_table;          //  typedef std::vector<Strip> StripTable;  StripTable strip_table;
//    Vector wild_id;              //std::vector<uint8_t> wild_id;
//    Vector symbols;             // std::vector<Symbol> symbols;
//    Vector ind_multiplier;      // std::vector<IndMultiplier> ind_multiplier;
//
//}SlotModuleConfig;

//typedef struct _SlotRtpConfig {
//    char rtp[ID_MAX];
//    char rtp_id[ID_MAX];
//}SlotRtpConfig;

//typedef struct _BetConfig {
//    char project_id[ID_MAX];
//    uint32_t bet;
//    uint16_t select_line_count;
//}BetConfig;

//typedef struct _RTPConfig {
//    char rtp[ID_MAX];
//    char rtp_id[ID_MAX];
//    Vector bet_list;
//}RTPConfig;

//typedef struct _NormalGameConfig {
//    uint32_t gameType;
//    char game_id[ID_MAX];
//    Vector bet;
//    map_void_t rtp_map;
//}NormalGameConfig;

typedef struct _TriggerBonus {
    uint8_t icon_id;
    uint16_t get_times;
    uint16_t multiplier;
    uint16_t bonus_id;
}TriggerBonus;

typedef struct _FixedSymbol {
    uint8_t id;
    uint8_t pos;
}FixedSymbol;

typedef struct _ChangeCandidate {
    uint8_t symbol_id;
    uint32_t weight;
}ChangeCandidate;

//typedef struct _MultiplierForChange {
//    uint8_t symbol_id;
//    uint16_t multiplier[1];                  //vector<uint16_t> multiplier;
//    uint16_t weight[1];                      //vector<uint32_t> weight;
//}MultiplierForChange;

typedef struct _ChangeLink {
    uint8_t change_symbol_size;
    uint8_t change_symbol[6];               //std::vector<uint8_t> change_symbol;
    uint8_t candidate_size;
    ChangeCandidate candidate[5];                   //std::vector<ChangeCandidate> candidate;
   // MultiplierForChange multiplier_for_change[1];       //std::vector<MultiplierForChange> multiplier_for_change;
}ChangeLink;

typedef struct _ChangeCandidateCfg {
    uint8_t IconID;
    uint32_t Weight;
}ChangeCandidateCfg;

typedef struct _ChangeLinkCfg {
    uint8_t ChangeIconId[6];               //std::vector<uint8_t> change_symbol;
    ChangeCandidateCfg ChangeCandidate[5];
}ChangeLinkCfg;

//typedef struct _ChangeLinkSlotModuleConfig {
//    SlotModuleConfig moduleConfig;
//    ChangeLink change_link_;
//    Vector fixed_symbol_vec;                // FixedSymbolVector fixed_symbol_vec; std::vector<FixedSymbol>
//}ChangeLinkSlotModuleConfig;

typedef struct _FeatureBuy {
    uint8_t symbol_id;
    uint32_t cost;
    uint16_t bonus_id;
    /*
    *       typedef vector<uint16_t> rng;
			typedef vector<rng> rng_vec;
			typedef vector<uint32_t> weight;
    */
   /* Vector rng;
    Vector rng_vec;
    Vector weight;*/
    uint32_t bonus_weight[1];            //vector<uint32_t> bonus_weight;
    uint32_t bonus_times[1];             //vector<uint32_t> bonus_times;
    uint32_t script_rng_vec[1];          //std::vector<rng_vec> script_rng_vec;
    uint32_t script_weight[1];           //std::vector<weight> script_weight;
}FeatureBuy;

//typedef struct _FeatureBuyModule {
//    Vector feature_buy_vec;         //vector<FeatureBuy> feature_buy_vec;
//}FeatureBuyModule;

//typedef struct _FeatureBuySlot{
//    SlotModuleConfig moduleConfig;
//    FeatureBuyModule featureBuyModule;
//}FeatureBuySlot;

typedef struct _ChangeMultiplier {
    uint16_t base_cost;
    uint16_t multi_level[10];                         // vector<uint16_t> multi_level
    uint16_t multiple[10][7];                            //vector<vector<uint16_t>> multiple;
    uint16_t multi_weight[10][7];                            //vector<vector<uint32_t>> multi_weight;
}ChangeMultiplier;

typedef struct _ChangeMultiplierCfg {
    uint16_t BaseCost;
    uint16_t MutiLevel[10];                         // vector<uint16_t> multi_level
    uint8_t Multiple[10][7];                            //vector<vector<uint16_t>> multiple;
    uint8_t MultiWeight[10][7];                            //vector<vector<uint32_t>> multi_weight;
}ChangeMultiplierCfg;

typedef struct _OneLineDefMask
{
    uint16_t id;
    BOOL is_used;
    int64_t mask;
}OneLineDefMask;

typedef struct _CreateResultArg
{
    uint8_t change_icon_;
    uint16_t multiplier_vec_[10];
    uint16_t external_multiplier_;
    uint32_t rng_[ARRAY_SIZE];
    uint16_t symbol_multiplier_[ARRAY_SIZE];
    uint8_t symbol_pattern_[ARRAY_SIZE];
    OneLineDefMask lineDef;
    BonusCanTriggerTimes bonus_data_[2]; //const BonusData* bonus_data_;
}CreateResultArg;

typedef struct _WinLine
{
    BOOL use_multiplier;
    uint8_t icon_id;
    uint8_t link_cnt;
    uint16_t multiplier;
    uint16_t line_id;
    uint16_t max_multiplier_alone;
    uint32_t pay;
    uint32_t total_pay;
    int64_t  winposmask;
}WinLine;

typedef struct _WinLineList
{
    uint32_t total_pay;
    uint8_t lineSize;
    WinLine line[10];
}WinLineList;

typedef struct _ComputeIcocLinkArg
{
    uint16_t external_multiplier_;
    int64_t line_;
    Symbol* symbol;
    uint8_t symbol_pattern_[ARRAY_SIZE];
    uint16_t symbol_multiplier_[ARRAY_SIZE];
    uint8_t compute_link_way_;
    uint8_t wild_id_;        //vector<uint8_t>
}ComputeIcocLinkArg;

typedef struct _GetWinMaskParm
{
    uint32_t noise_win_mask;
    uint32_t icon_mask;
    uint32_t reel_mask[MAX_REEL_SIZE];
    uint32_t win_mask_filter[MAX_REEL_SIZE];
}GetWinMaskParm;

typedef struct _ComputeMultiplierArg
{
    uint8_t multiplier_ccc_type_;
    uint8_t compute_link_way_;
    int64_t result_mask_;
    uint16_t multiplier_pattern_[ARRAY_SIZE];
}ComputeMultiplierArg;

typedef struct _ComputeMultiplierResult
{
    uint16_t total_multiplier;
    uint16_t max_multiplier_alone;
}ComputeMultiplierResult;

typedef struct _ComputeScatterArg
{
    uint16_t external_multiplier_;
    int64_t line_;
    OneLineDefMask lineDef;
    Symbol* symbol;
    uint8_t icon_pattern_[ARRAY_SIZE];
    uint16_t symbol_multiplier_[ARRAY_SIZE];
    uint8_t compute_link_way_;
    uint8_t wild_id_;        //vector<uint8_t>
    BonusCanTriggerTimes bonus_data_[2];
}ComputeScatterArg;

typedef struct _ScatterWinData
{
   // uint8_t curStatus;
    WinLineList win_line_list;
    //TODO 先实现常规
    // TriggerBonusVec trigger_bonus_vec;
    TriggerBonus trigger_bonus_vec[2];
}ScatterWinData;

typedef struct _OneScatterWinData
{
   // uint8_t curStatus;
    WinLine win_line;
    TriggerBonus trigger_bonus_vec[2];     //TriggerBonusVec trigger_bonus_vec;
}OneScatterWinData;

typedef struct _SlotResult {
    uint8_t s_less;
    uint8_t isChange;
    int8_t result;
    uint16_t freeTimes;
    uint16_t curFreeTimes;
    uint16_t module_id;
    uint32_t s_NowFreeRtp;
    uint32_t curStatus;
    uint32_t nextStatus;
    uint32_t total_pay;
    uint16_t external_multiplier;
    uint16_t can_trigger_bonus_times[BONUS_MODULE_SIZE];
    //uint8_t trigger_bonus_size;
    TriggerBonus trigger_bonus_vec[2];           // TriggerBonusVec trigger_bonus_vec;
    uint32_t jp_pay_cent;
    uint32_t Bet;
    uint8_t icon_pattern[ARRAY_SIZE];
    uint16_t icon_multiplier[ARRAY_SIZE];
    uint16_t select_line_count;
    double times;
    /*
    *  WinLineList r2l_winline_vec;
      WinLineList scatter_winline_vec;
      WinLineList winline_vec;
    */
    WinLineList r2l_winline_vec;
    WinLineList scatter_winline_vec;
    WinLineList winline_vec;
    uint16_t multiplier_vec[10];
    //for ChangeSymbolSlotResult
    uint8_t change_candidate;
    uint16_t multiplier_for_change;
    WinLineList change_winline_vec;
}SlotResult;

typedef struct _ga_conf_context {
    uint32_t TypeOfPlace;                     // 场地类型，0：普通，1：技巧，2：专家
    uint32_t difficulty;                      // 难度，0~8
    uint32_t odds;                            // 倍率，0：低倍率，1：高倍率，2：随机
    uint32_t discount;                        // 放水
    uint32_t funded;                          // 自备款
    uint32_t wave;                            // 波动分数
    uint32_t BetsMinOfJackpot;                // 中彩金最小押分值
    uint32_t LimitBetsWins;                   // 限红
} ga_conf_context;
#endif // !__COMMON_STRUCT_H__
