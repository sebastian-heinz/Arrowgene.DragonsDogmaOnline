using Arrowgene.Ddon.Shared.Model;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        /* ddon_partner_pawn_last_affection_increase */
        protected static readonly string[] PartnerPawnLastAffectionIncreaseFields = new string[]
        {
            "character_id", "pawn_id", "action"
        };

        private readonly string SqlSelectPartnerPawnLastAffectionIncreaseRecord = $"SELECT {BuildQueryField(PartnerPawnLastAffectionIncreaseFields)} FROM \"ddon_partner_pawn_last_affection_increase\" WHERE \"character_id\"=@character_id AND \"pawn_id\"=@pawn_id AND \"action\"=@action;";
        private readonly string SqlInsertPartnerPawnLastAffectionIncreaseRecord = $"INSERT INTO \"ddon_partner_pawn_last_affection_increase\" ({BuildQueryField(PartnerPawnLastAffectionIncreaseFields)}) VALUES ({BuildQueryInsert(PartnerPawnLastAffectionIncreaseFields)});";
        private readonly string SqlDeleteDailyPartnerPawnLastAffectionIncreaseRecords = $"DELETE FROM ddon_partner_pawn_last_affection_increase;";

        public bool InsertPartnerPawnLastAffectionIncreaseRecord(uint characterId, uint pawnId, PartnerPawnAffectionAction action, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlInsertPartnerPawnLastAffectionIncreaseRecord, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "pawn_id", pawnId);
                    AddParameter(command, "action", (byte) action);
                }) == 1;
            });
        }

        public bool HasPartnerPawnLastAffectionIncreaseRecord(uint characterId, uint pawnId, PartnerPawnAffectionAction action, DbConnection? connectionIn = null)
        {
            bool foundRecord = false;
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectPartnerPawnLastAffectionIncreaseRecord, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "pawn_id", pawnId);
                    AddParameter(command, "action", (byte) action);
                }, reader =>
                {
                    foundRecord = reader.Read();
                });
            });
            return foundRecord;
        }

        public void DeleteAllPartnerPawnLastAffectionIncreaseRecords(DbConnection? connectionIn = null)
        {
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteNonQuery(connection, SqlDeleteDailyPartnerPawnLastAffectionIncreaseRecords, command => { });
            });
        }
    }
}
