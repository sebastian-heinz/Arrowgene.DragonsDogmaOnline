using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobEmblemUpdateLevelReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_EMBLEM_UPDATE_LEVEL_REQ;

        public C2SJobEmblemUpdateLevelReq()
        {
        }

        public JobId JobId { get; set; }
        public byte Level { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobEmblemUpdateLevelReq>
        {
            public override void Write(IBuffer buffer, C2SJobEmblemUpdateLevelReq obj)
            {
                WriteByte(buffer, (byte)obj.JobId);
                WriteByte(buffer, obj.Level);
            }

            public override C2SJobEmblemUpdateLevelReq Read(IBuffer buffer)
            {
                C2SJobEmblemUpdateLevelReq obj = new C2SJobEmblemUpdateLevelReq();
                obj.JobId = (JobId)ReadByte(buffer);
                obj.Level = ReadByte(buffer);
                return obj;
            }
        }
    }
}
