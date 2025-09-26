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
    public static int EPROM_ERROR_CHECKSUM_VERSION_CHANGED = 0x36;              // 36 Q EPROM error (different checksum �C version changed)
    public static int EPROM_ERROR_BAD_CHECKSUM_COMPARE = 0x37;              // 37 Q EPROM error (bad checksum compare)
    public static int PARTITIONED_EPROM_ERROR_CHECKSUM_VERSION_CHANGED = 0x38;              // 38 Q Partitioned EPROM error (checksum �C version changed)
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
    // Total coin in credits������Ͷ�������Ӳ���ܻ���
    public static int TOTAL_COIN_IN_CREDITS = 0x0000;
    // Total coin out credits��������������Ӳ���ܻ���
    public static int TOTAL_COIN_OUT_CREDITS = 0x0001;
    // Total jackpot credits�������ۻ���ͷ������
    public static int TOTAL_JACKPOT_CREDITS = 0x0002;
    // Total hand paid cancelled credits�������ֶ�֧����ȡ���Ļ����ܺ�
    public static int TOTAL_HAND_PAID_CANCELLED_CREDITS = 0x0003;
    // Total cancelled credits��������ȡ���Ļ����ܺ�
    public static int TOTAL_CANCELLED_CREDITS = 0x0004;
    // Games played�������������Ϸ����
    public static int GAMES_PLAYED = 0x0005;
    // Games won������Ӯ�õ���Ϸ����
    public static int GAMES_WON = 0x0006;
    // Games lost�������������Ϸ����
    public static int GAMES_LOST = 0x0007;
    // Total credits from coin acceptor�������Ӳ�ҽ�������ȡ���ܻ���
    public static int TOTAL_CREDITS_FROM_COIN_ACCEPTOR = 0x0008;
    // Total credits paid from hopper��������϶�֧�����ܻ���
    public static int TOTAL_CREDITS_PAID_FROM_HOPPER = 0x0009;
    // Total credits from coins to drop������ҪͶ�ŵ�Ӳ���ܻ���
    public static int TOTAL_CREDITS_FROM_COINS_TO_DROP = 0x000A;
    // Total credits from bills accepted�������ѽ���ֽ�Ҷ�Ӧ���ܻ���
    public static int TOTAL_CREDITS_FROM_BILLS_ACCEPTED = 0x000B;
    // Current credits������ǰ�Ļ������
    public static int CURRENT_CREDITS = 0x000C;
    // Total SAS cashable ticket in, including nonrestricted tickets (cents)���������������Ʊȯ���ڵ�SAS�ɶ���Ʊȯ�����ܽ������ּƣ�
    public static int TOTAL_SAS_CASHABLE_TICKET_IN_CENTS = 0x000D;
    // Total SAS cashable ticket out, including debit tickets (cents)������������Ʊȯ���ڵ�SAS�ɶ���Ʊȯ����ܽ������ּƣ�
    public static int TOTAL_SAS_CASHABLE_TICKET_OUT_CENTS = 0x000E;
    // Total SAS restricted ticket in (cents)����������SASƱȯ�����ܽ������ּƣ�
    public static int TOTAL_SAS_RESTRICTED_TICKET_IN_CENTS = 0x000F;
    // Total SAS restricted ticket out (cents)����������SASƱȯ����ܽ������ּƣ�
    public static int TOTAL_SAS_RESTRICTED_TICKET_OUT_CENTS = 0x0010;
    // Total SAS cashable ticket in, including nonrestricted tickets (quantity)���������������Ʊȯ���ڵ�SAS�ɶ���Ʊȯ����������
    public static int TOTAL_SAS_CASHABLE_TICKET_IN_QUANTITY = 0x0011;
    // Total SAS cashable ticket out, including debit tickets (quantity)������������Ʊȯ���ڵ�SAS�ɶ���Ʊȯ���������
    public static int TOTAL_SAS_CASHABLE_TICKET_OUT_QUANTITY = 0x0012;
    // Total SAS restricted ticket in (quantity)����������SASƱȯ����������
    public static int TOTAL_SAS_RESTRICTED_TICKET_IN_QUANTITY = 0x0013;
    // Total SAS restricted ticket out (quantity)����������SASƱȯ���������
    public static int TOTAL_SAS_RESTRICTED_TICKET_OUT_QUANTITY = 0x0014;
    // Total ticket in, including cashable, nonrestricted and restricted tickets (credits)����������ɶ��֡������޼�����Ʊȯ���ڵ�������Ʊȯ����
    public static int TOTAL_TICKET_IN_CREDITS = 0x0015;
    // Total ticket out, including cashable, nonrestricted, restricted and debit tickets (credits)����������ɶ��֡������ޡ����޼����Ʊȯ���ڵ������Ʊȯ����
    public static int TOTAL_TICKET_OUT_CREDITS = 0x0016;
    // Total electronic transfers to gaming machine, including cashable, nonrestricted, restricted and debit, whether transfer is to credit meter or to ticket (credits)����������Ϸ�����еĵ���ת���ܻ��֣��������������Ҳ���ת�˵����ֱ���Ʊȯ
    public static int TOTAL_ELECTRONIC_TRANSFERS_TO_GAMING_MACHINE_CREDITS = 0x0017;
    // Total electronic transfers to host, including cashable, nonrestricted, restricted and win amounts (credits)���������������еĵ���ת���ܻ��֣������������ͼ��н��������
    public static int TOTAL_ELECTRONIC_TRANSFERS_TO_HOST_CREDITS = 0x0018;
    // Total restricted amount played (credits)���������޽���Ѳ�����Ϸ���ܻ���
    public static int TOTAL_RESTRICTED_AMOUNT_PLAYED_CREDITS = 0x0019;
    // Total nonrestricted amount played (credits)����������޽���Ѳ�����Ϸ���ܻ���
    public static int TOTAL_NONRESTRICTED_AMOUNT_PLAYED_CREDITS = 0x001A;
    // Current restricted credits������ǰ���޵Ļ������
    public static int CURRENT_RESTRICTED_CREDITS = 0x001B;
    // Total machine paid paytable win, not including progressive or external bonus amounts (credits)���������֧���Ľ��ػ�ʤ���������۽����ⲿ������ܻ���
    public static int TOTAL_MACHINE_PAID_PAYTABLE_WIN_CREDITS = 0x001C;
    // Total machine paid progressive win (credits)���������֧�����۽������ʤ����ܻ���
    public static int TOTAL_MACHINE_PAID_PROGRESSIVE_WIN_CREDITS = 0x001D;
    // Total machine paid external bonus win (credits)���������֧�����ⲿ�����ʤ����ܻ���
    public static int TOTAL_MACHINE_PAID_EXTERNAL_BONUS_WIN_CREDITS = 0x001E;
    // Total attendant paid paytable win, not including progressive or external bonus amounts (credits)����������Ա֧���Ľ��ػ�ʤ���������۽����ⲿ������ܻ���
    public static int TOTAL_ATTENDANT_PAID_PAYTABLE_WIN_CREDITS = 0x001F;
    // Total attendant paid progressive win (credits)����������Ա֧�����۽������ʤ����ܻ���
    public static int TOTAL_ATTENDANT_PAID_PROGRESSIVE_WIN_CREDITS = 0x0020;
    // Total attendant paid external bonus win (credits)����������Ա֧�����ⲿ�����ʤ����ܻ���
    public static int TOTAL_ATTENDANT_PAID_EXTERNAL_BONUS_WIN_CREDITS = 0x0021;
    // Total won credits (sum of total coin out and total jackpot)������Ӯ�õ��ܻ��֣�Ӳ��������ֺ�ͷ�������ܺͣ�
    public static int TOTAL_WON_CREDITS = 0x0022;
    // Total hand paid credits (sum of total hand paid cancelled credits and total jackpot)�������ֶ�֧�����ܻ��֣��ֶ�֧����ȡ�����ֺ�ͷ�������ܺͣ�
    public static int TOTAL_HAND_PAID_CREDITS = 0x0023;
    // Total drop, including but not limited to coins to drop, bills to drop, tickets to drop, and electronic in (credits)��������Ͷ������������������Ӳ��Ͷ�š�ֽ��Ͷ�š�ƱȯͶ���Լ���������ȵĻ����ܺ�
    public static int TOTAL_DROP_CREDITS = 0x0024;
    // Games since last power reset���������ϴε�Դ���ú��������Ϸ����
    public static int GAMES_SINCE_LAST_POWER_RESET = 0x0025;
    // Games since slot door closure��������Ͷ�ҿڹرպ��������Ϸ����
    public static int GAMES_SINCE_SLOT_DOOR_CLOSURE = 0x0026;
    // Total credits from external coin acceptor��������ⲿӲ�ҽ�������ȡ���ܻ���
    public static int TOTAL_CREDITS_FROM_EXTERNAL_COIN_ACCEPTOR = 0x0027;
    // Total cashable ticket in, including nonrestricted promotional tickets (credits)��������������޴���Ʊȯ���ڵĿɶ���Ʊȯ�����ܻ���
    public static int TOTAL_CASHABLE_TICKET_IN_CREDITS = 0x0028;
    // Total regular cashable ticket in (credits)��������ɶ���Ʊȯ�����ܻ���
    public static int TOTAL_REGULAR_CASHABLE_TICKET_IN_CREDITS = 0x0029;
    // Total restricted promotional ticket in (credits)���������޴���Ʊȯ�����ܻ���
    public static int TOTAL_RESTRICTED_PROMOTIONAL_TICKET_IN_CREDITS = 0x002A;
    // Total nonrestricted promotional ticket in (credits)����������޴���Ʊȯ�����ܻ���
    public static int TOTAL_NONRESTRICTED_PROMOTIONAL_TICKET_IN_CREDITS = 0x002B;
    // Total cashable ticket out, including debit tickets (credits)������������Ʊȯ���ڵĿɶ���Ʊȯ����ܻ���
    public static int TOTAL_CASHABLE_TICKET_OUT_CREDITS = 0x002C;
    // Total restricted promotional ticket out (credits)���������޴���Ʊȯ����ܻ���
    public static int TOTAL_RESTRICTED_PROMOTIONAL_TICKET_OUT_CREDITS = 0x002D;
    // Electronic regular cashable transfers to gaming machine, not including external bonus awards (credits)����������Ϸ�����еĳ�����ӿɶ���ת���ܻ��֣��������ⲿ��������
    public static int ELECTRONIC_REGULAR_CASHABLE_TRANSFERS_TO_GAMING_MACHINE_CREDITS = 0x002E;
    // Electronic restricted promotional transfers to gaming machine, not including external bonus awards (credits)����������Ϸ�����е����޴�������ת���ܻ��֣��������ⲿ��������
    public static int ELECTRONIC_RESTRICTED_PROMOTIONAL_TRANSFERS_TO_GAMING_MACHINE_CREDITS = 0x002F;
    // Electronic nonrestricted promotional transfers to gaming machine, not including external bonus awards (credits)����������Ϸ�����еķ����޴�������ת���ܻ��֣��������ⲿ��������
    public static int ELECTRONIC_NONRESTRICTED_PROMOTIONAL_TRANSFERS_TO_GAMING_MACHINE_CREDITS = 0x0030;
    // Electronic debit transfers to gaming machine (credits)����������Ϸ�����еĽ�ǵ���ת���ܻ���
    public static int ELECTRONIC_DEBIT_TRANSFERS_TO_GAMING_MACHINE_CREDITS = 0x0031;
    // Electronic regular cashable transfers to host (credits)���������������еĳ�����ӿɶ���ת���ܻ���
    public static int ELECTRONIC_REGULAR_CASHABLE_TRANSFERS_TO_HOST_CREDITS = 0x0032;
    // Electronic restricted promotional transfers to host (credits)���������������е����޴�������ת���ܻ���
    public static int ELECTRONIC_RESTRICTED_PROMOTIONAL_TRANSFERS_TO_HOST_CREDITS = 0x0032;
    // Electronic nonrestricted promotional transfers to host (credits)���������������еķ����޴�������ת���ܻ���
    public static int ELECTRONIC_NONRESTRICTED_PROMOTIONAL_TRANSFERS_TO_HOST_CREDITS = 0x0034;
    // Total regular cashable ticket in (quantity)��������ɶ���Ʊȯ����������
    public static int TOTAL_REGULAR_CASHABLE_TICKET_IN_QUANTITY = 0x0035;
    // Total restricted promotional ticket in (quantity)���������޴���Ʊȯ����������
    public static int TOTAL_RESTRICTED_PROMOTIONAL_TICKET_IN_QUANTITY = 0x0036;
    // Total nonrestricted promotional ticket in (quantity)����������޴���Ʊȯ����������
    public static int TOTAL_NONRESTRICTED_PROMOTIONAL_TICKET_IN_QUANTITY = 0x0037;
    // Total cashable ticket out, including debit tickets (quantity)������������Ʊȯ���ڵĿɶ���Ʊȯ���������
    public static int TOTAL_CASHABLE_TICKET_OUT_QUANTITY = 0x0038;
    // Total restricted promotional ticket out (quantity)���������޴���Ʊȯ���������
    public static int TOTAL_RESTRICTED_PROMOTIONAL_TICKET_OUT_QUANTITY = 0x0039;
    // Number of bills currently in the stacker������ǰ�ѵ����е�ֽ������
    public static int NUMBER_OF_BILLS_CURRENTLY_IN_THE_STACKER = 0x003E;
    // Total value of bills currently in the stacker (credits)������ǰ�ѵ�����ֽ�ҵ��ܼ�ֵ��������ʽ��
    public static int TOTAL_VALUE_OF_BILLS_CURRENTLY_IN_THE_STACKER_CREDITS = 0x003F;
    // Total number of $1.00 bills accepted�������ѽ��ܵ�1��Ԫֽ������
    public static int TOTAL_NUMBER_OF_1_DOLLAR_BILLS_ACCEPTED = 0x0040;
    // Total number of $2.00 bills accepted�������ѽ��ܵ�2��Ԫֽ������
    public static int TOTAL_NUMBER_OF_2_DOLLAR_BILLS_ACCEPTED = 0x0041;
    // Total number of $5.00 bills accepted�������ѽ��ܵ�5��Ԫֽ������
    public static int TOTAL_NUMBER_OF_5_DOLLAR_BILLS_ACCEPTED = 0x0042;
    // Total number of $10.00 bills accepted�������ѽ��ܵ�10��Ԫֽ������
    public static int TOTAL_NUMBER_OF_10_DOLLAR_BILLS_ACCEPTED = 0x0043;
    // Total number of $20.00 bills accepted�������ѽ��ܵ�20��Ԫֽ������
    public static int TOTAL_NUMBER_OF_20_DOLLAR_BILLS_ACCEPTED = 0x0044;
    // Total number of $25.00 bills accepted�������ѽ��ܵ�25��Ԫֽ������
    public static int TOTAL_NUMBER_OF_25_DOLLAR_BILLS_ACCEPTED = 0x0045;
    // Total number of $50.00 bills accepted�������ѽ��ܵ�50��Ԫֽ������
    public static int TOTAL_NUMBER_OF_50_DOLLAR_BILLS_ACCEPTED = 0x0046;
    // Total number of $100.00 bills accepted�������ѽ��ܵ�100��Ԫֽ������
    public static int TOTAL_NUMBER_OF_100_DOLLAR_BILLS_ACCEPTED = 0x0047;
    // Total number of $200.00 bills accepted�������ѽ��ܵ�200��Ԫֽ������
    public static int TOTAL_NUMBER_OF_200_DOLLAR_BILLS_ACCEPTED = 0x0048;
    // Total number of $250.00 bills accepted�������ѽ��ܵ�250��Ԫֽ������
    public static int TOTAL_NUMBER_OF_250_DOLLAR_BILLS_ACCEPTED = 0x0049;
    // Total number of $500.00 bills accepted�������ѽ��ܵ�500��Ԫֽ������
    public static int TOTAL_NUMBER_OF_500_DOLLAR_BILLS_ACCEPTED = 0x004A;
    // Total number of $1,000.00 bills accepted�������ѽ��ܵ�1000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_1000_DOLLAR_BILLS_ACCEPTED = 0x004B;
    // Total number of $2,000.00 bills accepted�������ѽ��ܵ�2000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_2000_DOLLAR_BILLS_ACCEPTED = 0x004C;
    // Total number of $2,500.00 bills accepted�������ѽ��ܵ�2500��Ԫֽ������
    public static int TOTAL_NUMBER_OF_2500_DOLLAR_BILLS_ACCEPTED = 0x004D;
    // Total number of $5,000.00 bills accepted�������ѽ��ܵ�5000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_5000_DOLLAR_BILLS_ACCEPTED = 0x004E;
    // Total number of $10,000.00 bills accepted�������ѽ��ܵ�10000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_10000_DOLLAR_BILLS_ACCEPTED = 0x004F;
    // Total number of $20,000.00 bills accepted�������ѽ��ܵ�20000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_20000_DOLLAR_BILLS_ACCEPTED = 0x0050;
    // Total number of $25,000.00 bills accepted�������ѽ��ܵ�25000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_25000_DOLLAR_BILLS_ACCEPTED = 0x0051;
    // Total number of $50,000.00 bills accepted�������ѽ��ܵ�50000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_50000_DOLLAR_BILLS_ACCEPTED = 0x0052;
    // Total number of $100,000.00 bills accepted�������ѽ��ܵ�100000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_100000_DOLLAR_BILLS_ACCEPTED = 0x0053;
    // Total number of $200,000.00 bills accepted�������ѽ��ܵ�200000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_200000_DOLLAR_BILLS_ACCEPTED = 0x0054;
    // Total number of $250,000.00 bills accepted�������ѽ��ܵ�250000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_250000_DOLLAR_BILLS_ACCEPTED = 0x0055;
    // Total number of $500,000.00 bills accepted�������ѽ��ܵ�500000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_500000_DOLLAR_BILLS_ACCEPTED = 0x0056;
    // Total number of $1,000,000.00 bills accepted�������ѽ��ܵ�1000000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_1000000_DOLLAR_BILLS_ACCEPTED = 0x0057;
    // Total credits from bills to drop������ҪͶ�ŵ�ֽ���ܻ���
    public static int TOTAL_CREDITS_FROM_BILLS_TO_DROP = 0x0058;
    // Total number of $1.00 bills to drop������ҪͶ�ŵ�1��Ԫֽ������
    public static int TOTAL_NUMBER_OF_1_DOLLAR_BILLS_TO_DROP = 0x0059;
    // Total number of $2.00 bills to drop������ҪͶ�ŵ�2��Ԫֽ������
    public static int TOTAL_NUMBER_OF_2_DOLLAR_BILLS_TO_DROP = 0x005A;
    // Total number of $5.00 bills to drop������ҪͶ�ŵ�5��Ԫֽ������
    public static int TOTAL_NUMBER_OF_5_DOLLAR_BILLS_TO_DROP = 0x005B;
    // Total number of $10.00 bills to drop������ҪͶ�ŵ�10��Ԫֽ������
    public static int TOTAL_NUMBER_OF_10_DOLLAR_BILLS_TO_DROP = 0x005C;
    // Total number of $20.00 bills to drop������ҪͶ�ŵ�20��Ԫֽ������
    public static int TOTAL_NUMBER_OF_20_DOLLAR_BILLS_TO_DROP = 0x005D;
    // Total number of $50.00 bills to drop������ҪͶ�ŵ�50��Ԫֽ������
    public static int TOTAL_NUMBER_OF_50_DOLLAR_BILLS_TO_DROP = 0x005E;
    // Total number of $100.00 bills to drop������ҪͶ�ŵ�100��Ԫֽ������
    public static int TOTAL_NUMBER_OF_100_DOLLAR_BILLS_TO_DROP = 0x005F;
    // Total number of $200.00 bills to drop������ҪͶ�ŵ�200��Ԫֽ������
    public static int TOTAL_NUMBER_OF_200_DOLLAR_BILLS_TO_DROP = 0x0060;
    // Total number of $500.00 bills to drop������ҪͶ�ŵ�500��Ԫֽ������
    public static int TOTAL_NUMBER_OF_500_DOLLAR_BILLS_TO_DROP = 0x0061;
    // Total number of $1000.00 bills to drop������ҪͶ�ŵ�1000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_1000_DOLLAR_BILLS_TO_DROP = 0x0062;
    // Total credits from bills diverted to hopper������ת���϶���ֽ���ܻ���
    public static int TOTAL_CREDITS_FROM_BILLS_DIVERTED_TO_HOPPER = 0x0063;

    // Total number of $1.00 bills diverted to hopper������ת���϶���1��Ԫֽ������
    public static int TOTAL_NUMBER_OF_1_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0064;
    // Total number of $2.00 bills diverted to hopper������ת���϶���2��Ԫֽ������
    public static int TOTAL_NUMBER_OF_2_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0065;
    // Total number of $5.00 bills diverted to hopper������ת���϶���5��Ԫֽ������
    public static int TOTAL_NUMBER_OF_5_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0066;
    // Total number of $10.00 bills diverted to hopper������ת���϶���10��Ԫֽ������
    public static int TOTAL_NUMBER_OF_10_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0067;
    // Total number of $20.00 bills diverted to hopper������ת���϶���20��Ԫֽ������
    public static int TOTAL_NUMBER_OF_20_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0068;
    // Total number of $50.00 bills diverted to hopper������ת���϶���50��Ԫֽ������
    public static int TOTAL_NUMBER_OF_50_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x0069;
    // Total number of $100.00 bills diverted to hopper������ת���϶���100��Ԫֽ������
    public static int TOTAL_NUMBER_OF_100_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x006A;
    // Total number of $200.00 bills diverted to hopper������ת���϶���200��Ԫֽ������
    public static int TOTAL_NUMBER_OF_200_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x006B;
    // Total number of $500.00 bills diverted to hopper������ת���϶���500��Ԫֽ������
    public static int TOTAL_NUMBER_OF_500_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x006C;
    // Total number of $1000.00 bills diverted to hopper������ת���϶���1000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_1000_DOLLAR_BILLS_DIVERTED_TO_HOPPER = 0x006D;
    // Total credits from bills dispensed from hopper��������϶��з����ȥ��ֽ���ܻ���
    public static int TOTAL_CREDITS_FROM_BILLS_DISPENSED_FROM_HOPPER = 0x006E;
    // Total number of $1.00 bills dispensed from hopper��������϶��з����ȥ��1��Ԫֽ������
    public static int TOTAL_NUMBER_OF_1_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x006F;
    // Total number of $2.00 bills dispensed from hopper��������϶��з����ȥ��2��Ԫֽ������
    public static int TOTAL_NUMBER_OF_2_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0070;
    // Total number of $5.00 bills dispensed from hopper��������϶��з����ȥ��5��Ԫֽ������
    public static int TOTAL_NUMBER_OF_5_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0071;
    // Total number of $10.00 bills dispensed from hopper��������϶��з����ȥ��10��Ԫֽ������
    public static int TOTAL_NUMBER_OF_10_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0072;
    // Total number of $20.00 bills dispensed from hopper��������϶��з����ȥ��20��Ԫֽ������
    public static int TOTAL_NUMBER_OF_20_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0073;
    // Total number of $50.00 bills dispensed from hopper��������϶��з����ȥ��50��Ԫֽ������
    public static int TOTAL_NUMBER_OF_50_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0074;
    // Total number of $100.00 bills dispensed from hopper��������϶��з����ȥ��100��Ԫֽ������
    public static int TOTAL_NUMBER_OF_100_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0075;
    // Total number of $200.00 bills dispensed from hopper��������϶��з����ȥ��200��Ԫֽ������
    public static int TOTAL_NUMBER_OF_200_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0076;
    // Total number of $500.00 bills dispensed from hopper��������϶��з����ȥ��500��Ԫֽ������
    public static int TOTAL_NUMBER_OF_500_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0077;
    // Total number of $1000.00 bills dispensed from hopper��������϶��з����ȥ��1000��Ԫֽ������
    public static int TOTAL_NUMBER_OF_1000_DOLLAR_BILLS_DISPENSED_FROM_HOPPER = 0x0078;
    // 0079 - 007E Reserved for future use��Ԥ����δ��ʹ�ã��˴��ݲ���������붨��

    // Weighted average theoretical payback percentage in hundredths of a percent (see note below)���԰ٷ�֮һΪ��λ�ļ�Ȩƽ�����ۻر��ʣ������ע��
    public static int WEIGHTED_AVERAGE_THEORETICAL_PAYBACK_PERCENTAGE = 0x007F;

    // Regular cashable ticket in (cents)��������ɶ���Ʊȯ����Ľ������ּƣ�
    public static int REGULAR_CASHABLE_TICKET_IN_CENTS = 0x0080;
    // Regular cashable ticket in (quantity)��������ɶ���Ʊȯ���������
    public static int REGULAR_CASHABLE_TICKET_IN_QUANTITY = 0x0081;
    // Restricted ticket in (cents)����������Ʊȯ����Ľ������ּƣ�
    public static int RESTRICTED_TICKET_IN_CENTS = 0x0082;
    // Restricted ticket in (quantity)����������Ʊȯ���������
    public static int RESTRICTED_TICKET_IN_QUANTITY = 0x0083;
    // Nonrestricted ticket in (cents)�����������Ʊȯ����Ľ������ּƣ�
    public static int NONRESTRICTED_TICKET_IN_CENTS = 0x0084;
    // Nonrestricted ticket in (quantity)�����������Ʊȯ���������
    public static int NONRESTRICTED_TICKET_IN_QUANTITY = 0x0085;
    // Regular cashable ticket out (cents)��������ɶ���Ʊȯ����Ľ������ּƣ�
    public static int REGULAR_CASHABLE_TICKET_OUT_CENTS = 0x0086;
    // Regular cashable ticket out (quantity)��������ɶ���Ʊȯ���������
    public static int REGULAR_CASHABLE_TICKET_OUT_QUANTITY = 0x0087;
    // Restricted ticket out (cents)����������Ʊȯ����Ľ������ּƣ�
    public static int RESTRICTED_TICKET_OUT_CENTS = 0x0088;
    // Restricted ticket out (quantity)����������Ʊȯ���������
    public static int RESTRICTED_TICKET_OUT_QUANTITY = 0x0089;
    // Debit ticket out (cents)��������Ʊȯ����Ľ������ּƣ�
    public static int DEBIT_TICKET_OUT_CENTS = 0x008A;
    // Debit ticket out (quantity)��������Ʊȯ���������
    public static int DEBIT_TICKET_OUT_QUANTITY = 0x008B;
    // Validated cancelled credit handpay, receipt printed (cents)����������֤����ȡ�������ֶ�֧�����Ѵ�ӡ�վݵĽ������ּƣ�
    public static int VALIDATED_CANCELLED_CREDIT_HANDPAY_RECEIPT_PRINTED_CENTS = 0x008C;
    // Validated cancelled credit handpay, receipt printed (quantity)����������֤����ȡ�������ֶ�֧�����Ѵ�ӡ�վݵ�����
    public static int VALIDATED_CANCELLED_CREDIT_HANDPAY_RECEIPT_PRINTED_QUANTITY = 0x008D;
    // Validated jackpot handpay, receipt printed (cents)����������֤��ͷ���ֶ�֧�����Ѵ�ӡ�վݵĽ������ּƣ�
    public static int VALIDATED_JACKPOT_HANDPAY_RECEIPT_PRINTED_CENTS = 0x008E;
    // Validated jackpot handpay, receipt printed (quantity)����������֤��ͷ���ֶ�֧�����Ѵ�ӡ�վݵ�����
    public static int VALIDATED_JACKPOT_HANDPAY_RECEIPT_PRINTED_QUANTITY = 0x008F;
    // Validated cancelled credit handpay, no receipt (cents)����������֤����ȡ�������ֶ�֧����δ��ӡ�վݵĽ������ּƣ�
    public static int VALIDATED_CANCELLED_CREDIT_HANDPAY_NO_RECEIPT_CENTS = 0x0090;
    // Validated cancelled credit handpay, no receipt (quantity)����������֤����ȡ�������ֶ�֧����δ��ӡ�վݵ�����
    public static int VALIDATED_CANCELLED_CREDIT_HANDPAY_NO_RECEIPT_QUANTITY = 0x0091;
    // Validated jackpot handpay, no receipt (cents)����������֤��ͷ���ֶ�֧����δ��ӡ�վݵĽ������ּƣ�
    public static int VALIDATED_JACKPOT_HANDPAY_NO_RECEIPT_CENTS = 0x0092;
    // Validated jackpot handpay, no receipt (quantity)����������֤��ͷ���ֶ�֧����δ��ӡ�վݵ�����
    public static int VALIDATED_JACKPOT_HANDPAY_NO_RECEIPT_QUANTITY = 0x0093;

    // In-house cashable transfers to gaming machine (cents)�������ڲ��ɶ��ֽ������Ϸ����ת�˽������ּƣ�
    public static int IN_HOUSE_CASHABLE_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00A0;
    // In-House transfers to gaming machine that included cashable amounts (quantity)����������ɶ��ֽ����ڲ�����Ϸ��ת�˵�����
    public static int IN_HOUSE_TRANSFERS_TO_GAMING_MACHINE_THAT_INCLUDED_CASHABLE_AMOUNTS_QUANTITY = 0x00A1;
    // In-house restricted transfers to gaming machine (cents)�������ڲ����޽������Ϸ����ת�˽������ּƣ�
    public static int IN_HOUSE_RESTRICTED_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00A2;
    // In-house transfers to gaming machine that included restricted amounts (quantity)������������޽����ڲ�����Ϸ��ת�˵�����
    public static int IN_HOUSE_TRANSFERS_TO_GAMING_MACHINE_THAT_INCLUDED_RESTRICTED_AMOUNTS_QUANTITY = 0x00A3;
    // In-house nonrestricted transfers to gaming machine (cents)�������ڲ������޽������Ϸ����ת�˽������ּƣ�
    public static int IN_HOUSE_NONRESTRICTED_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00A4;
    // In-house transfers to gaming machine that included nonrestricted amounts (quantity)��������������޽����ڲ�����Ϸ��ת�˵�����
    public static int IN_HOUSE_TRANSFERS_TO_GAMING_MACHINE_THAT_INCLUDED_NONRESTRICTED_AMOUNTS_QUANTITY = 0x00A5;
    // Debit transfers to gaming machine (cents)����������Ϸ���Ľ��ת�˽������ּƣ�
    public static int DEBIT_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00A6;
    // Debit transfers to gaming machine (quantity)����������Ϸ���Ľ��ת������
    public static int DEBIT_TRANSFERS_TO_GAMING_MACHINE_QUANTITY = 0x00A7;
    // In-house cashable transfers to ticket (cents)�������ڲ��ɶ��ֽ����Ʊȯ��ת�˽������ּƣ�
    public static int IN_HOUSE_CASHABLE_TRANSFERS_TO_TICKET_CENTS = 0x00A8;
    // In-house cashable transfers to ticket (quantity)�������ڲ��ɶ��ֽ����Ʊȯ��ת������
    public static int IN_HOUSE_CASHABLE_TRANSFERS_TO_TICKET_QUANTITY = 0x00A9;
    // In-house restricted transfers to ticket (cents)�������ڲ����޽����Ʊȯ��ת�˽������ּƣ�
    public static int IN_HOUSE_RESTRICTED_TRANSFERS_TO_TICKET_CENTS = 0x00AA;
    // In-house restricted transfers to ticket (quantity)�������ڲ����޽����Ʊȯ��ת������
    public static int IN_HOUSE_RESTRICTED_TRANSFERS_TO_TICKET_QUANTITY = 0x00AB;
    // Debit transfers to ticket (cents)��������Ʊȯ�Ľ��ת�˽������ּƣ�
    public static int DEBIT_TRANSFERS_TO_TICKET_CENTS = 0x00AC;
    // Debit transfers to ticket (quantity)��������Ʊȯ�Ľ��ת������
    public static int DEBIT_TRANSFERS_TO_TICKET_QUANTITY = 0x00AD;
    // Bonus cashable transfers to gaming machine (cents)��������ɶ��ֽ������Ϸ����ת�˽������ּƣ�
    public static int BONUS_CASHABLE_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00AE;
    // Bonus transfers to gaming machine that included cashable amounts (quantity)����������ɶ��ֽ��Ľ�������Ϸ��ת�˵�����
    public static int BONUS_TRANSFERS_TO_GAMING_MACHINE_THAT_INCLUDED_CASHABLE_AMOUNTS_QUANTITY = 0x00AF;
    // Bonus nonrestricted transfers to gaming machine (cents)������������޽������Ϸ����ת�˽������ּƣ�
    public static int BONUS_NONRESTRICTED_TRANSFERS_TO_GAMING_MACHINE_CENTS = 0x00B0;
    // Bonus transfers to gaming machine that included nonrestricted amounts (quantity)��������������޽��Ľ�������Ϸ��ת�˵�����
    public static int BONUS_TRANSFERS_TO_GAMING_MACHINE_THAT_INCLUDED_NONRESTRICTED_AMOUNTS_QUANTITY = 0x00B1;
    // In-house cashable transfers to host (cents)�������ڲ��ɶ��ֽ����������ת�˽������ּƣ�
    public static int IN_HOUSE_CASHABLE_TRANSFERS_TO_HOST_CENTS = 0x00B8;
    // In-house transfers to host that included cashable amounts (quantity)����������ɶ��ֽ����ڲ�������ת�˵�����
    public static int IN_HOUSE_TRANSFERS_TO_HOST_THAT_INCLUDED_CASHABLE_AMOUNTS_QUANTITY = 0x00B9;
    // In-house restricted transfers to host (cents)�������ڲ����޽����������ת�˽������ּƣ�
    public static int IN_HOUSE_RESTRICTED_TRANSFERS_TO_HOST_CENTS = 0x00BA;
    // In-house transfers to host that included restricted amounts (quantity)������������޽����ڲ�������ת�˵�����
    public static int IN_HOUSE_TRANSFERS_TO_HOST_THAT_INCLUDED_RESTRICTED_AMOUNTS_QUANTITY = 0x00BB;

    // In-house nonrestricted transfers to host (cents)�������ڲ������޽����������ת�˽������ּƣ�
    public static int IN_HOUSE_NONRESTRICTED_TRANSFERS_TO_HOST_CENTS = 0x00BC;
    // In-house transfers to host that included nonrestricted amounts��������������޽����ڲ�������ת�˵�����
    public static int IN_HOUSE_TRANSFERS_TO_HOST_THAT_INCLUDED_NONRESTRICTED_AMOUNTS = 0x00BD;

}

public class SASMessage
{
    public int eventId;
    public string data;
}
