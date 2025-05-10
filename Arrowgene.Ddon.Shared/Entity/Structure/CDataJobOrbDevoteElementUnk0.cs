using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobOrbDevoteElementUnk0
    {
        public CDataJobOrbDevoteElementUnk0()
        {
            Unk1 = string.Empty;
        }

        public uint Unk0 {  get; set; }
        public string Unk1 {  get; set; }
        public bool Unk2 {  get; set; }

        public class Serializer : EntitySerializer<CDataJobOrbDevoteElementUnk0>
        {
            public override void Write(IBuffer buffer, CDataJobOrbDevoteElementUnk0 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1);
                WriteBool(buffer, obj.Unk2);
            }

            public override CDataJobOrbDevoteElementUnk0 Read(IBuffer buffer)
            {
                CDataJobOrbDevoteElementUnk0 obj = new CDataJobOrbDevoteElementUnk0();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadMtString(buffer);
                obj.Unk2 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
