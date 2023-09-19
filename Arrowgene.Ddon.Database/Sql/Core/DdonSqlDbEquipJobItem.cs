using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] CDataEquipJobItemFields = new string[]
        {
            "item_uid", "character_common_id", "job", "equip_slot"
        };

        private readonly string SqlInsertEquipJobItem = $"INSERT INTO \"ddon_equip_job_item\" ({BuildQueryField(CDataEquipJobItemFields)}) VALUES ({BuildQueryInsert(CDataEquipJobItemFields)});";

        protected virtual string SqlReplaceEquipJobItem { get; } =
            $"INSERT OR REPLACE INTO \"ddon_equip_job_item\" ({BuildQueryField(CDataEquipJobItemFields)}) VALUES ({BuildQueryInsert(CDataEquipJobItemFields)});";

        private static readonly string SqlUpdateEquipJobItem = $"UPDATE \"ddon_equip_job_item\" SET {BuildQueryUpdate(CDataEquipJobItemFields)} WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job AND \"equip_slot\"=@equip_slot;";
        private static readonly string SqlSelectEquipJobItems = $"SELECT {BuildQueryField(CDataEquipJobItemFields)} FROM \"ddon_equip_job_item\" WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job;";
        private static readonly string SqlSelectEquipJobItemsByCharacter = $"SELECT {BuildQueryField(CDataEquipJobItemFields)} FROM \"ddon_equip_job_item\" WHERE \"character_common_id\" = @character_common_id;";
        private const string SqlDeleteEquipJobItem = "DELETE FROM \"ddon_equip_job_item\" WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"equip_slot\"=@equip_slot;";

        public bool InsertEquipJobItem(TCon conn, string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            return ExecuteNonQuery(conn, SqlInsertEquipJobItem, command =>
            {
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "job", (byte) job);
                AddParameter(command, "equip_slot", slotNo);
            }) == 1;
        }

        public bool InsertEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            return InsertEquipJobItem(null, itemUId, commonId, job, slotNo);
        }

        public bool ReplaceEquipJobItem(TCon conn, string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            return ExecuteNonQuery(conn, SqlReplaceEquipJobItem, command =>
            {
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "job", (byte) job);
                AddParameter(command, "equip_slot", slotNo);
            }) == 1;
        }

        public bool ReplaceEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            return ReplaceEquipJobItem(null, itemUId, commonId, job, slotNo);
        }

        public bool DeleteEquipJobItem(uint commonId, JobId job, ushort slotNo)
        {
            return ExecuteNonQuery(SqlDeleteEquipJobItem, command =>
            {
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "job", (byte) job);
                AddParameter(command, "equip_slot", slotNo);
            }) == 1;
        }
    }
}
