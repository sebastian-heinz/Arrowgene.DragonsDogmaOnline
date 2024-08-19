#nullable enable
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Data.Common;

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
        private static readonly string SqlDeleteEquipItem = "DELETE FROM \"ddon_equip_item\" WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"equip_type\"=@equip_type AND \"equip_slot\"=@equip_slot;";

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
     
        public bool ReplaceEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                Logger.Debug("Inserting equip item.");
                if (!InsertIfNotExistsEquipItem(connection, commonId, job, equipType, equipSlot, itemUId))
                {
                    Logger.Debug("Equip item already exists, replacing.");
                    return UpdateEquipItem(connection, commonId, job, equipType, equipSlot, itemUId);
                }
                return true;
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
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

        public bool DeleteEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                return ExecuteNonQuery(connection, SqlDeleteEquipItem, command =>
                {
                    AddParameter((TCom)command, commonId, job, equipType, equipSlot);
                }) == 1;
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
        }

        public List<EquipItem> SelectEquipItemByCharacter(uint characterCommonId)
        {
            using TCon connection = OpenNewConnection();
            return SelectEquipItemByCharacter(connection, characterCommonId);
        }

        public List<EquipItem> SelectEquipItemByCharacter(TCon connection, uint characterCommonId)
        {
            List<EquipItem> results = new List<EquipItem>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectEquipItemByCharacter,
                    command => {
                        AddParameter(command, "@character_common_id", characterCommonId);
                    }, reader => {
                        while (reader.Read())
                        {
                            results.Add(ReadEquipItem(reader));
                        }
                    });
            });

            return results;
        }

        private EquipItem ReadEquipItem(TReader reader)
        {
            EquipItem obj = new EquipItem();
            obj.UId = GetString(reader, "item_uid");
            obj.CharacterCommonId = GetUInt32(reader, "character_common_id");
            obj.Job = (JobId)GetByte(reader, "job");
            obj.EquipType = (EquipType)GetByte(reader, "equip_type");
            obj.EquipSlot = GetByte(reader, "equip_slot");
            return obj;
        }


        private void AddParameter(TCom command, uint commonId, JobId job, EquipType equipType, byte equipSlot)

        {
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "job", (byte) job);
            AddParameter(command, "equip_type", (byte) equipType);
            AddParameter(command, "equip_slot", equipSlot);
        }

        private void AddParameter(TCom command, uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId)
        {
            AddParameter(command, "item_uid", itemUId);
            AddParameter(command, commonId, job, equipType, equipSlot);
        }
    }
}
