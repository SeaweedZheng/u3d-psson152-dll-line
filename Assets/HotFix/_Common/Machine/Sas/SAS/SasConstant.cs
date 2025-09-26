using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SasConstant 
{
    static public string SAS_EXCEPTION = "exception";

    static public string SAS_SUB_TICKET = "egm";
    //General poll exceoption codes enum
   
    public static int DOOR_OPEND = 0x11;              //Slot door was opened
    public static int DOOR_CLOSED = 0x12;             //Slot door was closed
    public static int DROP_DOOR_OPENED = 0x13;        //Drop door was opend
    public static int DROP_DOOR_CLOSED = 0x14;        //Drop door was closed
    public static int CARD_CAGE_OPENED = 0x15;        //card cage was opened
    public static int CARD_CAGE_CLOSED = 0x16;        //card cage was closed
    public static int AC_POWER_APPLIED = 0x17;        //AC power was applied to gaming machine
    public static int AC_POWER_LOST = 0x18;           //AC power was lost from gaming machine
    public static int CASH_BOX_DOOR_OPENED = 0x19;    //Cashbox door was opened
    public static int CASH_BOX_DOOR_CLOSED = 0x1A;    //Cashbox door was closed
    public static int CASH_BOX_DOOR_REMOVED = 0x1B;   //Cashbox was removed 
    public static int CASH_BOX_DOOR_INSTALLED = 0x1C; //Cashbox was installed
    public static int BELLY_DOOR_OPENED = 0x1D;       //Belly door was opened
    public static int BELLY_DOOR_CLOSED = 0x1E;       //Belly door was closed 

    public static int NO_ACTIVITY_1F = 0x1F;              // 1F 12-6, No activity and waiting for player input (obsolete)
    public static int GENERAL_TILT = 0x20;              // 20 Q General tilt (Use this tilt when other exception tilt codes do not apply or when the tilt condition cannot be determined.)
    public static int COIN_IN_TILT = 0x21;              // 21 Q Coin in tilt
    public static int COIN_OUT_TILT = 0x22;              // 22 Q Coin out tilt
    public static int HOPPER_EMPTY_DETECTED = 0x23;              // 23 Q Hopper empty detected
    public static int EXTRA_COIN_PAID = 0x24;              // 24 Q Extra coin paid
    public static int DIVERTER_MALFUNCTION = 0x25;              // 25 Q Diverter malfunction (controls coins to drop or hopper)
    public static int CASHBOX_FULL_DETECTED = 0x27;              // 27 Q Cashbox full detected
    public static int BILL_JAM = 0x28;              // 28 Q Bill jam
    public static int BILL_ACCEPTOR_HARDWARE_FAILURE = 0x29;              // 29 Q Bill acceptor hardware failure
    public static int REVERSE_BILL_DETECTED = 0x2A;              // 2A Q Reverse bill detected
    public static int BILL_REJECTED = 0x2B;              // 2B Q Bill rejected
    public static int COUNTERFEIT_BILL_DETECTED = 0x2C;              // 2C Q Counterfeit bill detected
    public static int REVERSE_COIN_IN_DETECTED = 0x2D;              // 2D Q Reverse coin in detected
    public static int CASHBOX_NEAR_FULL_DETECTED = 0x2E;              // 2E Q Cashbox near full detected
    public static int CMOS_RAM_ERROR_DATA_RECOVERED = 0x31;              // 31 Q CMOS RAM error (data recovered from EEPROM)
    public static int CMOS_RAM_ERROR_NO_DATA_RECOVERED = 0x32;              // 32 Q CMOS RAM error (no data recovered from EEPROM)
    public static int CMOS_RAM_ERROR_BAD_DEVICE = 0x33;              // 33 Q CMOS RAM error (bad device)
    public static int EEPROM_ERROR_DATA_ERROR = 0x34;              // 34 Q EEPROM error (data error)
    public static int EEPROM_ERROR_BAD_DEVICE = 0x35;              // 35 Q EEPROM error (bad device)
    public static int EPROM_ERROR_CHECKSUM_VERSION_CHANGED = 0x36;              // 36 Q EPROM error (different checksum – version changed)
    public static int EPROM_ERROR_BAD_CHECKSUM_COMPARE = 0x37;              // 37 Q EPROM error (bad checksum compare)
    public static int PARTITIONED_EPROM_ERROR_CHECKSUM_VERSION_CHANGED = 0x38;              // 38 Q Partitioned EPROM error (checksum – version changed)
    public static int PARTITIONED_EPROM_ERROR_BAD_CHECKSUM_COMPARE = 0x39;              // 39 Q Partitioned EPROM error (bad checksum compare)
    public static int MEMORY_ERROR_RESET_BY_OPERATOR = 0x3A;              // 3A Q Memory error reset (operator used self test switch)
    public static int LOW_BACKUP_BATTERY_DETECTED = 0x3B;              // 3B Q Low backup battery detected
    public static int OPERATOR_CHANGED_OPTIONS = 0x3C;              // 3C Q Operator changed options (This is sent whenever the operator changes configuration options. This includes, but is not limited to, denomination, gaming machine address, or any option that affects the response to long polls 1F, 53, 54, 56, A0, B2, B3, B4, or B5.)
    public static int CASH_OUT_TICKET_PRINTED = 0x3D;              // 3D Q/P 15-1 A cash out ticket has been printed
    public static int HANDPAY_VALIDATED = 0x3E;              // 3E Q/P 15-1 A handpay has been validated
    public static int VALIDATION_ID_NOT_CONFIGURED = 0x3F;              // 3F P 15-1 Validation ID not configured
    public static int REEL_TILT_UNSPECIFIED = 0x40;              // 40 Q Reel Tilt (Which reel is not specified.)
    public static int REEL_1_TILT = 0x41;              // 41 Q Reel 1 tilt
    public static int REEL_2_TILT = 0x42;              // 42 Q Reel 2 tilt
    public static int REEL_3_TILT = 0x43;              // 43 Q Reel 3 tilt
    public static int REEL_4_TILT = 0x44;              // 44 Q Reel 4 tilt
    public static int REEL_5_TILT = 0x45;              // 45 Q Reel 5 tilt
    public static int REEL_MECHANISM_DISCONNECTED = 0x46;              // 46 Q Reel mechanism disconnected
    public static int BILL_ACCEPTED_1_DOLLAR = 0x47;              // 47 Q 7-15 $1.00 bill accepted (non-RTE only)
    public static int BILL_ACCEPTED_5_DOLLARS = 0x48;              // 48 Q 7-15 $5.00 bill accepted (non-RTE only)
    public static int BILL_ACCEPTED_10_DOLLARS = 0x49;              // 49 Q 7-15 $10.00 bill accepted (non-RTE only)
    public static int BILL_ACCEPTED_20_DOLLARS = 0x4A;              // 4A Q 7-15 $20.00 bill accepted (non-RTE only)
    public static int BILL_ACCEPTED_50_DOLLARS = 0x4B;              // 4B Q 7-15 $50.00 bill accepted (non-RTE only)
    public static int BILL_ACCEPTED_100_DOLLARS = 0x4C;              // 4C Q 7-15 $100.00 bill accepted (non-RTE only)
    public static int BILL_ACCEPTED_2_DOLLARS = 0x4D;              // 4D Q 7-15 $2.00 bill accepted (non-RTE only)
    public static int BILL_ACCEPTED_500_DOLLARS = 0x4E;              // 4E Q 7-15 $500.00 bill accepted (non-RTE only)
    public static int BILL_ACCEPTED_GENERAL = 0x4F;              // 4F Q 7-15, Bill accepted (In non-RTE mode, use this exception for all bills without a
    public static int BILL_ACCEPTED_200_DOLLARS = 0x50;              // 50 Q 7-15 $200.00 bill accepted (non-RTE only)
    public static int HANDPAY_PENDING = 0x51;              // 51 Q/P 7-13 Handpay is pending (Progressive, non-progressive or cancelled credits)
    public static int HANDPAY_RESET = 0x52;              // 52 Q/P 7-13 Handpay was reset (Jackpot reset switch activated)
    public static int NO_PROGRESSIVE_INFO_RECEIVED = 0x53;              // 53 Q 10-2 No progressive information has been received for 5 seconds
    public static int PROGRESSIVE_WIN = 0x54;              // 54 Q 10-2 Progressive win (cashout device/credit paid)
    public static int PLAYER_CANCELLED_HANDPAY_REQUEST = 0x55;              // 55 Q/P Player has cancelled the handpay request
    public static int SAS_PROGRESSIVE_LEVEL_HIT = 0x56;              // 56 P 10-3 SAS progressive level hit
    public static int SYSTEM_VALIDATION_REQUEST = 0x57;              // 57 P 15-11 System validation request
    public static int PRINTER_COMMUNICATION_ERROR = 0x60;              // 60 Q Printer communication error
    public static int PRINTER_PAPER_OUT_ERROR = 0x61;              // 61 Q Printer paper out error
    public static int CASH_OUT_BUTTON_PRESSED = 0x66;              // 66 Q 8-22 Cash out button pressed
    public static int TICKET_INSERTED = 0x67;              // 67 P 15-17 Ticket has been inserted
    public static int TICKET_TRANSFER_COMPLETE = 0x68;              // 68 P 15-18 Ticket transfer complete
    public static int AFT_TRANSFER_COMPLETE = 0x69;              // 69 P 8-8 AFT transfer complete
    public static int AFT_REQUEST_FOR_HOST_CASHOUT = 0x6A;              // 6A P 8-21 AFT request for host cashout
    public static int AFT_REQUEST_FOR_HOST_TO_CASH_OUT_WIN = 0x6B;              // 6B P 8-21 AFT request for host to cash out win
    public static int AFT_REQUEST_TO_REGISTER = 0x6C;              // 6C P 8-2 AFT request to register
    public static int AFT_REGISTRATION_ACKNOWLEDGED = 0x6D;              // 6D P 8-2 AFT registration acknowledged
    public static int AFT_REGISTRATION_CANCELLED = 0x6E;              // 6E Q 8-2 AFT registration cancelled
    public static int GAME_LOCKED = 0x6F;              // 6F P 8-5 Game locked
    public static int EXCEPTION_BUFFER_OVERFLOW = 0x70;              // 70 P 2-1 Exception buffer overflow
    public static int CHANGE_LAMP_ON = 0x71;              // 71 Q Change lamp on
    public static int CHANGE_LAMP_OFF = 0x72;              // 72 Q Change lamp off
    public static int PRINTER_PAPER_LOW = 0x74;              // 74 Q Printer paper low
    public static int PRINTER_POWER_OFF = 0x75;              // 75 Q Printer power off
    public static int PRINTER_POWER_ON = 0x76;              // 76 Q Printer power on
    public static int REPLACE_PRINTER_RIBBON = 0x77;              // 77 Q Replace printer ribbon
    public static int PRINTER_CARRIAGE_JAMMED = 0x78;              // 78 Q Printer carriage jammed
    public static int COIN_IN_LOCKOUT_MALFUNCTION = 0x79;              // 79 Q Coin in lockout malfunction (coin accepted while coin mech disabled)
    public static int GAMING_MACHINE_SOFT_METERS_RESET = 0x7A;              // 7A Q Gaming machine soft (lifetime-to-date) meters reset to zero
    public static int BILL_VALIDATOR_TOTALS_RESET = 0x7B;              // 7B Q Bill validator (period) totals have been reset by an attendant/operator
    public static int LEGACY_BONUS_PAY_AWARDED_MULTIPLIED_JACKPOT = 0x7C;              // 7C Q 12-2, A legacy bonus pay awarded and/or a multiplied jackpot occurred
    public static int GAME_STARTED = 0x7E;              // 7E Q 12-3 Game has started
    public static int GAME_ENDED = 0x7F;              // 7F Q 12-4 Game has ended
    public static int HOPPER_FULL_DETECTED = 0x80;              // 80 Q Hopper full detected
    public static int HOPPER_LEVEL_LOW_DETECTED = 0x81;              // 81 Q Hopper level low detected
    public static int DISPLAY_METERS_OR_ATTENDANT_MENU_ENTERED = 0x82;              // 82 Q Display meters or attendant menu has been entered
    public static int DISPLAY_METERS_OR_ATTENDANT_MENU_EXITED = 0x83;              // 83 Q Display meters or attendant menu has been exited
    public static int SELF_TEST_OR_OPERATOR_MENU_ENTERED = 0x84;              // 84 Q Self test or operator menu has been entered
    public static int SELF_TEST_OR_OPERATOR_MENU_EXITED = 0x85;              // 85 Q Self test or operator menu has been exited
    public static int GAMING_MACHINE_OUT_OF_SERVICE = 0x86;              // 86 Q Gaming machine is out of service (by attendant)
    public static int PLAYER_REQUESTED_DRAW_CARDS = 0x87;              // 87 Q Player has requested draw cards (only send when in RTE mode)
    public static int REEL_N_STOPPED = 0x88;              // 88 Q 12-5 Reel N has stopped (only send when in RTE mode)
    public static int COIN_CREDIT_WAGERED = 0x89;              // 89 Q 12-5 Coin/credit wagered (only send when in RTE mode, and only send if the configured max bet is 10 or less)
    public static int GAME_RECALL_ENTRY_DISPLAYED = 0x8A;              // 8A Q 12-5 Game recall entry has been displayed
    public static int CARD_HELD_NOT_HELD = 0x8B;              // 8B Q 12-5 Card held/not held (only send when in RTE mode)
    public static int GAME_SELECTED = 0x8C;              // 8C Q 12-6 Game selected
    public static int COMPONENT_LIST_CHANGED = 0x8E;              // 8E Q 17-1 Component list changed
    public static int AUTHENTICATION_COMPLETE = 0x8F;              // 8F P 17-6 Authentication complete
    public static int POWER_OFF_CARD_CAGE_ACCESS = 0x98;              // 98 Q Power off card cage access
    public static int POWER_OFF_SLOT_DOOR_ACCESS = 0x99;              // 99 Q Power off slot door access
    public static int POWER_OFF_CASHBOX_DOOR_ACCESS = 0x9A;              // 9A Q Power off cashbox door access
    public static int POWER_OFF_DROP_DOOR_ACCESS = 0x9B;              // 9B Q Power off drop door access




    //meter code
    // Total coin in credits，代表投入机器的硬币总积分
    public static int TOTAL_COIN_IN_CREDITS = 0x0000;
    // Total coin out credits，代表机器输出的硬币总积分
    public static int TOTAL_COIN_OUT_CREDITS = 0x0001;
    // Total jackpot credits，代表累积的头奖积分
    public static int TOTAL_JACKPOT_CREDITS = 0x0002;
    // Total hand paid cancelled credits，代表手动支付已取消的积分总和
    public static int TOTAL_HAND_PAID_CANCELLED_CREDITS = 0x0003;
    // Total cancelled credits，代表已取消的积分总和
    public static int TOTAL_CANCELLED_CREDITS = 0x0004;
    // Games played，代表已玩的游戏次数
    public static int GAMES_PLAYED = 0x0005;
    // Games won，代表赢得的游戏次数
    public static int GAMES_WON = 0x0006;
    // Games lost，代表输掉的游戏次数
    public static int GAMES_LOST = 0x0007;
    // Total credits from coin acceptor，代表从硬币接收器获取的总积分
    public static int TOTAL_CREDITS_FROM_COIN_ACCEPTOR = 0x0008;
    // Total credits paid from hopper，代表从料斗支付的总积分
    public static int TOTAL_CREDITS_PAID_FROM_HOPPER = 0x0009;
    // Total credits from coins to drop，代表要投放的硬币总积分
    public static int TOTAL_CREDITS_FROM_COINS_TO_DROP = 0x000A;
    // Total credits from bills accepted，代表已接受纸币对应的总积分
    public static int TOTAL_CREDITS_FROM_BILLS_ACCEPTED = 0x000B;
    // Current credits，代表当前的积分余额
    public static int CURRENT_CREDITS = 0x000C;
    // Total SAS cashable ticket in, including nonrestricted tickets (cents)，代表包含非受限票券在内的SAS可兑现票券输入总金额（以美分计）
    public static int TOTAL_SAS_CASHABLE_TICKET_IN_CENTS = 0x000D;
    // Total SAS cashable ticket out, including debit tickets (cents)，代表包含借记票券在内的SAS可兑现票券输出总金额（以美分计）
    public static int TOTAL_SAS_CASHABLE_TICKET_OUT_CENTS = 0x000E;
    // Total SAS restricted ticket in (cents)，代表受限SAS票券输入总金额（以美分计）
    public static int TOTAL_SAS_RESTRICTED_TICKET_IN_CENTS = 0x000F;
    // Total SAS restricted ticket out (cents)，代表受限SAS票券输出总金额（以美分计）
    public static int TOTAL_SAS_RESTRICTED_TICKET_OUT_CENTS = 0x0010;
    // Total SAS cashable ticket in, including nonrestricted tickets (quantity)，代表包含非受限票券在内的SAS可兑现票券输入总数量
    public static int TOTAL_SAS_CASHABLE_TICKET_IN_QUANTITY = 0x0011;
    // Total SAS cashable ticket out, including debit tickets (quantity)，代表包含借记票券在内的SAS可兑现票券输出总数量
    public static int TOTAL_SAS_CASHABLE_TICKET_OUT_QUANTITY = 0x0012;
    // Total SAS restricted ticket in (quantity)，代表受限SAS票券输入总数量
    public static int TOTAL_SAS_RESTRICTED_TICKET_IN_QUANTITY = 0x0013;
    // Total SAS restricted ticket out (quantity)，代表受限SAS票券输出总数量
    public static int TOTAL_SAS_RESTRICTED_TICKET_OUT_QUANTITY = 0x0014;
    // Total ticket in, including cashable, nonrestricted and restricted tickets (credits)，代表包含可兑现、非受限及受限票券在内的总输入票券积分
    public static int TOTAL_TICKET_IN_CREDITS = 0x0015;
    // Total ticket out, including cashable, nonrestricted, restricted and debit tickets (credits)，代表包含可兑现、非受限、受限及借记票券在内的总输出票券积分
    public static int TOTAL_TICKET_OUT_CREDITS = 0x0016;
    // Total electronic transfers to gaming machine, including cashable, nonrestricted, restricted and debit, whether transfer is to credit meter or to ticket (credits)，代表向游戏机进行的电子转账总积分，包括各种类型且不论转账到积分表还是票券
    public static int TOTAL_ELECTRONIC_TRANSFERS_TO_GAMING_MACHINE_CREDITS = 0x0017;
    // Total electronic transfers to host, including cashable, nonrestricted, restricted and win amounts (credits)，代表向主机进行的电子转账总积分，包含各种类型及中奖金额等情况
    public static int TOTAL_ELECTRONIC_TRANSFERS_TO_HOST_CREDITS = 0x0018;
    // Total restricted amount played (credits)，代表受限金额已参与游戏的总积分
    public static int TOTAL_RESTRICTED_AMOUNT_PLAYED_CREDITS = 0x0019;
    // Total nonrestricted amount played (credits)，代表非受限金额已参与游戏的总积分
    public static int TOTAL_NONRESTRICTED_AMOUNT_PLAYED_CREDITS = 0x001A;
    // Current restricted credits，代表当前受限的积分余额
    public static int CURRENT_RESTRICTED_CREDITS = 0x001B;
    // Total machine paid paytable win, not including progressive or external bonus amounts (credits)，代表机器支付的奖池获胜金额（不包括累进或外部奖金金额）总积分
    public static int TOTAL_MACHINE_PAID_PAYTABLE_WIN_CREDITS = 0x001C;
    // Total machine paid progressive win (credits)，代表机器支付的累进奖金获胜金额总积分
    public static int TOTAL_MACHINE_PAID_PROGRESSIVE_WIN_CREDITS = 0x001D;
    // Total machine paid external bonus win (credits)，代表机器支付的外部奖金获胜金额总积分
    public static int TOTAL_MACHINE_PAID_EXTERNAL_BONUS_WIN_CREDITS = 0x001E;
    // Total attendant paid paytable win, not including progressive or external bonus amounts (credits)，代表工作人员支付的奖池获胜金额（不包括累进或外部奖金金额）总积分
    public static int TOTAL_ATTENDANT_PAID_PAYTABLE_WIN_CREDITS = 0x001F;
    // Total attendant paid progressive win (credits)，代表工作人员支付的累进奖金获胜金额总积分
    public static int TOTAL_ATTENDANT_PAID_PROGRESSIVE_WIN_CREDITS = 0x0020;
    // Total attendant paid external bonus win (credits)，代表工作人员支付的外部奖金获胜金额总积分
    public static int TOTAL_ATTENDANT_PAID_EXTERNAL_BONUS_WIN_CREDITS = 0x0021;
    // Total won credits (sum of total coin out and total jackpot)，代表赢得的总积分（硬币输出积分和头奖积分总和）
    public static int TOTAL_WON_CREDITS = 0x0022;
    // Total hand paid credits (sum of total hand paid cancelled credits and total jackpot)，代表手动支付的总积分（手动支付已取消积分和头奖积分总和）
    public static int TOTAL_HAND_PAID_CREDITS = 0x0023;
    // Total drop, including but not limited to coins to drop, bills to drop, tickets to drop, and electronic in (credits)，代表总投放量，包括但不限于硬币投放、纸币投放、票券投放以及电子输入等的积分总和
    public static int TOTAL_DROP_CREDITS = 0x0024;
    // Games since last power reset，代表自上次电源重置后玩过的游戏次数
    public static int GAMES_SINCE_LAST_POWER_RESET = 0x0025;
    // Games since slot door closure，代表自投币口关闭后玩过的游戏次数
    public static int GAMES_SINCE_SLOT_DOOR_CLOSURE = 0x0026;
    // Total credits from external coin acceptor，代表从外部硬币接收器获取的总积分
    public static int TOTAL_CREDITS_FROM_EXTERNAL_COIN_ACCEPTOR = 0x0027;
    // Total cashable ticket in, including nonrestricted promotional tickets (credits)，代表包含非受限促销票券在内的可兑现票券输入总积分
    public static int TOTAL_CASHABLE_TICKET_IN_CREDITS = 0x0028;
    // Total regular cashable ticket in (credits)，代表常规可兑现票券输入总积分
    public static int TOTAL_REGULAR_CASHABLE_TICKET_IN_CREDITS = 0x0029;
    // Total restricted promotional ticket in (credits)，代表受限促销票券输入总积分
    public static int TOTAL_RESTRICTED_PROMOTIONAL_TICKET_IN_CREDITS = 0x002A;
    // Total nonrestricted promotional ticket in (credits)，代表非受限促销票券输入总积分
    public static int TOTAL_NONRESTRICTED_PROMOTIONAL_TICKET_IN_CREDITS = 0x002B;
    // Total cashable ticket out, including debit tickets (credits)，代表包含借记票券在内的可兑现票券输出总积分
    public static int TOTAL_CASHABLE_TICKET_OUT_CREDITS = 0x002C;
    // Total restricted promotional ticket out (credits)，代表受限促销票券输出总积分
    public static int TOTAL_RESTRICTED_PROMOTIONAL_TICKET_OUT_CREDITS = 0x002D;
    // Electronic regular cashable transfers to gaming machine, not including external bonus awards (credits)，代表向游戏机进行的常规电子可兑现转账总积分（不包括外部奖金奖励）
    public static int ELECTRONIC_REGULAR_CASHABLE_TRANSFERS_TO_GAMING_MACHINE_CREDITS = 0x002E;
    // Electronic restricted promotional transfers to gaming machine, not including external bonus awards (credits)，代表向游戏机进行的受限促销电子转账总积分（不包括外部奖金奖励）
    public static int ELECTRONIC_RESTRICTED_PROMOTIONAL_TRANSFERS_TO_GAMING_MACHINE_CREDITS = 0x002F;
    // Electronic nonrestricted promotional transfers to gaming machine, not including external bonus awards (credits)，代表向游戏机进行的非受限促销电子转账总积分（不包括外部奖金奖励）
    public static int ELECTRONIC_NONRESTRICTED_PROMOTIONAL_TRANSFERS_TO_GAMING_MACHINE_CREDITS = 0x0030;
    // Electronic debit transfers to gaming machine (credits)，代表向游戏机进行的借记电子转账总积分
    public static int ELECTRONIC_DEBIT_TRANSFERS_TO_GAMING_MACHINE_CREDITS = 0x0031;
    // Electronic regular cashable transfers to host (credits)，代表向主机进行的常规电子可兑现转账总积分
    public static int ELECTRONIC_REGULAR_CASHABLE_TRANSFERS_TO_HOST_CREDITS = 0x0032;
    // Electronic restricted promotional transfers to host (credits)，代表向主机进行的受限促销电子转账总积分
    public static int ELECTRONIC_RESTRICTED_PROMOTIONAL_TRANSFERS_TO_HOST_CREDITS = 0x0032;
    // Electronic nonrestricted promotional transfers to host (credits)，代表向主机进行的非受限促销电子转账总积分
    public static int ELECTRONIC_NONRESTRICTED_PROMOTIONAL_TRANSFERS_TO_HOST_CREDITS = 0x0034;
    // Total regular cashable ticket in (quantity)，代表常规可兑现票券输入总数量
    public static int TOTAL_REGULAR_CASHABLE_TICKET_IN_QUANTITY = 0x0035;
    // Total restricted promotional ticket in (quantity)，代表受限促销票券输入总数量
    public static int TOTAL_RESTRICTED_PROMOTIONAL_TICKET_IN_QUANTITY = 0x0036;
    // Total nonrestricted promotional ticket in (quantity)，代表非受限促销票券输入总数量
    public static int TOTAL_NONRESTRICTED_PROMOTIONAL_TICKET_IN_QUANTITY = 0x0037;
    // Total cashable ticket out, including debit tickets (quantity)，代表包含借记票券在内的可兑现票券输出总数量
    public static int TOTAL_CASHABLE_TICKET_OUT_QUANTITY = 0x0038;
    // Total restricted promotional ticket out (quantity)，代表受限促销票券输出总数量
    public static int TOTAL_RESTRICTED_PROMOTIONAL_TICKET_OUT_QUANTITY = 0x0039;
    // Number of bills currently in the stacker，代表当前堆叠器中的纸币数量
    public static int NUMBER_OF_BILLS_CURRENTLY_IN_THE_STACKER = 0x003E;
    // Total value of bills currently in the stacker (credits)，代表当前堆叠器中纸币的总价值（积分形式）
    public static int TOTAL_VALUE_OF_BILLS_CURRENTLY_IN_THE_STACKER_CREDITS = 0x003F;
    // Total number of $1.00 bills accepted，代表已接受的1美元纸币数量
    public static int TOTAL_NUMBER_OF_1_DOLLAR_BILLS_ACCEPTED = 0x0040;
    // Total number of $2.00 bills accepted，代表已接受的2美元纸币数量
    public static int TOTAL_NUMBER_OF_2_DOLLAR_BILLS_ACCEPTED = 0x0041;
    // Total number of $5.00 bills accepted，代表已接受的5美元纸币数量
    public static int TOTAL_NUMBER_OF_5_DOLLAR_BILLS_ACCEPTED = 0x0042;
    // Total number of $10.00 bills accepted，代表已接受的10美元纸币数量
    public static int TOTAL_NUMBER_OF_10_DOLLAR_BILLS_ACCEPTED = 0x0043;
    // Total number of $20.00 bills accepted，代表已接受的20美元纸币数量
    public static int TOTAL_NUMBER_OF_20_DOLLAR_BILLS_ACCEPTED = 0x0044;
    // Total number of $25.00 bills accepted，代表已接受的25美元纸币数量
    public static int TOTAL_NUMBER_OF_25_DOLLAR_BILLS_ACCEPTED = 0x0045;
    // Total number of $50.00 bills accepted，代表已接受的50美元纸币数量
    public static int TOTAL_NUMBER_OF_50_DOLLAR_BILLS_ACCEPTED = 0x0046;
    // Total number of $100.00 bills accepted，代表已接受的100美元纸币数量
    public static int TOTAL_NUMBER_OF_100_DOLLAR_BILLS_ACCEPTED = 0x0047;
    // Total number of $200.00 bills accepted，代表已接受的200美元纸币数量
    public static int TOTAL_NUMBER_OF_200_DOLLAR_BILLS_ACCEPTED = 0x0048;
    // Total number of $250.00 bills accepted，代表已接受的250美元纸币数量
    public static int TOTAL_NUMBER_OF_250_DOLLAR_BILLS_ACCEPTED = 0x0049;
    // Total number of $500.00 bills accepted，代表已接受的500美元纸币数量
    public static int TOTAL_NUMBER_OF_500_DOLLAR_BILLS_ACCEPTED = 0x004A;
    // Total number of $1,000.00 bills accepted，代表已接受的1000美元纸币数量
    public static int TOTAL_NUMBER_OF_1000_DOLLAR_BILLS_ACCEPTED = 0x004B;
    // Total number of $2,000.00 bills accepted，代表已接受的2000美元纸币数量
    public static int TOTAL_NUMBER_OF_2000_DOLLAR_BILLS_ACCEPTED = 0x004C;
    // Total number of $2,500.00 bills accepted，代表已接受的2500美元纸币数量
    public static int TOTAL_NUMBER_OF_2500_DOLLAR_BILLS_ACCEPTED = 0x004D;
    // Total number of $5,000.00 bills accepted，代表已接受的5000美元纸币数量
    public static int TOTAL_NUMBER_OF_5000_DOLLAR_BILLS_ACCEPTED = 0x004E;
    // Total number of $10,000.00 bills accepted，代表已接受的10000美元纸币数量
    public static int TOTAL_NUMBER_OF_10000_DOLLAR_BILLS_ACCEPTED = 0x004F;
    // Total number of $20,000.00 bills accepted，代表已接受的20000美元纸币数量
    public static int TOTAL_NUMBER_OF_20000_DOLLAR_BILLS_ACCEPTED = 0x0050;
    // Total number of $25,000.00 bills accepted，代表已接受的25000美元纸币数量
    public static int TOTAL_NUMBER_OF_25000_DOLLAR_BILLS_ACCEPTED = 0x0051;
    // Total number of $50,000.00 bills accepted，代表已接受的50000美元纸币数量
    public static int TOTAL_NUMBER_OF_50000_DOLLAR_BILLS_ACCEPTED = 0x0052;
    // Total number of $100,000.00 bills accepted，代表已接受的100000美元纸币数量
    public static int TOTAL_NUMBER_OF_100000_DOLLAR_BILLS_ACCEPTED = 0x0053;
    // Total number of $200,000.00 bills accepted，代表已接受的200000美元纸币数量
    public static int TOTAL_NUMBER_OF_200000_DOLLAR_BILLS_ACCEPTED = 0x0054;
    // Total number of $250,000.00 bills accepted，代表已接受的250000美元纸币数量
    public static int TOTAL_NUMBER_OF_250000_DOLLAR_BILLS_ACCEPTED = 0x0055;
    // Total number of $500,000.00 bills accepted，代表已接受的500000美元纸币数量
    public static int TOTAL_NUMBER_OF_500000_DOLLAR_BILLS_ACCEPTED = 0x0056;
    // Total number of $1,000,000.00 bills accepted，代表已接受的1000000美元纸币数量
    public static int TOTAL_NUMBER_OF_1000000_DOLLAR_BILLS_ACCEPTED = 0x0057;
    // Total credits from bills to drop，代表要投放的纸币总积分
    public static int TOTAL_CREDITS_FROM_BILLS_TO_DROP = 0x0058;
    // Total number of $1.00 bills to drop，代表要投放的1美元纸币数量
    public static int TOTAL_NUMBER_OF_1_DOLLAR_BILLS_TO_DROP = 0x0059;
    // Total number of $2.00 bills to drop，代表要投放的2美元纸币数量
    public static int TOTAL_NUMBER_OF_2_DOLLAR_BILLS_TO_DROP = 0x005A;
    // Total number of $5.00 bills to drop，代表要投放的5美元纸币数量
    public static int TOTAL_NUMBER_OF_5_DOLLAR_BILLS_TO_DROP = 0x005B;
    // Total number of $10.00 bills to drop，代表要投放的10美元纸币数量
    public static int TOTAL_NUMBER_OF_10_DOLLAR_BILLS_TO_DROP = 0x005C;
    // Total number of $20.00 bills to drop，代表要投放的20美元纸币数量
    public static int TOTAL_NUMBER_OF_20_DOLLAR_BILLS_TO_DROP = 0x005D;
    // Total number of $50.00 bills to drop，代表要投放的50美元纸币数量
    public static int TOTAL_NUMBER_OF_50_DOLLAR_BILLS_TO_DROP = 0x005E;
    // Total number of $100.00 bills to drop，代表要投放的100美元纸币数量
    public static int TOTAL_NUMBER_OF_100_DOLLAR_BILLS_TO_DROP = 0x005F;
    // Total number of $200.00 bills to drop，代表要投放的200美元纸币数量
    public static int TOTAL_NUMBER_OF_200_DOLLAR_BILLS_TO_DROP = 0x0060;
    // Total number of $500.00 bills to drop，代表要投放的500美元纸币数量
    public static int TOTAL_NUMBER_OF_500_DOLLAR_BILLS_TO_DROP = 0x0061;
    // Total number of $1000.00 bills to drop，代表要投放的1000美元纸币数量
    public static int TOTAL_NUMBER_OF_1000_DOLLAR_BILLS_TO_DROP = 0x0062;
    // Total credits from bills diverted to hopper，代表转向料斗的纸币总积分
    public static int TOTAL_CREDITS_FROM_BILLS_DIVERTED_TO_HOPPER = 0x0063;

    // Total number of $1.00 bills diverted to hopper，代表转向料斗的1美元纸币数量
    public static int TOTAL_NUMBER_OF_1_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0064;
    // Total number of $2.00 bills diverted to hopper，代表转向料斗的2美元纸币数量
    public static int TOTAL_NUMBER_OF_2_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0065;
    // Total number of $5.00 bills diverted to hopper，代表转向料斗的5美元纸币数量
    public static int TOTAL_NUMBER_OF_5_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0066;
    // Total number of $10.00 bills diverted to hopper，代表转向料斗的10美元纸币数量
    public static int TOTAL_NUMBER_OF_10_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0067;
    // Total number of $20.00 bills diverted to hopper，代表转向料斗的20美元纸币数量
    public static int TOTAL_NUMBER_OF_20_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0068;
    // Total number of $50.00 bills diverted to hopper，代表转向料斗的50美元纸币数量
    public static int TOTAL_NUMBER_OF_50_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0069;
    // Total number of $100.00 bills diverted to hopper，代表转向料斗的100美元纸币数量
    public static int TOTAL_NUMBER_OF_100_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x006A;
    // Total number of $200.00 bills diverted to hopper，代表转向料斗的200美元纸币数量
    public static int TOTAL_NUMBER_OF_200_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x006B;
    // Total number of $500.00 bills diverted to hopper，代表转向料斗的500美元纸币数量
    public static int TOTAL_NUMBER_OF_500_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x006C;
    // Total number of $1000.00 bills diverted to hopper，代表转向料斗的1000美元纸币数量
    public static int TOTAL_NUMBER_OF_1000_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x006D;
    // Total credits from bills dispensed from hopper，代表从料斗中分配出去的纸币总积分
    public static int TOTAL_CREDITS_FROM_BILLS_DISPENSED_FROM_HOPPER = 0x006E;
    // Total number of $1.00 bills dispensed from hopper，代表从料斗中分配出去的1美元纸币数量
    public static int TOTAL_NUMBER_OF_1_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x006F;
    // Total number of $2.00 bills dispensed from hopper，代表从料斗中分配出去的2美元纸币数量
    public static int TOTAL_NUMBER_OF_2_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0070;
    // Total number of $5.00 bills dispensed from hopper，代表从料斗中分配出去的5美元纸币数量
    public static int TOTAL_NUMBER_OF_5_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0071;
    // Total number of $10.00 bills dispensed from hopper，代表从料斗中分配出去的10美元纸币数量
    public static int TOTAL_NUMBER_OF_10_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0072;
    // Total number of $20.00 bills dispensed from hopper，代表从料斗中分配出去的20美元纸币数量
    public static int TOTAL_NUMBER_OF_20_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0073;
    // Total number of $50.00 bills dispensed from hopper，代表从料斗中分配出去的50美元纸币数量
    public static int TOTAL_NUMBER_OF_50_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0074;
    // Total number of $100.00 bills dispensed from hopper，代表从料斗中分配出去的100美元纸币数量
    public static int TOTAL_NUMBER_OF_100_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0075;
    // Total number of $200.00 bills dispensed from hopper，代表从料斗中分配出去的200美元纸币数量
    public static int TOTAL_NUMBER_OF_200_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0076;
    // Total number of $500.00 bills dispensed from hopper，代表从料斗中分配出去的500美元纸币数量
    public static int TOTAL_NUMBER_OF_500_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0077;
    // Total number of $1000.00 bills dispensed from hopper，代表从料斗中分配出去的1000美元纸币数量
    public static int TOTAL_NUMBER_OF_1000_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0078;
    // 0079 - 007E Reserved for future use，预留作未来使用，此处暂不做具体代码定义

    // Weighted average theoretical payback percentage in hundredths of a percent (see note below)，以百分之一为单位的加权平均理论回报率（详见备注）
    public static int WEIGHTED_AVERAGE_THEORETICAL_PAYBACK_PERCENTAGE = 0x007F;

    // Regular cashable ticket in (cents)，代表常规可兑现票券输入的金额（以美分计）
    public static int REGULAR_CASHABLE_TICKET_IN_CENTS = 0x0080;
    // Regular cashable ticket in (quantity)，代表常规可兑现票券输入的数量
    public static int REGULAR_CASHABLE_TICKET_IN_QUANTITY = 0x0081;
    // Restricted ticket in (cents)，代表受限票券输入的金额（以美分计）
    public static int RESTRICTED_TICKET_IN_CENTS = 0x0082;
    // Restricted ticket in (quantity)，代表受限票券输入的数量
    public static int RESTRICTED_TICKET_IN_QUANTITY = 0x0083;
    // Nonrestricted ticket in (cents)，代表非受限票券输入的金额（以美分计）
    public static int NONRESTRICTED_TICKET_IN_CENTS = 0x0084;
    // Nonrestricted ticket in (quantity)，代表非受限票券输入的数量
    public static int NONRESTRICTED_TICKET_IN_QUANTITY = 0x0085;
    // Regular cashable ticket out (cents)，代表常规可兑现票券输出的金额（以美分计）
    public static int REGULAR_CASHABLE_TICKET_OUT_CENTS = 0x0086;
    // Regular cashable ticket out (quantity)，代表常规可兑现票券输出的数量
    public static int REGULAR_CASHABLE_TICKET_OUT_QUANTITY = 0x0087;
    // Restricted ticket out (cents)，代表受限票券输出的金额（以美分计）
    public static int RESTRICTED_TICKET_OUT_CENTS = 0x0088;
    // Restricted ticket out (quantity)，代表受限票券输出的数量
    public static int RESTRICTED_TICKET_OUT_QUANTITY = 0x0089;
    // Debit ticket out (cents)，代表借记票券输出的金额（以美分计）
    public static int DEBIT_TICKET_OUT_CENTS = 0x008A;
    // Debit ticket out (quantity)，代表借记票券输出的数量
    public static int DEBIT_TICKET_OUT_QUANTITY = 0x008B;
    // Validated cancelled credit handpay, receipt printed (cents)，代表已验证的已取消信用手动支付且已打印收据的金额（以美分计）
    public static int VALIDATED_CANCELLED_CREDIT_HANDPAY_RECEIPT_PRINTED_CENTS = 0x008C;
    // Validated cancelled credit handpay, receipt printed (quantity)，代表已验证的已取消信用手动支付且已打印收据的数量
    public static int VALIDATED_CANCELLED_CREDIT_HANDPAY_RECEIPT_PRINTED_QUANTITY = 0x008D;
    // Validated jackpot handpay, receipt printed (cents)，代表已验证的头奖手动支付且已打印收据的金额（以美分计）
    public static int VALIDATED_JACKPOT_HANDPAY_RECEIPT_PRINTED_CENTS = 0x008E;
    // Validated jackpot handpay, receipt printed (quantity)，代表已验证的头奖手动支付且已打印收据的数量
    public static int VALIDATED_JACKPOT_HANDPAY_RECEIPT_PRINTED_QUANTITY = 0x008F;
    // Validated cancelled credit handpay, no receipt (cents)，代表已验证的已取消信用手动支付但未打印收据的金额（以美分计）
    public static int VALIDATED_CANCELLED_CREDIT_HANDPAY_NO_RECEIPT_CENTS = 0x0090;
    // Validated cancelled credit handpay, no receipt (quantity)，代表已验证的已取消信用手动支付但未打印收据的数量
    public static int VALIDATED_CANCELLED_CREDIT_HANDPAY_NO_RECEIPT_QUANTITY = 0x0091;
    // Validated jackpot handpay, no receipt (cents)，代表已验证的头奖手动支付但未打印收据的金额（以美分计）
    public static int VALIDATED_JACKPOT_HANDPAY_NO_RECEIPT_CENTS = 0x0092;
    // Validated jackpot handpay, no receipt (quantity)，代表已验证的头奖手动支付但未打印收据的数量
    public static int VALIDATED_JACKPOT_HANDPAY_NO_RECEIPT_QUANTITY = 0x0093;

    // In-house cashable transfers to gaming machine (cents)，代表内部可兑现金额向游戏机的转账金额（以美分计）
    public static int IN_HOUSE_CASHABLE_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00A0;
    // In-House transfers to gaming machine that included cashable amounts (quantity)，代表包含可兑现金额的内部向游戏机转账的数量
    public static int IN_HOUSE_TRANSFERS_TO_GAMING_MACHINE_THAT_INCLUDED_CASHABLE_AMOUNTS_QUANTITY = 0x00A1;
    // In-house restricted transfers to gaming machine (cents)，代表内部受限金额向游戏机的转账金额（以美分计）
    public static int IN_HOUSE_RESTRICTED_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00A2;
    // In-house transfers to gaming machine that included restricted amounts (quantity)，代表包含受限金额的内部向游戏机转账的数量
    public static int IN_HOUSE_TRANSFERS_TO_GAMING_MACHINE_THAT_INCLUDED_RESTRICTED_AMOUNTS_QUANTITY = 0x00A3;
    // In-house nonrestricted transfers to gaming machine (cents)，代表内部非受限金额向游戏机的转账金额（以美分计）
    public static int IN_HOUSE_NONRESTRICTED_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00A4;
    // In-house transfers to gaming machine that included nonrestricted amounts (quantity)，代表包含非受限金额的内部向游戏机转账的数量
    public static int IN_HOUSE_TRANSFERS_TO_GAMING_MACHINE_THAT_INCLUDED_NONRESTRICTED_AMOUNTS_QUANTITY = 0x00A5;
    // Debit transfers to gaming machine (cents)，代表向游戏机的借记转账金额（以美分计）
    public static int DEBIT_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00A6;
    // Debit transfers to gaming machine (quantity)，代表向游戏机的借记转账数量
    public static int DEBIT_TRANSFERS_TO_GAMING_MACHINE_QUANTITY = 0x00A7;
    // In-house cashable transfers to ticket (cents)，代表内部可兑现金额向票券的转账金额（以美分计）
    public static int IN_HOUSE_CASHABLE_TRANSFERS_TO_TICKET_CENTS = 0x00A8;
    // In-house cashable transfers to ticket (quantity)，代表内部可兑现金额向票券的转账数量
    public static int IN_HOUSE_CASHABLE_TRANSFERS_TO_TICKET_QUANTITY = 0x00A9;
    // In-house restricted transfers to ticket (cents)，代表内部受限金额向票券的转账金额（以美分计）
    public static int IN_HOUSE_RESTRICTED_TRANSFERS_TO_TICKET_CENTS = 0x00AA;
    // In-house restricted transfers to ticket (quantity)，代表内部受限金额向票券的转账数量
    public static int IN_HOUSE_RESTRICTED_TRANSFERS_TO_TICKET_QUANTITY = 0x00AB;
    // Debit transfers to ticket (cents)，代表向票券的借记转账金额（以美分计）
    public static int DEBIT_TRANSFERS_TO_TICKET_CENTS = 0x00AC;
    // Debit transfers to ticket (quantity)，代表向票券的借记转账数量
    public static int DEBIT_TRANSFERS_TO_TICKET_QUANTITY = 0x00AD;
    // Bonus cashable transfers to gaming machine (cents)，代表奖金可兑现金额向游戏机的转账金额（以美分计）
    public static int BONUS_CASHABLE_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00AE;
    // Bonus transfers to gaming machine that included cashable amounts (quantity)，代表包含可兑现金额的奖金向游戏机转账的数量
    public static int BONUS_TRANSFERS_TO_GAMING_MACHINE_THAT_INCLUDED_CASHABLE_AMOUNTS_QUANTITY = 0x00AF;
    // Bonus nonrestricted transfers to gaming machine (cents)，代表奖金非受限金额向游戏机的转账金额（以美分计）
    public static int BONUS_NONRESTRICTED_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00B0;
    // Bonus transfers to gaming machine that included nonrestricted amounts (quantity)，代表包含非受限金额的奖金向游戏机转账的数量
    public static int BONUS_TRANSFERS_TO_GAMING_MACHINE_THAT_INCLUDED_NONRESTRICTED_AMOUNTS_QUANTITY = 0x00B1;
    // In-house cashable transfers to host (cents)，代表内部可兑现金额向主机的转账金额（以美分计）
    public static int IN_HOUSE_CASHABLE_TRANSFERS_TO_HOST_CENTS = 0x00B8;
    // In-house transfers to host that included cashable amounts (quantity)，代表包含可兑现金额的内部向主机转账的数量
    public static int IN_HOUSE_TRANSFERS_TO_HOST_THAT_INCLUDED_CASHABLE_AMOUNTS_QUANTITY = 0x00B9;
    // In-house restricted transfers to host (cents)，代表内部受限金额向主机的转账金额（以美分计）
    public static int IN_HOUSE_RESTRICTED_TRANSFERS_TO_HOST_CENTS = 0x00BA;
    // In-house transfers to host that included restricted amounts (quantity)，代表包含受限金额的内部向主机转账的数量
    public static int IN_HOUSE_TRANSFERS_TO_HOST_THAT_INCLUDED_RESTRICTED_AMOUNTS_QUANTITY = 0x00BB;

    // In-house nonrestricted transfers to host (cents)，代表内部非受限金额向主机的转账金额（以美分计）
    public static int IN_HOUSE_NONRESTRICTED_TRANSFERS_TO_HOST_CENTS = 0x00BC;
    // In-house transfers to host that included nonrestricted amounts，代表包含非受限金额的内部向主机转账的数量
    public static int IN_HOUSE_TRANSFERS_TO_HOST_THAT_INCLUDED_NONRESTRICTED_AMOUNTS = 0x00BD;

}

public class SASMessage
{
    public int eventId;
    public string data;
}
