using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPackageQuestList
    {
        public CDataPackageQuestList()
        {
        }

        public uint Unk0 { get; set; }
        public List<CDataPackageQuestDetail> Details { get; set; } = new();


        public class Serializer : EntitySerializer<CDataPackageQuestList>
        {
            public override void Write(IBuffer buffer, CDataPackageQuestList obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Details);
            }

            public override CDataPackageQuestList Read(IBuffer buffer)
            {
                CDataPackageQuestList obj = new CDataPackageQuestList();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Details = ReadEntityList<CDataPackageQuestDetail>(buffer);
                return obj;
            }
        }
    }
}
