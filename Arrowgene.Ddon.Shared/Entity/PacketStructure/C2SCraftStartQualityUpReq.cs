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
            ItemUID = string.Empty;
            RefineUID = string.Empty;
            CraftMaterialList = new List<CDataCraftMaterial>();
            CraftSupportPawnIDList = new List<CDataCraftSupportPawnID>();
            CraftMasterLegendPawnIDList = new List<CDataCraftSupportPawnID>();
        }

        public string ItemUID { get; set; }
        public string RefineUID { get; set; } 
        public ushort AddStatusID { get; set; }
        public List<CDataCraftMaterial> CraftMaterialList { get; set; }
        public uint CraftMainPawnID { get; set; }
        public List<CDataCraftSupportPawnID> CraftSupportPawnIDList { get; set; }
        public List<CDataCraftSupportPawnID> CraftMasterLegendPawnIDList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftStartQualityUpReq>
        {
            public override void Write(IBuffer buffer, C2SCraftStartQualityUpReq obj)
            {
                WriteMtString(buffer, obj.ItemUID);
                WriteMtString(buffer, obj.RefineUID);
                WriteUInt16(buffer, obj.AddStatusID);
                WriteEntityList<CDataCraftMaterial>(buffer, obj.CraftMaterialList);
                WriteUInt32(buffer, obj.CraftMainPawnID);
                WriteEntityList<CDataCraftSupportPawnID>(buffer, obj.CraftSupportPawnIDList);
                WriteEntityList<CDataCraftSupportPawnID>(buffer, obj.CraftMasterLegendPawnIDList);
            }

            public override C2SCraftStartQualityUpReq Read(IBuffer buffer)
            {
                C2SCraftStartQualityUpReq obj = new C2SCraftStartQualityUpReq();
                obj.ItemUID = ReadMtString(buffer);
                obj.RefineUID = ReadMtString(buffer);
                obj.AddStatusID = ReadUInt16(buffer);
                obj.CraftMaterialList = ReadEntityList<CDataCraftMaterial>(buffer);
                obj.CraftMainPawnID = ReadUInt32(buffer);
                obj.CraftSupportPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                obj.CraftMasterLegendPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                return obj;
            }
        }

    }
}
