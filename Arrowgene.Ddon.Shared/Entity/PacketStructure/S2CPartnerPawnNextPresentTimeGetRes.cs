using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartnerPawnNextPresentTimeGetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTNER_PAWN_PARTNER_PAWN_NEXT_PRESENT_TIME_GET_RES;

        public S2CPartnerPawnNextPresentTimeGetRes()
        {
        }

        public uint RemainSec { get; set; }
        public bool IsMax { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartnerPawnNextPresentTimeGetRes>
        {
            public override void Write(IBuffer buffer, S2CPartnerPawnNextPresentTimeGetRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.RemainSec);
                WriteBool(buffer, obj.IsMax);
            }

            public override S2CPartnerPawnNextPresentTimeGetRes Read(IBuffer buffer)
            {
                S2CPartnerPawnNextPresentTimeGetRes obj = new S2CPartnerPawnNextPresentTimeGetRes();
                ReadServerResponse(buffer, obj);
                obj.RemainSec = ReadUInt32(buffer);
                obj.IsMax = ReadBool(buffer);
                return obj;
            }
        }
    }
}
