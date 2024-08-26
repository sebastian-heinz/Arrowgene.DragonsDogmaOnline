using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentUnk5
    {
        public CDataBattleContentUnk5()
        {
        }

        public uint Unk0 { get; set; }
        public ushort Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentUnk5>
        {
            public override void Write(IBuffer buffer, CDataBattleContentUnk5 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.Unk1);
            }

            public override CDataBattleContentUnk5 Read(IBuffer buffer)
            {
                CDataBattleContentUnk5 obj = new CDataBattleContentUnk5();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}



