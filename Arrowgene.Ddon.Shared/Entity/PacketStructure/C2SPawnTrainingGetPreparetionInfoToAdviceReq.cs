using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnTrainingGetPreparetionInfoToAdviceReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_TRAINING_GET_PREPARETION_INFO_TO_ADVICE_REQ;

        public class Serializer : PacketEntitySerializer<C2SPawnTrainingGetPreparetionInfoToAdviceReq>
        {
            public override void Write(IBuffer buffer, C2SPawnTrainingGetPreparetionInfoToAdviceReq obj)
            {
            }

            public override C2SPawnTrainingGetPreparetionInfoToAdviceReq Read(IBuffer buffer)
            {
                C2SPawnTrainingGetPreparetionInfoToAdviceReq obj = new C2SPawnTrainingGetPreparetionInfoToAdviceReq();
                return obj;
            }
        }
    }
}
