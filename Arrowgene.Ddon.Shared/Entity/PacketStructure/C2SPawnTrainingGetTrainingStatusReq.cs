using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnTrainingGetTrainingStatusReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_TRAINING_GET_TRAINING_STATUS_REQ;

        public uint PawnId { get; set; }
        public JobId Job { get; set;}

        public class Serializer : PacketEntitySerializer<C2SPawnTrainingGetTrainingStatusReq>
        {
            public override void Write(IBuffer buffer, C2SPawnTrainingGetTrainingStatusReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
            }

            public override C2SPawnTrainingGetTrainingStatusReq Read(IBuffer buffer)
            {
                C2SPawnTrainingGetTrainingStatusReq obj = new C2SPawnTrainingGetTrainingStatusReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                return obj;
            }
        }

    }
}