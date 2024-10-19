using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMobHuntQuestOrderList
    {
        public CDataMobHuntQuestOrderList()
        {
            Param = new CDataQuestOrderList();
            Detail = new CDataMobHuntQuestDetail();
        }

        public CDataQuestOrderList Param { get; set; }
        public CDataMobHuntQuestDetail Detail { get; set; }

        public class Serializer : EntitySerializer<CDataMobHuntQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataMobHuntQuestOrderList obj)
            {
                WriteEntity(buffer, obj.Param);
                WriteEntity(buffer, obj.Detail);
            }

            public override CDataMobHuntQuestOrderList Read(IBuffer buffer)
            {
                CDataMobHuntQuestOrderList obj = new CDataMobHuntQuestOrderList
                {
                    Param = ReadEntity<CDataQuestOrderList>(buffer),
                    Detail = ReadEntity<CDataMobHuntQuestDetail>(buffer)
                };
                return obj;
            }
        }
    }
}
