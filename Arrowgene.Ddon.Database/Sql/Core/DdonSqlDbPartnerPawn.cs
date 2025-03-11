using Arrowgene.Ddon.Shared.Model;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        /* ddon_partner_pawn */
        protected static readonly string[] PartnerPawnFields= new string[]
        {
            "character_id", "pawn_id", "personality", "num_gifts", "num_crafts", "num_adventures"
        };

        private readonly string SqlSelectPartnerPawnRecord = $"SELECT {BuildQueryField(PartnerPawnFields)} FROM \"ddon_partner_pawn\" WHERE \"character_id\"=@character_id AND \"pawn_id\" = @pawn_id;";
        private readonly string SqlInsertPartnerPawnRecord = $"INSERT INTO \"ddon_partner_pawn\" ({BuildQueryField(PartnerPawnFields)}) VALUES ({BuildQueryInsert(PartnerPawnFields)});";
        private readonly string SqlUpdatePartnerPawnRecord = $"UPDATE \"ddon_partner_pawn\" SET {BuildQueryUpdate(PartnerPawnFields)} WHERE \"character_id\" = @character_id AND \"pawn_id\" = @pawn_id;";

        public bool InsertPartnerPawnRecord(uint characterId, PartnerPawnData partnerPawnData, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlInsertPartnerPawnRecord, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "pawn_id", partnerPawnData.PawnId);
                    AddParameter(command, "personality", (byte)partnerPawnData.Personality);
                    AddParameter(command, "num_gifts", partnerPawnData.NumGifts);
                    AddParameter(command, "num_crafts", partnerPawnData.NumCrafts);
                    AddParameter(command, "num_adventures", partnerPawnData.NumAdventures);
                }) == 1;
            });
        }

        public bool UpdatePartnerPawnRecord(uint characterId, PartnerPawnData partnerPawnData, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlUpdatePartnerPawnRecord, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "pawn_id", partnerPawnData.PawnId);
                    AddParameter(command, "personality", (byte) partnerPawnData.Personality);
                    AddParameter(command, "num_gifts", partnerPawnData.NumGifts);
                    AddParameter(command, "num_crafts", partnerPawnData.NumCrafts);
                    AddParameter(command, "num_adventures", partnerPawnData.NumAdventures);
                }) == 1;
            });
        }

        public PartnerPawnData GetPartnerPawnRecord(uint characterId, uint pawnId, DbConnection? connectionIn = null)
        {
            PartnerPawnData result = null;
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectPartnerPawnRecord, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "pawn_id", pawnId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        result = new PartnerPawnData()
                        {
                            PawnId = GetUInt32(reader, "pawn_id"),
                            Personality = (PawnPersonality) GetByte(reader, "personality"),
                            NumGifts = GetUInt32(reader, "num_gifts"),
                            NumCrafts = GetUInt32(reader, "num_crafts"),
                            NumAdventures = GetUInt32(reader, "num_adventures"),
                        };
                    }
                });
            });
            return result;
        }

        public bool SetPartnerPawn(uint characterId, uint pawnId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlUpdatePartnerPawnId, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "partner_pawn_id", pawnId);
                }) == 1;
            });
        }
    }
}
