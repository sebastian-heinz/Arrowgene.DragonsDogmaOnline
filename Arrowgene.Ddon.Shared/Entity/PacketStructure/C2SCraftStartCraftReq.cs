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
            RefineMaterialUID = string.Empty;
            AdditionalStatusItems = new List<CDataCraftMaterial>();
            CraftSupportPawnIDList = new List<CDataCraftSupportPawnID>();
        }

        public uint RecipeID { get; set; }
        public List<CDataCraftMaterial> CraftMaterialList { get; set; }
        public string RefineMaterialUID { get; set; } // Refine material UID
        public ushort Unk0 { get; set; } // Looks like this might be an ID for the Additional Status?
        public List<CDataCraftMaterial> AdditionalStatusItems { get; set; } // List of Additional Status items I guess? can there be more than 1 though? weird.
        public uint CraftMainPawnID { get; set; }
        public List<CDataCraftSupportPawnID> CraftSupportPawnIDList { get; set; }
        public List<CDataCommonU32> Unk3 { get; set; } // Was empty despite filling all the crafting menus? truly unknown I suppose.
        public uint CreateCount { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftStartCraftReq>
        {
            public override void Write(IBuffer buffer, C2SCraftStartCraftReq obj)
            {
                WriteUInt32(buffer, obj.RecipeID);
                WriteEntityList<CDataCraftMaterial>(buffer, obj.CraftMaterialList);
                WriteMtString(buffer, obj.RefineMaterialUID);
                WriteUInt16(buffer, obj.Unk0);
                WriteEntityList<CDataCraftMaterial>(buffer, obj.AdditionalStatusItems);
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
                obj.RefineMaterialUID = ReadMtString(buffer);
                obj.Unk0 = ReadUInt16(buffer);
                obj.AdditionalStatusItems = ReadEntityList<CDataCraftMaterial>(buffer);
                obj.CraftMainPawnID = ReadUInt32(buffer);
                obj.CraftSupportPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                obj.Unk3 = ReadEntityList<CDataCommonU32>(buffer);
                obj.CreateCount = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}