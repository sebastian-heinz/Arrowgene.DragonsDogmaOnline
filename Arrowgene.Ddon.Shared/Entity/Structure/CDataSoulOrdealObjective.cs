using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSoulOrdealObjective
    {
        public CDataSoulOrdealObjective()
        {
            ObjectiveLabel = string.Empty;
        }

        public SoulOrdealObjective Type { get; set; }
        public SoulOrdealObjectivePriority Priority { get; set; }
        public uint Param1 { get; set; }
        public uint Param2 { get; set; }
        public string ObjectiveLabel { get; set; }

        public class Serializer : EntitySerializer<CDataSoulOrdealObjective>
        {
            public override void Write(IBuffer buffer, CDataSoulOrdealObjective obj)
            {
                WriteByte(buffer, (byte) obj.Type);
                WriteByte(buffer, (byte) obj.Priority);
                WriteUInt32(buffer, obj.Param1);
                WriteUInt32(buffer, obj.Param2);
                WriteMtString(buffer, obj.ObjectiveLabel);
            }

            public override CDataSoulOrdealObjective Read(IBuffer buffer)
            {
                CDataSoulOrdealObjective obj = new CDataSoulOrdealObjective();
                obj.Type = (SoulOrdealObjective) ReadByte(buffer);
                obj.Priority = (SoulOrdealObjectivePriority) ReadByte(buffer);
                obj.Param1 = ReadUInt32(buffer);
                obj.Param2 = ReadUInt32(buffer);
                obj.ObjectiveLabel = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
