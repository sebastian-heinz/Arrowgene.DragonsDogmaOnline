using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBlackListRemoveBlackListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BLACK_LIST_REMOVE_BLACK_LIST_REQ;

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBlackListRemoveBlackListReq>
        {
            public override void Write(IBuffer buffer, C2SBlackListRemoveBlackListReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SBlackListRemoveBlackListReq Read(IBuffer buffer)
            {
                C2SBlackListRemoveBlackListReq obj = new C2SBlackListRemoveBlackListReq();

                obj.CharacterId = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
