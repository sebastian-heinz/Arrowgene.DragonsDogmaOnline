using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaRankUpQuestInfo
    {
        public CDataAreaRankUpQuestInfo()
        {
        }

        public uint Rank { get; set; }
        public uint QuestId { get; set; }

        public class Serializer : EntitySerializer<CDataAreaRankUpQuestInfo>
        {
            public override void Write(IBuffer buffer, CDataAreaRankUpQuestInfo obj)
            {
                WriteUInt32(buffer, obj.Rank);
                WriteUInt32(buffer, obj.QuestId);
            }

            public override CDataAreaRankUpQuestInfo Read(IBuffer buffer)
            {
                CDataAreaRankUpQuestInfo obj = new CDataAreaRankUpQuestInfo();
                obj.Rank = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
