using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnTrainingPreparationInfoToAdvice
    {
        public uint PawnId { get; set; }
        public uint Unk0 { get; set; } // CDataPawnList.Unk2
        public uint Unk1 { get; set; }    
    
        public class Serializer : EntitySerializer<CDataPawnTrainingPreparationInfoToAdvice>
        {
            public override void Write(IBuffer buffer, CDataPawnTrainingPreparationInfoToAdvice obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }
        
            public override CDataPawnTrainingPreparationInfoToAdvice Read(IBuffer buffer)
            {
                CDataPawnTrainingPreparationInfoToAdvice obj = new CDataPawnTrainingPreparationInfoToAdvice();
                obj.PawnId = ReadUInt32(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}