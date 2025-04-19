using System;
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private static readonly string BazaarExhibitionTableName = "ddon_bazaar_exhibition";

    private static readonly string[] BazaarExhibitionFields =
    [
        /* bazaar_id */ "character_id", "sequence", "item_id", "num", "price", "exhibition_time", "state", "proceeds", "expire"
    ];

    private static readonly string SqlInsertBazaarExhibition =
        $"INSERT INTO \"{BazaarExhibitionTableName}\" ({BuildQueryField(BazaarExhibitionFields)}) VALUES ({BuildQueryInsert(BazaarExhibitionFields)});";

    private static readonly string SqlSelectBazaarExhibitionByBazaarId =
        $"SELECT \"bazaar_id\", {BuildQueryField(BazaarExhibitionFields)} FROM \"{BazaarExhibitionTableName}\" WHERE \"bazaar_id\"=@bazaar_id;";

    private static readonly string SqlSelectBazaarExhibitionsByCharacterId =
        $"SELECT \"bazaar_id\", {BuildQueryField(BazaarExhibitionFields)} FROM \"{BazaarExhibitionTableName}\" WHERE \"character_id\"=@character_id;";

    private static readonly string SqlSelectActiveBazaarExhibitionsByItemIdExcludingOwn =
        $"SELECT \"bazaar_id\", {BuildQueryField(BazaarExhibitionFields)} FROM \"{BazaarExhibitionTableName}\" " +
        $"WHERE \"item_id\" = @item_id " +
        $"AND \"state\" = {(byte)BazaarExhibitionState.OnSale} " +
        $"AND \"expire\" > @now " +
        $"AND \"character_id\" <> @excluded_character_id " +
        $"ORDER BY price ASC;";

    private static readonly string SqlDeleteBazaarExhibitionByBazaarId = $"DELETE FROM \"{BazaarExhibitionTableName}\" WHERE \"bazaar_id\"=@bazaar_id;";

    private static readonly string SqlDeleteBazaarExhibitionOutdated =
        $"DELETE FROM \"{BazaarExhibitionTableName}\" WHERE \"state\"={(byte)BazaarExhibitionState.Idle} AND \"expire\" < @now;";


    private static readonly string SqlUpdateBazaarExhibitionByBazaarId =
        $"UPDATE \"{BazaarExhibitionTableName}\" SET {BuildQueryUpdate(BazaarExhibitionFields)} WHERE \"bazaar_id\"=@bazaar_id";


    public override ulong InsertBazaarExhibition(BazaarExhibition exhibition)
    {
        using DbConnection conn = OpenNewConnection();
        return InsertBazaarExhibition(conn, exhibition);
    }

    public ulong InsertBazaarExhibition(DbConnection conn, BazaarExhibition exhibition)
    {
        int rowsAffected = ExecuteNonQuery(conn, SqlInsertBazaarExhibition, command => { AddParameter(command, exhibition); }, out long autoIncrement);

        if (rowsAffected > NoRowsAffected) return (ulong)autoIncrement;

        return 0;
    }

    public override int UpdateBazaarExhibiton(BazaarExhibition exhibition)
    {
        using DbConnection conn = OpenNewConnection();
        return UpdateBazaarExhibiton(conn, exhibition);
    }

    public int UpdateBazaarExhibiton(DbConnection conn, BazaarExhibition exhibition)
    {
        int rowsAffected = ExecuteNonQuery(conn, SqlUpdateBazaarExhibitionByBazaarId, command => { AddParameter(command, exhibition); });

        return rowsAffected;
    }

    public override int DeleteBazaarExhibition(ulong bazaarId)
    {
        using DbConnection conn = OpenNewConnection();
        return DeleteBazaarExhibition(conn, bazaarId);
    }

    public int DeleteBazaarExhibition(DbConnection conn, ulong bazaarId)
    {
        int rowsAffected = ExecuteNonQuery(conn, SqlDeleteBazaarExhibitionByBazaarId, command => { AddParameter(command, "@bazaar_id", bazaarId); });
        return rowsAffected;
    }

    public int DeleteBazaarExhibitionsOutdated()
    {
        using DbConnection conn = OpenNewConnection();
        return DeleteBazaarExhibitionsOutdated(conn);
    }

    public int DeleteBazaarExhibitionsOutdated(DbConnection conn)
    {
        int rowsAffected = ExecuteNonQuery(conn, SqlDeleteBazaarExhibitionOutdated, command => { AddParameter(command, "@now", DateTimeOffset.UtcNow.UtcDateTime); });
        return rowsAffected;
    }

    public override BazaarExhibition SelectBazaarExhibitionByBazaarId(ulong bazaarId)
    {
        using DbConnection conn = OpenNewConnection();
        return SelectBazaarExhibitionByBazaarId(conn, bazaarId);
    }

    public BazaarExhibition SelectBazaarExhibitionByBazaarId(DbConnection conn, ulong bazaarId)
    {
        BazaarExhibition entity = null;
        ExecuteReader(conn, SqlSelectBazaarExhibitionByBazaarId,
            command => { AddParameter(command, "@bazaar_id", bazaarId); }, reader =>
            {
                if (reader.Read()) entity = ReadBazaarExhibition(reader);
            });

        return entity;
    }

    public override List<BazaarExhibition> FetchCharacterBazaarExhibitions(uint characterId)
    {
        using DbConnection conn = OpenNewConnection();
        DeleteBazaarExhibitionsOutdated(conn);
        return SelectBazaarExhibitionsByCharacterId(conn, characterId);
    }

    public List<BazaarExhibition> SelectBazaarExhibitionsByCharacterId(uint characterId)
    {
        using DbConnection conn = OpenNewConnection();
        return SelectBazaarExhibitionsByCharacterId(conn, characterId);
    }

    public List<BazaarExhibition> SelectBazaarExhibitionsByCharacterId(DbConnection conn, uint characterId)
    {
        List<BazaarExhibition> entities = new();
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

    public override List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdExcludingOwn(uint itemId, uint excludedCharacterId, DbConnection? connectionIn = null)
    {
        List<BazaarExhibition> entities = new();
        ExecuteQuerySafe(connectionIn, conn =>
        {
            ExecuteReader(conn, SqlSelectActiveBazaarExhibitionsByItemIdExcludingOwn,
                command =>
                {
                    AddParameter(command, "@item_id", itemId);
                    AddParameter(command, "@excluded_character_id", excludedCharacterId);
                    AddParameter(command, "@now", DateTimeOffset.UtcNow.UtcDateTime);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        BazaarExhibition e = ReadBazaarExhibition(reader);
                        entities.Add(e);
                    }
                });
        });

        return entities;
    }

    public override List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdsExcludingOwn(List<uint> itemIds, uint excludedCharacterId, DbConnection? connectionIn = null)
    {
        List<BazaarExhibition> entities = new();
        ExecuteQuerySafe(connectionIn, conn =>
        {
            foreach (uint itemId in itemIds)
            {
                List<BazaarExhibition> exhibitionsForItemId = SelectActiveBazaarExhibitionsByItemIdExcludingOwn(itemId, excludedCharacterId, conn);
                entities.AddRange(exhibitionsForItemId);
            }
        });
        return entities;
    }

    private BazaarExhibition ReadBazaarExhibition(DbDataReader reader)
    {
        BazaarExhibition exhibition = new();
        exhibition.CharacterId = GetUInt32(reader, "character_id");
        exhibition.Info.State = (BazaarExhibitionState)GetByte(reader, "state");
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

    private void AddParameter(DbCommand command, BazaarExhibition exhibition)
    {
        AddParameter(command, "character_id", exhibition.CharacterId);
        AddParameter(command, "state", (byte)exhibition.Info.State);
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
