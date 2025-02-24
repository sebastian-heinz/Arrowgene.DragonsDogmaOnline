using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentStage
    {
        public uint Id { get; set; }
        public string StageName { get; set; } = string.Empty;
        public BattleContentMode Mode { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentStage>
        {
            public override void Write(IBuffer buffer, CDataBattleContentStage obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteMtString(buffer, obj.StageName);
                WriteUInt32(buffer, (uint) obj.Mode);
            }

            public override CDataBattleContentStage Read(IBuffer buffer)
            {
                CDataBattleContentStage obj = new CDataBattleContentStage();
                obj.Id = ReadUInt32(buffer);
                obj.StageName = ReadMtString(buffer);
                obj.Mode = (BattleContentMode) ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
