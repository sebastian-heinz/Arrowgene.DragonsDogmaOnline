using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailSystemMailGetListDataReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_SYSTEM_MAIL_GET_LIST_DATA_REQ;

        public C2SMailSystemMailGetListDataReq()
        {
        }

        public uint Offset {  get; set; }
        public uint Num {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SMailSystemMailGetListDataReq>
        {
            public override void Write(IBuffer buffer, C2SMailSystemMailGetListDataReq obj)
            {
                WriteUInt32(buffer, obj.Offset);
                WriteUInt32(buffer, obj.Num);
            }

            public override C2SMailSystemMailGetListDataReq Read(IBuffer buffer)
            {
                C2SMailSystemMailGetListDataReq obj = new C2SMailSystemMailGetListDataReq();
                obj.Offset = ReadUInt32(buffer);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

