using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftStartQualityUpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_START_QUALITY_UP_REQ;

        public C2SCraftStartQualityUpReq()
        {
            Unk0 = string.Empty;
            Unk1 = string.Empty;
            CraftMaterialList = new List<CDataCraftMaterial>();
            CraftSupportPawnIDList = new List<CDataCraftSupportPawnID>();
            Unk3 = new List<CDataCommonU32>();
        }

        public string Unk0 { get; set; } // The gear you want to upgrade UID
        public string Unk1 { get; set; } // The Rock you're using to upgrade UID 
        public ushort Unk2 { get; set; } // Additional Status ID?
        public List<CDataCraftMaterial> CraftMaterialList { get; set; }
        public uint CraftMainPawnID { get; set; }
        public List<CDataCraftSupportPawnID> CraftSupportPawnIDList { get; set; }
        public List<CDataCommonU32> Unk3 { get; set; } // Same list seen in GradeUp Request? Potentially crests or Dragonforce?

        public class Serializer : PacketEntitySerializer<C2SCraftStartQualityUpReq>
        {
            public override void Write(IBuffer buffer, C2SCraftStartQualityUpReq obj)
            {
                WriteMtString(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1);
                WriteUInt16(buffer, obj.Unk2);
                WriteEntityList<CDataCraftMaterial>(buffer, obj.CraftMaterialList);
                WriteUInt32(buffer, obj.CraftMainPawnID);
                WriteEntityList<CDataCraftSupportPawnID>(buffer, obj.CraftSupportPawnIDList);
                WriteEntityList<CDataCommonU32>(buffer, obj.Unk3);
            }

            public override C2SCraftStartQualityUpReq Read(IBuffer buffer)
            {
                C2SCraftStartQualityUpReq obj = new C2SCraftStartQualityUpReq();
                obj.Unk0 = ReadMtString(buffer);
                obj.Unk1 = ReadMtString(buffer);
                obj.Unk2 = ReadUInt16(buffer);
                obj.CraftMaterialList = ReadEntityList<CDataCraftMaterial>(buffer);
                obj.CraftMainPawnID = ReadUInt32(buffer);
                obj.CraftSupportPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                obj.Unk3 = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }

    }
}