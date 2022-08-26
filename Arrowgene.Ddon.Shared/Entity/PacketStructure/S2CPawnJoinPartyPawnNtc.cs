using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnJoinPartyPawnNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_JOIN_PARTY_PAWN_NTC;

        public S2CPawnJoinPartyPawnNtc()
        {
            PartyMember = new CDataPartyMember();
        }

        public CDataPartyMember PartyMember { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnJoinPartyPawnNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnJoinPartyPawnNtc obj)
            {
                WriteEntity<CDataPartyMember>(buffer, obj.PartyMember);
            }

            public override S2CPawnJoinPartyPawnNtc Read(IBuffer buffer)
            {
                S2CPawnJoinPartyPawnNtc obj = new S2CPawnJoinPartyPawnNtc();
                obj.PartyMember = ReadEntity<CDataPartyMember>(buffer);
                return obj;
            }

        }
    }
}
