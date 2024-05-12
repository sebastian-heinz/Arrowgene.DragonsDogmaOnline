using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnTrainingGetPreparetionInfoToAdviceRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_TRAINING_GET_PREPARETION_INFO_TO_ADVICE_RES;

        public S2CPawnTrainingGetPreparetionInfoToAdviceRes()
        {
            PawnTrainingPreparationInfoToAdvices = new List<CDataPawnTrainingPreparationInfoToAdvice>();
        }

        public uint Unk0 { get; set; }
        public List<CDataPawnTrainingPreparationInfoToAdvice> PawnTrainingPreparationInfoToAdvices { get; set; } 

        public class Serializer : PacketEntitySerializer<S2CPawnTrainingGetPreparetionInfoToAdviceRes>
        {
            public override void Write(IBuffer buffer, S2CPawnTrainingGetPreparetionInfoToAdviceRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList<CDataPawnTrainingPreparationInfoToAdvice>(buffer, obj.PawnTrainingPreparationInfoToAdvices);
            }

            public override S2CPawnTrainingGetPreparetionInfoToAdviceRes Read(IBuffer buffer)
            {
                S2CPawnTrainingGetPreparetionInfoToAdviceRes obj = new S2CPawnTrainingGetPreparetionInfoToAdviceRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadUInt32(buffer);
                obj.PawnTrainingPreparationInfoToAdvices = ReadEntityList<CDataPawnTrainingPreparationInfoToAdvice>(buffer);
                return obj;
            }
        }

    }
}
