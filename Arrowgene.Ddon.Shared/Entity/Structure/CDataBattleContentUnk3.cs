using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentUnk3
    {
        public CDataBattleContentUnk3()
        {
            Unk0 = new CDataBattleContentUnk0();
            Unk1 = new List<CDataBattleContentUnk2>();
        }

        public CDataBattleContentUnk0 Unk0 { get; set; }
        public List<CDataBattleContentUnk2> Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentUnk3>
        {
            public override void Write(IBuffer buffer, CDataBattleContentUnk3 obj)
            {
                WriteEntity(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1);
            }

            public override CDataBattleContentUnk3 Read(IBuffer buffer)
            {
                CDataBattleContentUnk3 obj = new CDataBattleContentUnk3();
                obj.Unk0 = ReadEntity<CDataBattleContentUnk0>(buffer);
                obj.Unk1 = ReadEntityList<CDataBattleContentUnk2>(buffer);
                return obj;
            }
        }
    }
}


