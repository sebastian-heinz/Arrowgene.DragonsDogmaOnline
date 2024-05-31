using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        
        private static readonly string BazaarExhibitionTableName = "ddon_bazaar_exhibition";
        
        private static readonly string[] BazaarExhibitionFields = new string[]
        { 
           /* bazaar_id */ "character_id", "sequence", "item_id", "num", "price", "exhibition_time", "state", "proceeds", "expire"
        };

        private static readonly string SqlInsertBazaarExhibition = $"INSERT INTO \"{BazaarExhibitionTableName}\" ({BuildQueryField(BazaarExhibitionFields)}) VALUES ({BuildQueryInsert(BazaarExhibitionFields)});";
        private static readonly string SqlSelectBazaarExhibitionByBazaarId = $"SELECT \"bazaar_id\", {BuildQueryField(BazaarExhibitionFields)} FROM \"{BazaarExhibitionTableName}\" WHERE \"bazaar_id\"=@bazaar_id;";
        private static readonly string SqlSelectBazaarExhibitionsByCharacterId = $"SELECT \"bazaar_id\", {BuildQueryField(BazaarExhibitionFields)} FROM \"{BazaarExhibitionTableName}\" WHERE \"character_id\"=@character_id;";
        
        private static readonly string SqlSelectActiveBazaarExhibitionsByItemIdExcludingOwn = $"SELECT \"bazaar_id\", {BuildQueryField(BazaarExhibitionFields)} FROM \"{BazaarExhibitionTableName}\" " +
                                                               $"WHERE \"item_id\" = @item_id " +
                                                               $"AND \"state\" = {(byte)BazaarExhibitionState.OnSale} " +
                                                               $"AND \"expire\" > DATETIME(\"now\") " +
                                                               $"AND \"character_id\" <> @excluded_character_id;";
        
        private static readonly string SqlDeleteBazaarExhibitionByBazaarId = $"DELETE FROM \"{BazaarExhibitionTableName}\" WHERE \"bazaar_id\"=@bazaar_id;";
        private static readonly string SqlDeleteBazaarExhibitionOutdated = $"DELETE FROM \"{BazaarExhibitionTableName}\" WHERE \"state\"={(byte)BazaarExhibitionState.Idle} AND \"expire\" < DATETIME(\"now\");";


        private static readonly string SqlUpdateBazaarExhibitionByBazaarId = $"UPDATE \"{BazaarExhibitionTableName}\" SET {BuildQueryUpdate(BazaarExhibitionFields)} WHERE \"bazaar_id\"=@bazaar_id";


        public ulong InsertBazaarExhibition(BazaarExhibition exhibition)
        {
            using TCon conn = OpenNewConnection();
            return InsertBazaarExhibition(conn, exhibition);
        }
        
        public ulong InsertBazaarExhibition(TCon conn, BazaarExhibition exhibition)
        {
            int rowsAffected = ExecuteNonQuery(conn, SqlInsertBazaarExhibition, command =>
            {
                AddParameter(command, exhibition);
            }, out long autoIncrement);

            if (rowsAffected > NoRowsAffected)
            {
                return (ulong)autoIncrement;
            }

            return 0;
        }
        
        public int UpdateBazaarExhibiton(BazaarExhibition exhibition)
        {
            using TCon conn = OpenNewConnection();
            return UpdateBazaarExhibiton(conn, exhibition);
        }
        
        public int UpdateBazaarExhibiton(TCon conn, BazaarExhibition exhibition)
        {
            int rowsAffected = ExecuteNonQuery(conn, SqlUpdateBazaarExhibitionByBazaarId, command =>
            {
                AddParameter(command, exhibition);
            });

            return rowsAffected;
        }
        
        public int DeleteBazaarExhibition(ulong bazaarId)
        {
            using TCon conn = OpenNewConnection();
            return DeleteBazaarExhibition(conn, bazaarId);
        }
        
        public int DeleteBazaarExhibition(TCon conn, ulong bazaarId)
        {
            int rowsAffected = ExecuteNonQuery(conn, SqlDeleteBazaarExhibitionByBazaarId, command =>
            {
                AddParameter(command, "@bazaar_id", bazaarId);
            });
            return rowsAffected;
        }
        
        public int DeleteBazaarExhibitionsOutdated()
        {
            using TCon conn = OpenNewConnection();
            return DeleteBazaarExhibitionsOutdated(conn);
        }
        
        public int DeleteBazaarExhibitionsOutdated(TCon conn)
        {
            int rowsAffected = ExecuteNonQuery(conn, SqlDeleteBazaarExhibitionOutdated, command => {});
            return rowsAffected;
        }
        
        public BazaarExhibition SelectBazaarExhibitionByBazaarId(ulong bazaarId)
        {
            using TCon conn = OpenNewConnection();
            return SelectBazaarExhibitionByBazaarId(conn, bazaarId);
        }
        
        public BazaarExhibition SelectBazaarExhibitionByBazaarId(TCon conn, ulong bazaarId)
        {
            BazaarExhibition entity = null;
            ExecuteReader(conn, SqlSelectBazaarExhibitionByBazaarId,
                command =>
                {
                    AddParameter(command, "@bazaar_id", bazaarId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        entity = ReadBazaarExhibition(reader);
                    }
                });

            return entity;
        }

        public List<BazaarExhibition> FetchCharacterBazaarExhibitions(uint characterId)
        {
            using TCon conn = OpenNewConnection();
            DeleteBazaarExhibitionsOutdated(conn);
            return SelectBazaarExhibitionsByCharacterId(conn, characterId);
        }

        public List<BazaarExhibition> SelectBazaarExhibitionsByCharacterId(uint characterId)
        {
            using TCon conn = OpenNewConnection();
            return SelectBazaarExhibitionsByCharacterId(conn, characterId);
        }
        
        public List<BazaarExhibition> SelectBazaarExhibitionsByCharacterId(TCon conn, uint characterId)
        {
            List<BazaarExhibition> entities = new List<BazaarExhibition>();
            ExecuteReader(conn, SqlSelectBazaarExhibitionsByCharacterId,
                command => { AddParameter(command, "@character_id", characterId); }, reader =>
                {
                    while (reader.Read())
                    {
                        BazaarExhibition e = ReadBazaarExhibition(reader);
                        entities.Add(e);
                    }
                });

            return entities;
        }
        
        
        public List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdExcludingOwn(uint itemId, uint excludedCharacterId)
        {
            using TCon conn = OpenNewConnection();
            return SelectActiveBazaarExhibitionsByItemIdExcludingOwn(conn, itemId, excludedCharacterId);
        }
        
        public List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdExcludingOwn(TCon conn, uint itemId, uint excludedCharacterId)
        {
            List<BazaarExhibition> entities = new List<BazaarExhibition>();
            ExecuteReader(conn, SqlSelectActiveBazaarExhibitionsByItemIdExcludingOwn,
                command =>
                {
                    AddParameter(command, "@item_id", itemId);
                    AddParameter(command, "@excluded_character_id", excludedCharacterId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        BazaarExhibition e = ReadBazaarExhibition(reader);
                        entities.Add(e);
                    }
                });

            return entities;
        }

        public List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdsExcludingOwn(List<uint> itemIds, uint excludedCharacterId)
        {
            using TCon conn = OpenNewConnection();
            return SelectActiveBazaarExhibitionsByItemIdsExcludingOwn(conn, itemIds, excludedCharacterId);
        }

        public List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdsExcludingOwn(TCon conn, List<uint> itemIds, uint excludedCharacterId)
        {
            List<BazaarExhibition> entities = new List<BazaarExhibition>();
            foreach (uint itemId in itemIds)
            {
                List<BazaarExhibition> exhibitionsForItemId = SelectActiveBazaarExhibitionsByItemIdExcludingOwn(conn, itemId, excludedCharacterId);
                entities.AddRange(exhibitionsForItemId);
            }
            return entities;
        }

        private BazaarExhibition ReadBazaarExhibition(TReader reader)
        {
            BazaarExhibition exhibition = new BazaarExhibition();
            exhibition.CharacterId = GetUInt32(reader, "character_id");
            exhibition.Info.State = (BazaarExhibitionState) GetByte(reader, "state");
            exhibition.Info.Proceeds = GetUInt32(reader, "proceeds");
            exhibition.Info.Expire = GetDateTime(reader, "expire");
            exhibition.Info.ItemInfo.BazaarId = GetUInt64(reader, "bazaar_id");
            exhibition.Info.ItemInfo.Sequence = GetUInt16(reader, "sequence");
            exhibition.Info.ItemInfo.ExhibitionTime = GetDateTime(reader, "exhibition_time");
            exhibition.Info.ItemInfo.ItemBaseInfo.ItemId = GetUInt32(reader, "item_id");
            exhibition.Info.ItemInfo.ItemBaseInfo.Num = GetUInt16(reader, "num");
            exhibition.Info.ItemInfo.ItemBaseInfo.Price = GetUInt32(reader, "price");
            return exhibition;
        }

        private void AddParameter(TCom command, BazaarExhibition exhibition)
        {
            AddParameter(command, "character_id", exhibition.CharacterId);
            AddParameter(command, "state", (byte) exhibition.Info.State);
            AddParameter(command, "proceeds", exhibition.Info.Proceeds);
            AddParameter(command, "expire", exhibition.Info.Expire.UtcDateTime);
            AddParameter(command, "bazaar_id", exhibition.Info.ItemInfo.BazaarId);
            AddParameter(command, "sequence", exhibition.Info.ItemInfo.Sequence);
            AddParameter(command, "exhibition_time", exhibition.Info.ItemInfo.ExhibitionTime.UtcDateTime);
            AddParameter(command, "item_id", exhibition.Info.ItemInfo.ItemBaseInfo.ItemId);
            AddParameter(command, "num", exhibition.Info.ItemInfo.ItemBaseInfo.Num);
            AddParameter(command, "price", exhibition.Info.ItemInfo.ItemBaseInfo.Price);
        }
    }
}
