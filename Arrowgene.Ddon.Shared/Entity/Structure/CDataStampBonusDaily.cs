using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStampBonusDaily
    {
        public CDataStampBonusDaily()
        {
            StampBonus = new List<CDataStampBonus>();
        }

        public uint Unk0 = 1; //Nested list index? Weird MtArray stuff.
        public List<CDataStampBonus> StampBonus { get; set; }
        public ushort StampNum {  get; set; }
        public byte RecieveState { get; set; }

        public class Serializer : EntitySerializer<CDataStampBonusDaily>
        {
            public override void Write(IBuffer buffer, CDataStampBonusDaily obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList<CDataStampBonus>(buffer, obj.StampBonus);
                WriteUInt16(buffer, obj.StampNum);
                WriteByte(buffer, obj.RecieveState);
            }

            public override CDataStampBonusDaily Read(IBuffer buffer)
            {
                CDataStampBonusDaily obj = new CDataStampBonusDaily();
                obj.Unk0 = ReadUInt32(buffer);
                obj.StampBonus = ReadEntityList<CDataStampBonus>(buffer);
                obj.StampNum = ReadUInt16(buffer);
                obj.RecieveState = ReadByte(buffer);
                return obj;
            }
        }
    }
}
