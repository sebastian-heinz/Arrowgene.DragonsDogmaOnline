using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CParty_6_8_16_Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_6_8_16_NTC;
        
        public S2CParty_6_8_16_Ntc()
        {
            Error = 0;
            LeaderCharacterId = 0;
            HostCharacterId = 0;
            PartyMembers = new List<CDataPartyMember>();
            IsReceived = false;
        }
        
        public uint Error { get; set; }
        public uint LeaderCharacterId { get; set; }
        public uint HostCharacterId { get; set; }
        public List<CDataPartyMember> PartyMembers { get; set; }
        public bool IsReceived { get; set; }

        public class Serializer : PacketEntitySerializer<S2CParty_6_8_16_Ntc>
        {
            public override void Write(IBuffer buffer, S2CParty_6_8_16_Ntc obj)
            {
                WriteUInt32(buffer, obj.LeaderCharacterId);
                WriteUInt32(buffer, obj.HostCharacterId);
                WriteEntityList(buffer, obj.PartyMembers);
            }

            public override S2CParty_6_8_16_Ntc Read(IBuffer buffer)
            {
                S2CParty_6_8_16_Ntc obj = new S2CParty_6_8_16_Ntc();
                obj.LeaderCharacterId = ReadUInt32(buffer);
                obj.HostCharacterId = ReadUInt32(buffer);
                obj.PartyMembers = ReadEntityList<CDataPartyMember>(buffer);
                return obj;
            }
        }
    }
}
