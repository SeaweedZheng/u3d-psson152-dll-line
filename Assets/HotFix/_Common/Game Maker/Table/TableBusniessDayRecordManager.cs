using GameMaker;
using Mono.Data.Sqlite;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

//public class TableBusniessDayRecordManager : MonoSingleton<TableBusniessDayRecordManager>

public class TableBusniessDayRecordManager : MonoSingleton<TableBusniessDayRecordManager>
{
    [Button]
    public void AddTotalPrinterOut(long credit, long money, long creditAfter)
    {
        //Printer Out 的账目归到 Coin Out 中

        return;



        long creditBefore = creditAfter - credit;


        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {

            DateTime targetDate = DateTime.Today;
            DateTime startOfDay = targetDate.Date;// 获取该日期的零点时间
            DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// 获取该日期的结束时间（即第二天的零点前一毫秒）
            long startTimestamp = GetMillisecondsTimestamp(startOfDay);// 将零点时间转换为毫秒级时间戳
            long endTimestamp = GetMillisecondsTimestamp(endOfDay);// 将结束时间转换为毫秒级时间戳




            // 检查是否存在数据
            string selectQuery = @"
                SELECT * FROM bussiness_day_record
                WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
            ;

            using (var selectCommand = new SqliteCommand(selectQuery, connect))
            {
                selectCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                selectCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                selectCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read()) // 如果找到数据
                    {
                        // 更新数据
                        string updateQuery = @"
                            UPDATE bussiness_day_record
                            SET total_printer_out_credit = total_printer_out_credit + :credit,
                                total_printer_out_as_money = total_printer_out_as_money + :money,
                                credit_after = :creditAfter
                            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                        ;

                        using (var updateCommand = new SqliteCommand(updateQuery, connect))
                        {
                            updateCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                            updateCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                            updateCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);
                            updateCommand.Parameters.AddWithValue(":credit", credit);
                            updateCommand.Parameters.AddWithValue(":money", money);
                            updateCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                            updateCommand.ExecuteNonQuery();
                        }

                        DebugUtils.Log("Data updated successfully.");
                        return; // 退出方法，因为数据已经更新
                    }
                }
            }

            // 如果没有找到数据，则插入新数据
            string insertQuery = @"
                INSERT INTO bussiness_day_record (
                    created_at,
                    total_printer_out_credit,
                    total_printer_out_as_money,
                    credit_brfore,
                    credit_after
                ) VALUES (
                    :createdAt,
                    :credit, 
                    :money,
                    :creditBefore,
                    :creditAfter
                )";

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                //insertCommand.Parameters.AddWithValue(":createdAt", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                insertCommand.Parameters.AddWithValue(":createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                insertCommand.Parameters.AddWithValue(":credit", credit);
                insertCommand.Parameters.AddWithValue(":money", money);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... 为其他字段设置参数值，如果需要的话 ...

                insertCommand.ExecuteNonQuery();

                DebugUtils.Log("New data inserted successfully.");
            }

        });
    }

    [Button]
    public void AddTotalBillIn(long credit, long money, long creditAfter)
    {

        //Bill In 的账目归到 Coin In 中

        return;

        long creditBefore = creditAfter - credit;

        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {


            DateTime targetDate = DateTime.Today;
            DateTime startOfDay = targetDate.Date;// 获取该日期的零点时间
            DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// 获取该日期的结束时间（即第二天的零点前一毫秒）
            long startTimestamp = GetMillisecondsTimestamp(startOfDay);// 将零点时间转换为毫秒级时间戳
            long endTimestamp = GetMillisecondsTimestamp(endOfDay);// 将结束时间转换为毫秒级时间戳



            // 检查是否存在数据
            string selectQuery = @"
                SELECT * FROM bussiness_day_record
                WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
            ;

            using (var selectCommand = new SqliteCommand(selectQuery, connect))
            {
                selectCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                selectCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                selectCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read()) // 如果找到数据
                    {
                        // 更新数据
                        string updateQuery = @"
                            UPDATE bussiness_day_record
                            SET total_bill_in_credit = total_bill_in_credit + :credit,
                                total_bill_in_as_money = total_bill_in_as_money + :money,
                                credit_after = :creditAfter
                            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                        ;

                        using (var updateCommand = new SqliteCommand(updateQuery, connect))
                        {
                            updateCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                            updateCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                            updateCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);
                            updateCommand.Parameters.AddWithValue(":credit", credit);
                            updateCommand.Parameters.AddWithValue(":money", money);
                            updateCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                            updateCommand.ExecuteNonQuery();
                        }

                        DebugUtils.Log("Data updated successfully.");
                        return; // 退出方法，因为数据已经更新
                    }
                }
            }

            // 如果没有找到数据，则插入新数据
            string insertQuery = @"
                INSERT INTO bussiness_day_record (
                    created_at,
                    total_bill_in_credit,
                    total_bill_in_as_money,
                    credit_before,
                    credit_after
                ) VALUES (
                    :createdAt,
                    :credit,
                    :money,
                    :creditBefore,
                    :creditAfter
                )";

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                //insertCommand.Parameters.AddWithValue(":createdAt", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                insertCommand.Parameters.AddWithValue(":createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                insertCommand.Parameters.AddWithValue(":credit", credit);
                insertCommand.Parameters.AddWithValue(":money", money);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... 为其他字段设置参数值，如果需要的话 ...

                insertCommand.ExecuteNonQuery();

                DebugUtils.Log("New data inserted successfully.");
            }

        });
    }

    [Button]
    public void AddTotalCoinIn(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;
 

        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {


            DateTime targetDate = DateTime.Today;
            DateTime startOfDay = targetDate.Date;// 获取该日期的零点时间
            DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// 获取该日期的结束时间（即第二天的零点前一毫秒）
            long startTimestamp = GetMillisecondsTimestamp(startOfDay);// 将零点时间转换为毫秒级时间戳
            long endTimestamp = GetMillisecondsTimestamp(endOfDay);// 将结束时间转换为毫秒级时间戳



            // 检查是否存在数据
            string selectQuery = @"
                SELECT * FROM bussiness_day_record
                WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
            ;

            using (var selectCommand = new SqliteCommand(selectQuery, connect))
            {
                selectCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                selectCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                selectCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read()) // 如果找到数据
                    {
                        // 更新数据
                        string updateQuery = @"
                            UPDATE bussiness_day_record
                            SET total_coin_in_credit = total_coin_in_credit + :credit,
                                credit_after = :creditAfter
                            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                        ;

                        using (var updateCommand = new SqliteCommand(updateQuery, connect))
                        {
                            updateCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                            updateCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                            updateCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);
                            updateCommand.Parameters.AddWithValue(":credit", credit);
                            updateCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                            updateCommand.ExecuteNonQuery();
                        }

                        DebugUtils.Log("Data updated successfully.");
                        return; // 退出方法，因为数据已经更新
                    }
                }
            }

            // 如果没有找到数据，则插入新数据
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

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                //insertCommand.Parameters.AddWithValue(":createdAt", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                insertCommand.Parameters.AddWithValue(":createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                insertCommand.Parameters.AddWithValue(":credit", credit);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... 为其他字段设置参数值，如果需要的话 ...

                insertCommand.ExecuteNonQuery();

                DebugUtils.Log("New data inserted successfully.");
            }

        });
    }


    [Button]
    public void AddTotalCoinOut(long credit, long creditAfter)
    {
        long creditBefore = creditAfter + credit;


        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {


            DateTime targetDate = DateTime.Today;
            DateTime startOfDay = targetDate.Date;// 获取该日期的零点时间
            DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// 获取该日期的结束时间（即第二天的零点前一毫秒）
            long startTimestamp = GetMillisecondsTimestamp(startOfDay);// 将零点时间转换为毫秒级时间戳
            long endTimestamp = GetMillisecondsTimestamp(endOfDay);// 将结束时间转换为毫秒级时间戳



            // 检查是否存在数据
            string selectQuery = @"
                SELECT * FROM bussiness_day_record
                WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
            ;

            using (var selectCommand = new SqliteCommand(selectQuery, connect))
            {
                selectCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                selectCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                selectCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read()) // 如果找到数据
                    {
                        // 更新数据
                        string updateQuery = @"
                            UPDATE bussiness_day_record
                            SET total_coin_out_credit = total_coin_out_credit + :credit,
                                credit_after = :creditAfter
                            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                        ;

                        using (var updateCommand = new SqliteCommand(updateQuery, connect))
                        {
                            updateCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                            updateCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                            updateCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);
                            updateCommand.Parameters.AddWithValue(":credit", credit);
                            updateCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                            updateCommand.ExecuteNonQuery();
                        }

                        DebugUtils.Log("Data updated successfully.");
                        return; // 退出方法，因为数据已经更新
                    }
                }
            }

            // 如果没有找到数据，则插入新数据
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

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                //insertCommand.Parameters.AddWithValue(":createdAt", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                insertCommand.Parameters.AddWithValue(":createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                insertCommand.Parameters.AddWithValue(":credit", credit);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... 为其他字段设置参数值，如果需要的话 ...

                insertCommand.ExecuteNonQuery();

                DebugUtils.Log("New data inserted successfully.");
            }

        });
    }



    [Button]
    public void AddTotalScoreUp(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;


        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {


            DateTime targetDate = DateTime.Today;
            DateTime startOfDay = targetDate.Date;// 获取该日期的零点时间
            DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// 获取该日期的结束时间（即第二天的零点前一毫秒）
            long startTimestamp = GetMillisecondsTimestamp(startOfDay);// 将零点时间转换为毫秒级时间戳
            long endTimestamp = GetMillisecondsTimestamp(endOfDay);// 将结束时间转换为毫秒级时间戳



            // 检查是否存在数据
            string selectQuery = @"
                SELECT * FROM bussiness_day_record
                WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
            ;

            using (var selectCommand = new SqliteCommand(selectQuery, connect))
            {
                selectCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                selectCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                selectCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read()) // 如果找到数据
                    {
                        // 更新数据
                        string updateQuery = @"
                            UPDATE bussiness_day_record
                            SET total_score_up_credit = total_score_up_credit + :credit,
                                credit_after = :creditAfter
                            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                        ;

                        using (var updateCommand = new SqliteCommand(updateQuery, connect))
                        {
                            updateCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                            updateCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                            updateCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);
                            updateCommand.Parameters.AddWithValue(":credit", credit);
                            updateCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                            updateCommand.ExecuteNonQuery();
                        }

                        //DebugUtil.Log("Data updated successfully.");
                        return; // 退出方法，因为数据已经更新
                    }
                }
            }

            // 如果没有找到数据，则插入新数据
            string insertQuery = @"
                INSERT INTO bussiness_day_record (
                    created_at,
                    total_score_up_credit,
                    credit_before,
                    credit_after
                ) VALUES (
                    :createdAt,
                    :credit,
                    :creditBefore,
                    :creditAfter
                )";

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                //insertCommand.Parameters.AddWithValue(":createdAt", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                insertCommand.Parameters.AddWithValue(":createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                insertCommand.Parameters.AddWithValue(":credit", credit);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... 为其他字段设置参数值，如果需要的话 ...

                insertCommand.ExecuteNonQuery();

                DebugUtils.Log("New data inserted successfully.");
            }

        });
    }


    [Button]
    public void AddTotalScoreDown(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;

        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {

            DateTime targetDate = DateTime.Today;
            DateTime startOfDay = targetDate.Date;// 获取该日期的零点时间
            DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// 获取该日期的结束时间（即第二天的零点前一毫秒）
            long startTimestamp = GetMillisecondsTimestamp(startOfDay);// 将零点时间转换为毫秒级时间戳
            long endTimestamp = GetMillisecondsTimestamp(endOfDay);// 将结束时间转换为毫秒级时间戳



            // 检查是否存在数据
            string selectQuery = @"
                SELECT * FROM bussiness_day_record
                WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
            ;

            using (var selectCommand = new SqliteCommand(selectQuery, connect))
            {
                selectCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                selectCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                selectCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read()) // 如果找到数据
                    {
                        // 更新数据
                        string updateQuery = @"
                            UPDATE bussiness_day_record
                            SET total_score_down_credit = total_score_down_credit + :credit,
                                credit_after = :creditAfter
                            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                        ;

                        using (var updateCommand = new SqliteCommand(updateQuery, connect))
                        {
                            updateCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                            updateCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                            updateCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);
                            updateCommand.Parameters.AddWithValue(":credit", credit);
                            updateCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                            updateCommand.ExecuteNonQuery();
                        }

                        DebugUtils.Log("Data updated successfully.");
                        return; // 退出方法，因为数据已经更新
                    }
                }
            }

            // 如果没有找到数据，则插入新数据
            string insertQuery = @"
                INSERT INTO bussiness_day_record (
                    created_at,
                    total_score_down_credit,
                    credit_before,
                    credit_after
                ) VALUES (
                    :createdAt,
                    :credit,
                    :creditBefore,
                    :creditAfter
                )";

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                //insertCommand.Parameters.AddWithValue(":createdAt", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                insertCommand.Parameters.AddWithValue(":createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                insertCommand.Parameters.AddWithValue(":credit", credit);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... 为其他字段设置参数值，如果需要的话 ...

                insertCommand.ExecuteNonQuery();

                DebugUtils.Log("New data inserted successfully.");
            }

        });
    }


    [Button]
    public void AddTotalBet(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;

        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {


            DateTime targetDate = DateTime.Today;
            DateTime startOfDay = targetDate.Date;// 获取该日期的零点时间
            DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// 获取该日期的结束时间（即第二天的零点前一毫秒）
            long startTimestamp = GetMillisecondsTimestamp(startOfDay);// 将零点时间转换为毫秒级时间戳
            long endTimestamp = GetMillisecondsTimestamp(endOfDay);// 将结束时间转换为毫秒级时间戳




            // 检查是否存在数据
            string selectQuery = @"
                SELECT * FROM bussiness_day_record
                WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
            ;

            using (var selectCommand = new SqliteCommand(selectQuery, connect))
            {
                selectCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                selectCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                selectCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read()) // 如果找到数据
                    {
                        // 更新数据
                        string updateQuery = @"
                            UPDATE bussiness_day_record
                            SET total_bet_credit = total_bet_credit + :credit,
                                credit_after = :creditAfter
                            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                        ;

                        using (var updateCommand = new SqliteCommand(updateQuery, connect))
                        {
                            updateCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                            updateCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                            updateCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);
                            updateCommand.Parameters.AddWithValue(":credit", credit);
                            updateCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                            updateCommand.ExecuteNonQuery();
                        }

                        DebugUtils.Log("Data updated successfully.");
                        return; // 退出方法，因为数据已经更新
                    }
                }
            }

            // 如果没有找到数据，则插入新数据
            string insertQuery = @"
                INSERT INTO bussiness_day_record (
                    created_at,
                    total_bet_credit,
                    credit_before,
                    credit_after
                ) VALUES (
                    :createdAt,
                    :credit,
                    :creditBefore,
                    :creditAfter
                )";

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                //insertCommand.Parameters.AddWithValue(":createdAt", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                insertCommand.Parameters.AddWithValue(":createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                insertCommand.Parameters.AddWithValue(":credit", credit);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... 为其他字段设置参数值，如果需要的话 ...

                insertCommand.ExecuteNonQuery();

                DebugUtils.Log("New data inserted successfully.");
            }

        });
    }


    [Button]
    public void AddTotalWin(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;

        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {


            DateTime targetDate = DateTime.Today;
            DateTime startOfDay = targetDate.Date;// 获取该日期的零点时间
            DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// 获取该日期的结束时间（即第二天的零点前一毫秒）
            long startTimestamp = GetMillisecondsTimestamp(startOfDay);// 将零点时间转换为毫秒级时间戳
            long endTimestamp = GetMillisecondsTimestamp(endOfDay);// 将结束时间转换为毫秒级时间戳



            // 检查是否存在数据
            string selectQuery = @"
                SELECT * FROM bussiness_day_record
                WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
            ;

            using (var selectCommand = new SqliteCommand(selectQuery, connect))
            {
                selectCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                selectCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                selectCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read()) // 如果找到数据
                    {
                        // 更新数据
                        string updateQuery = @"
                            UPDATE bussiness_day_record
                            SET total_win_credit = total_win_credit + :credit,
                                credit_after = :creditAfter
                            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                        ;

                        using (var updateCommand = new SqliteCommand(updateQuery, connect))
                        {
                            updateCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                            updateCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                            updateCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);
                            updateCommand.Parameters.AddWithValue(":credit", credit);
                            updateCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                            updateCommand.ExecuteNonQuery();
                        }

                        DebugUtils.Log("Data updated successfully.");
                        return; // 退出方法，因为数据已经更新
                    }
                }
            }

            // 如果没有找到数据，则插入新数据
            string insertQuery = @"
                INSERT INTO bussiness_day_record (
                    created_at,
                    total_win_credit,
                    credit_before,
                    credit_after
                ) VALUES (
                    :createdAt,
                    :credit,
                    :creditBefore,
                    :creditAfter
                )";

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                //insertCommand.Parameters.AddWithValue(":createdAt", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                insertCommand.Parameters.AddWithValue(":createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                insertCommand.Parameters.AddWithValue(":credit", credit);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... 为其他字段设置参数值，如果需要的话 ...

                insertCommand.ExecuteNonQuery();

                DebugUtils.Log("New data inserted successfully.");
            }

        });
    }



    [Button]
    void TestTimestamp()
    {

        // 指定日期
        DateTime targetDate = new DateTime(2025, 2, 17);  
        //  DateTime targetDate = DateTime.Today;

        // 获取该日期的零点时间
        DateTime startOfDay = targetDate.Date;

        // 获取该日期的结束时间（即第二天的零点前一毫秒）
        DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);

        // 将零点时间转换为毫秒级时间戳
        long startTimestamp = GetMillisecondsTimestamp(startOfDay);

        // 将结束时间转换为毫秒级时间戳
        long endTimestamp = GetMillisecondsTimestamp(endOfDay);

        DebugUtils.Log($" startTimestamp = {startTimestamp}  endTimestamp ={endTimestamp} ");
    }

    // 将 DateTime 转换为毫秒级时间戳的方法
    static long GetMillisecondsTimestamp(DateTime dateTime)
    {
        // 定义 Unix 时间戳的起始时间
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // 将输入的日期时间转换为 UTC 时间
        DateTime utcDateTime = dateTime.ToUniversalTime();

        // 计算时间差并转换为毫秒
        return (long)(utcDateTime - unixEpoch).TotalMilliseconds;
    }





    /// <summary>
    /// 基本游戏赢分
    /// </summary>
    /// <param name="bet"></param>
    /// <param name="win"></param>
    /// <param name="creditAfter"></param>
    [Button]
    public void AddTotalBetWin(long bet, long win, long creditAfter)
    {
        long creditBefore = creditAfter - win + bet;


        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {

            DateTime targetDate = DateTime.Today;
            DateTime startOfDay = targetDate.Date;// 获取该日期的零点时间
            DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// 获取该日期的结束时间（即第二天的零点前一毫秒）
            long startTimestamp = GetMillisecondsTimestamp(startOfDay);// 将零点时间转换为毫秒级时间戳
            long endTimestamp = GetMillisecondsTimestamp(endOfDay);// 将结束时间转换为毫秒级时间戳



            // 检查是否存在数据
            /**/
            string selectQuery = @"
                 SELECT * FROM bussiness_day_record
                 WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
             ;


            //DebugUtil.Log($"selectQuery1 = {selectQuery}");
            //string selectQuery2 = $" SELECT * FROM {ConsoleTableName.TABLE_BUSINESS_DAY_RECORD} WHERE created_at >= {startTimestamp} AND created_at {endTimestamp}" ;
            using (var selectCommand = new SqliteCommand(selectQuery, connect))
            {
                //selectCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                selectCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                selectCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);

                //DebugUtil.Log($"selectQuery = {selectQuery2}");

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read()) // 如果找到数据
                    {
                        // 更新数据
                        string updateQuery = @"
                            UPDATE bussiness_day_record
                            SET total_bet_credit = total_bet_credit + :bet,
                                total_win_credit = total_win_credit + :win,
                                credit_after = :creditAfter
                            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                        ;

                        using (var updateCommand = new SqliteCommand(updateQuery, connect))
                        {
                            updateCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                            updateCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                            updateCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);
                            updateCommand.Parameters.AddWithValue(":bet", bet);
                            updateCommand.Parameters.AddWithValue(":win", win);
                            updateCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                            updateCommand.ExecuteNonQuery();
                        }

                        DebugUtils.Log("Data updated successfully.");
                        return; // 退出方法，因为数据已经更新
                    }
                }
            }

            // 如果没有找到数据，则插入新数据
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

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                //insertCommand.Parameters.AddWithValue(":createdAt", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                insertCommand.Parameters.AddWithValue(":createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                insertCommand.Parameters.AddWithValue(":bet", bet);
                insertCommand.Parameters.AddWithValue(":win", win);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... 为其他字段设置参数值，如果需要的话 ...

                insertCommand.ExecuteNonQuery();

                DebugUtils.Log("New data inserted successfully.");
            }

        });
    }



    [Button]
    public void AddJackpotWin(long win, long myCredit)
    {
        long creditBefore = myCredit - win;
        long creditAfter = myCredit;

        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {

            DateTime targetDate = DateTime.Today;
            DateTime startOfDay = targetDate.Date;// 获取该日期的零点时间
            DateTime endOfDay = startOfDay.AddDays(1).AddMilliseconds(-1);// 获取该日期的结束时间（即第二天的零点前一毫秒）
            long startTimestamp = GetMillisecondsTimestamp(startOfDay);// 将零点时间转换为毫秒级时间戳
            long endTimestamp = GetMillisecondsTimestamp(endOfDay);// 将结束时间转换为毫秒级时间戳



            // 检查是否存在数据
            /**/
            string selectQuery = @"
                 SELECT * FROM bussiness_day_record
                 WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
             ;


            //DebugUtil.Log($"selectQuery1 = {selectQuery}");
            //string selectQuery2 = $" SELECT * FROM {ConsoleTableName.TABLE_BUSINESS_DAY_RECORD} WHERE created_at >= {startTimestamp} AND created_at {endTimestamp}" ;
            using (var selectCommand = new SqliteCommand(selectQuery, connect))
            {
                //selectCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                selectCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                selectCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);

                //DebugUtil.Log($"selectQuery = {selectQuery2}");

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read()) // 如果找到数据
                    {
                        // 更新数据
                        string updateQuery = @"
                            UPDATE bussiness_day_record
                            SET total_win_credit = total_win_credit + :win,
                                credit_after = :creditAfter
                            WHERE created_at >= :startTimestamp AND created_at < :endTimestamp"
                        ;

                        using (var updateCommand = new SqliteCommand(updateQuery, connect))
                        {
                            updateCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                            updateCommand.Parameters.AddWithValue(":startTimestamp", startTimestamp);
                            updateCommand.Parameters.AddWithValue(":endTimestamp", endTimestamp);
                            updateCommand.Parameters.AddWithValue(":win", win);
                            updateCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                            updateCommand.ExecuteNonQuery();
                        }

                        DebugUtils.Log("Data updated successfully.");
                        return; // 退出方法，因为数据已经更新
                    }
                }
            }

            // 如果没有找到数据，则插入新数据
            string insertQuery = @"
                INSERT INTO bussiness_day_record (
                    created_at,
                    total_win_credit,
                    credit_before,
                    credit_after
                ) VALUES (
                    :createdAt,
                    :win,
                    :creditBefore,
                    :creditAfter
                )";

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":tabName", ConsoleTableName.TABLE_BUSINESS_DAY_RECORD);
                //insertCommand.Parameters.AddWithValue(":createdAt", DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                insertCommand.Parameters.AddWithValue(":createdAt", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                insertCommand.Parameters.AddWithValue(":win", win);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... 为其他字段设置参数值，如果需要的话 ...

                insertCommand.ExecuteNonQuery();

                DebugUtils.Log("New data inserted successfully.");
            }

        });
    }






}
