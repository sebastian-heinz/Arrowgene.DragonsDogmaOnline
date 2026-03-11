using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public partial class DdonSqlDb : SqlDb
    {
        private static readonly string[] DispelSealFields =
        [
            "character_id", "seal_index"
        ];

        private readonly string SqlInsertDispelSeal =
            $"INSERT INTO \"ddon_dispel_seals\" ({BuildQueryField(DispelSealFields)}) VALUES ({BuildQueryInsert(DispelSealFields)});";

        private readonly string SqlDeleteDispelSeal =
            "DELETE FROM \"ddon_dispel_seals\" WHERE \"character_id\"=@character_id AND \"seal_index\"=@seal_index;";

        private readonly string SqlSelectDispelSeal =
            $"SELECT {BuildQueryField(DispelSealFields)} FROM \"ddon_dispel_seals\" WHERE \"character_id\" = @character_id;";

        public override HashSet<uint> SelectDispelSeals(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                HashSet<uint> seals = new();
                ExecuteReader(connection,
                    SqlSelectDispelSeal,
                    command => { AddParameter(command, "@character_id", characterId); },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            uint sealIndex = GetUInt32(reader, "seal_index");

                            seals.Add(sealIndex);
                        }
                    });
                return seals;
            });
        }

        public override bool InsertDispelSeal(uint characterId, uint sealIndex, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(
                    connection,
                    SqlInsertDispelSeal,
                    command =>
                    {
                        AddParameter(command, "character_id", characterId);
                        AddParameter(command, "seal_index", sealIndex);
                    }
                ) == 1;
            });
        }

        public override bool DeleteDispelSeal(uint characterId, uint sealIndex, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlDeleteDispelSeal, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "seal_index", sealIndex);
                }) == 1;
            });
        }
    }
}
