using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentUnk1
    {
        public CDataBattleContentUnk1()
        {
            Unk2 = new List<CDataCommonU32>();
        }

        public uint Unk0 {  get; set; }
        public uint Unk1 { get; set; }
        public List<CDataCommonU32> Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentUnk1>
        {
            public override void Write(IBuffer buffer, CDataBattleContentUnk1 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.Unk2);
            }

            public override CDataBattleContentUnk1 Read(IBuffer buffer)
            {
                CDataBattleContentUnk1 obj = new CDataBattleContentUnk1();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
