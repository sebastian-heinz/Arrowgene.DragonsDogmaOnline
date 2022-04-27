using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnTrainingGetPreparetionInfoToAdviceReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_TRAINING_GET_PREPARETION_INFO_TO_ADVICE_REQ;

        public uint Data0 { get; set; }

        public C2SPawnTrainingGetPreparetionInfoToAdviceReq()
        {
            Data0 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SPawnTrainingGetPreparetionInfoToAdviceReq>
        {
            public override void Write(IBuffer buffer, C2SPawnTrainingGetPreparetionInfoToAdviceReq obj)
            {
                WriteUInt32(buffer, obj.Data0);
            }

            public override C2SPawnTrainingGetPreparetionInfoToAdviceReq Read(IBuffer buffer)
            {
                C2SPawnTrainingGetPreparetionInfoToAdviceReq obj = new C2SPawnTrainingGetPreparetionInfoToAdviceReq();
                obj.Data0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
