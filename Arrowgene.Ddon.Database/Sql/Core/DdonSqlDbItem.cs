using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] ItemFields = new string[]
        {
            "uid", "item_id", "item_num", "unk3", "color", "plus_value"
        };

        private readonly string SqlInsertItem = $"INSERT INTO `ddon_item` ({BuildQueryField(ItemFields)}) VALUES ({BuildQueryInsert(ItemFields)});";
        private static readonly string SqlUpdateItem = $"UPDATE `ddon_item` SET {BuildQueryUpdate(ItemFields)} WHERE `uid`=@uid;";
        private static readonly string SqlSelectItem = $"SELECT {BuildQueryField(ItemFields)} FROM `ddon_item` WHERE `uid`=@uid;";
        private static readonly string SqlDeleteItem = "DELETE FROM `ddon_item` WHERE `uid`=@uid;";

        public bool InsertItem(TCon conn, Item item)
        {
            return ExecuteNonQuery(conn, SqlInsertItem, command =>
            {
                AddParameter(command, item);
            }) == 1;
        }

        public bool InsertItem(Item item)
        {
            return this.InsertItem(null, item);
        }

        public bool UpdateItem(Item item)
        {
            return ExecuteNonQuery(SqlUpdateItem, command =>
            {
                AddParameter(command, item);
            }) == 1;
        }

        public bool DeleteItem(string uid)
        {
            return ExecuteNonQuery(SqlDeleteItem, command =>
            {
                AddParameter(command, "uid", uid);
            }) == 1;
        }


        private Item ReadItem(DbDataReader reader)
        {
            Item item = new Item();
            item.UId = GetString(reader, "uid");
            item.ItemId = GetUInt32(reader, "item_id");
            item.ItemNum = GetUInt32(reader, "item_num");
            item.Unk3 = GetByte(reader, "unk3");
            item.Color = GetByte(reader, "color");
            item.PlusValue = GetByte(reader, "plus_value");
            return item;
        }

        private void AddParameter(TCom command, Item item)
        {
            AddParameter(command, "uid", item.UId);
            AddParameter(command, "item_id", item.ItemId);
            AddParameter(command, "item_num", item.ItemNum);
            AddParameter(command, "unk3", item.Unk3);
            AddParameter(command, "color", item.Color);
            AddParameter(command, "plus_value", item.PlusValue);
        }
    }
}