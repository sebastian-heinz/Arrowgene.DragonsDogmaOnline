using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftGetCraftIRCollectionValueListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_GET_CRAFT_IR_COLLECTION_VALUE_LIST_RES;

        public S2CCraftGetCraftIRCollectionValueListRes()
        {
            SkillRateList = new List<CDataCraftSkillRate>();
        }

        public List<CDataCraftSkillRate> SkillRateList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftGetCraftIRCollectionValueListRes>
        {
            public override void Write(IBuffer buffer, S2CCraftGetCraftIRCollectionValueListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCraftSkillRate>(buffer, obj.SkillRateList);
            }

            public override S2CCraftGetCraftIRCollectionValueListRes Read(IBuffer buffer)
            {
                S2CCraftGetCraftIRCollectionValueListRes obj = new S2CCraftGetCraftIRCollectionValueListRes();
                ReadServerResponse(buffer, obj);
                obj.SkillRateList = ReadEntityList<CDataCraftSkillRate>(buffer);
                return obj;
            }
        }
    }
}