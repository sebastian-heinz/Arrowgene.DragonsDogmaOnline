using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomGetOtherRoomPermissionRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MY_ROOM_GET_OTHER_ROOM_PERMISSION_RES;

        public uint PermissionSetting { get; set; }

        public class Serializer : PacketEntitySerializer<S2CMyRoomGetOtherRoomPermissionRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomGetOtherRoomPermissionRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PermissionSetting);
            }

            public override S2CMyRoomGetOtherRoomPermissionRes Read(IBuffer buffer)
            {
                S2CMyRoomGetOtherRoomPermissionRes obj = new S2CMyRoomGetOtherRoomPermissionRes();
                ReadServerResponse(buffer, obj);
                obj.PermissionSetting = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
