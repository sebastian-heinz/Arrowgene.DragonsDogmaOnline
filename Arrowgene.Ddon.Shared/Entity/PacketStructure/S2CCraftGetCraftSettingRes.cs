using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftGetCraftSettingRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_GET_CRAFT_SETTING_RES;

        public S2CCraftGetCraftSettingRes()
        {
            ColorRegulateItemList = new List<CDataCommonU32>();
            TimeSaveCostList = new List<CDataCraftTimeSaveCost>();
            CraftMasterLegendPawnInfoList = new List<CDataRegisteredLegendPawnInfo>();
            RefiningMaterialInfoList = new List<CDataRefiningMaterialInfo>();
        }

        /// <summary>
        /// List of items whose color can not be changed/dyed via crafting regardless of stars
        /// </summary>
        public List<CDataCommonU32> ColorRegulateItemList { get; set; }
        public List<CDataCraftTimeSaveCost> TimeSaveCostList { get; set; }
        public uint ReasonableCraftLv { get; set; }
        public uint CraftItemLv { get; set; }
        public byte CreateCountMax { get; set; }
        /// See CPacket_S2C_GET_LEGEND_PAWN_LIST_RES + MtTypedArray<CDataRegisterdPawnList> LegendPawnList;
        public List<CDataRegisteredLegendPawnInfo> CraftMasterLegendPawnInfoList { get; set; }
        public uint Unk1 { get; set; } // Value 49 => maybe craft Filter ID / Preview character ID / settings for Craig?
        public uint Unk2 { get; set; } // Value 30 => maybe craft Filter ID / Preview character ID / settings for Craig?
        public List<CDataRefiningMaterialInfo> RefiningMaterialInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftGetCraftSettingRes>
        {
            public override void Write(IBuffer buffer, S2CCraftGetCraftSettingRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCommonU32>(buffer, obj.ColorRegulateItemList);
                WriteEntityList<CDataCraftTimeSaveCost>(buffer, obj.TimeSaveCostList);
                WriteUInt32(buffer, obj.ReasonableCraftLv);
                WriteUInt32(buffer, obj.CraftItemLv);
                WriteByte(buffer, obj.CreateCountMax);
                WriteEntityList<CDataRegisteredLegendPawnInfo>(buffer, obj.CraftMasterLegendPawnInfoList);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteEntityList<CDataRefiningMaterialInfo>(buffer, obj.RefiningMaterialInfoList);
            }

            public override S2CCraftGetCraftSettingRes Read(IBuffer buffer)
            {
                S2CCraftGetCraftSettingRes obj = new S2CCraftGetCraftSettingRes();
                ReadServerResponse(buffer, obj);
                obj.ColorRegulateItemList = ReadEntityList<CDataCommonU32>(buffer);
                obj.TimeSaveCostList = ReadEntityList<CDataCraftTimeSaveCost>(buffer);
                obj.ReasonableCraftLv = ReadUInt32(buffer);
                obj.CraftItemLv = ReadUInt32(buffer);
                obj.CreateCountMax = ReadByte(buffer);
                obj.CraftMasterLegendPawnInfoList = ReadEntityList<CDataRegisteredLegendPawnInfo>(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.RefiningMaterialInfoList = ReadEntityList<CDataRefiningMaterialInfo>(buffer);
                return obj;
            }
        }
    }
}
