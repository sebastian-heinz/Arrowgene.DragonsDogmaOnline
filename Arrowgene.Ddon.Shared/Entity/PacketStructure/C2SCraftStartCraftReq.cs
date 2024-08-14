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
        // Directly correlated with CraftProgress Unk0
        public ushort Unk0 { get; set; } // Looks like this might be an ID for the Additional Status?
        public List<CDataCraftMaterial> AdditionalStatusItems { get; set; } // List of Additional Status items I guess? can there be more than 1 though? weird.
        public uint CraftMainPawnID { get; set; }
        public List<CDataCraftSupportPawnID> CraftSupportPawnIDList { get; set; }
        // Directly correlated with CraftProgress CraftMasterPawnInfoList
        // Contains list of craft master / legend pawn IDs
        // TODO: support craft master / legend pawn involvement
        public List<CDataCraftSupportPawnID> CraftMasterPawnIDList { get; set; } 
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
                WriteEntityList<CDataCraftSupportPawnID>(buffer, obj.CraftMasterPawnIDList);
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
                obj.CraftMasterPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                obj.CreateCount = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
