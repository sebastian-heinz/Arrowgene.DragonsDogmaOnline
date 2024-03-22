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
            Unk0 = new List<CDataS2CCraftGetCraftSettingResUnk0>();
            Unk3 = new List<CDataS2CCraftGetCraftSettingResUnk3>();
        }

        public List<CDataCommonU32> ColorRegulateItemList { get; set; }
        public List<CDataCraftTimeSaveCost> TimeSaveCostList { get; set; }
        public uint ReasonableCraftLv { get; set; }
        public uint CraftItemLv { get; set; }
        public byte CreateCountMax { get; set; }
        public List<CDataS2CCraftGetCraftSettingResUnk0> Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public List<CDataS2CCraftGetCraftSettingResUnk3> Unk3 { get; set; }

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
                WriteEntityList<CDataS2CCraftGetCraftSettingResUnk0>(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteEntityList<CDataS2CCraftGetCraftSettingResUnk3>(buffer, obj.Unk3);
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
                obj.Unk0 = ReadEntityList<CDataS2CCraftGetCraftSettingResUnk0>(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadEntityList<CDataS2CCraftGetCraftSettingResUnk3>(buffer);
                return obj;
            }
        }
    }
}