using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobOrbDevoteElementSpecialCondition
    {
        public CDataJobOrbDevoteElementSpecialCondition()
        {
            Message = string.Empty;
        }

        public uint ConditionId {  get; set; }
        public string Message {  get; set; }
        public bool IsReleased {  get; set; }

        public class Serializer : EntitySerializer<CDataJobOrbDevoteElementSpecialCondition>
        {
            public override void Write(IBuffer buffer, CDataJobOrbDevoteElementSpecialCondition obj)
            {
                WriteUInt32(buffer, obj.ConditionId);
                WriteMtString(buffer, obj.Message);
                WriteBool(buffer, obj.IsReleased);
            }

            public override CDataJobOrbDevoteElementSpecialCondition Read(IBuffer buffer)
            {
                CDataJobOrbDevoteElementSpecialCondition obj = new CDataJobOrbDevoteElementSpecialCondition();
                obj.ConditionId = ReadUInt32(buffer);
                obj.Message = ReadMtString(buffer);
                obj.IsReleased = ReadBool(buffer);
                return obj;
            }
        }
    }
}
