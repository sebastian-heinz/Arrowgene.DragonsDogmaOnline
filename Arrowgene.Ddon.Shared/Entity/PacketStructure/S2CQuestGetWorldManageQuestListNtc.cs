using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetWorldManageQuestListNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_GET_WORLD_MANAGE_QUEST_LIST_NTC;

        public S2CQuestGetWorldManageQuestListNtc()
        {
            WorldManageQuestList = new List<CDataWorldManageQuestList>();
        }

        public List<CDataWorldManageQuestList> WorldManageQuestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetWorldManageQuestListNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestGetWorldManageQuestListNtc obj)
            {
                WriteEntityList<CDataWorldManageQuestList>(buffer, obj.WorldManageQuestList);
            }

            public override S2CQuestGetWorldManageQuestListNtc Read(IBuffer buffer)
            {
                S2CQuestGetWorldManageQuestListNtc obj = new S2CQuestGetWorldManageQuestListNtc();
                obj.WorldManageQuestList = ReadEntityList<CDataWorldManageQuestList>(buffer);
                return obj;
            }
        }

    }
}