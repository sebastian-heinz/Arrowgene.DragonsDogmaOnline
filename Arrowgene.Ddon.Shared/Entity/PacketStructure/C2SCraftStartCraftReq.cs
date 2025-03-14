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
            AdditionalStatusMaterialList = new List<CDataCraftMaterial>();
            CraftSupportPawnIDList = new List<CDataCraftSupportPawnID>();
            CraftMasterLegendPawnIDList = new();
        }

        public uint RecipeID { get; set; }
        public List<CDataCraftMaterial> CraftMaterialList { get; set; }
        public string RefineMaterialUID { get; set; } // Refine material UID
        /// <summary>
        /// ID for the Additional Status
        /// 3/0x3 == Blow Power +15
        /// 20/0x14 == Ogre Slaying +15
        /// 18/0x12 == Demihuman Slaying +10
        /// </summary>
        public ushort AdditionalStatusId { get; set; }
        /// <summary>
        /// List of Additional Status material items, but there can't be more than 1
        /// </summary>
        public List<CDataCraftMaterial> AdditionalStatusMaterialList { get; set; }
        public uint CraftMainPawnID { get; set; }
        public List<CDataCraftSupportPawnID> CraftSupportPawnIDList { get; set; }
        /// <summary>
        /// Directly correlated with CraftProgress CraftMasterLegendPawnInfoList
        /// Contains list of craft master / legend pawn IDs
        /// TODO: support craft master / legend pawn involvement
        /// </summary>
        public List<CDataCraftSupportPawnID> CraftMasterLegendPawnIDList { get; set; } 
        public uint CreateCount { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftStartCraftReq>
        {
            public override void Write(IBuffer buffer, C2SCraftStartCraftReq obj)
            {
                WriteUInt32(buffer, obj.RecipeID);
                WriteEntityList<CDataCraftMaterial>(buffer, obj.CraftMaterialList);
                WriteMtString(buffer, obj.RefineMaterialUID);
                WriteUInt16(buffer, obj.AdditionalStatusId);
                WriteEntityList<CDataCraftMaterial>(buffer, obj.AdditionalStatusMaterialList);
                WriteUInt32(buffer, obj.CraftMainPawnID);
                WriteEntityList<CDataCraftSupportPawnID>(buffer, obj.CraftSupportPawnIDList);
                WriteEntityList<CDataCraftSupportPawnID>(buffer, obj.CraftMasterLegendPawnIDList);
                WriteUInt32(buffer, obj.CreateCount);
            }

            public override C2SCraftStartCraftReq Read(IBuffer buffer)
            {
                C2SCraftStartCraftReq obj = new C2SCraftStartCraftReq();
                obj.RecipeID = ReadUInt32(buffer);
                obj.CraftMaterialList = ReadEntityList<CDataCraftMaterial>(buffer);
                obj.RefineMaterialUID = ReadMtString(buffer);
                obj.AdditionalStatusId = ReadUInt16(buffer);
                obj.AdditionalStatusMaterialList = ReadEntityList<CDataCraftMaterial>(buffer);
                obj.CraftMainPawnID = ReadUInt32(buffer);
                obj.CraftSupportPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                obj.CraftMasterLegendPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                obj.CreateCount = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
