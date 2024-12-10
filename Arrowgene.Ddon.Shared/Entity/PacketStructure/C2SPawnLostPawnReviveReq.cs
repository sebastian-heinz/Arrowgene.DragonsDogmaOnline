using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnLostPawnReviveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_LOST_PAWN_REVIVE_REQ;

        public uint PawnId {get; set;}

        public class Serializer : PacketEntitySerializer<C2SPawnLostPawnReviveReq>
        {
            public override void Write(IBuffer buffer, C2SPawnLostPawnReviveReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SPawnLostPawnReviveReq Read(IBuffer buffer)
            {
                C2SPawnLostPawnReviveReq obj = new C2SPawnLostPawnReviveReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}