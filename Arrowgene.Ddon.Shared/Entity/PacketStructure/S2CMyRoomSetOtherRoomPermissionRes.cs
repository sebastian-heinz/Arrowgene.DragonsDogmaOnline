using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomSetOtherRoomPermissionRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MY_ROOM_SET_OTHER_ROOM_PERMISSION_RES;

        public class Serializer : PacketEntitySerializer<S2CMyRoomSetOtherRoomPermissionRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomSetOtherRoomPermissionRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CMyRoomSetOtherRoomPermissionRes Read(IBuffer buffer)
            {
                S2CMyRoomSetOtherRoomPermissionRes obj = new S2CMyRoomSetOtherRoomPermissionRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
