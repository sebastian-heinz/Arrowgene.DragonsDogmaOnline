using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTimeGainQuestUnk1Unk2
    {
        public CDataTimeGainQuestUnk1Unk2()
        {
        }

        public uint Unk0 {  get; set; }
        public bool Unk1 {  get; set; }

        public class Serializer : EntitySerializer<CDataTimeGainQuestUnk1Unk2>
        {
            public override void Write(IBuffer buffer, CDataTimeGainQuestUnk1Unk2 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteBool(buffer, obj.Unk1);
            }

            public override CDataTimeGainQuestUnk1Unk2 Read(IBuffer buffer)
            {
                CDataTimeGainQuestUnk1Unk2 obj = new CDataTimeGainQuestUnk1Unk2();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
