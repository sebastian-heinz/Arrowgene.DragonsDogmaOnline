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
            "id", "character_id", "job", "item_id", "equip_type", "equip_slot", "color", "plus_value"
        };

        private readonly string SqlInsertEquipItem = $"INSERT INTO `ddon_equip_item` ({BuildQueryField(CDataEquipItemFields)}) VALUES ({BuildQueryInsert(CDataEquipItemFields)});";
        private readonly string SqlReplaceEquipItem = $"INSERT OR REPLACE INTO `ddon_equip_item` ({BuildQueryField(CDataEquipItemFields)}) VALUES ({BuildQueryInsert(CDataEquipItemFields)});";
        private static readonly string SqlUpdateEquipItem = $"UPDATE `ddon_equip_item` SET {BuildQueryUpdate(CDataEquipItemFields)} WHERE `character_id` = @character_id AND `job` = @job AND `equip_type`=@equip_type AND `equip_slot`=@equip_slot;";
        private static readonly string SqlSelectEquipItem = $"SELECT {BuildQueryField(CDataEquipItemFields)} FROM `ddon_equip_item` WHERE `character_id` = @character_id AND `job` = @job AND `equip_type`=1;";
        private static readonly string SqlSelectVisualEquipItem = $"SELECT {BuildQueryField(CDataEquipItemFields)} FROM `ddon_equip_item` WHERE `character_id` = @character_id AND `job` = @job AND `equip_type`=2;";
        private static readonly string SqlSelectEquipItemByCharacter = $"SELECT {BuildQueryField(CDataEquipItemFields)} FROM `ddon_equip_item` WHERE `character_id` = @character_id AND `equip_type`=1;";
        private static readonly string SqlSelectVisualEquipItemByCharacter = $"SELECT {BuildQueryField(CDataEquipItemFields)} FROM `ddon_equip_item` WHERE `character_id` = @character_id AND `equip_type`=2;";
        private const string SqlDeleteEquipItemInfo = "DELETE FROM `ddon_equip_item` WHERE `character_id`=@character_id AND `job`=@job AND `equip_type`=@equip_type AND `equip_slot`=@equip_slot;";

        private EquipItem ReadEquipItem(DbDataReader reader)
        {
            EquipItem equipItem = new EquipItem();
            equipItem.EquipItemUId = GetString(reader, "id");
            equipItem.ItemId = GetUInt32(reader, "item_id");
            equipItem.EquipType = GetByte(reader, "equip_type");
            equipItem.EquipSlot = GetUInt16(reader, "equip_slot");
            equipItem.Color = GetByte(reader, "color");
            equipItem.PlusValue = GetByte(reader, "plus_value");
            return equipItem;
        }

        private void AddParameter(TCom command, uint characterId, JobId job, EquipItem equipItem)
        {
            AddParameter(command, "id", equipItem.EquipItemUId);
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "job", (byte) job);
            AddParameter(command, "item_id", equipItem.ItemId);
            AddParameter(command, "equip_type", equipItem.EquipType);
            AddParameter(command, "equip_slot", equipItem.EquipSlot);
            AddParameter(command, "color", equipItem.Color);
            AddParameter(command, "plus_value", equipItem.PlusValue);
        }
    }
}