using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnTrainingSetTrainingStatusRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_TRAINING_SET_TRAINING_STATUS_RES;

        public class Serializer : PacketEntitySerializer<S2CPawnTrainingSetTrainingStatusRes>
        {
            public override void Write(IBuffer buffer, S2CPawnTrainingSetTrainingStatusRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnTrainingSetTrainingStatusRes Read(IBuffer buffer)
            {
                S2CPawnTrainingSetTrainingStatusRes obj = new S2CPawnTrainingSetTrainingStatusRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}