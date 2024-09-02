using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnTrainingProfile
    {
        public uint TrainingExp { get; set; }
        public uint DialogCount { get; set; }
        public uint DialogCountMax { get; set; }
        public uint AttackFrequencyAndDistance { get; set; }
        public uint TrainingLv { get; set; }

        public class Serializer : EntitySerializer<CDataPawnTrainingProfile>
        {
            public override void Write(IBuffer buffer, CDataPawnTrainingProfile obj)
            {
                WriteUInt32(buffer, obj.TrainingExp);
                WriteUInt32(buffer, obj.DialogCount);
                WriteUInt32(buffer, obj.DialogCountMax);
                WriteUInt32(buffer, obj.AttackFrequencyAndDistance);
                WriteUInt32(buffer, obj.TrainingLv);
            }

            public override CDataPawnTrainingProfile Read(IBuffer buffer)
            {
                CDataPawnTrainingProfile obj = new CDataPawnTrainingProfile();
                obj.TrainingExp = ReadUInt32(buffer);
                obj.DialogCount = ReadUInt32(buffer);
                obj.DialogCountMax = ReadUInt32(buffer);
                obj.AttackFrequencyAndDistance = ReadUInt32(buffer);
                obj.TrainingLv = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
