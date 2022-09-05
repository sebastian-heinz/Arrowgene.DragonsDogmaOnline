using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyJoinNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_JOIN_NTC;
        
        public S2CPartyPartyJoinNtc()
        {
            LeaderCharacterId = 0;
            HostCharacterId = 0;
            PartyMembers = new List<CDataPartyMember>();
        }
        
        public uint LeaderCharacterId { get; set; }
        public uint HostCharacterId { get; set; }
        public List<CDataPartyMember> PartyMembers { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyJoinNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyJoinNtc obj)
            {
                WriteUInt32(buffer, obj.LeaderCharacterId);
                WriteUInt32(buffer, obj.HostCharacterId);
                WriteEntityList(buffer, obj.PartyMembers);
            }

            public override S2CPartyPartyJoinNtc Read(IBuffer buffer)
            {
                S2CPartyPartyJoinNtc obj = new S2CPartyPartyJoinNtc();
                obj.LeaderCharacterId = ReadUInt32(buffer);
                obj.HostCharacterId = ReadUInt32(buffer);
                obj.PartyMembers = ReadEntityList<CDataPartyMember>(buffer);
                return obj;
            }
        }
    }
}
