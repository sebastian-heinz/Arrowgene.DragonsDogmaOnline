using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetMyPawnListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_MYPAWN_LIST_RES;

        public S2CPawnGetMyPawnListRes()
        {
            PawnList = new List<CDataPawnList>();
            PartnerInfo = new CDataPartnerPawnData();
        }

        public List<CDataPawnList> PawnList { get; set; }
        public CDataPartnerPawnData PartnerInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetMyPawnListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetMyPawnListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataPawnList>(buffer, obj.PawnList);
                WriteEntity<CDataPartnerPawnData>(buffer, obj.PartnerInfo);
            }

            public override S2CPawnGetMyPawnListRes Read(IBuffer buffer)
            {
                S2CPawnGetMyPawnListRes obj = new S2CPawnGetMyPawnListRes();
                ReadServerResponse(buffer, obj);
                obj.PawnList = ReadEntityList<CDataPawnList>(buffer);
                obj.PartnerInfo = ReadEntity<CDataPartnerPawnData>(buffer);
                return obj;
            }
        }
    }
}
