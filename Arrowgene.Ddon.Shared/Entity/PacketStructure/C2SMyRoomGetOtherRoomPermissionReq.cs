using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMyRoomGetOtherRoomPermissionReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MY_ROOM_GET_OTHER_ROOM_PERMISSION_REQ;

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SMyRoomGetOtherRoomPermissionReq>
        {
            public override void Write(IBuffer buffer, C2SMyRoomGetOtherRoomPermissionReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SMyRoomGetOtherRoomPermissionReq Read(IBuffer buffer)
            {
                C2SMyRoomGetOtherRoomPermissionReq obj = new C2SMyRoomGetOtherRoomPermissionReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
