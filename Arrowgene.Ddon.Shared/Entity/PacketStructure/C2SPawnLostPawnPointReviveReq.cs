using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnLostPawnPointReviveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_LOST_PAWN_POINT_REVIVE_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnLostPawnPointReviveReq>
        {
            public override void Write(IBuffer buffer, C2SPawnLostPawnPointReviveReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SPawnLostPawnPointReviveReq Read(IBuffer buffer)
            {
                C2SPawnLostPawnPointReviveReq obj = new C2SPawnLostPawnPointReviveReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}