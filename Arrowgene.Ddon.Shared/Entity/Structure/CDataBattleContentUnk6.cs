using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentUnk6
    {
        public CDataBattleContentUnk6()
        {
            Unk2 = new CDataBattleContentUnk0();
            Unk3 = new List<CDataBattleContentUnk2>();
        }

        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public CDataBattleContentUnk0 Unk2 {  get; set; }
        public List<CDataBattleContentUnk2> Unk3 {  get; set; }
        public bool Unk4 {  get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentUnk6>
        {
            public override void Write(IBuffer buffer, CDataBattleContentUnk6 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteEntity(buffer, obj.Unk2);
                WriteEntityList(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
            }

            public override CDataBattleContentUnk6 Read(IBuffer buffer)
            {
                CDataBattleContentUnk6 obj = new CDataBattleContentUnk6();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadEntity<CDataBattleContentUnk0>(buffer);
                obj.Unk3 = ReadEntityList<CDataBattleContentUnk2>(buffer);
                obj.Unk4 = ReadBool(buffer);
                return obj;
            }
        }
    }
}




