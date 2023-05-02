using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobChangePawnJobReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_CHANGE_PAWN_JOB_REQ;

        public uint PawnId { get; set; }
        public JobId JobId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobChangePawnJobReq>
        {
            public override void Write(IBuffer buffer, C2SJobChangePawnJobReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.JobId);
            }

            public override C2SJobChangePawnJobReq Read(IBuffer buffer)
            {
                C2SJobChangePawnJobReq obj = new C2SJobChangePawnJobReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.JobId = (JobId) ReadByte(buffer);
                return obj;
            }
        }

    }
}