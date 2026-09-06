using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnUpdatePawnShareRangeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_UPDATE_PAWN_SHARE_RANGE_RES;

        public S2CPawnUpdatePawnShareRangeRes()
        {
        }

        public uint PawnId { get; set; }
        public byte ShareRange { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnUpdatePawnShareRangeRes>
        {
            public override void Write(IBuffer buffer, S2CPawnUpdatePawnShareRangeRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnUpdatePawnShareRangeRes Read(IBuffer buffer)
            {
                S2CPawnUpdatePawnShareRangeRes obj = new S2CPawnUpdatePawnShareRangeRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
