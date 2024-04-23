using System;
using System.Collections.Generic;
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
        protected static readonly string[] DragonForceAugmentationFields = new string[]
        {
            "character_common_id", "element_id", "page_no", "group_no", "index_no"
        };

        private readonly string SqlInsertDragonForceAugment = $"INSERT INTO \"ddon_dragon_force_augmentation\" ({BuildQueryField(DragonForceAugmentationFields)}) VALUES ({BuildQueryInsert(DragonForceAugmentationFields)});";
        private readonly string SqlInsertIfNotExistsDragonForceAugment = $"INSERT INTO \"ddon_dragon_force_augmentation\" ({BuildQueryField(DragonForceAugmentationFields)}) SELECT " +
                                                                         $"{BuildQueryInsert(DragonForceAugmentationFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_dragon_force_augmentation\" WHERE " +
                                                                         $"\"character_common_id\" = @character_common_id AND \"element_id\" = @element_id);";
        private readonly string SqlSelectAllDragonForceAugment = $"SELECT {BuildQueryField(DragonForceAugmentationFields)} FROM \"ddon_dragon_force_augmentation\" WHERE \"character_common_id\" = @character_common_id;";

        public bool InsertIfNotExistsDragonForceAugmentation(uint commonId, uint elementId, uint pageNo, uint groupNo, uint indexNo)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsDragonForceAugmentation(connection, commonId, elementId, pageNo, groupNo, indexNo);
        }

        public bool InsertIfNotExistsDragonForceAugmentation(TCon conn, uint commonId, uint elementId, uint pageNo, uint groupNo, uint indexNo)
        {
            return ExecuteNonQuery(conn, SqlInsertIfNotExistsDragonForceAugment, command =>
            {
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "element_id", elementId);
                AddParameter(command, "page_no", pageNo);
                AddParameter(command, "group_no", groupNo);
                AddParameter(command, "index_no", indexNo);
            }) == 1;
        }

        public List<CDataReleaseOrbElement> SelectOrbReleaseElementFromDragonForceAugmentation(uint commonId)
        {
            using TCon connection = OpenNewConnection();
            return SelectOrbReleaseElementFromDragonForceAugmentation(connection, commonId);
        }

        public List<CDataReleaseOrbElement> SelectOrbReleaseElementFromDragonForceAugmentation(TCon conn, uint commonId)
        {
            List<CDataReleaseOrbElement> Results = new List<CDataReleaseOrbElement>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectAllDragonForceAugment,
                    command => {
                        AddParameter(command, "@character_common_id", commonId);
                    }, reader => {
                        while (reader.Read())
                        {
                            CDataReleaseOrbElement ReleaseOrbElement = ReadReleaseOrbElement(reader);
                            Results.Add(ReleaseOrbElement);
                        }
                    });
            });

            return Results;
        }

        private CDataReleaseOrbElement ReadReleaseOrbElement(TReader reader)
        {
            CDataReleaseOrbElement obj = new CDataReleaseOrbElement();
            obj.ElementId = GetUInt32(reader, "element_id");
            obj.PageNo = GetByte(reader, "page_no");
            obj.GroupNo = GetByte(reader, "group_no");
            obj.Index = GetByte(reader, "index_no");
            return obj;
        }

        private void AddParameter(TCom command, uint commonId, CDataReleaseOrbElement obj)
        {
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "element_id", obj.ElementId);
            AddParameter(command, "page_no", obj.PageNo);
            AddParameter(command, "group_no", obj.GroupNo);
            AddParameter(command, "index_no", obj.Index);
        }
    }
}
