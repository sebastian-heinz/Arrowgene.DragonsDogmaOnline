using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestLayoutFlagSetInfo
    {
        public QuestLayoutFlagSetInfo()
        {
        }

        public uint FlagNo { get; set; }
        public uint StageNo { get; set; }
        public uint GroupId { get; set; }

        public CDataQuestLayoutFlagSetInfo AsCDataQuestLayoutFlagSetInfo()
        {
            return new CDataQuestLayoutFlagSetInfo()
            {
                LayoutFlagNo = FlagNo,
                SetInfoList = new List<CDataQuestSetInfo>()
                {
                    new CDataQuestSetInfo() { StageNo = StageNo, GroupId = GroupId }
                }
            };
        }

        public static QuestLayoutFlagSetInfo FromQuestAssetJson(JsonElement jLayoutFlagSetInfo)
        {
            QuestLayoutFlagSetInfo result = new QuestLayoutFlagSetInfo();
            result.FlagNo = jLayoutFlagSetInfo.GetProperty("flag_no").GetUInt32();
            result.StageNo = jLayoutFlagSetInfo.GetProperty("stage_no").GetUInt32();
            result.GroupId = jLayoutFlagSetInfo.GetProperty("group_id").GetUInt32();
            return result;
        }
    }
}
