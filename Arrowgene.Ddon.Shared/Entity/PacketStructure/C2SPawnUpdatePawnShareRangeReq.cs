using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnUpdatePawnShareRangeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_UPDATE_PAWN_SHARE_RANGE_REQ;

        public C2SPawnUpdatePawnShareRangeReq()
        {
        }

        public uint PawnId { get; set; }
        public byte ShareRange { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnUpdatePawnShareRangeReq>
        {
            public override void Write(IBuffer buffer, C2SPawnUpdatePawnShareRangeReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.ShareRange);
            }

            public override C2SPawnUpdatePawnShareRangeReq Read(IBuffer buffer)
            {
                C2SPawnUpdatePawnShareRangeReq obj = new C2SPawnUpdatePawnShareRangeReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.ShareRange = ReadByte(buffer);
                return obj;
            }
        }
    }
}
