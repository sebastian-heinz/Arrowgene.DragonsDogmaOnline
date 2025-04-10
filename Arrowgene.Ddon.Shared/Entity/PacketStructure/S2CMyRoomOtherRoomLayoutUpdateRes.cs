using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomOtherRoomLayoutUpdateRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MY_ROOM_OTHER_ROOM_LAYOUT_UPDATE_RES;

        public class Serializer : PacketEntitySerializer<S2CMyRoomOtherRoomLayoutUpdateRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomOtherRoomLayoutUpdateRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CMyRoomOtherRoomLayoutUpdateRes Read(IBuffer buffer)
            {
                S2CMyRoomOtherRoomLayoutUpdateRes obj = new S2CMyRoomOtherRoomLayoutUpdateRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
