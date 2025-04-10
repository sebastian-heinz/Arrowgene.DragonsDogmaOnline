using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomOtherRoomLayoutUpdateNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_MY_ROOM_OTHER_ROOM_LAYOUT_UPDATE_NTC;

        public List<CDataOtherRoomLayoutUpdate> UpdateList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CMyRoomOtherRoomLayoutUpdateNtc>
        {
            public override void Write(IBuffer buffer, S2CMyRoomOtherRoomLayoutUpdateNtc obj)
            {
                WriteEntityList(buffer, obj.UpdateList);
            }

            public override S2CMyRoomOtherRoomLayoutUpdateNtc Read(IBuffer buffer)
            {
                S2CMyRoomOtherRoomLayoutUpdateNtc obj = new S2CMyRoomOtherRoomLayoutUpdateNtc();
                obj.UpdateList = ReadEntityList<CDataOtherRoomLayoutUpdate>(buffer);
                return obj;
            }
        }
    }
}
