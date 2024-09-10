using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestRecruitListItem
    {
        public CDataQuestRecruitListItem()
        {
        }

        public uint QuestScheduleId { get; set; } // ID?
        public uint QuestId { get; set; } // NoMembers?
        public uint GroupsRecruiting { get; set; } // MaxMembers?

        public class Serializer : EntitySerializer<CDataQuestRecruitListItem>
        {
            public override void Write(IBuffer buffer, CDataQuestRecruitListItem obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt32(buffer, obj.GroupsRecruiting);
            }

            public override CDataQuestRecruitListItem Read(IBuffer buffer)
            {
                CDataQuestRecruitListItem obj = new CDataQuestRecruitListItem();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.GroupsRecruiting = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
