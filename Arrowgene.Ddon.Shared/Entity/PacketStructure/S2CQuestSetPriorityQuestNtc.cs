using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSetPriorityQuestNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_SET_PRIORITY_QUEST_NTC;

        public S2CQuestSetPriorityQuestNtc()
        {
            PriorityQuestList = new List<CDataPriorityQuest>();
        }

        public uint CharacterId {  get; set; }
        public List<CDataPriorityQuest> PriorityQuestList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestSetPriorityQuestNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestSetPriorityQuestNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntityList<CDataPriorityQuest>(buffer, obj.PriorityQuestList);
            }

            public override S2CQuestSetPriorityQuestNtc Read(IBuffer buffer)
            {
                S2CQuestSetPriorityQuestNtc obj = new S2CQuestSetPriorityQuestNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PriorityQuestList = ReadEntityList<CDataPriorityQuest>(buffer);
                return obj;
            }
        }
    }
}
