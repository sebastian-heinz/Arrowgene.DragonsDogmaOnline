using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftGetCraftProgressListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_GET_CRAFT_PROGRESS_LIST_RES;

        public S2CCraftGetCraftProgressListRes()
        {
            CraftProgressList = new List<CDataCraftProgress>();
            CraftMyPawnList = new List<CDataCraftPawnList>();
            CreatedRecipeList = new List<CDataCommonU32>();
        }

        public List<CDataCraftProgress> CraftProgressList { get; set; }
        public List<CDataCraftPawnList> CraftMyPawnList { get; set; }
        public List<CDataCommonU32> CreatedRecipeList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftGetCraftProgressListRes>
        {
            public override void Write(IBuffer buffer, S2CCraftGetCraftProgressListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCraftProgress>(buffer, obj.CraftProgressList);
                WriteEntityList<CDataCraftPawnList>(buffer, obj.CraftMyPawnList);
                WriteEntityList<CDataCommonU32>(buffer, obj.CreatedRecipeList);
            }

            public override S2CCraftGetCraftProgressListRes Read(IBuffer buffer)
            {
                S2CCraftGetCraftProgressListRes obj = new S2CCraftGetCraftProgressListRes();
                ReadServerResponse(buffer, obj);
                obj.CraftProgressList = ReadEntityList<CDataCraftProgress>(buffer);
                obj.CraftMyPawnList = ReadEntityList<CDataCraftPawnList>(buffer);
                obj.CreatedRecipeList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}