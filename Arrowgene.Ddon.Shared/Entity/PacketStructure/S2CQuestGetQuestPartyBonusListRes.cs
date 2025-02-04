using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetQuestPartyBonusListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_QUEST_PARTY_BONUS_LIST_RES;

        public S2CQuestGetQuestPartyBonusListRes()
        {
            PartyBonusList = new();
            NextReloadTime = DateTimeOffset.MaxValue;
        }

        public List<CDataQuestPartyBonusInfo> PartyBonusList { get; set; }
        public DateTimeOffset NextReloadTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetQuestPartyBonusListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetQuestPartyBonusListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.PartyBonusList);

                // Probably actually an int64 and the debug table is lying.
                WriteUInt64(buffer, (ulong)obj.NextReloadTime.ToUnixTimeSeconds()); 
            }

            public override S2CQuestGetQuestPartyBonusListRes Read(IBuffer buffer)
            {
                S2CQuestGetQuestPartyBonusListRes obj = new S2CQuestGetQuestPartyBonusListRes();
                ReadServerResponse(buffer, obj);
                obj.PartyBonusList = ReadEntityList<CDataQuestPartyBonusInfo>(buffer);
                obj.NextReloadTime = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                return obj;
            }
        }
    }
}
