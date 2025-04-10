using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMyRoomSetOtherRoomPermissionReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MY_ROOM_SET_OTHER_ROOM_PERMISSION_REQ;

        /// <summary>
        /// 1 = Deny, 2 = Allow
        /// </summary>
        public uint PermissionSetting { get; set; }

        public class Serializer : PacketEntitySerializer<C2SMyRoomSetOtherRoomPermissionReq>
        {
            public override void Write(IBuffer buffer, C2SMyRoomSetOtherRoomPermissionReq obj)
            {
                WriteUInt32(buffer, obj.PermissionSetting);
            }

            public override C2SMyRoomSetOtherRoomPermissionReq Read(IBuffer buffer)
            {
                C2SMyRoomSetOtherRoomPermissionReq obj = new C2SMyRoomSetOtherRoomPermissionReq();
                obj.PermissionSetting = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
