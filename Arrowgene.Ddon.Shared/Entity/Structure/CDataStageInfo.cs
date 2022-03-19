using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStageInfo
    {
        public uint Id { get; set; }
        public uint StageNo { get; set; }
        public uint RandomStageGroupId { get; set; }
        public uint Type { get; set; }
        public CDataStageAttribute StageAttribute { get; set; }
        public bool IsAutoSetBloodEnemy { get; set; }

        public CDataStageInfo()
        {
            Id = 0;
            StageNo = 0;
            RandomStageGroupId = 0;
            Type = 0;
            StageAttribute = new CDataStageAttribute();
            IsAutoSetBloodEnemy = false;
        }

        public class Serializer : EntitySerializer<CDataStageInfo>
        {
            public override void Write(IBuffer buffer, CDataStageInfo obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteUInt32(buffer, obj.StageNo);
                WriteUInt32(buffer, obj.RandomStageGroupId);
                WriteUInt32(buffer, obj.Type);
                WriteEntity<CDataStageAttribute>(buffer, obj.StageAttribute);
                WriteBool(buffer, obj.IsAutoSetBloodEnemy);
            }

            public override CDataStageInfo Read(IBuffer buffer)
            {
                CDataStageInfo obj = new CDataStageInfo();
                obj.Id = ReadUInt32(buffer);
                obj.StageNo = ReadUInt32(buffer);
                obj.RandomStageGroupId = ReadUInt32(buffer);
                obj.Type = ReadUInt32(buffer);
                obj.StageAttribute = ReadEntity<CDataStageAttribute>(buffer);
                obj.IsAutoSetBloodEnemy = ReadBool(buffer);
                return obj;
            }
        }
    }
}
