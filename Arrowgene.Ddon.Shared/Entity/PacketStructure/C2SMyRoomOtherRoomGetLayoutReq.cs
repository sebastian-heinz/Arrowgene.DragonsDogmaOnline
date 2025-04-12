using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMyRoomOtherRoomGetLayoutReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MY_ROOM_OTHER_ROOM_LAYOUT_GET_REQ;

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SMyRoomOtherRoomGetLayoutReq>
        {
            public override void Write(IBuffer buffer, C2SMyRoomOtherRoomGetLayoutReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SMyRoomOtherRoomGetLayoutReq Read(IBuffer buffer)
            {
                C2SMyRoomOtherRoomGetLayoutReq obj = new C2SMyRoomOtherRoomGetLayoutReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
