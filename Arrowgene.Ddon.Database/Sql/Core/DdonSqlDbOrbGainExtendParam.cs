using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] OrbGainExtendParamFields = new string[]
        {
            "character_common_id", "hp_max", "stamina_max", "physical_attack", "physical_defence", "magic_attack", "magic_defence",
            "ability_cost", "jewelry_slot", "use_item_slot", "material_item_slot", "equip_item_slot", "main_pawn_slot", "support_pawn_slot"
        };

        private readonly string SqlInsertOrbGainExtendParam = $"INSERT INTO \"ddon_orb_gain_extend_param\" ({BuildQueryField(OrbGainExtendParamFields)}) VALUES ({BuildQueryInsert(OrbGainExtendParamFields)});";
        private readonly string SqlInsertIfNotExistsOrbGainExtendParam = $"INSERT INTO \"ddon_orb_gain_extend_param\" ({BuildQueryField(OrbGainExtendParamFields)}) SELECT " +
                                                                         $"{BuildQueryInsert(OrbGainExtendParamFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_orb_gain_extend_param\" WHERE " +
                                                                         $"\"character_common_id\" = @character_common_id);";
        private readonly string SqlSelectOrbGainExtendParam = $"SELECT {BuildQueryField(OrbGainExtendParamFields)} FROM \"ddon_orb_gain_extend_param\" WHERE \"character_common_id\" = @character_common_id;";
        private readonly string SqlUpdateOrbGainExtendParam = $"UPDATE \"ddon_orb_gain_extend_param\" SET {BuildQueryUpdate(OrbGainExtendParamFields)} WHERE \"character_common_id\" = @character_common_id;";

        public bool InsertGainExtendParam(uint commonId, CDataOrbGainExtendParam Param)
        {
            using TCon connection = OpenNewConnection();
            return InsertGainExtendParam(connection, commonId, Param);
        }

        public bool InsertGainExtendParam(TCon conn, uint commonId, CDataOrbGainExtendParam Param)
        {
            return ExecuteNonQuery(conn, SqlInsertIfNotExistsOrbGainExtendParam, command =>
            {
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "hp_max", Param.HpMax);
                AddParameter(command, "stamina_max", Param.StaminaMax);
                AddParameter(command, "physical_attack", Param.Attack);
                AddParameter(command, "physical_defence", Param.Defence);
                AddParameter(command, "magic_attack", Param.MagicAttack);
                AddParameter(command, "magic_defence", Param.MagicDefence);
                AddParameter(command, "ability_cost", Param.AbilityCost);
                AddParameter(command, "jewelry_slot", Param.JewelrySlot);
                AddParameter(command, "use_item_slot", Param.UseItemSlot);
                AddParameter(command, "material_item_slot", Param.MaterialItemSlot);
                AddParameter(command, "equip_item_slot", Param.EquipItemSlot);
                AddParameter(command, "main_pawn_slot", Param.MainPawnSlot);
                AddParameter(command, "support_pawn_slot", Param.SupportPawnSlot);
            }) == 1;
        }

        public bool UpdateOrbGainExtendParam(uint commonId, CDataOrbGainExtendParam Param)
        {
            using TCon connection = OpenNewConnection();
            return UpdateOrbGainExtendParam(connection, commonId, Param);
        }

        public bool UpdateOrbGainExtendParam(TCon conn, uint commonId, CDataOrbGainExtendParam Param)
        {
            return ExecuteNonQuery(conn, SqlUpdateOrbGainExtendParam, command => { AddParameter(command, commonId, Param); }) == 1;
        }

        public CDataOrbGainExtendParam SelectOrbGainExtendParam(uint commonId, DbConnection? connectionIn = null)
        {
            CDataOrbGainExtendParam results = new CDataOrbGainExtendParam();

            ExecuteQuerySafe(connectionIn, conn =>
            {
                ExecuteReader(conn, SqlSelectOrbGainExtendParam,
                    command => {
                        AddParameter(command, "@character_common_id", commonId);
                    }, reader => {
                        while (reader.Read())
                        {
                            results = ReadOrbGainExtendParam(reader);
                        }
                    });
            });

            return results;
        }

        private void AddParameter(TCom command, uint commonId, CDataOrbGainExtendParam obj)
        {
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "hp_max", obj.HpMax);
            AddParameter(command, "stamina_max", obj.StaminaMax);
            AddParameter(command, "physical_attack", obj.Attack);
            AddParameter(command, "physical_defence", obj.Defence);
            AddParameter(command, "magic_attack", obj.MagicAttack);
            AddParameter(command, "magic_defence", obj.MagicDefence);
            AddParameter(command, "ability_cost", obj.AbilityCost);
            AddParameter(command, "jewelry_slot", obj.JewelrySlot);
            AddParameter(command, "use_item_slot", obj.UseItemSlot);
            AddParameter(command, "material_item_slot", obj.MaterialItemSlot);
            AddParameter(command, "equip_item_slot", obj.EquipItemSlot);
            AddParameter(command, "main_pawn_slot", obj.MainPawnSlot);
            AddParameter(command, "support_pawn_slot", obj.SupportPawnSlot);
        }

        private CDataOrbGainExtendParam ReadOrbGainExtendParam(TReader reader)
        {
            CDataOrbGainExtendParam obj = new CDataOrbGainExtendParam();
            obj.HpMax = GetUInt16(reader, "hp_max");
            obj.StaminaMax = GetUInt16(reader, "stamina_max");
            obj.Attack = GetUInt16(reader, "physical_attack");
            obj.Defence = GetUInt16(reader, "physical_defence");
            obj.MagicAttack = GetUInt16(reader, "magic_attack");
            obj.MagicDefence = GetUInt16(reader, "magic_defence");
            obj.AbilityCost = GetUInt16(reader, "ability_cost");
            obj.JewelrySlot = GetUInt16(reader, "jewelry_slot");
            obj.UseItemSlot = GetUInt16(reader, "use_item_slot");
            obj.MaterialItemSlot = GetUInt16(reader, "material_item_slot");
            obj.EquipItemSlot = GetUInt16(reader, "equip_item_slot");
            obj.MainPawnSlot = GetUInt16(reader, "main_pawn_slot");
            obj.SupportPawnSlot = GetUInt16(reader, "support_pawn_slot");
            return obj;
        }
    }
}
