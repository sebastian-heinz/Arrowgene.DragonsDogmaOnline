using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBoxGachaListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BOX_GACHA_BOX_GACHA_LIST_RES;

        public List<CDataBoxGachaInfo> BoxGachaList { get; set; }

        public S2CBoxGachaListRes()
        {
            BoxGachaList = new List<CDataBoxGachaInfo>();
        }

        public class Serializer : PacketEntitySerializer<S2CBoxGachaListRes>
        {
            public override void Write(IBuffer buffer, S2CBoxGachaListRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList<CDataBoxGachaInfo>(buffer, obj.BoxGachaList);
            }

            public override S2CBoxGachaListRes Read(IBuffer buffer)
            {
                S2CBoxGachaListRes obj = new S2CBoxGachaListRes();

                ReadServerResponse(buffer, obj);

                obj.BoxGachaList = ReadEntityList<CDataBoxGachaInfo>(buffer);

                return obj;
            }
        }
    }
}
