using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] CDataEquipItemFields = new string[]
        {
            "item_uid", "character_id", "job", "equip_type", "equip_slot"
        };

        private readonly string SqlInsertEquipItem = $"INSERT INTO `ddon_equip_item` ({BuildQueryField(CDataEquipItemFields)}) VALUES ({BuildQueryInsert(CDataEquipItemFields)});";
        private readonly string SqlReplaceEquipItem = $"INSERT OR REPLACE INTO `ddon_equip_item` ({BuildQueryField(CDataEquipItemFields)}) VALUES ({BuildQueryInsert(CDataEquipItemFields)});";
        private static readonly string SqlUpdateEquipItem = $"UPDATE `ddon_equip_item` SET {BuildQueryUpdate(CDataEquipItemFields)} WHERE `item_uid`=@item_uid AND `character_id`=@character_id AND `job`=@job AND `equip_type`=@equip_type AND `equip_slot`=@equip_slot;";
        private static readonly string SqlSelectEquipItemByCharacter = $"SELECT {BuildQueryField(CDataEquipItemFields)} FROM `ddon_equip_item` WHERE `character_id`=@character_id;";
        private static readonly string SqlDeleteEquipItem = "DELETE FROM `ddon_equip_item` WHERE `item_uid`=@item_uid AND `character_id`=@character_id AND `job`=@job AND `equip_type`=@equip_type AND `equip_slot`=@equip_slot;";

        public bool InsertEquipItem(TCon conn, uint characterId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            return ExecuteNonQuery(conn, SqlInsertEquipItem, command =>
            {
                AddParameter(command, characterId, job, equipType, equipSlot, itemUId);
            }) == 1;
        }

        public bool InsertEquipItem(uint characterId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            return this.InsertEquipItem(null, characterId, job, equipType, equipSlot, itemUId);
        }

        public bool UpdateEquipItem(uint characterId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            return ExecuteNonQuery(SqlUpdateEquipItem, command =>
            {
                AddParameter(command, characterId, job, equipType, equipSlot, itemUId);
            }) == 1;
        }

        public bool DeleteEquipItem(uint characterId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            return ExecuteNonQuery(SqlDeleteEquipItem, command =>
            {
                AddParameter(command, characterId, job, equipType, equipSlot, itemUId);
            }) == 1;
        }

        private void AddParameter(TCom command, uint characterId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            AddParameter(command, "item_uid", itemUId);
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "job", (byte) job);
            AddParameter(command, "equip_type", (byte) equipType);
            AddParameter(command, "equip_slot", equipSlot);
        }
    }
}