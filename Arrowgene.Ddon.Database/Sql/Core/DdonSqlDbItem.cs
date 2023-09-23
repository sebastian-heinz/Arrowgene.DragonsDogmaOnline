using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] ItemFields = new string[]
        {
            "uid", "item_id", "unk3", "color", "plus_value"
        };

        // Items don't get updated or deleted once created as the same row is shared among all players.
        // Making a distinction wouldn't make sense, as upgrading/changin crests would generate a new item with a different UID
        protected virtual string SqlInsertOrIgnoreItem { get; } =
            $"INSERT INTO \"ddon_item\" ({BuildQueryField(ItemFields)}) SELECT {BuildQueryInsert(ItemFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_item\" WHERE \"uid\"=@uid);";

        private static readonly string SqlSelectItem = $"SELECT {BuildQueryField(ItemFields)} FROM \"ddon_item\" WHERE \"uid\"=@uid;";

        public bool InsertItem(TCon conn, Item item)
        {
            return ExecuteNonQuery(conn, SqlInsertOrIgnoreItem, command =>
            {
                AddParameter(command, item);
            }) == 1;
        }

        public bool InsertItem(Item item)
        {
            using TCon connection = OpenNewConnection();
            return InsertItem(connection, item);
        }

        public Item SelectItem(string uid)
        {
            Item item = null;
            ExecuteReader(SqlSelectItem, 
            command => 
            {
                AddParameter(command, "uid", uid);
            },
            reader => 
            {
                if(reader.Read())
                {
                    item = ReadItem(reader);
                }
            });
            return item;
        }

        private Item ReadItem(TReader reader)
        {
            Item item = new Item();
            item.UId = GetString(reader, "uid");
            item.ItemId = GetUInt32(reader, "item_id");
            item.Unk3 = GetByte(reader, "unk3");
            item.Color = GetByte(reader, "color");
            item.PlusValue = GetByte(reader, "plus_value");
            return item;
        }

        private void AddParameter(TCom command, Item item)
        {
            AddParameter(command, "uid", item.UId);
            AddParameter(command, "item_id", item.ItemId);
            AddParameter(command, "unk3", item.Unk3);
            AddParameter(command, "color", item.Color);
            AddParameter(command, "plus_value", item.PlusValue);
        }
    }
}
