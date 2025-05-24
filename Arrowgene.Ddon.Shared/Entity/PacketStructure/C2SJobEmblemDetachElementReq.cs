using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobEmblemDetachElementReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_EMBLEM_DETACH_ELEMENT_REQ;

        public C2SJobEmblemDetachElementReq()
        {
            EmblemUIDs = new();
        }

        public JobId JobId { get; set; }
        public byte InheritanceSlot { get; set; }
        public List<string> EmblemUIDs { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobEmblemDetachElementReq>
        {
            public override void Write(IBuffer buffer, C2SJobEmblemDetachElementReq obj)
            {
                WriteByte(buffer, (byte) obj.JobId);
                WriteByte(buffer, obj.InheritanceSlot);
                WriteMtStringList(buffer, obj.EmblemUIDs);
            }

            public override C2SJobEmblemDetachElementReq Read(IBuffer buffer)
            {
                C2SJobEmblemDetachElementReq obj = new C2SJobEmblemDetachElementReq();
                obj.JobId = (JobId) ReadByte(buffer);
                obj.InheritanceSlot = ReadByte(buffer);
                obj.EmblemUIDs = ReadMtStringList(buffer);
                return obj;
            }
        }
    }
}
