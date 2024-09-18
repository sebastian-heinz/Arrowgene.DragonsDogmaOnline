using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestTimeGainQuestPlayStartNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_TIME_GAIN_QUEST_PLAY_START_NTC;

        public S2CQuestTimeGainQuestPlayStartNtc()
        {
            TimeGainQuestPlayStartData = new CDataContentsPlayStartData();
        }

        public CDataContentsPlayStartData TimeGainQuestPlayStartData {get; set;}

        public class Serializer : PacketEntitySerializer<S2CQuestTimeGainQuestPlayStartNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestTimeGainQuestPlayStartNtc obj)
            {
                WriteEntity(buffer, obj.TimeGainQuestPlayStartData);
            }

            public override S2CQuestTimeGainQuestPlayStartNtc Read(IBuffer buffer)
            {
                S2CQuestTimeGainQuestPlayStartNtc obj = new S2CQuestTimeGainQuestPlayStartNtc();
                obj.TimeGainQuestPlayStartData = ReadEntity<CDataContentsPlayStartData>(buffer);
                return obj;
            }
        }
    }
}
