using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWildHuntQuestOrderList
    {
        public CDataWildHuntQuestOrderList()
        {
            Param = new CDataQuestOrderList();
            Detail = new CDataWildHuntQuestDetail();
        }

        public CDataQuestOrderList Param { get; set; }
        public CDataWildHuntQuestDetail Detail { get; set; }

        public class Serializer : EntitySerializer<CDataWildHuntQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataWildHuntQuestOrderList obj)
            {
                WriteEntity(buffer, obj.Param);
                WriteEntity(buffer, obj.Detail);
            }

            public override CDataWildHuntQuestOrderList Read(IBuffer buffer)
            {
                CDataWildHuntQuestOrderList obj = new CDataWildHuntQuestOrderList
                {
                    Param = ReadEntity<CDataQuestOrderList>(buffer),
                    Detail = ReadEntity<CDataWildHuntQuestDetail>(buffer)
                };
                return obj;
            }
        }
    }
}
