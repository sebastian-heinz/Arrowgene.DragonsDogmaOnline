using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestEnemyInfo
{
    public CDataQuestEnemyInfo()
    {

    }
    
    public class Serializer : EntitySerializer<CDataQuestEnemyInfo>
    {
        public override void Write(IBuffer buffer, CDataQuestEnemyInfo obj)
        {

        }

        public override CDataQuestEnemyInfo Read(IBuffer buffer)
        {
            CDataQuestEnemyInfo obj = new CDataQuestEnemyInfo();
            return obj;
        }
    }
}
