using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyQuestProgressNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_11_119_16_NTC;

        public S2CPartyPartyQuestProgressNtc()
        {
            PartyQuestProgressInfo = new CDataPartyQuestProgressInfo();
        }

        public UInt32 CharacterId {  get; set; }
        public CDataPartyQuestProgressInfo PartyQuestProgressInfo {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyQuestProgressNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyQuestProgressNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataPartyQuestProgressInfo>(buffer, obj.PartyQuestProgressInfo);
            }

            public override S2CPartyPartyQuestProgressNtc Read(IBuffer buffer)
            {
                S2CPartyPartyQuestProgressNtc obj = new S2CPartyPartyQuestProgressNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PartyQuestProgressInfo = ReadEntity<CDataPartyQuestProgressInfo>(buffer);
                return obj;
            }
        }

    }
}
