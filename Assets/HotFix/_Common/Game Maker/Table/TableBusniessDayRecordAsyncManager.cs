using Mono.Data.Sqlite;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
public partial class TableBusniessDayRecordAsyncManager : MonoSingleton<TableBusniessDayRecordAsyncManager>
{

    [Button]
    public void AddTotalCoinIn01(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;


        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���


        string key = $"bussiness_day_record_{startTimestamp}_{endTimestamp}";  // ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,


        // ����Ƿ��������
        string selectQuery = @"
            SELECT * FROM bussiness_day_record
            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
        ;

        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(selectQuery, new Dictionary<string, object>()
        {
            [":tabName"] = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,
            [":startTimestamp"] = startTimestamp,
            [":endTimestamp"] = endTimestamp,
        }, (SqliteDataReader reader) =>
        {
            if (reader.Read()) //��������
            {
                // ��������
                string updateQuery = @"
                        UPDATE bussiness_day_record
                        SET total_coin_in_credit = total_coin_in_credit + :credit,
                            credit_after = :creditAfter
                        WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                ;

                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(updateQuery, new Dictionary<string, object>()
                {
                    [":tabName"] = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,
                    [":startTimestamp"] = startTimestamp,
                    [":endTimestamp"] = endTimestamp,
                    [":credit"] = credit,
                    [":creditAfter"] = creditAfter,
                }, (isOk) =>
                {
                });

            }
            else
            {
                // ��������
                string insertQuery = @"
                    INSERT INTO bussiness_day_record (
                        created_at,
                        total_coin_in_credit,
                        credit_before,
                        credit_after
                    ) VALUES (
                        :createdAt,
                        :credit,
                        :creditBefore,
                        :creditAfter
                    )";

                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(insertQuery, new Dictionary<string, object>()
                {
                    [":tabName"] = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,
                    [":createdAt"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    //[":startTimestamp"] = startTimestamp,
                    //[":endTimestamp"] = endTimestamp,
                    [":credit"] = credit,
                    [":creditBefore"] = creditBefore,
                    [":creditAfter"] = creditAfter,
                }, (isOk) =>
                {
                });

            }

        });


    }



    [Button]
    public void AddTotalCoinOut01(long credit, long creditAfter)
    {
        long creditBefore = creditAfter + credit;


        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���


        string key = $"bussiness_day_record_{startTimestamp}_{endTimestamp}";  // ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,


        // ����Ƿ��������
        string selectQuery = @"
            SELECT * FROM bussiness_day_record
            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
        ;

        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(selectQuery, new Dictionary<string, object>()
        {
            [":tabName"] = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,
            [":startTimestamp"] = startTimestamp,
            [":endTimestamp"] = endTimestamp,
        }, (SqliteDataReader reader) =>
        {
            if (reader.Read()) //��������
            {
                // ��������
                string updateQuery = @"
                        UPDATE bussiness_day_record
                        SET total_coin_out_credit = total_coin_out_credit + :credit,
                            credit_after = :creditAfter
                        WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                ;

                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(updateQuery, new Dictionary<string, object>()
                {
                    [":tabName"] = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,
                    [":startTimestamp"] = startTimestamp,
                    [":endTimestamp"] = endTimestamp,
                    [":credit"] = credit,
                    [":creditAfter"] = creditAfter,
                }, (isOk) =>
                {

                });

            }
            else
            {
                // ��������
                string insertQuery = @"
                    INSERT INTO bussiness_day_record (
                        created_at,
                        total_coin_out_credit,
                        credit_before,
                        credit_after
                    ) VALUES (
                        :createdAt,
                        :credit,
                        :creditBefore,
                        :creditAfter
                    )";

                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(insertQuery, new Dictionary<string, object>()
                {
                    [":tabName"] = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,
                    [":createdAt"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    //[":startTimestamp"] = startTimestamp,
                    //[":endTimestamp"] = endTimestamp,
                    [":credit"] = credit,
                    [":creditBefore"] = creditBefore,
                    [":creditAfter"] = creditAfter,
                }, (isOk) =>
                {

                });

            }

        });


    }



    [Button]
    public void AddTotalBetWin01(long bet, long win, long creditAfter)
    {
        long creditBefore = creditAfter - win + bet;

        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���


        string key = $"bussiness_day_record_{startTimestamp}_{endTimestamp}";  // ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,

        // ����Ƿ��������
        string selectQuery = @"
        SELECT * FROM bussiness_day_record
        WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
        ;

        SQLiteAsyncHelper.Instance.ExecuteQueryAsync(selectQuery, new Dictionary<string, object>()
        {
            [":startTimestamp"] = startTimestamp,
            [":endTimestamp"] = endTimestamp,
        }, (SqliteDataReader reader) =>
        {
            if (reader.Read()) //��������
            {
                // ��������
                string updateQuery = @"
                    UPDATE bussiness_day_record
                    SET total_bet_credit = total_bet_credit + :bet,
                        total_win_credit = total_win_credit + :win,
                        credit_after = :creditAfter
                    WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                ;

                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(updateQuery, new Dictionary<string, object>()
                {
                    [":tabName"] = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,
                    [":startTimestamp"] = startTimestamp,
                    [":endTimestamp"] = endTimestamp,
                    [":bet"] = bet,
                    [":win"] = win,
                    [":creditAfter"] = creditAfter,
                }, (isOk) => {

                });
            }
            else
            {
                // ��������
                string insertQuery = @"
                INSERT INTO bussiness_day_record (
                    created_at,
                    total_bet_credit,
                    total_win_credit,
                    credit_before,
                    credit_after
                ) VALUES (
                    :createdAt,
                    :bet,
                    :win,
                    :creditBefore,
                    :creditAfter
                )";

                SQLiteAsyncHelper.Instance.ExecuteNonQueryAsync(insertQuery, new Dictionary<string, object>()
                {
                    [":tabName"] = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD,
                    [":createdAt"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    //[":startTimestamp"] = startTimestamp,
                    //[":endTimestamp"] = endTimestamp,
                    [":bet"] = bet,
                    [":win"] = win,
                    [":creditBefore"] = creditBefore,
                    [":creditAfter"] = creditAfter,
                }, (isOk) => {

                });
            }

        });


    }



}


public partial class TableBusniessDayRecordAsyncManager : MonoSingleton<TableBusniessDayRecordAsyncManager>
{

    [Button]
    public void AddTotalPrinterOut(long credit, long money, long creditAfter)
    {
        //Printer Out ����Ŀ�鵽 Coin Out ��

        return;

        long creditBefore = creditAfter - credit;


        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //string tabName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        string selectQuery = $" SELECT * FROM bussiness_day_record WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp}";

        string updateQuery = $"UPDATE bussiness_day_record " +
                            $"SET total_printer_out_credit = total_printer_out_credit + {credit}, " +
                                $"total_printer_out_as_money = total_printer_out_as_money + {money}, " +
                                $"credit_after = {creditAfter} " +
                            $"WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";

        string insertQuery = $"INSERT INTO bussiness_day_record ( created_at, total_printer_out_credit, total_printer_out_as_money, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit}, {money}, {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });
        Debug.LogError("û�н���Ӫ����ͳ��");
    }

    [Button]
    public void AddTotalBillIn(long credit, long money, long creditAfter)
    {

        //Bill In ����Ŀ�鵽 Coin In ��

        return;

        long creditBefore = creditAfter - credit;



        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //string tabName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        string selectQuery = $" SELECT * FROM bussiness_day_record WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp}";

        string updateQuery = $"UPDATE bussiness_day_record " +
                            $"SET total_bill_in_credit = total_bill_in_credit + {credit}, " +
                                $"total_bill_in_as_money = total_bill_in_as_money + {money}, " +
                                $"credit_after = {creditAfter} " +
                            $"WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";

        string insertQuery = $"INSERT INTO bussiness_day_record ( created_at, total_bill_in_credit, total_bill_in_as_money, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit}, {money}, {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });
        Debug.LogError("û�н���Ӫ����ͳ��");
    }



    public void AddTotalCoinIn(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;


        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //string tabName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        string selectQuery = $" SELECT * FROM bussiness_day_record WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp}";


        string updateQuery = $"UPDATE bussiness_day_record " +
                            $"SET total_coin_in_credit = total_coin_in_credit + {credit}, " +
                                $"credit_after = {creditAfter} " +
                            $"WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";

        string insertQuery = $"INSERT INTO bussiness_day_record ( created_at, total_coin_in_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit},  {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });

        TableBusniessTotalRecordAsyncManager.Instance.AddTotalCoinIn(credit, creditAfter);
    }




    [Button]
    public void AddTotalCoinOut(long credit, long creditAfter)
    {
        long creditBefore = creditAfter + credit;


        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //string tabName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        string selectQuery = $" SELECT * FROM bussiness_day_record WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";


        string updateQuery = $"UPDATE bussiness_day_record " +
                            $"SET total_coin_out_credit = total_coin_out_credit + {credit}, " +
                                $"credit_after = {creditAfter} " +
                            $"WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";

        string insertQuery = $"INSERT INTO bussiness_day_record ( created_at, total_coin_out_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit},  {creditBefore}, {creditAfter} ) ";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });


        TableBusniessTotalRecordAsyncManager.Instance.AddTotalCoinOut(credit, creditAfter);
    }



    [Button]
    public void AddTotalScoreUp(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;


        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //string tabName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        string selectQuery = $" SELECT * FROM bussiness_day_record WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp}";


        string updateQuery = $"UPDATE bussiness_day_record " +
                            $"SET total_score_up_credit = total_score_up_credit + {credit}, " +
                                $"credit_after = {creditAfter} " +
                            $"WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";


        string insertQuery = $"INSERT INTO bussiness_day_record ( created_at, total_score_up_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit},  {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });

        TableBusniessTotalRecordAsyncManager.Instance.AddTotalScoreUp(credit, creditAfter);
    }


    [Button]
    public void AddTotalScoreDown(long credit, long creditAfter)
    {
        //long creditBefore = creditAfter - credit;  // �ɰ汾��bug����
        long creditBefore = creditAfter + credit;

        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //string tabName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        string selectQuery = $" SELECT * FROM bussiness_day_record WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp}";


        string updateQuery = $"UPDATE bussiness_day_record " +
                            $"SET total_score_down_credit = total_score_down_credit + {credit}, " +
                                $"credit_after = {creditAfter} " +
                            $"WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";

        string insertQuery = $"INSERT INTO bussiness_day_record ( created_at, total_score_down_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit},  {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });

        TableBusniessTotalRecordAsyncManager.Instance.AddTotalScoreDown(credit, creditAfter);
    }


    [Button]
    public void AddTotalBet(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;


        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //string tabName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        string selectQuery = $" SELECT * FROM bussiness_day_record WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp}";

        string updateQuery = $"UPDATE bussiness_day_record " +
                        $"SET total_bet_credit = total_bet_credit + {credit}, " +
                            $"credit_after = {creditAfter} " +
                        $"WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";


        string insertQuery = $"INSERT INTO bussiness_day_record ( created_at, total_bet_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit}, {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });
        Debug.LogError("û�н���Ӫ����ͳ��");
    }


    [Button]
    public void AddTotalWin(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;


        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //string tabName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        string selectQuery = $" SELECT * FROM bussiness_day_record WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp}";

        string updateQuery = $"UPDATE bussiness_day_record " +
                        $"SET total_win_credit = total_win_credit + {credit}, " +
                            $"credit_after = {creditAfter} " +
                        $"WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";

        string insertQuery = $"INSERT INTO bussiness_day_record ( created_at, total_win_credit,  credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit}, {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });
        Debug.LogError("û�н���Ӫ����ͳ��");
    }



    [Button]
    void TestTimestamp()
    {

        // ָ������
        DateTime targetDate = new DateTime(2025, 2, 17);
        //  DateTime targetDate = DateTime.Today;

        // ��ȡ�����ڵ����ʱ��
        DateTime startOfDay = targetDate.Date;

        // ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);

        // �����ʱ��ת��Ϊ���뼶ʱ���
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);

        // ������ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);

        DebugUtils.Log($" startTimestamp = {startTimestamp}  endTimestamp ={endTimestamp} ");
    }

