using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] CDataEquipItemFields = new string[]
        {
            "item_uid", "character_common_id", "job", "equip_type", "equip_slot"
        };

        private readonly string SqlInsertEquipItem = $"INSERT INTO \"ddon_equip_item\" ({BuildQueryField(CDataEquipItemFields)}) VALUES ({BuildQueryInsert(CDataEquipItemFields)});";
        private readonly string SqlInsertIfNotExistsEquipItem = $"INSERT INTO \"ddon_equip_item\" ({BuildQueryField(CDataEquipItemFields)}) SELECT {BuildQueryInsert(CDataEquipItemFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_equip_item\" WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"equip_type\"=@equip_type AND \"equip_slot\"=@equip_slot);";
        private static readonly string SqlUpdateEquipItem = $"UPDATE \"ddon_equip_item\" SET {BuildQueryUpdate(CDataEquipItemFields)} WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"equip_type\"=@equip_type AND \"equip_slot\"=@equip_slot;";
        private static readonly string SqlSelectEquipItemByCharacter = $"SELECT {BuildQueryField(CDataEquipItemFields)} FROM \"ddon_equip_item\" WHERE \"character_common_id\"=@character_common_id;";
        private static readonly string SqlDeleteEquipItem = "DELETE FROM \"ddon_equip_item\" WHERE \"item_uid\"=@item_uid AND \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"equip_type\"=@equip_type AND \"equip_slot\"=@equip_slot;";
        
        public bool InsertIfNotExistsEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsEquipItem(connection, commonId, job, equipType, equipSlot, itemUId);
        }
        
        public bool InsertIfNotExistsEquipItem(TCon conn, uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            return ExecuteNonQuery(conn, SqlInsertIfNotExistsEquipItem, command =>
            {
                AddParameter(command, commonId, job, equipType, equipSlot, itemUId);
            }) == 1;
        }        
        
        public bool InsertEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            using TCon connection = OpenNewConnection();
            return InsertEquipItem(connection, commonId, job, equipType, equipSlot, itemUId);
        }
        
        public bool InsertEquipItem(TCon conn, uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            return ExecuteNonQuery(conn, SqlInsertEquipItem, command =>
            {
                AddParameter(command, commonId, job, equipType, equipSlot, itemUId);
            }) == 1;
        }
        
        public bool ReplaceEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceEquipItem(connection, commonId, job, equipType, equipSlot, itemUId);
        }
        
        public bool ReplaceEquipItem(TCon conn, uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            Logger.Debug("Inserting equip item.");
            if (!InsertIfNotExistsEquipItem(conn, commonId, job, equipType, equipSlot, itemUId))
            {
                Logger.Debug("Equip item already exists, replacing.");
                return UpdateEquipItem(conn, commonId, job, equipType, equipSlot, itemUId);
            }
            return true;
        }

        public bool UpdateEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            using TCon connection = OpenNewConnection();
            return UpdateEquipItem(connection, commonId, job, equipType, equipSlot, itemUId);
        }
        
        public bool UpdateEquipItem(TCon connection, uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            return ExecuteNonQuery(connection, SqlUpdateEquipItem, command =>
            {
                AddParameter(command, commonId, job, equipType, equipSlot, itemUId);
            }) == 1;
        }

        public bool DeleteEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            return ExecuteNonQuery(SqlDeleteEquipItem, command =>
            {
                AddParameter(command, commonId, job, equipType, equipSlot, itemUId);
            }) == 1;
        }

        private void AddParameter(TCom command, uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            AddParameter(command, "item_uid", itemUId);
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "job", (byte) job);
            AddParameter(command, "equip_type", (byte) equipType);
            AddParameter(command, "equip_slot", equipSlot);
        }
    }
}
