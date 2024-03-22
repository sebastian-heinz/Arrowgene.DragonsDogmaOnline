using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftStartCraftReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_START_CRAFT_REQ;

        public C2SCraftStartCraftReq()
        {
            CraftMaterialList = new List<CDataCraftMaterial>();
            wstrToppingUID = string.Empty;
            Unk1 = new List<CDataCraftMaterial>();
            CraftSupportPawnIDList = new List<CDataCraftSupportPawnID>();
        }

        public uint RecipeID { get; set; }
        public List<CDataCraftMaterial> CraftMaterialList { get; set; }
        public string wstrToppingUID { get; set; }
        public ushort Unk0 { get; set; }
        public List<CDataCraftMaterial> Unk1 { get; set; } // Probably the refining material?
        public uint CraftMainPawnID { get; set; }
        public List<CDataCraftSupportPawnID> CraftSupportPawnIDList { get; set; }
        public List<CDataCommonU32> Unk3 { get; set; }
        public uint CreateCount { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftStartCraftReq>
        {
            public override void Write(IBuffer buffer, C2SCraftStartCraftReq obj)
            {
                WriteUInt32(buffer, obj.RecipeID);
                WriteEntityList<CDataCraftMaterial>(buffer, obj.CraftMaterialList);
                WriteMtString(buffer, obj.wstrToppingUID);
                WriteUInt16(buffer, obj.Unk0);
                WriteEntityList<CDataCraftMaterial>(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.CraftMainPawnID);
                WriteEntityList<CDataCraftSupportPawnID>(buffer, obj.CraftSupportPawnIDList);
                WriteEntityList<CDataCommonU32>(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.CreateCount);
            }

            public override C2SCraftStartCraftReq Read(IBuffer buffer)
            {
                C2SCraftStartCraftReq obj = new C2SCraftStartCraftReq();
                obj.RecipeID = ReadUInt32(buffer);
                obj.CraftMaterialList = ReadEntityList<CDataCraftMaterial>(buffer);
                obj.wstrToppingUID = ReadMtString(buffer);
                obj.Unk0 = ReadUInt16(buffer);
                obj.Unk1 = ReadEntityList<CDataCraftMaterial>(buffer);
                obj.CraftMainPawnID = ReadUInt32(buffer);
                obj.CraftSupportPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                obj.Unk3 = ReadEntityList<CDataCommonU32>(buffer);
                obj.CreateCount = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}