using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceGetOmInstantKeyValueAllRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_GET_OM_INSTANT_KEY_VALUE_ALL_RES;

        public S2CInstanceGetOmInstantKeyValueAllRes()
        {
            Values = new List<CDataOmData>();
        }

        public uint StageId { get; set; }
        public List<CDataOmData> Values {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceGetOmInstantKeyValueAllRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceGetOmInstantKeyValueAllRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.StageId);
                WriteEntityList<CDataOmData>(buffer, obj.Values);
            }

            public override S2CInstanceGetOmInstantKeyValueAllRes Read(IBuffer buffer)
            {
                S2CInstanceGetOmInstantKeyValueAllRes obj = new S2CInstanceGetOmInstantKeyValueAllRes();
                ReadServerResponse(buffer, obj);
                obj.StageId = ReadUInt32(buffer);
                obj.Values = ReadEntityList<CDataOmData>(buffer);
                return obj;
            }
        }
    }
}

