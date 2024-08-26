using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetRegisteredPawnListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_REGISTERED_PAWN_LIST_RES;

        public S2CPawnGetRegisteredPawnListRes()
        {
            RegisterdPawnList = new List<CDataRegisterdPawnList>();
        }

        public List<CDataRegisterdPawnList> RegisterdPawnList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetRegisteredPawnListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetRegisteredPawnListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.RegisterdPawnList);
            }

            public override S2CPawnGetRegisteredPawnListRes Read(IBuffer buffer)
            {
                S2CPawnGetRegisteredPawnListRes obj = new S2CPawnGetRegisteredPawnListRes();
                ReadServerResponse(buffer, obj);
                obj.RegisterdPawnList = ReadEntityList<CDataRegisterdPawnList>(buffer);
                return obj;
            }
        }
    }
}
