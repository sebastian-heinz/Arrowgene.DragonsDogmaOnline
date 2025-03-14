using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSpotEnemyInfo
    {
        public uint EnemyId { get; set; }
        public byte Level { get; set; }

        public class Serializer : EntitySerializer<CDataSpotEnemyInfo>
        {
            public override void Write(IBuffer buffer, CDataSpotEnemyInfo obj)
            {
                WriteUInt32(buffer, obj.EnemyId);
                WriteByte(buffer, obj.Level);
            }

            public override CDataSpotEnemyInfo Read(IBuffer buffer)
            {
                CDataSpotEnemyInfo obj = new CDataSpotEnemyInfo();
                obj.EnemyId = ReadUInt32(buffer);
                obj.Level = ReadByte(buffer);
                return obj;
            }
        }
    }
}
