using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentUnk4
    {
        public CDataBattleContentUnk4()
        {
        }

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public String StageName { get; set; }
        public List<CDataWalletPoint> WalletPoints {  get; set; }
        public List<CDataBattleContentUnk5> Unk3 { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentUnk4>
        {
            public override void Write(IBuffer buffer, CDataBattleContentUnk4 obj)
            {
            }

            public override CDataBattleContentUnk4 Read(IBuffer buffer)
            {
                CDataBattleContentUnk4 obj = new CDataBattleContentUnk4();
                return obj;
            }
        }
    }
}



