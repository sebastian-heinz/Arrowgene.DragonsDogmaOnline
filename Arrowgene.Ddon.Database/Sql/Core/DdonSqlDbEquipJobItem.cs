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
        private readonly string SqlInsertIfNotExistsEquipJobItem = $"INSERT INTO \"ddon_equip_job_item\" ({BuildQueryField(CDataEquipJobItemFields)}) SELECT {BuildQueryInsert(CDataEquipJobItemFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_equip_job_item\" WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job);";
        private static readonly string SqlUpdateEquipJobItem = $"UPDATE \"ddon_equip_job_item\" SET {BuildQueryUpdate(CDataEquipJobItemFields)} WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job AND \"equip_slot\"=@equip_slot;";
        private static readonly string SqlSelectEquipJobItems = $"SELECT {BuildQueryField(CDataEquipJobItemFields)} FROM \"ddon_equip_job_item\" WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job;";
        private static readonly string SqlSelectEquipJobItemsByCharacter = $"SELECT {BuildQueryField(CDataEquipJobItemFields)} FROM \"ddon_equip_job_item\" WHERE \"character_common_id\" = @character_common_id;";
        private const string SqlDeleteEquipJobItem = "DELETE FROM \"ddon_equip_job_item\" WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"equip_slot\"=@equip_slot;";
        
        public bool InsertIfNotExistsEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsEquipJobItem(connection, itemUId, commonId, job, slotNo);
        }

        public bool InsertIfNotExistsEquipJobItem(TCon conn, string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            return ExecuteNonQuery(conn, SqlInsertIfNotExistsEquipJobItem, command =>
            {
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "job", (byte) job);
                AddParameter(command, "equip_slot", slotNo);
            }) == 1;
        }      
        
        public bool InsertEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            using TCon connection = OpenNewConnection();
            return InsertEquipJobItem(connection, itemUId, commonId, job, slotNo);
        }

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

        public bool ReplaceEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceEquipJobItem(connection, itemUId, commonId, job, slotNo);
        }
        
        public bool ReplaceEquipJobItem(TCon conn, string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            Logger.Debug("Inserting equp job item.");
            if (!InsertIfNotExistsEquipJobItem(conn, itemUId, commonId, job, slotNo))
            {
                Logger.Debug("Equip job item already exists, replacing.");
                return UpdateEquipJobItem(conn, itemUId, commonId, job, slotNo);
            }
            return true;
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
        
        public bool UpdateEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            using TCon connection = OpenNewConnection();
            return UpdateEquipJobItem(connection, itemUId, commonId, job, slotNo);
        }        
        
        public bool UpdateEquipJobItem(TCon connection, string itemUId, uint commonId, JobId job, ushort slotNo)
        {
            return ExecuteNonQuery(connection, SqlUpdateEquipJobItem, command =>
            {
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "job", (byte) job);
                AddParameter(command, "equip_slot", slotNo);
            }) == 1;
        }
    }
}
