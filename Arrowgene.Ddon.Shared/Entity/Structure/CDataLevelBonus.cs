using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLevelBonus
    {
        public uint Unk0 { get; set; }
        public List<CDataLevelBonusElement> BonusList { get; set; } = new();

        public class Serializer : EntitySerializer<CDataLevelBonus>
        {
            public override void Write(IBuffer buffer, CDataLevelBonus obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.BonusList);
            }

            public override CDataLevelBonus Read(IBuffer buffer)
            {
                CDataLevelBonus obj = new CDataLevelBonus();
                obj.Unk0 = ReadUInt32(buffer);
                obj.BonusList = ReadEntityList<CDataLevelBonusElement>(buffer);
                return obj;
            }
        }
    }
}
