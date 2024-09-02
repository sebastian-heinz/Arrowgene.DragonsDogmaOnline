using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnTrainingGetTrainingStatusRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_TRAINING_GET_TRAINING_STATUS_RES;

        public S2CPawnTrainingGetTrainingStatusRes()
        {
            TrainingStatus = new byte[64];
        }

        public byte[] TrainingStatus { get; set; }
        public uint TrainingPoints { get; set; }
        public uint AvailableTraining { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnTrainingGetTrainingStatusRes>
        {
            public override void Write(IBuffer buffer, S2CPawnTrainingGetTrainingStatusRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByteArray(buffer, obj.TrainingStatus);
                WriteUInt32(buffer, obj.TrainingPoints);
                WriteUInt32(buffer, obj.AvailableTraining);
            }

            public override S2CPawnTrainingGetTrainingStatusRes Read(IBuffer buffer)
            {
                S2CPawnTrainingGetTrainingStatusRes obj = new S2CPawnTrainingGetTrainingStatusRes();
                ReadServerResponse(buffer, obj);
                obj.TrainingStatus = ReadByteArray(buffer, 64);
                obj.TrainingPoints = ReadUInt32(buffer);
                obj.AvailableTraining = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}