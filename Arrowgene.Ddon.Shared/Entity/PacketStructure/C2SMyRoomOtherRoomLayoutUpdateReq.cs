using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMyRoomOtherRoomLayoutUpdateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MY_ROOM_OTHER_ROOM_LAYOUT_UPDATE_REQ;

        public List<CDataOtherRoomLayoutUpdate> UpdateList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SMyRoomOtherRoomLayoutUpdateReq>
        {
            public override void Write(IBuffer buffer, C2SMyRoomOtherRoomLayoutUpdateReq obj)
            {
                WriteEntityList(buffer, obj.UpdateList);
            }

            public override C2SMyRoomOtherRoomLayoutUpdateReq Read(IBuffer buffer)
            {
                C2SMyRoomOtherRoomLayoutUpdateReq obj = new C2SMyRoomOtherRoomLayoutUpdateReq();
                obj.UpdateList = ReadEntityList<CDataOtherRoomLayoutUpdate>(buffer);
                return obj;
            }
        }
    }
}
