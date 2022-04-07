using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnTrainingGetPreparetionInfoToAdviceRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_TRAINING_GET_PREPARETION_INFO_TO_ADVICE_RES;

        public S2CPawnTrainingGetPreparetionInfoToAdviceRes()
        {
        }

        public C2SPawnTrainingGetPreparetionInfoToAdviceReq PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnTrainingGetPreparetionInfoToAdviceRes>
        {
            public override void Write(IBuffer buffer, S2CPawnTrainingGetPreparetionInfoToAdviceRes obj)
            {
                WriteByteArray(buffer, Data);
            }

            public override S2CPawnTrainingGetPreparetionInfoToAdviceRes Read(IBuffer buffer)
            {
                S2CPawnTrainingGetPreparetionInfoToAdviceRes obj = new S2CPawnTrainingGetPreparetionInfoToAdviceRes();
                return obj;
            }


            private readonly byte[] Data =
            {
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x6, 0x0, 0x0, 0x0, 0x3,
                0x0, 0xDA, 0x5D, 0x4E, 0x0, 0x0, 0x0, 0x3, 0x0, 0x0, 0x0, 0x0, 0x0, 0xDA, 0x66, 0x8D,
                0x0, 0x0, 0x0, 0x3, 0x0, 0x0, 0x0, 0x1, 0x0, 0xDA, 0xB2, 0xF3, 0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x6, 0x41, 0x6, 0x6A
            };
        }

    }
}
