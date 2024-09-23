using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSituationObjective
    {
        public CDataSituationObjective()
        {
            Message = string.Empty;
        }

        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public string Message { get; set; }

        public class Serializer : EntitySerializer<CDataSituationObjective>
        {
            public override void Write(IBuffer buffer, CDataSituationObjective obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteMtString(buffer, obj.Message);
            }

            public override CDataSituationObjective Read(IBuffer buffer)
            {
                CDataSituationObjective obj = new CDataSituationObjective();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Message = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
