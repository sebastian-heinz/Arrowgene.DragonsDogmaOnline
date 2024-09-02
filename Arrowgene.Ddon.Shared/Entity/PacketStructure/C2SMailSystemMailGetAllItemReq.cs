using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailSystemMailGetAllItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_SYSTEM_MAIL_GET_ALL_ITEM_REQ;

        public C2SMailSystemMailGetAllItemReq()
        {
        }

        public ulong MessageId { get; set; }
        public byte StorageType { get; set; }

        public class Serializer : PacketEntitySerializer<C2SMailSystemMailGetAllItemReq>
        {
            public override void Write(IBuffer buffer, C2SMailSystemMailGetAllItemReq obj)
            {
                WriteUInt64(buffer, obj.MessageId);
                WriteByte(buffer, obj.StorageType);
            }

            public override C2SMailSystemMailGetAllItemReq Read(IBuffer buffer)
            {
                C2SMailSystemMailGetAllItemReq obj = new C2SMailSystemMailGetAllItemReq();
                obj.MessageId = ReadUInt64(buffer);
                obj.StorageType = ReadByte(buffer);
                return obj;
            }
        }
    }
}