    // �� DateTime ת��Ϊ���뼶ʱ����ķ���
    static long GetMillisecondsTimestamp(DateTime dateTime)
    {
        // ���� Unix ʱ�������ʼʱ��
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // �����������ʱ��ת��Ϊ UTC ʱ��
        DateTime utcDateTime = dateTime.ToUniversalTime();

        // ����ʱ��ת��Ϊ����
        return (long)(utcDateTime - unixEpoch).TotalMilliseconds;
    }




    public void AddTotalBetWin(long bet, long win, long creditAfter)
    {

        long creditBefore = creditAfter - win + bet;

        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //string tabName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        string selectQuery = $" SELECT * FROM bussiness_day_record WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp}";

        string updateQuery = $"UPDATE bussiness_day_record " +
                        $"SET total_bet_credit = total_bet_credit + {bet}, " +
                            $"total_win_credit = total_win_credit + {win}, " +
                            $"credit_after = {creditAfter} " +
                        $"WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";

        string insertQuery = $"INSERT INTO bussiness_day_record ( created_at, total_bet_credit, total_win_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {bet}, {win}, {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });

        TableBusniessTotalRecordAsyncManager.Instance.AddTotalBetWin(bet, win, creditAfter);
    }



    [Button]
    public void AddJackpotWin(long win, long myCredit)
    {
        long creditBefore = myCredit - win;
        long creditAfter = myCredit;


        DateTime targetDate = DateTime.Today;
        DateTime startOfDay = targetDate.Date;// ��ȡ�����ڵ����ʱ��
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// ��ȡ�����ڵĽ���ʱ�䣨���ڶ�������ǰһ���룩
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);// �����ʱ��ת��Ϊ���뼶ʱ���
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);// ������ʱ��ת��Ϊ���뼶ʱ���

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //string tabName = ConsoleTableName.TABLE_BUSINESS_DAY_RECORD;

        string selectQuery = $" SELECT * FROM bussiness_day_record WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp}";


        string updateQuery = $"UPDATE bussiness_day_record " +
                        $"SET total_win_credit = total_win_credit + {win}, " +
                            $"credit_after = {creditAfter} " +
                        $"WHERE created_at >= {startTimestamp} AND created_at < {endTimestamp} ";

        string insertQuery = $"INSERT INTO bussiness_day_record ( created_at, total_win_credit,  credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {win}, {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });

        Debug.LogError("û�н���Ӫ����ͳ��");
    }

}
