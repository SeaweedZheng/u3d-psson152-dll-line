using GameMaker;
using Mono.Data.Sqlite;
using Sirenix.OdinInspector;
using System;
using _mainBB = GameMaker.MainBlackboard;


/// <summary>
/// ������ű������á�
/// </summary>
//public class TableJackpotRecordManager : MonoSingleton<TableJackpotRecordManager>
public class TableJackpotRecordManager : MonoSingleton<TableJackpotRecordManager>
{

    [Button]
    public void AddJackpotRecord(int jpLevel, string jpName, long winCredit, long creditBefore, long creditAfter, string gameUID = "-1", long? createdAt = null) 
    {
        if (createdAt == null)
        {
            createdAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }


        SQLiteHelper.Instance.OpenDB(ConsoleTableName.DB_NAME, (connect) =>
        {
            // ���û���ҵ����ݣ������������
            string insertQuery = @"
                INSERT INTO jackpot_record (
                    created_at,
                    user_id,
                    game_id,
                    game_uid,
                    jp_name,
                    jp_level,
                    win_credit,
                    credit_before,
                    credit_after
                ) VALUES (
                    :createdAt,
                    :userID,
                    :gameID,
                    :gameUID,
                    :jpName,
                    :jplevel,
                    :winCredit,
                    :creditBefore,
                    :creditAfter
                )";

            using (var insertCommand = new SqliteCommand(insertQuery, connect))
            {
                insertCommand.Parameters.AddWithValue(":createdAt", createdAt);
                insertCommand.Parameters.AddWithValue(":userID", 0);
                //insertCommand.Parameters.AddWithValue(":gameID", 152);
                insertCommand.Parameters.AddWithValue(":gameID", _mainBB.Instance.gameID);
                insertCommand.Parameters.AddWithValue(":gameUID", gameUID);
                insertCommand.Parameters.AddWithValue(":jpName", jpName);
                insertCommand.Parameters.AddWithValue(":jpLevel", jpLevel);
                insertCommand.Parameters.AddWithValue(":winCredit", winCredit);
                insertCommand.Parameters.AddWithValue(":creditBefore", creditBefore);
                insertCommand.Parameters.AddWithValue(":creditAfter", creditAfter);
                // ... Ϊ�����ֶ����ò���ֵ�������Ҫ�Ļ� ...

                insertCommand.ExecuteNonQuery();
            }

        });
    }
}
