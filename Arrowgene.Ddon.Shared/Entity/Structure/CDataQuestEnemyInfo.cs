using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestEnemyInfo
{
    public EnemyUIId GroupId { get; set; }
    public uint Unk0 { get; set; }
    public ushort Lv { get; set; }
    public bool IsPartyRecommend { get; set; }
    
    public class Serializer : EntitySerializer<CDataQuestEnemyInfo>
    {
        public override void Write(IBuffer buffer, CDataQuestEnemyInfo obj)
        {
            WriteUInt32(buffer, (uint)obj.GroupId);
            WriteUInt32(buffer, obj.Unk0);
            WriteUInt16(buffer, obj.Lv);
            WriteBool(buffer, obj.IsPartyRecommend);
        }

        public override CDataQuestEnemyInfo Read(IBuffer buffer)
        {
            CDataQuestEnemyInfo obj = new CDataQuestEnemyInfo();
            obj.GroupId = (EnemyUIId)ReadUInt32(buffer);
            obj.Unk0 = ReadUInt32(buffer);
            obj.Lv = ReadUInt16(buffer);
            obj.IsPartyRecommend = ReadBool(buffer);
            return obj;
        }
    }
}
