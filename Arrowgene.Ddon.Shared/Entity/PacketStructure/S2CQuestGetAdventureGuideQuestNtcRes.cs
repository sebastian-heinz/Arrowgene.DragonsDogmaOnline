using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetAdventureGuideQuestNtcRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_ADVENTURE_GUIDE_QUEST_NTC_RES;

        public S2CQuestGetAdventureGuideQuestNtcRes()
        {
        }

        public bool Unk0 {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetAdventureGuideQuestNtcRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetAdventureGuideQuestNtcRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteBool(buffer, obj.Unk0);
            }

            public override S2CQuestGetAdventureGuideQuestNtcRes Read(IBuffer buffer)
            {
                S2CQuestGetAdventureGuideQuestNtcRes obj = new S2CQuestGetAdventureGuideQuestNtcRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
