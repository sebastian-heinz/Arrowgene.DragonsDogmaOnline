using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetMainQuestNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_GET_MAIN_QUEST_LIST_NTC;

        public S2CQuestGetMainQuestNtc()
        {
            MainQuestList = new List<CDataMainQuestList>();
        }

        public List<CDataMainQuestList> MainQuestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetMainQuestNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestGetMainQuestNtc obj)
            {
                WriteEntityList<CDataMainQuestList>(buffer, obj.MainQuestList);
            }

            public override S2CQuestGetMainQuestNtc Read(IBuffer buffer)
            {
                S2CQuestGetMainQuestNtc obj = new S2CQuestGetMainQuestNtc();
                obj.MainQuestList = ReadEntityList<CDataMainQuestList>(buffer);
                return obj;
            }
        }
    }
}
