using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailMailGetListDataReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_MAIL_GET_LIST_DATA_REQ;

        public C2SMailMailGetListDataReq()
        {
        }

        public uint Offset {  get; set; }
        public uint Num {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SMailMailGetListDataReq>
        {
            public override void Write(IBuffer buffer, C2SMailMailGetListDataReq obj)
            {
                WriteUInt32(buffer, obj.Offset);
                WriteUInt32(buffer, obj.Num);
            }

            public override C2SMailMailGetListDataReq Read(IBuffer buffer)
            {
                C2SMailMailGetListDataReq obj = new C2SMailMailGetListDataReq();
                obj.Offset = ReadUInt32(buffer);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

