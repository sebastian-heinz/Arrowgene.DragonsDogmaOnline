using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMyRoomMyRoomBgmUpdateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MY_ROOM_MY_ROOM_BGM_UPDATE_REQ;

        public uint BgmAcquirementNo { get; set; }

        public C2SMyRoomMyRoomBgmUpdateReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMyRoomMyRoomBgmUpdateReq>
        {
            public override void Write(IBuffer buffer, C2SMyRoomMyRoomBgmUpdateReq obj)
            {
                WriteUInt32(buffer, obj.BgmAcquirementNo);
            }

            public override C2SMyRoomMyRoomBgmUpdateReq Read(IBuffer buffer)
            {
                C2SMyRoomMyRoomBgmUpdateReq obj = new C2SMyRoomMyRoomBgmUpdateReq();
                obj.BgmAcquirementNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
