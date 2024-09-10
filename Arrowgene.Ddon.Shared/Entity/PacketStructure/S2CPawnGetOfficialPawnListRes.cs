using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetOfficialPawnListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_OFFICIAL_PAWN_LIST_RES;

        public S2CPawnGetOfficialPawnListRes()
        {
            OfficialPawnList = new List<CDataRegisterdPawnList>();
        }

        public List<CDataRegisterdPawnList> OfficialPawnList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetOfficialPawnListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetOfficialPawnListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.OfficialPawnList);
            }

            public override S2CPawnGetOfficialPawnListRes Read(IBuffer buffer)
            {
                S2CPawnGetOfficialPawnListRes obj = new S2CPawnGetOfficialPawnListRes();
                ReadServerResponse(buffer, obj);
                obj.OfficialPawnList = ReadEntityList<CDataRegisterdPawnList>(buffer);
                return obj;
            }
        }
    }
}
