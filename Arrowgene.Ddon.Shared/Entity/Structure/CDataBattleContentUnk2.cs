using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentUnk2
    {
        public CDataBattleContentUnk2()
        {
        }

        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentUnk2>
        {
            public override void Write(IBuffer buffer, CDataBattleContentUnk2 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
            }

            public override CDataBattleContentUnk2 Read(IBuffer buffer)
            {
                CDataBattleContentUnk2 obj = new CDataBattleContentUnk2();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                return obj;
            }
        }
    }
}

