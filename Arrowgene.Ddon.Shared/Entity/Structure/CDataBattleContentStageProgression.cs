using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentStageProgression
    {
        public CDataBattleContentStageProgression()
        {
            ConnectionList = new List<CDataCommonU32>();
        }

        public uint Id {  get; set; }
        public uint Tier { get; set; }
        public List<CDataCommonU32> ConnectionList { get; set; } // List of ContentId's this element can link to?

        public class Serializer : EntitySerializer<CDataBattleContentStageProgression>
        {
            public override void Write(IBuffer buffer, CDataBattleContentStageProgression obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteUInt32(buffer, obj.Tier);
                WriteEntityList(buffer, obj.ConnectionList);
            }

            public override CDataBattleContentStageProgression Read(IBuffer buffer)
            {
                CDataBattleContentStageProgression obj = new CDataBattleContentStageProgression();
                obj.Id = ReadUInt32(buffer);
                obj.Tier = ReadUInt32(buffer);
                obj.ConnectionList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
