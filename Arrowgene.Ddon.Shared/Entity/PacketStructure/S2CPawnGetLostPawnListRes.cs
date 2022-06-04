using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetLostPawnListRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_GET_LOST_PAWN_LIST_RES;

        public S2CPawnGetLostPawnListRes()
        {
        }

        public List<CDataLostPawn> LostPawnList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetLostPawnListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetLostPawnListRes obj)
            {
                WriteEntityList<CDataLostPawn>(buffer, obj.LostPawnList); 
            }

            public override S2CPawnGetLostPawnListRes Read(IBuffer buffer)
            {
                S2CPawnGetLostPawnListRes obj = new S2CPawnGetLostPawnListRes();
                obj.LostPawnList = ReadEntityList<CDataLostPawn>(buffer);
                return obj;
            }
        }

    }
}
