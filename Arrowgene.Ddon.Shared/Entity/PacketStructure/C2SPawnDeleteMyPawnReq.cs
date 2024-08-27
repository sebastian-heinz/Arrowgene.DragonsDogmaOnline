using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnDeleteMyPawnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_DELETE_MYPAWN_REQ;

        public byte SlotNo { get; set; }
        public bool IsKeepEquip { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnDeleteMyPawnReq>
        {
            public override void Write(IBuffer buffer, C2SPawnDeleteMyPawnReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteBool(buffer, obj.IsKeepEquip);
            }

            public override C2SPawnDeleteMyPawnReq Read(IBuffer buffer)
            {
                C2SPawnDeleteMyPawnReq obj = new C2SPawnDeleteMyPawnReq();
                obj.SlotNo = ReadByte(buffer);
                obj.IsKeepEquip = ReadBool(buffer);
                return obj;
            }
        }

    }
}
