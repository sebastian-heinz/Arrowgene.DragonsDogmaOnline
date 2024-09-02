using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnTrainingSetTrainingStatusReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_TRAINING_SET_TRAINING_STATUS_REQ;

        public C2SPawnTrainingSetTrainingStatusReq()
        {
            TrainingStatus = new byte[64];
        }

        public uint PawnId { get; set; }
        public JobId Job { get; set; }
        public byte[] TrainingStatus { get; set; }
        public uint SpentTrainingPoints { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnTrainingSetTrainingStatusReq>
        {
            public override void Write(IBuffer buffer, C2SPawnTrainingSetTrainingStatusReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
                WriteByteArray(buffer, obj.TrainingStatus);
                WriteUInt32(buffer, obj.SpentTrainingPoints);
            }

            public override C2SPawnTrainingSetTrainingStatusReq Read(IBuffer buffer)
            {
                C2SPawnTrainingSetTrainingStatusReq obj = new C2SPawnTrainingSetTrainingStatusReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.TrainingStatus = ReadByteArray(buffer, 64);
                obj.SpentTrainingPoints = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}