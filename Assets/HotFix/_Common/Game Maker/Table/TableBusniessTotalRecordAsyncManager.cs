using GameMaker;
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ����ű�����ͳ����Ӫ�ռ�¼����Ӫ�ռ�¼
/// </summary>
public class TableBusniessTotalRecordAsyncManager : MonoSingleton<TableBusniessTotalRecordAsyncManager>
{

    /// <summary> ��ʷ��ѹע </summary>
    public long historyTotalBet
    {
        get => targer.total_bet_credit;
    }


    /// <summary> ��ʷ��Ӯ </summary>
    public long historyTotalWin
    {
        get => targer.total_win_credit;
    }



    /*
    /// <summary> ��ʷ��Ͷ�Ҹ��� </summary>
    public long historyTotalCoinInNums
    {
        get => _historyTotalCoinInNums;
    }
    long _historyTotalCoinInNums;
    */

    /// <summary> ��ʷ��Ͷ�� </summary>
    public long historyTotalCoinInCredit
    {
        get => targer.total_coin_in_credit;
    }



    /*
    /// <summary> ��ʷ����Ʊ���� ��֮ǰ�ǶԵģ�CoinOut���ص�������������汾ȱ����˷������� </summary>
    private long historyTotalCoinOutNums
    {
        get => _historyTotalCoinOutNums;
    }
    long _historyTotalCoinOutNums;
    */

    /// <summary> ��ʷ����Ʊ </summary>
    public long historyTotalCoinOutCredit
    {
        get => targer.total_coin_out_credit;
    }




    /// <summary> ��ʷ���Ϸ� </summary>
    public long historyTotalScoreUpCredit
    {
        get => targer.total_score_up_credit;
    }
 

    /// <summary> ��ʷ���·� </summary>
    public long historyTotalScoreDownCredit
    {
        get => targer.total_score_down_credit;
    }


    TableBussinessTotalRecordItem targer = null;


   /*public void OnEventClearAllOrderCache()
    {
        if(targer != null)
        {
            targer.credit_before = 0;
            targer.credit_after = 0;
            targer.total_bet_credit = 0;
            targer.total_win_credit = 0;
            targer.total_coin_in_credit = 0;
            targer.total_coin_out_credit = 0;
            targer.total_score_up_credit = 0;
            targer.total_score_down_credit = 0;
            targer.total_bill_in_credit = 0;
            targer.total_bill_in_as_money = 0;
            targer.total_printer_out_credit = 0;
            targer.total_printer_out_as_money = 0;
        }
    }*/


    public void GetTotalBusniess()
    {
        SQLiteAsyncHelper.Instance.GetDataAsync<TableBussinessTotalRecordItem>(
            ConsoleTableName.TABLE_BUSINESS_TOTAL_RECORD,
            $"SELECT * FROM bussiness_total_record WHERE id = 1 ",  //{ConsoleTableName.TABLE_BUSINESS_TOTAL_RECORD}
            TableBussinessTotalRecordItem.DefaultTable(),
            (object[] res ) =>
            {
                if ( (int)res[0] == 0)
                {
                    List<TableBussinessTotalRecordItem> lst = JsonConvert.DeserializeObject<List<TableBussinessTotalRecordItem>>((string)res[1]);
                    for (int i=0; i< lst.Count; i++)
                    {
                        if (lst[i].id == 1)
                        {
                            targer = lst[i];
                            break;
                        }
                    }
                    if(targer == null)
                    {
                        Debug.LogError($"û�л�ȡĿ�����: ��ʷ��Ͷ�ˡ���Ѻ��Ӯ");
                    }
                }
                else
                {
                    Debug.LogError((string)res[1]);
                }
            }

            );
            
    }



    public void AddTotalCoinIn(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        string selectQuery = $" SELECT * FROM bussiness_total_record WHERE id = 1 ";

        string updateQuery = $"UPDATE bussiness_total_record " +
                            $"SET total_coin_in_credit = total_coin_in_credit + {credit}, " +
                                $"credit_after = {creditAfter} " +
                            $"WHERE id = 1 ";

        string insertQuery = $"INSERT INTO bussiness_total_record ( created_at, total_coin_in_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit},  {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });

        targer.total_coin_in_credit += credit;
    }




    [Button]
    public void AddTotalCoinOut(long credit, long creditAfter)
    {
        long creditBefore = creditAfter + credit;

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        string selectQuery = $" SELECT * FROM bussiness_total_record WHERE id = 1 ";


        string updateQuery = $"UPDATE bussiness_total_record " +
                            $"SET total_coin_out_credit = total_coin_out_credit + {credit}, " +
                                $"credit_after = {creditAfter} " +
                            $"WHERE id = 1 ";

        string insertQuery = $"INSERT INTO bussiness_total_record ( created_at, total_coin_out_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit},  {creditBefore}, {creditAfter} ) ";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });

        targer.total_coin_out_credit += credit;
    }



    [Button]
    public void AddTotalScoreUp(long credit, long creditAfter)
    {
        long creditBefore = creditAfter - credit;

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        string selectQuery = $" SELECT * FROM bussiness_total_record WHERE id = 1 ";

        string updateQuery = $"UPDATE bussiness_total_record " +
                            $"SET total_score_up_credit = total_score_up_credit + {credit}, " +
                                $"credit_after = {creditAfter} " +
                            $"WHERE id = 1 ";

        string insertQuery = $"INSERT INTO bussiness_total_record ( created_at, total_score_up_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit},  {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });
        targer.total_score_up_credit += credit;
    }


    [Button]
    public void AddTotalScoreDown(long credit, long creditAfter)
    {
        long creditBefore = creditAfter + credit;

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        string selectQuery = $" SELECT * FROM bussiness_total_record WHERE id = 1 ";


        string updateQuery = $"UPDATE bussiness_total_record " +
                            $"SET total_score_down_credit = total_score_down_credit + {credit}, " +
                                $"credit_after = {creditAfter} " +
                            $"WHERE id = 1  ";

        string insertQuery = $"INSERT INTO bussiness_total_record ( created_at, total_score_down_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {credit},  {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });

        targer.total_score_down_credit += credit;
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

        long createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        string selectQuery = $" SELECT * FROM bussiness_total_record WHERE id = 1 ";

        string updateQuery = $"UPDATE bussiness_total_record " +
                        $"SET total_bet_credit = total_bet_credit + {bet}, " +
                            $"total_win_credit = total_win_credit + {win}, " +
                            $"credit_after = {creditAfter} " +
                        $"WHERE id = 1 ";

        string insertQuery = $"INSERT INTO bussiness_total_record ( created_at, total_bet_credit, total_win_credit, credit_before, credit_after  ) " +
            $"VALUES ( {createdAt}, {bet}, {win}, {creditBefore}, {creditAfter} )";

        SQLiteAsyncHelper.Instance.ExecuteUpdateOrInsertAsync(selectQuery, updateQuery, insertQuery, (isOk) =>
        {

        });

        targer.total_bet_credit += bet;

        targer.total_win_credit += win;
    }


}
