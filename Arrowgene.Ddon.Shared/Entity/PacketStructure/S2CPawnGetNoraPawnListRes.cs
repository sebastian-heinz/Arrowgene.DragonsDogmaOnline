using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetNoraPawnListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_NORA_PAWN_LIST_RES;

        public S2CPawnGetNoraPawnListRes()
        {
            NoraPawnList = new List<CDataRegisterdPawnList>();
        }

        public List<CDataRegisterdPawnList> NoraPawnList { get; set; }


        public class Serializer : PacketEntitySerializer<S2CPawnGetNoraPawnListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetNoraPawnListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.NoraPawnList);
            }

            public override S2CPawnGetNoraPawnListRes Read(IBuffer buffer)
            {
                S2CPawnGetNoraPawnListRes obj = new S2CPawnGetNoraPawnListRes();
                ReadServerResponse(buffer, obj);
                obj.NoraPawnList = ReadEntityList<CDataRegisterdPawnList>(buffer);
                return obj;
            }
        }
    }
}
