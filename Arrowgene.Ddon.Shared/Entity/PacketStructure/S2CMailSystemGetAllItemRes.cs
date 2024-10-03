using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailSystemGetAllItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_SYSTEM_MAIL_GET_ALL_ITEM_RES;

        public S2CMailSystemGetAllItemRes()
        {
            AttachmentList = new CDataMailAttachmentList();
        }

        public ulong MessageId {  get; set; }
        public CDataMailAttachmentList AttachmentList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CMailSystemGetAllItemRes>
        {
            public override void Write(IBuffer buffer, S2CMailSystemGetAllItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.MessageId);
                WriteEntity(buffer, obj.AttachmentList);
            }

            public override S2CMailSystemGetAllItemRes Read(IBuffer buffer)
            {
                S2CMailSystemGetAllItemRes obj = new S2CMailSystemGetAllItemRes();
                ReadServerResponse(buffer, obj);
                obj.MessageId = ReadUInt64(buffer);
                obj.AttachmentList = ReadEntity<CDataMailAttachmentList>(buffer);
                return obj;
            }
        }
    }
}
