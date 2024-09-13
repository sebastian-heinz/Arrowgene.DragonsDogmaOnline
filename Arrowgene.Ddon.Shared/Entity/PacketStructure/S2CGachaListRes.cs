using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGachaListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GACHA_GACHA_LIST_RES;

        public List<CDataGachaInfo> GachaList { get; set; }

        public S2CGachaListRes()
        {
            GachaList = new List<CDataGachaInfo>();
        }

        public class Serializer : PacketEntitySerializer<S2CGachaListRes>
        {
            public override void Write(IBuffer buffer, S2CGachaListRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList<CDataGachaInfo>(buffer, obj.GachaList);
            }

            public override S2CGachaListRes Read(IBuffer buffer)
            {
                S2CGachaListRes obj = new S2CGachaListRes();

                ReadServerResponse(buffer, obj);

                obj.GachaList = ReadEntityList<CDataGachaInfo>(buffer);

                return obj;
            }
        }
    }
}
