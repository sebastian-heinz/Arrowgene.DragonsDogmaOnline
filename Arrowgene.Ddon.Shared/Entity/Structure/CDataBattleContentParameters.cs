using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentParameters
    {
        public CDataBattleContentParameters()
        {
            Unk2 = new List<CDataCommonU32>();
        }

        public uint Id {  get; set; }
        public uint Tier { get; set; }
        public List<CDataCommonU32> Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentParameters>
        {
            public override void Write(IBuffer buffer, CDataBattleContentParameters obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteUInt32(buffer, obj.Tier);
                WriteEntityList(buffer, obj.Unk2);
            }

            public override CDataBattleContentParameters Read(IBuffer buffer)
            {
                CDataBattleContentParameters obj = new CDataBattleContentParameters();
                obj.Id = ReadUInt32(buffer);
                obj.Tier = ReadUInt32(buffer);
                obj.Unk2 = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
