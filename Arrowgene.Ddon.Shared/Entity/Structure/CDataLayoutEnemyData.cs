using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class CDataLayoutEnemyData
    {
        public byte PositionIndex { get; set; }
        public CDataStageLayoutEnemyPresetEnemyInfoClient EnemyInfo { get; set; }

        public CDataLayoutEnemyData()
        {
            PositionIndex = 0;
            EnemyInfo = new CDataStageLayoutEnemyPresetEnemyInfoClient();
        }

        public class Serializer : EntitySerializer<CDataLayoutEnemyData>
        {
            public override void Write(IBuffer buffer, CDataLayoutEnemyData obj)
            {
                WriteByte(buffer, obj.PositionIndex);
                WriteEntity<CDataStageLayoutEnemyPresetEnemyInfoClient>(buffer, obj.EnemyInfo);
            }

            public override CDataLayoutEnemyData Read(IBuffer buffer)
            {
                CDataLayoutEnemyData obj = new CDataLayoutEnemyData();
                obj.PositionIndex = ReadByte(buffer);
                obj.EnemyInfo = ReadEntity<CDataStageLayoutEnemyPresetEnemyInfoClient>(buffer);
                return obj;
            }
        }
    }
}
